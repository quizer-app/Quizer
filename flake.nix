{
  description = "Quizer development environment and deployable packages";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    flake-utils.url = "github:numtide/flake-utils";
    devshell.url = "github:numtide/devshell";
  };

  outputs = {
    self,
    nixpkgs,
    flake-utils,
    devshell,
  }: let
    nixosModule = import ./nix/modules/quizer.nix;
  in
    flake-utils.lib.eachDefaultSystem (system: let
      pkgs = import nixpkgs {
        inherit system;
        overlays = [devshell.overlays.default];
      };

      backend = pkgs.buildDotnetModule {
        pname = "quizer-backend";
        version = "0.1.0";
        src = ./.;
        projectFile = "src/Quizer.Api/Quizer.Api.csproj";
        nugetDeps = ./deps.json;
        dotnet-sdk = pkgs.dotnet-sdk_10;
        dotnet-runtime = pkgs.dotnet-runtime_10;
        executables = ["Quizer.Api"];
      };

      frontend = pkgs.stdenv.mkDerivation {
        pname = "quizer-frontend";
        version = "0.1.0";
        src = ./frontend;

        nativeBuildInputs = [
          pkgs.nodejs_22
          pkgs.pnpm_11
          pkgs.pnpmConfigHook
        ];

        pnpmDeps = pkgs.fetchPnpmDeps {
          pname = "quizer-frontend";
          version = "0.1.0";
          src = ./frontend;
          fetcherVersion = 3;
          hash = "sha256-kxxp6erd6q/6hv6mvdJ00CaaNa3N4Jh77o6kvUpFrWs=";
        };

        buildPhase = ''
          runHook preBuild
          pnpm run build
          runHook postBuild
        '';

        installPhase = ''
          runHook preInstall
          mkdir -p $out
          cp -r dist/* $out/
          runHook postInstall
        '';
      };

      app = pkgs.symlinkJoin {
        name = "quizer";
        paths = [backend];
        nativeBuildInputs = [pkgs.makeWrapper];
        postBuild = ''
          install -d $out/lib/quizer/wwwroot
          cp -r ${frontend}/* $out/lib/quizer/wwwroot/

          wrapProgram $out/bin/Quizer.Api \
            --set ASPNETCORE_ENVIRONMENT Production
        '';
      };
    in {
      devShells.default = pkgs.devshell.mkShell {
        name = "quizer";

        packages = with pkgs; [
          dotnet-sdk_10
          nodejs_22
          pnpm_11
          nuget-to-json
        ];

        env = [
          {
            name = "DOTNET_ROOT";
            value = "${pkgs.dotnet-sdk_10}";
          }
          {
            name = "DOTNET_CLI_TELEMETRY_OPTOUT";
            value = "1";
          }
          {
            name = "DOTNET_NOLOGO";
            value = "1";
          }
        ];

        commands = [
          {
            name = "api";
            help = "Run the ASP.NET API";
            command = "dotnet run --project src/Quizer.Api/Quizer.Api.csproj";
          }
          {
            name = "web";
            help = "Run the frontend dev server";
            command = "pnpm --dir frontend dev";
          }
          {
            name = "restore";
            help = "Restore backend and frontend dependencies";
            command = "dotnet restore Quizer.slnx && pnpm --dir frontend install";
          }
        ];

        devshell.startup.quizer.text = ''
          echo "Quizer dev shell"
          echo "  dotnet: $(dotnet --version)"
          echo "  node:   $(node --version)"
          echo "  pnpm:   $(pnpm --version)"
        '';
      };

      packages = {
        quizer-backend = backend;
        quizer-frontend = frontend;
        quizer = app;
        default = app;
      };
    })
    // {
      nixosModules.default = nixosModule;
      nixosModules.quizer = nixosModule;
    };
}

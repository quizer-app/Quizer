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
    backendPackage = import ./nix/quizer-backend.nix;
    frontendPackage = import ./nix/quizer-frontend.nix;
  in
    flake-utils.lib.eachDefaultSystem (system: let
      pkgs = import nixpkgs {
        inherit system;
        overlays = [devshell.overlays.default];
      };

      backend = backendPackage {
        inherit pkgs;
        src = ./.;
      };

      frontend = frontendPackage {
        inherit pkgs;
        frontendSrc = ./frontend;
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
          {
            name = "test-seed";
            help = "Run seed scenario against the API";
            command = "dotnet run --project tests/Quizer.Seed/Quizer.Seed.csproj";
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

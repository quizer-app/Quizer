{
  description = "Quizer development environment";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    flake-utils.url = "github:numtide/flake-utils";
    devshell.url = "github:numtide/devshell";
  };

  outputs = {
    nixpkgs,
    flake-utils,
    devshell,
  }:
    flake-utils.lib.eachDefaultSystem (system: let
      pkgs = import nixpkgs {
        inherit system;
        overlays = [devshell.overlays.default];
      };
    in {
      devShells.default = pkgs.devshell.mkShell {
        name = "quizer";

        packages = with pkgs; [
          dotnet-sdk_10
          nodejs_22
          pnpm
        ];

        env = [
          {
            name = "DOTNET_ROOT";
            value = "${pkgs.dotnet-sdk_8}";
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
    });
}

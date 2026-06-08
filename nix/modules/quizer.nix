{
  lib,
  config,
  ...
}: let
  cfg = config.services.quizer;
in {
  options.services.quizer = {
    enable = lib.mkEnableOption "Quizer API with embedded frontend";
    package = lib.mkOption {
      type = lib.types.nullOr lib.types.package;
      default = null;
      example = lib.literalExpression "inputs.quizer.packages.<system>.quizer";
      description = "Quizer package that contains backend and frontend assets.";
    };
    user = lib.mkOption {
      type = lib.types.str;
      default = "quizer";
      description = "User account under which Quizer runs.";
    };
    group = lib.mkOption {
      type = lib.types.str;
      default = "quizer";
      description = "Group under which Quizer runs.";
    };
    host = lib.mkOption {
      type = lib.types.str;
      default = "0.0.0.0";
      description = "Host address used by the Quizer HTTP listener.";
    };
    port = lib.mkOption {
      type = lib.types.port;
      default = 5000;
      description = "TCP port used by the Quizer HTTP listener.";
    };
    environment = lib.mkOption {
      type = lib.types.attrsOf lib.types.str;
      default = {};
      description = "Additional environment variables for Quizer.";
    };
    environmentFiles = lib.mkOption {
      type = lib.types.listOf lib.types.path;
      default = [];
      example = ["/run/secrets/quizer.env"];
      description = "Additional environment files containing Quizer configuration.";
    };
  };

  config = lib.mkIf cfg.enable {
    assertions = [
      {
        assertion = cfg.package != null;
        message = "services.quizer.package must be set to the combined Quizer package.";
      }
    ];

    users.users = lib.mkIf (cfg.user == "quizer") {
      quizer = {
        inherit (cfg) group;
        isSystemUser = true;
      };
    };

    users.groups = lib.mkIf (cfg.group == "quizer") {
      quizer = {};
    };

    systemd.services.quizer = {
      description = "Quizer backend service";
      wantedBy = ["multi-user.target"];
      after = ["network-online.target"];
      wants = ["network-online.target"];

      environment =
        {
          ASPNETCORE_URLS = "http://${cfg.host}:${toString cfg.port}";
          ASPNETCORE_ENVIRONMENT = "Production";
        }
        // cfg.environment;

      serviceConfig = {
        EnvironmentFile = cfg.environmentFiles;
        Type = "simple";
        User = cfg.user;
        Group = cfg.group;
        WorkingDirectory = "${cfg.package}/lib/quizer-backend";
        ExecStart = "${cfg.package}/bin/Quizer.Api";
        Restart = "on-failure";
        RestartSec = "5s";
      };
    };
  };
}

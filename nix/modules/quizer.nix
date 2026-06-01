{ lib, config, ... }:
let
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
        isSystemUser = true;
        group = cfg.group;
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
          ASPNETCORE_URLS = "http://0.0.0.0:${toString cfg.port}";
          ASPNETCORE_ENVIRONMENT = "Production";
        }
        // cfg.environment;

      serviceConfig = {
        Type = "simple";
        User = cfg.user;
        Group = cfg.group;
        WorkingDirectory = "${cfg.package}/lib/quizer";
        ExecStart = "${cfg.package}/bin/Quizer.Api";
        Restart = "on-failure";
        RestartSec = "5s";
      };
    };
  };
}

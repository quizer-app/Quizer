{pkgs, src}:
pkgs.buildDotnetModule {
  pname = "quizer-backend";
  version = "0.1.0";
  inherit src;
  projectFile = "src/Quizer.Api/Quizer.Api.csproj";
  nugetDeps = ./../deps.json;
  dotnet-sdk = pkgs.dotnet-sdk_10;
  dotnet-runtime = pkgs.dotnet-runtime_10;
  executables = ["Quizer.Api"];
}

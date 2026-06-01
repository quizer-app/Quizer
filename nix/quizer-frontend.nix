{pkgs, frontendSrc}:
pkgs.stdenv.mkDerivation {
  pname = "quizer-frontend";
  version = "0.1.0";
  src = frontendSrc;

  nativeBuildInputs = [
    pkgs.nodejs_22
    pkgs.pnpm_11
    pkgs.pnpmConfigHook
  ];

  pnpmDeps = pkgs.fetchPnpmDeps {
    pname = "quizer-frontend";
    version = "0.1.0";
    src = frontendSrc;
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
}

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
    hash = "sha256-FTqw4oPLK+fzMTel7EGy8WuDVuj5uaLAN44V1Hd2LFM=";
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

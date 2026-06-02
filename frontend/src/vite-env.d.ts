/// <reference types="vite/client" />
interface ImportMetaEnv {
  readonly VITE_BETTERSTACK_SOURCE_TOKEN?: string;
  readonly VITE_BETTERSTACK_ENDPOINT?: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}

/// <reference types="vite/client" />

declare module '*.vue' {
  import type Vue, { DefineComponent } from 'vue'
  const component: DefineComponent<{}, {}, any>
  export const component
}


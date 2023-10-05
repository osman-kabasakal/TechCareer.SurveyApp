import vue from '@vitejs/plugin-vue'
import vuetify, {transformAssetUrls} from 'vite-plugin-vuetify'
import ViteFonts from 'unplugin-fonts/vite'
import pluginRewriteAll from 'vite-plugin-rewrite-all';
import {spawn} from 'child_process';
import fs from 'fs';
import {resolve} from 'path';

const cssPattern = /\.css$/;
const imagePattern = /\.(png|jpe?g|gif|svg|webp|avif)$/;

// Utilities
import {UserConfig, defineConfig} from 'vite'
import {fileURLToPath, URL} from 'node:url'
export default defineConfig(async () => {
  var config: UserConfig = {
    appType: 'custom',
    plugins: [
      vue({
        template: {transformAssetUrls}
      }),
      vuetify({
        autoImport: true,
      }),
      ViteFonts({
        google: {
          families: [{
            name: 'Roboto',
            styles: 'wght@100;300;400;500;700;900',
          }],
        },
      }),
    ],
    define: {'process.env': {}},
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url))
      },
      extensions: [
        '.js',
        '.json',
        '.jsx',
        '.mjs',
        '.ts',
        '.tsx',
        '.vue',
      ],
    },
    root: resolve(__dirname,'../VueApp'),
    publicDir: resolve(__dirname,'public'),
    build: {
      manifest: "assets.manifest.json",
      emptyOutDir: true,
      outDir: resolve(__dirname,'../wwwroot'),
      assetsDir: '',
      rollupOptions: {
        input: {index:resolve(__dirname,'src/views/Home/Index/main.ts')},
        output: {
          entryFileNames: 'js/[name].[hash].js',
          chunkFileNames: 'js/[name].[hash]-chunk.js',
          assetFileNames: (info) => {
            if (info.name) {
              if (cssPattern.test(info.name)) {
                return 'css/[name].[hash][extname]';
              }
              if (imagePattern.test(info.name)) {
                return 'images/[name].[hash][extname]';
              }

              return 'assets/[name].[hash][extname]';
            } else {
              return '[name].[hash][extname]';
            }
          },
        }
      },
    },
    server: {
      host: 'localhost',
      strictPort: false,
      port: 5173,
      https: false,
      // hmr: {
      //   host:'localhost',
      //   clientPort: 5173,

      // }
    },
    optimizeDeps: {
      include: []
    }
  };

  return config;
});

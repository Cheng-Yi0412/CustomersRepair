import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'

// https://vite.dev/config/
/*
export default defineConfig({
  plugins: [
    vue(),
    vueDevTools(),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    },
  },
})
*/
export default defineConfig({
  // 1. 原本的：處理 Vue 檔案
  plugins: [
    vue(),
    vueDevTools(),
  ],
  // 2. 原本的：處理 @ 捷徑
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    },
  },
  // 3. ✨ 新加的：處理跨網域 (CORS) 代理，指向你的 .NET 7173 房！
  server: {
    proxy: {
      '/api': {
        target: 'https://localhost:7173', 
        changeOrigin: true,
        secure: false,
      }
    }
  }
})
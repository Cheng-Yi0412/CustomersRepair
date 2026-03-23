//import './assets/main.css'
import 'bootstrap/dist/css/bootstrap.min.css' // 引入 CSS
import 'bootstrap/dist/js/bootstrap.bundle.min.js' // 引入 JS (包含 Popper)
import '@/assets/table.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'

const app = createApp(App)

app.use(createPinia())
app.use(router)

app.mount('#app')

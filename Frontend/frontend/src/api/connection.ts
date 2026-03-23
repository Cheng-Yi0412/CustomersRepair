// src/api/request.ts
import axios from 'axios'

const service = axios.create({
  baseURL: 'https://localhost:7173/api',
  timeout: 5000
})

export default service // 把這個「特製的 axios」匯出給別人用
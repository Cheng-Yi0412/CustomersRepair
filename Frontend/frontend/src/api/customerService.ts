// src/api/customerService.ts
import request from './connection' // 引入物流車隊

interface Customer {
  customerId: number;
  cna: string;   
  ctel: string;
  cteL1: string;
  cskind: string;
  caddr: string;
  cpostno: number;
  cco: string;
  cmemo: string;
}

// 1. 拿取所有客戶 (這就是你原本寫在 Vue 裡的那段 GET)
export const getAllCustomers = () => {
  return request.get('/Customer'); // 組合起來變成：https://localhost:7001/api/Customer
}

export const updateCustomers = (customerId: number, data: Customer) => {
  return request.put(`/Customer/${customerId}`, data);
}

// 未來如果你要寫「新增客戶」，就會加在這裡：
// export const createCustomer = (data) => request.post('/Customer', data)
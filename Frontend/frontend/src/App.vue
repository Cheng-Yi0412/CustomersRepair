<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { ref, onMounted, computed } from 'vue';
import {getAllCustomers, updateCustomers} from './api/customerService'


interface Customer {
  customerId: number;
  cna: string;   
  ctel: string;
  cteL1:string;
  cskind: string;
  caddr: string;
  cpostno: number;
  cco: string;
  cmemo: string;
}



// --------- 客戶資料編輯視窗 ---------
const isOpenEditor=ref(false);
const EditingCustomer = ref<Customer | null>(null);

const openEditModal = (customer: Customer) => {

  EditingCustomer.value = { ...customer }; 
  isOpenEditor.value = true;
};

//關閉視窗
const closeDetail = () => {
  isOpenEditor.value = false;
  EditingCustomer.value = null;
};

//存檔功能 
const saveChange = async () => {
  if (!EditingCustomer.value) return;

  try {
    const response = await updateCustomers(EditingCustomer.value.customerId, EditingCustomer.value);
    if (response.status === 200 || response.status === 204) {
      alert('資料儲存成功！');
      await fetchAllData();       
      closeDetail();
    }
  } catch (error) {
    console.error('存檔失敗：', error);
    alert('存檔過程發生錯誤，請稍後再試。');
  }
};
//-------------------------------------



const customerList = ref<Customer[]>([]);
const Currentpage = ref(1);
const Pagesize = ref(30);

const fetchAllData = async () => {
  try {
    const response = await getAllCustomers(); 
    customerList.value = response.data;
  } catch (error) {
    console.error("抓取失敗：", error);
  }
};

const Pagedata = computed(() => {
  const start = (Currentpage.value - 1) * Pagesize.value;
  const end = start + Pagesize.value;
  return customerList.value.slice(start, end);
});

onMounted(() => { //立即執行
  fetchAllData();
});
</script>

<template>
  <header>
    <nav class="navbar navbar-expand-lg bg-body-tertiary">
  <div class="container-fluid">
    <div class="collapse navbar-collapse" id="navbarSupportedContent">
      <ul class="navbar-nav me-auto mb-2 mb-lg-0">
        <li class="nav-item">
          <RouterLink class="nav-link active" to="/">主頁</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link" to="/about">地址搜尋</RouterLink>
        </li>
        <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
            Dropdown
          </a>
          <ul class="dropdown-menu">
            <li><a class="dropdown-item" href="#">Action</a></li>            
          </ul>
        </li>
        <li class="nav-item">
          <a class="nav-link disabled" aria-disabled="true">Disabled</a>
        </li>
      </ul>
      <form class="d-flex" role="search">
        <input class="form-control me-2" type="search" placeholder="Search" aria-label="Search"/>
        <button class="btn btn-outline-success" type="submit">Search</button>
      </form>
    </div>
  </div>
</nav>
  </header>
<div class="table-responsive">
  <table class="table table-striped table-bordered">
    <thead>
    <tr>
      <th scope="col">id</th>
      <th scope="col">客戶名稱</th>
      <th scope="col">電話</th>
      <th scope="col">手機號碼</th>
      <th scope="col">客戶類型</th>
      <th scope="col">地址</th>
      <th scope="col" style="display: none;">郵遞區號</th>
      <th scope="col">備註1</th>
      <th scope="col">備註2</th>
    </tr>
  </thead>
  <tbody>
    <tr v-on:dblclick="openEditModal(customer)" v-for="customer in Pagedata" :key="customer.customerId" style="cursor: pointer;" title="雙擊以編輯資料" >
        <td>{{ customer.cna }}</td>     
        <td>{{ customer.ctel }}</td>  
        <td>{{ customer.cteL1 }}</td>
        <td>{{ customer.cskind }}</td>
        <td>{{ customer.caddr }}</td>
        <td style="display: none;">{{ customer.cpostno }}</td>
        <td>{{ customer.cco }}</td>
        <td>{{ customer.cmemo }}</td> 
      </tr>
  </tbody>
  </table>   
</div>
  <div style="display: flex; justify-content: center; align-items: center; gap: 15px; margin-top: 2rem">
    <input type="number" v-model="Currentpage" class="form-control" style="width: 100px;" min="1">
    <button class="btn btn-primary" @click="Currentpage--" :disabled="Currentpage === 1">上一頁</button>
    <span>第 {{ Currentpage }} 頁</span>
    <button class="btn btn-primary" @click="Currentpage++" :disabled="Currentpage * Pagesize >= customerList.length">下一頁</button>
  </div>



<!--------- 客戶資料編輯視窗 --------->
  <div v-if="isOpenEditor" class="modal-overlay">
  <div class="modal-content">
    <h3>編輯客戶資料</h3>
    <hr>
    
    
    <div v-if="EditingCustomer">
       <div class="mb-3">
        <label>客戶Id：</label>
        <input v-model="EditingCustomer.customerId" class="form-control" readonly style="background-color: #eee;">
      </div>
      <div class="mb-3">
        <label>客戶姓名：</label>
        <input v-model="EditingCustomer.cna" class="form-control">
      </div>
      <div class="mb-3">
        <label>連絡電話：</label>
        <input v-model="EditingCustomer.ctel" class="form-control">
      </div>
    </div>

    <div class="text-end">
      <button class="btn btn-secondary me-2" @click="closeDetail">取消</button>
      <button class="btn btn-success" @click="saveChange">確認存檔</button>
    </div>
  </div>
</div>
<!------------------------>

  <RouterView />
</template>






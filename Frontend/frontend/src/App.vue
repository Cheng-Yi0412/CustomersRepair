<script setup lang="ts">
import { RouterView } from 'vue-router'
import { ref } from 'vue';
import CustomerList from './components/CustomerList.vue';
import TheNavber from './components/TheNavber.vue';
import CustomerRepair from './components/CustomerRepair.vue';

//---------客戶資料選擇---------
const selectedId = ref<number | null>(null);
const selectedTel = ref<string>(''); 

// 💡 修正 1：把 payload.id 改為 payload.customerId，與 CustomerList 發射的名稱一致
const handleCustomerSelect = (payload: { customerId: number, ctel: string }) => {
  selectedId.value = payload.customerId; 
  selectedTel.value = payload.ctel;
  console.log('Selected Customer ID:', selectedId.value);
};
</script>

<template>
  <header>
    <TheNavber />
  </header>

  <div class="container-fluid">
    <div class="row mt-3"> <div class="col-6">
        <CustomerList @select-customer="handleCustomerSelect" />
      </div>

      <div class="col-6">
        <CustomerRepair :receivedId="selectedId" :receivedTel="selectedTel" />
      </div>

    </div>
  </div>
  <RouterView />
</template>
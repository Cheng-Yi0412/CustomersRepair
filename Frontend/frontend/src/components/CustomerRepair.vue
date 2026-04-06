<script setup lang="ts">
import { ref, watch } from 'vue';
import { getcustomerRepair } from '@/api/customerService';

interface Repair {
    rtel?: string;
    Rdate1?: string;
    Rtime?: string;
    Rno?: string;
    Rprodno?: string;
    Rproduct?: string;
    Rnano?: number;
    Rna?: string;
    cmemo?: string;
}

const createEmptyRepair = (tel: string = ''): Repair => ({
    rtel: tel,
    Rdate1: '',
    Rtime: '',
    Rno: '',
    Rprodno: '',
    Rproduct: '',
    Rnano: 0,
    Rna: '',
    cmemo: ''
});

const repairData = ref<Repair>(createEmptyRepair());

// 💡 修正重點 1：加入一個用來存放「所有」維修紀錄的陣列
const repairList = ref<Repair[]>([]);

const props = defineProps<{
    receivedId?: number | null,
    receivedTel?: string
}>();

const loadData = async (tel: string) => {
    try {
        const res = await getcustomerRepair(tel);
        
        // 💡 修正重點 2：在瀏覽器控制台印出原始資料，讓你確認 API 到底讀到了什麼
        console.log(`API 回傳原始資料 (${tel}):`, res.data);

        // 將所有資料存入陣列 (如果 res.data 是 null 則給空陣列)
        repairList.value = res.data || [];

        // 判斷陣列內是否有資料
        if (repairList.value.length > 0) {
            // 將第一筆 (通常是最新) 放到 repairData 顯示在上方的 Input
            repairData.value = { ...repairList.value[0] }; 
        } else {
            // 若查無資料，重置為空物件但保留電話
            repairData.value = createEmptyRepair(tel);
        }
    } catch (error) {
        console.error('抓取維修資料失敗', error);
        repairData.value = createEmptyRepair(tel);
        repairList.value = []; // 發生錯誤時清空列表
    }
};

watch(() => props.receivedTel, (newVal) => {
    if (newVal) {
        repairData.value.rtel = newVal;
        loadData(newVal);
    } else {
        // 如果傳入的電話被清空，一併重置畫面資料
        repairData.value = createEmptyRepair('');
        repairList.value = [];
    }
}, { immediate: true }); 

</script>

<template>
  <div class="p-3">
    <div class="mb-2">
      <label>接收 ID：</label>
      <input type="text" :value="props.receivedId" placeholder="Received ID" readonly class="form-control" />
    </div>

    <div class="mb-2">
      <label>聯絡電話：</label>
      <input type="text" v-model="repairData.rtel" placeholder="Received Tel" class="form-control" />
    </div>

    <div class="mb-2">
      <label>最新維修產品：</label>
      <input type="text" v-model="repairData.Rproduct" placeholder="維修產品" class="form-control" />
    </div>

    <hr class="my-4" />

    <div>
      <h5>維修歷史清單 (共 {{ repairList.length }} 筆)</h5>
      
      <table v-if="repairList.length > 0" class="table table-sm table-bordered mt-2">
        <thead class="table-light">
          <tr>
            <th>維修單號</th>
            <th>日期</th>
            <th>產品名稱</th>
            <th>備註</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(item, index) in repairList" :key="item.Rno || index">
            <td>{{ item.Rno }}</td>
            <td>{{ item.Rdate1 }}</td>
            <td>{{ item.Rproduct }}</td>
            <td>{{ item.cmemo }}</td>
          </tr>
        </tbody>
      </table>
      
      <div v-else class="alert alert-secondary mt-2">
        此客戶目前沒有維修紀錄。
      </div>
    </div>
  </div>
</template>
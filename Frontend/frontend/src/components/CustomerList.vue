<script setup lang="ts">
//import { RouterLink, RouterView } from 'vue-router'
import { ref, onMounted, computed } from 'vue';
import { addCustomers, getAllCustomers, updateCustomers } from '@/api/customerService'
import axios from 'axios';

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


// --------- 客戶資料編輯視窗 ---------
const isOpenEditor = ref(false);
const EditingCustomer = ref<Customer | null>(null);
const ModalMode = ref<'add' | 'edit'>('add');

//開啟新增視窗
const OpenAddModal = () => {
    EditingCustomer.value = {
        customerId: 0,  //資料庫Id
        cna: '',  //客戶名稱
        ctel: '', //連絡電話
        cteL1: '',  //手機號碼
        cskind: '',//客戶類別
        caddr: '',  //地址
        cpostno: 0, //郵遞區號
        cco: '',  //備註1
        cmemo: '' //備註2
    };
    isOpenEditor.value = true;
};

//開啟編輯視窗
const openEditModal = (customer: Customer) => {
    ModalMode.value = 'edit';
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
    let response;

    try {
        if (ModalMode.value === 'add') {
            response = await addCustomers(EditingCustomer.value);
        } else {
            response = await updateCustomers(EditingCustomer.value.customerId, EditingCustomer.value);
        }

        if (response.status === 200 || response.status === 204 || response.status === 201) {
            alert('資料儲存成功！');
            await fetchAllData();
            closeDetail();
        }
    } catch (error) {
        if (axios.isAxiosError(error)) {
            const status = error.response?.status;
            const serverMsg = error.response?.data;

            if (status === 400) {
                console.log(serverMsg);
                alert(`錯誤${status}： ${serverMsg}`);
            }
        }
        else {
            console.error('存檔失敗：', error);
            alert('存檔過程發生錯誤，請稍後再試。');
        }
    }
};
// -------------搜尋功能--------------
const SearchQry = ref('');

// --------- 分頁功能 ---------
const customerList = ref<Customer[]>([]);
const Currentpage = ref(1);
const Pagesize = ref(30);


const filteredCustomers = computed(() => {
    const query = SearchQry.value.trim().toLowerCase();
    if (!query) {
        return customerList.value;
    } else {
        return customerList.value.filter(customer => {
            const name = (customer.cna ?? '').toLowerCase();
            const tel = (customer.ctel ?? '').toLowerCase();
            const tel1 = (customer.cteL1 ?? '').toLowerCase();
            const addr = (customer.caddr ?? '').toLowerCase();
            return (name.includes(query) ||
                tel.includes(query) ||
                tel1.includes(query) ||
                addr.includes(query)
            );
        });
    }
}
);
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
    return filteredCustomers.value.slice(start, end);
});
//-------傳資料給父層-------
const emit = defineEmits<{
  (e: 'select-customer', payload: { customerId: number, ctel: string }): void
}>();

const sendIdToHome = (customerId: number, ctel: string) => {
    emit('select-customer', {customerId, ctel}); // 觸發事件並帶上 ID
};
//-------------------------
onMounted(() => { //立即執行
    fetchAllData();
});
</script>

<template>
    <div class="d-flex align-items-center mb-3">
        <button @click="OpenAddModal()" class="btn btn-primary">
            新增客戶
        </button>
        <form class="d-flex" role="search">
            <input v-model="SearchQry" class="form-control me-2" type="search" placeholder="搜尋" />
        </form>
    </div>


    <div class="row justify-content-center">
        <div class="col-10">
            <table class="table table-striped table-bordered table-responsive">
                <thead>
                    <tr>
                        <th scope="col"> </th>
                        <th scope="col" style="display: none;">id</th>
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
                    <tr @dblclick="openEditModal(customer)" v-for="customer in Pagedata" :key="customer.customerId"
                        style="cursor: pointer;" title="雙擊以編輯資料">
                        <td><button @click="sendIdToHome(customer.customerId, customer.ctel)">選擇</button></td>
                        <td style="display: none;">{{ customer.customerId }}</td>
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
    </div>


    <div style="display: flex; justify-content: center; align-items: center; gap: 15px; margin-top: 2rem">
        <input type="number" v-model="Currentpage" class="form-control" style="width: 100px;" min="1">
        <button class="btn btn-primary" @click="Currentpage--" :disabled="Currentpage === 1">上一頁</button>
        <span>第 {{ Currentpage }} 頁</span>
        <button class="btn btn-primary" @click="Currentpage++"
            :disabled="Currentpage * Pagesize >= customerList.length">下一頁</button>
    </div>



    <!--------- 客戶資料編輯視窗 --------->
    <div v-if="isOpenEditor" class="modal-overlay">
        <div class="modal-content">
            <h3>編輯客戶資料</h3>
            <hr>


            <div v-if="EditingCustomer">
                <div class="mb-3">
                    <label>客戶Id：</label>
                    <input v-model="EditingCustomer.customerId" class="form-control" readonly
                        style="background-color: #eee;">
                </div>
                <div class="mb-3">
                    <label>客戶姓名：</label>
                    <input v-model="EditingCustomer.cna" class="form-control">
                </div>
                <div class="mb-3">
                    <label>電話：</label>
                    <input v-model="EditingCustomer.ctel" class="form-control">
                </div>
                <div class="mb-3">
                    <label>手機：</label>
                    <input v-model="EditingCustomer.cteL1" class="form-control">
                </div>
                <div class="mb-3">
                    <label>地址</label>
                    <input v-model="EditingCustomer.caddr" class="form-control">
                </div>
                <div class="mb-3">
                    <label>備註1</label>
                    <input v-model="EditingCustomer.cco" class="form-control">
                </div>
                <div class="mb-3">
                    <label>備註2</label>
                    <input v-model="EditingCustomer.cmemo" class="form-control">
                </div>
            </div>

            <div class="text-end">
                <button class="btn btn-secondary me-2" @click="closeDetail">取消</button>
                <button class="btn btn-success" @click="saveChange">確認存檔</button>
            </div>
        </div>
    </div>
    <!------------------------>
</template>

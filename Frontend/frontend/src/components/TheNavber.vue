<script setup lang="ts">

</script>

<template>
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
          <li class="nav-item">
            <a class="nav-link" href="#" @click.prevent>新增客戶</a>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>
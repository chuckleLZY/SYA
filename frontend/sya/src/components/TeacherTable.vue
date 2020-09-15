<template>
  <el-container style="height: 100%">
    <el-aside width="200px">
      <el-menu
        class="el-menu-vertical-demo"
        background-color="#D3DCE6"
        text-color="#333"
        active-text-color="#0A5AA8"
        style="height: 100%"
        :default-active="$route.path"
        router
      >
        <el-menu-item index="/ReceivedResume">
          <i class="el-icon-message-solid"></i>
          <span slot="title">查看申请</span>
        </el-menu-item>
        <el-menu-item index="/PublishWorkMessage">
          <i class="el-icon-edit-outline"></i>
          <span slot="title">发布工作</span>
        </el-menu-item>
        <el-menu-item index="/MyPublishedWork">
          <i class="el-icon-s-flag"></i>
          <span slot="title">发布记录</span>
        </el-menu-item>
      </el-menu>
    </el-aside>
    <el-main>
      <router-view></router-view>
    </el-main>
  </el-container>
</template>

<script>
</script>


<style scoped>
.el-header,
.el-footer {
  background-color: #b3c0d1;
  color: #333;
  text-align: center;
  /* line-height: 60px; */
}

.el-aside {
  background-color: #d3dce6;
  color: #333;
  text-align: center;
  /* line-height: 200px; */
}

.el-main {
  background-color: #e9eef3;
  color: #333;
  text-align: center;
  /* line-height: 160px; */
}

body > .el-container {
  margin-bottom: 40px;
}

/* .el-container:nth-child(5) .el-aside,
  .el-container:nth-child(6) .el-aside {
    line-height: 260px;
  }
  
  .el-container:nth-child(7) .el-aside {
    line-height: 320px;
  } */
</style>

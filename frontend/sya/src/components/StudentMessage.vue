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
        <el-menu-item index="/StudentSystemMessage">
          <i class="el-icon-menu"></i>
          <span slot="title">系统公告</span>
        </el-menu-item>
        <el-menu-item index="/StudentWorkMessage">
          <i class="el-icon-menu"></i>
          <span slot="title">工作通知</span>
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
  background-color: #cfdaeb;
  color: #333;
  text-align: center;
  /* line-height: 60px; */
}

.el-aside {
  background-color: #fff;
  
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

.el-menu{
  box-shadow: 10px 10px 5px #888888 !important
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

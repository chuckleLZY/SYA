<template>
  <el-container style="height: 100%">
    <el-aside width="200px">
      <el-menu
        class="el-menu-vertical-demo"
        background-color="#00008B"
        text-color="#fff"
        active-text-color="#ffd04b"
        style="height: 100%"
        :default-active="$route.path"
        router
      >
        <el-menu-item index="/TeacherInformation">
          <i class="el-icon-menu"></i>
          <span slot="title">老师个人资料</span>
        </el-menu-item>
        <el-menu-item index="/TeacherQualification">
          <i class="el-icon-menu"></i>
          <span slot="title">老师资格认证</span>
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

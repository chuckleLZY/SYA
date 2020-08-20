<template>
    <el-container style="height: 100%">
        <el-aside width="200px">
                <el-menu
                class="el-menu-vertical-demo"
                background-color="#545c64"
                text-color="#fff"
                active-text-color="#ffd04b"
                style="height: 100%"
                router>
                <el-menu-item index="/StudentInformation">
                    <i class="el-icon-menu"></i>
                    <span slot="title">学生我的资料</span>
                </el-menu-item>
                <el-menu-item index="/StudentResume">
                    <i class="el-icon-menu"></i>
                    <span slot="title">学生个人简历</span>
                </el-menu-item>
                <el-menu-item index="/StudentWorkManagement">
                    <i class="el-icon-menu"></i>
                    <span slot="title">学生工作管理</span>
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
  .el-header, .el-footer {
    background-color: #B3C0D1;
    color: #333;
    text-align: center;
    /* line-height: 60px; */
  }
  
  .el-aside {
    background-color: #D3DCE6;
    color: #333;
    text-align: center;
    /* line-height: 200px; */
  }
  
  .el-main {
    background-color: #E9EEF3;
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

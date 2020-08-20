<template>
  <div id="app">
    <el-container style="height: 100%">
      <el-header style="padding-left: 0px;padding-right: 0px;">
        <!-- 导航栏 -->
        <Header></Header>
      </el-header>
      <el-main style="padding-left: 0px;padding-right: 0px;padding-top: 0px;padding-bottom: 0px">
        <!-- 路由跳转 -->
        <router-view></router-view>
      </el-main>
      <el-footer style="padding-left: 0px;padding-right: 0px;">
        Footer
      </el-footer> 
    </el-container>
  </div>
</template>


<script>
import Header from './components/Header.vue';
export default {
  name: 'App',
  components: {
    Header,    
  }
}
</script>



<style scoped>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  height: 100%;
}



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

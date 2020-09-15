<template>
  <div id="app">
    <router-view></router-view>
  </div>
</template>


<script>
import Main from "./components/Main.vue";
import axios from "axios";
import { chownSync } from "fs";
export default {
  name: "App",
  components: {
    Main
  },
  async created() {
    const result = await axios.post(
      "http://localhost:5000/Account/LoginStatus",
      {},
      { withCredentials: true }
    );
    console.log("result", result);
    if (result.status == 200) {
      console.log("result", 200);
      if (result.data == -1) {
        //未登录，无cookies
        this.$router.push("/LogIn");
        return;
      } else {
        //已登录，有cookies
        console.log(this.$route.path);
        this.$store.commit("logIn", result.data.role);
      }
    } else {
      console.log("error");
      // this.$router.push("/LogIn");
    }
    // //在页面加载时读取sessionStorage里的状态信息
    // if (sessionStorage.getItem("store")) {
    //   this.$store.replaceState(
    //     Object.assign(
    //       {},
    //       this.$store.state,
    //       JSON.parse(sessionStorage.getItem("store"))
    //     )
    //   );
    // }

    // //在页面刷新时将vuex里的信息保存到sessionStorage里
    // window.addEventListener("beforeunload", () => {
    //   sessionStorage.setItem("store", JSON.stringify(this.$store.state));
    // });
  }
};
</script>

<style>
@import "./assets/font/font.css";
@import url("https://fonts.googleapis.com/css?family=Montserrat");

* {
  font-family: "Montserrat", "jf-openhuninn-1.1";
}
</style>

<style scoped>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  height: 100%;
}

.el-header,
.el-footer {
  background-color: #00008b;
  color: #333;
  text-align: center;
  /* line-height: 60px; */
}

.el-aside {
  background-color: #00008b;
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

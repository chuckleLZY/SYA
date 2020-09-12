<template>
  <div>
    <el-row>
      <el-col :span="8" style="padding-top:12px;padding-right:12px">
        <!-- <div> -->
        <!-- <el-container> -->
        <!-- <div class="logo"> -->
        <div>
          <img
            src="../assets/imagebox/sya_logo.png"
            alt
            height="38"
            style="float: left;padding-left:50px"
            @click="toHome()"
            class="img"
          />
        </div>
        <!-- </div> -->
        <!-- </el-container> -->
        <!-- </div> -->
      </el-col>
      <el-col :span="8">
        <div style="display: inline-block">
          <el-menu
            class="el-menu-demo"
            mode="horizontal"
            background-color="rgba(220,38,38,0)"
            text-color="#fff"
            active-text-color="#ffd04b"
            :default-active="$route.path"
            router
          >
            <el-menu-item index="/Home">首页</el-menu-item>
            <el-menu-item index="/Recruitment">招聘会</el-menu-item>
            <el-menu-item index="/TeacherTable" v-if="this.$store.state.role==2">老师工作台</el-menu-item>
            <!-- <el-menu-item index="/AdminTable" v-if="this.$store.state.role==0">管理员工作台</el-menu-item> -->
            <el-menu-item index="/PublishSystemMessage" v-if="this.$store.state.role==0">管理员工作台</el-menu-item>
            <el-menu-item index="/StudentMessage" v-if="this.$store.state.role==1">学生消息</el-menu-item>
            <el-menu-item index="/TeacherMessage" v-if="this.$store.state.role==2">老师消息</el-menu-item>
            <!--  <el-menu-item index="/AdminMessage" v-if="this.$store.state.role==2">管理员消息</el-menu-item>-->
            <el-menu-item index="/StudentMine" v-if="this.$store.state.role==1">学生我的</el-menu-item>
            <el-menu-item index="/TeacherMine" v-if="this.$store.state.role==2">老师我的</el-menu-item>
            <el-menu-item index="/AdminMine" v-if="this.$store.state.role==0">管理员我的</el-menu-item>
            <el-menu-item index="/StudentFavorites" v-if="this.$store.state.role==1">学生收藏夹</el-menu-item>
            <el-menu-item index="/TeacherFavorites" v-if="this.$store.state.role==2">老师收藏夹</el-menu-item>
          </el-menu>
        </div>
      </el-col>
      <el-col :span="8">
        <div
          height="38"
          style="padding-top:10px;float: right;padding-right:50px"
          @click="toInfo()"
          class="img"
          v-if="!this.$store.state.role==0"
        >
          <el-avatar :src="imgSrc"></el-avatar>
        </div>
      </el-col>
    </el-row>
  </div>
</template>


<script>
import axios from "axios";
export default {
  data() {
    return {
      imgSrc: ""
    };
  },
  methods: {
    toHome() {
      if (this.$route.path === "/Home") return;
      this.$router.push("/Home");
    },
    async toInfo() {
      if (this.$store.state.role == 1) {
        this.$router.push("/StudentInformation");
      } else if (this.$store.state.role == 2) {
        this.$router.push("/TeacherInformation");
      } else {
        console.log("toInfo error");
      }
    }
  },
  async created() {
    const { data: res } = await axios.post(
      "http://localhost:5000/User/GetUserInfo",
      {},
      { withCredentials: true }
    );
    console.log("header", res.avatar);

    if (res.avatar == "1") {
      this.imgSrc =
        "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1599575737996&di=22f8c24ab5079385e3ff45d8576179c0&imgtype=0&src=http%3A%2F%2Fb-ssl.duitang.com%2Fuploads%2Fitem%2F201809%2F15%2F20180915192557_JSFLK.jpeg";
    }
    if (res.avatar == "2") {
      this.imgSrc =
        "https://ss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=3076245205,742402026&fm=26&gp=0.jpg";
    }
    if (res.avatar == "3") {
      this.imgSrc =
        "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1599575929381&di=3bc01b19ce6059ac83dc1679c83c1829&imgtype=0&src=http%3A%2F%2Fb-ssl.duitang.com%2Fuploads%2Fitem%2F201811%2F05%2F20181105142239_uiyqs.jpg";
    }
    if (res.avatar == "4") {
      this.imgSrc =
        "https://ss0.bdstatic.com/70cFuHSh_Q1YnxGkpoWK1HF6hhy/it/u=1972236278,461088590&fm=26&gp=0.jpg";
    }
  }
};
</script>

<style scoped>
.el-menu-item {
  background-color: rgba(220, 38, 38, 0) !important;
}
.img {
  cursor: pointer;
}
</style>

<template>
  <div class="login_container">
    <el-carousel :height="bannerHeight + 'px'" class="login_img">
      <!-- 遍历图片地址,动态生成轮播图
      <el-carousel-item v-for="item in imgList" :key="item">
        <img :src="item" alt />
      </el-carousel-item>-->
    </el-carousel>
    <div class="login_box">
      <div class="avatar_box">
        <img src alt />
      </div>
      <el-form label-width="0px" class="login_form" :model="logInForm" :rules="loginFormRules">
        <!-- 用户名 -->
        <el-form-item prop="username">
          <el-input prefix-icon="el-icon-user" v-model="logInForm.username"></el-input>
        </el-form-item>
        <!-- 密码 -->
        <el-form-item prop="Password">
          <el-input prefix-icon="el-icon-unlock" v-model="logInForm.Password" type="password"></el-input>
        </el-form-item>
        <!-- 按钮 -->
        <el-form-item class="btns">
          <el-button type="primary" @click="login()">登录</el-button>
          <el-button type="info" @click="register()">注册</el-button>
        </el-form-item>
      </el-form>
    </div>
    <div class></div>
  </div>
</template>
<script>
import axios from "axios";

//这玩意用作密码的哈希，用之前先 npm install --save crypto-js
import sha512 from "crypto-js/sha512";
export default {
  name: "Banner",
  data() {
    return {
      //登录表单
      logInForm: {
        username: "student",
        Password: "123456"
      },
      loginFormRules:{
        username:[ { required: true, message: '请输入用户名', trigger: 'blur' },
         { min: 3, max: 10, message: '长度在 3 到 10 个字符', trigger: 'blur' }],
        Password:[{ required: true, message: '请输入密码', trigger: 'blur' }
        ,{ min: 3, max: 10, message: '长度在 3 到 10 个字符', trigger: 'blur' }]
      },

      // 图片地址数组
      imgList: [
        {
          url: require("@/assets/logo.png")
        },
        {
          url: require("@/assets/logo.png")
        },
        {
          url: require("@/assets/logo.png")
        }
      ],
      //图片父容器高度
      bannerHeight: 1000,
      //浏览器宽度
      screenWidth: 0
    };
  },
  methods: {
    async login() {
      console.log(this.logInForm);
      const result = await axios.post(
        "http://localhost:5000/Account/Login",
        {
          username: this.logInForm.username,
          // 密码哈希
          Password: sha512(this.logInForm.Password).toString()
        },
        { withCredentials: true }
      );
      console.log(result);
      if (result.status == 200) {
        this.$store.commit("logIn", result.data.role);
        this.$message.success("登录成功");
        this.$router.push("/Main");
      }
    },
    register(){
      this.$router.push("/Register");
    }
  }
};
</script>

<style lang="less" scoped>
.login_container {
  background-color: #0e758f;
  height: 800px;
  margin: 0px;
  background-size: cover;
}
.login_box {
  width: 450px;
  height: 300px;
  background-color: #fff;
  border-radius: 3px;
  position: absolute;
  left: 75%;
  top: 40%;
  transform: translate(-50%, -50%);

  .avatar_box {
    height: 130px;
    width: 130px;
    border: 1px solid #eee;
    border-radius: 50%;
    padding: 10px;
    box-shadow: 0 0 10px #ddd;
    position: absolute;
    left: 50%;
    transform: translate(-50%, -50%);
    background-color: #fff;
    img {
      width: 100%;
      height: 100%;
      border-radius: 50%;
      background-color: #eee;
    }
  }
  // .login_image{

  // }
  .btns {
    display: flex;
    justify-content: flex-end;
  }
  .login_form {
    position: absolute;
    bottom: 0;
    width: 100%;
    padding: 0 20px;
    box-sizing: border-box;
  }
}
</style>

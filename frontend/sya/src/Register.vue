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
        <el-input prefix-icon="el-icon-user" v-model="logInForm.username" placeholder="请输入用户名，长度为3-10字符"></el-input>
        </el-form-item>
        <!-- 密码 -->
        <el-form-item prop="Password">
          <el-input prefix-icon="el-icon-unlock" v-model="logInForm.Password" placeholder="请输入密码，长度为3-10字符"></el-input>
        </el-form-item>
        <!-- 邮箱 -->
        <el-form-item prop="email">
          <el-input prefix-icon="el-icon-user" v-model="logInForm.email" placeholder="请输入邮箱"></el-input>
        </el-form-item>
        <!-- 权限选择 -->
        <el-form-item prop="role">
          <el-input prefix-icon="el-icon-user" v-model="logInForm.role" placeholder="请输入权限等级"></el-input>
        </el-form-item>
        <!-- 按钮 -->
        <el-form-item class="btns">
          <el-button type="primary" @click="Register()">注册</el-button>
          <el-button type="info" @click="exit()">退出</el-button>
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
        username: "hzh",
        Password: "123456",
        email: "hzh@qq.com",
      },
      loginFormRules:{
        username:[ { required: true, message: '请输入用户名', trigger: 'blur' },
         { min: 3, max: 10, message: '长度在 3 到 10 个字符', trigger: 'blur' }],
        Password:[{ required: true, message: '请输入密码', trigger: 'blur' }
        ,{ min: 3, max: 10, message: '长度在 3 到 10 个字符', trigger: 'blur' }],
        email:[ { required: true, message: '请输入用户名', trigger: 'blur' },
         { min: 5, max: 20, message: '长度在 5 到 20 个字符', trigger: 'blur' }],
         role:[ { required: true, message: '请输入权限等级', trigger: 'blur' },
         { message: '为1或0', trigger: 'blur' }]
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
    async Register() {
      console.log(this.logInForm);
      const result = await axios.post(
        "http://localhost:5000/Account/Register",
        {
          username: this.logInForm.username,
          email: this.logInForm.email,
          // 密码哈希
          Password: sha512(this.logInForm.Password).toString(),
          role:parseInt(this.logInForm.role)
        },
        { withCredentials: true }
      );
      console.log(result);
      if (result.status == 200) {
        this.$message.success("注册成功");
        this.$router.push("/Main");
      }
      else{
        this.$message.success("该账户已经被占用");
      }
    },
    exit(){
      this.$router.push("/");
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
  height: 430px;
  background-color: #fff;
  border-radius: 3px;
  position: absolute;
  left: 75%;
  top: 50%;
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
    bottom: 20px;
    width: 100%;
    padding: 0 20px;
    box-sizing: border-box;
  }
}
</style>

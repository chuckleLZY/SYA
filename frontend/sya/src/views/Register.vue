、
<template>
  <div class="login_container">
<el-carousel :interval="4000" type="card" height="200px">
      <el-carousel-item v-for="item in imagebox" :key="item.id">
        <img :src="item.idView" class="image" />
      </el-carousel-item>
    </el-carousel>
  <el-divider><tr style="font-size : 30px;">
    <td class="color-dark-light">注册</td>
    </tr></el-divider>
  <el-divider direction="vertical"  ></el-divider>
<br><el-divider direction="vertical"  ></el-divider>
<br><el-divider direction="vertical"  ></el-divider><br><el-divider direction="vertical"  ></el-divider><br><el-divider direction="vertical"  ></el-divider>
<br><el-divider direction="vertical"  ></el-divider><br><el-divider direction="vertical"  ></el-divider><br><el-divider direction="vertical"  ></el-divider>
<br><el-divider direction="vertical"  ></el-divider><br><el-divider direction="vertical"  ></el-divider><br><el-divider direction="vertical"  ></el-divider>
<br><el-divider direction="vertical"  ></el-divider><br><el-divider direction="vertical"  ></el-divider><br><el-divider direction="vertical"  ></el-divider>
<br><el-divider direction="vertical"  ></el-divider><br><el-divider direction="vertical"  ></el-divider><br><el-divider direction="vertical"  ></el-divider>
<br><el-divider direction="vertical"  ></el-divider><br><el-divider direction="vertical"  ></el-divider><br><el-divider direction="vertical"  ></el-divider>
<br><el-divider direction="vertical"  ></el-divider><br><el-divider direction="vertical"  ></el-divider><br><el-divider direction="vertical"  ></el-divider>
<br><el-divider direction="vertical"  ></el-divider><br><el-divider direction="vertical"  ></el-divider><br><el-divider direction="vertical"  ></el-divider><br>
<el-divider direction="vertical"  ></el-divider><br>
<span style="font-size: 12px">注册代表您同意我们的</span><el-link style="font-size:12px" type="primary">隐私政策</el-link><span style="font-size: 12px">和</span><el-link style="font-size:12px" type="primary">用户协议</el-link>
  <div class="verticalBar">
  </div>
    <div class="erweima_box">
      <b>满意的话，请给我们项目一点支持</b>
      <br>
      <img src="../assets/imagebox/login_mid2.png" alt="" class="mid">
      <img src="../assets/imagebox/login_left2.png" alt="" class="left">
      <img src="../assets/imagebox/login_right2.png" alt="" class="right">
    </div>
     <div class="login_box">
      <!-- <div class="avatar_box">
        <img src="https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1599575737996&di=22f8c24ab5079385e3ff45d8576179c0&imgtype=0&src=http%3A%2F%2Fb-ssl.duitang.com%2Fuploads%2Fitem%2F201809%2F15%2F20180915192557_JSFLK.jpeg" alt="">
      </div> -->
      <el-form ref="loginFormRef" label-width="0px" class="login_form" :model="logInForm" :rules="loginFormRules">
        <!-- 用户名 -->
        <el-form-item prop="username">
        <el-input prefix-icon="el-icon-user" v-model="logInForm.username" placeholder="请输入用户名，长度为3-10字符"></el-input>
        </el-form-item>
        <!-- 密码 -->
        <el-form-item prop="Password">
          <el-input prefix-icon="el-icon-unlock" v-model="logInForm.Password" placeholder="请输入密码，长度为3-10字符" type="password"></el-input>
        </el-form-item>
        <!-- 邮箱 -->
        <el-form-item prop="email">
          <el-input prefix-icon="el-icon-user" v-model="logInForm.email" placeholder="请输入正确的邮箱格式"></el-input>
        </el-form-item>
        <!-- 权限选择 -->
        <el-form-item prop="role">
        <el-select v-model="logInForm.role" placeholder="请选择你的注册身份" style="width:100%" >
             <el-option
                v-for="item in options"
                :key="item.value"
                :label="item.label"
                :value="item.value">
             </el-option>
        </el-select>
        </el-form-item>
        <!-- 按钮 -->
        <el-form-item class="btns" >
          <el-button type="primary" @click="Register()">注册</el-button>
          <el-button type="info" @click="exit()">退出</el-button>
          <el-button  @click="resetLoginForm">重置</el-button>
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
      //**
        options: [{
          value: 1,
          label: '学生'
        }, {
          value: 2,
          label: '老师'
        }, ],
        value: '',
      //登录表单
      logInForm: {
        username: "",
        Password: "",
        email: "",
      },
      loginFormRules:{
        username:[ { required: true, message: '请输入用户名', trigger: 'blur' },
         { min: 3, max: 10, message: '长度在 3 到 10 个字符', trigger: 'blur' }],
        Password:[{ required: true, message: '请输入密码', trigger: 'blur' }
        ,{ min: 3, max: 10, message: '长度在 3 到 10 个字符', trigger: 'blur' }],
        email:[ { required: true, message: '请输入邮箱', trigger: 'blur' },
         { min: 5, max: 20, message: '长度在 5 到 20 个字符', trigger: 'blur' }],
         role:[ { required: true, message: '请输入权限等级', trigger: 'blur' },
         { message: '为1或0', trigger: 'blur' }]
      },
      imagebox: [
         { id: 0, idView: require("../assets/imagebox/login1.png") },
        { id: 1, idView: require("../assets/imagebox/login2.png") },
        { id: 2, idView: require("../assets/imagebox/login3.png") },
        { id: 3, idView: require("../assets/imagebox/login4.png") },
        //imagebox是assets下一个放图片的文件夹
      ],
      screenWidth: 0,
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
     resetLoginForm () {
      // console.log(this)
      // resetFields：element-ui提供的表单方法
      this.$refs.loginFormRef.resetFields()
    },
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
        this.$message({
          message: '注册成功',
          type: 'success',
          duration:500
        });
        this.$router.push("/");
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
.login_container.line{
float: left;
border-right: 1px solid #ddd;
height: 300px;
margin-top: 28px;
}

.login_container {
  background-color: #fff;
  //background-image:url("../assets/imagebox/1.jpeg");
  position:absolute;
  background-size: cover;
   width:100%; height:100%;
}
.erweima_box{ 
  width: 650px;
  height: 400px;
  position: absolute;
  left: 25%;
  top: 60%;
  transform: translate(-50%, -50%);
 .mid{
   height: 200px;
    width: 200px;
    transform: translate(70%, 10%);
    img {
      width: 100%;
      height: 100%;
      border-radius: 50%;
      background-color: #eee;
    }
 }
 .left{
   height: 140px;
    width: 140px;
    transform: translate(-150%, 80%);
    img {
      width: 100%;
      height: 100%;
      border-radius: 50%;
      background-color: #eee;
    }
 }
  .right{
    height: 140px;
    width: 140px;
    transform: translate(5%, 80%);
    img {
      width: 100%;
      height: 100%;
      border-radius: 50%;
      background-color: #eee;
    }
 }
}
.el-carousel__item h3 {
  color: #475669;
  font-size: 14px;
  opacity: 0.75;
  line-height: 200px;
  margin: 0;
}

.el-carousel__item:nth-child(2n) {
  background-color: #99a9bf;
}

.el-carousel__item:nth-child(2n + 1) {
  background-color: #d3dce6;
}
img {
  /*设置图片宽度和浏览器宽度一致*/
  width: 100%;
  height: inherit;
}

.login_box {
  width: 450px;
  height: 430px;
  background-image:url("../assets/imagebox/login5.png");
  border-radius: 3px;
  position: absolute;
  left: 75%;
  top: 70%;
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

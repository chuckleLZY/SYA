<template>
  <div class="login_container">
    <el-carousel :interval="4000" type="card" height="200px">
      <el-carousel-item v-for="item in imagebox" :key="item.id">
        <img :src="item.idView" class="image" />
      </el-carousel-item>
    </el-carousel>
    <el-divider>
      <tr style="font-size : 30px;">
        <td class="color-dark-light">登录</td>
      </tr>
    </el-divider>
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <el-divider direction="vertical"></el-divider>
    <br />
    <span style="font-size: 12px">登录代表您同意我们的</span>
    <el-link style="font-size:12px" type="primary">隐私政策</el-link>
    <span style="font-size: 12px">和</span>
    <el-link style="font-size:12px" type="primary">用户协议</el-link>
    <div class="verticalBar"></div>
    <div class="erweima_box">
      <b>满意的话，请给我们项目一点支持</b>
      <br />
      <img src="../assets/imagebox/login_mid2.png" alt class="mid" />
      <img src="../assets/imagebox/login_left2.png" alt class="left" />
      <img src="../assets/imagebox/login_right2.png" alt class="right" />
    </div>
    <div class="login_box">
      <!-- <div class="avatar_box">
        <img src="https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1599575737996&di=22f8c24ab5079385e3ff45d8576179c0&imgtype=0&src=http%3A%2F%2Fb-ssl.duitang.com%2Fuploads%2Fitem%2F201809%2F15%2F20180915192557_JSFLK.jpeg" alt />
      </div>-->
      <el-form ref="loginFormRef" label-width="0px" class="login_form" :model="logInForm" :rules="loginFormRules">
        <!-- 用户名 -->
        <el-form-item prop="username">
          <el-input prefix-icon="el-icon-user" v-model="logInForm.username" placeholder="请输入用户名"></el-input>
        </el-form-item>
        <!-- 密码 -->
        <el-form-item prop="Password">
          <el-input prefix-icon="el-icon-unlock" v-model="logInForm.Password" type="password" placeholder="请输入密码"></el-input>
        </el-form-item>
        <!-- 按钮 -->
        <el-form-item class="btns">
          <el-button type="primary" @click="login()">登录</el-button>
          <el-button type="info" @click="$router.push('/Register')">注册</el-button>
          <el-button @click="resetLoginForm">重置</el-button>
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
        username: "",
        Password: "",
      },
      loginFormRules: {
        username: [
          { required: true, message: "请输入用户名", trigger: "blur" },
          { min: 3, max: 10, message: "长度在 3 到 10 个字符", trigger: "blur" },
        ],
        Password: [
          { required: true, message: "请输入密码", trigger: "blur" },
          { min: 3, max: 10, message: "长度在 3 到 10 个字符", trigger: "blur" },
        ],
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
          url: require("@/assets/logo.png"),
        },
        {
          url: require("@/assets/logo.png"),
        },
        {
          url: require("@/assets/logo.png"),
        },
      ],
      //图片父容器高度
      bannerHeight: 1000,
      //浏览器宽度
      screenWidth: 0,
    };
  },
  methods: {
    resetLoginForm() {
      // console.log(this)
      // resetFields：element-ui提供的表单方法
      this.$refs.loginFormRef.resetFields();
    },
    async login() {
      console.log(this.logInForm);
      try {
        const result = await axios.post(
          this.$helper.endpointUrl("/Account/Login"),
          {
            username: this.logInForm.username,
            // 密码哈希
            Password: sha512(this.logInForm.Password).toString(),
          },
          { withCredentials: true }
        );
        console.log(result);
        if (result.status == 200) {
          //将登录信息保存到vuex
          this.$store.commit("logIn", result.data.role);
          // //将vuex里的信息保存到sessionStorage里
          // sessionStorage.setItem("store", JSON.stringify(this.$store.state));
          this.$message({
            message: "登录成功",
            type: "success",
            duration: 500,
          });
          this.$router.push("/Main");
        }
      } catch (err) {
        this.$message({
          message: "登录失败",
          type: "error",
          duration: 500,
        });
      }
    }
  },
};
</script>

<style lang="less" scoped>
.login_container.line {
  float: left;
  border-right: 1px solid #ddd;
  height: 300px;
  margin-top: 28px;
}

.login_container {
  background-color: #fff;
  //background-image:url("../assets/imagebox/1.jpeg");
  position: absolute;
  background-size: cover;
  width: 100%;
  height: 100%;
}
.erweima_box {
  width: 650px;
  height: 400px;
  position: absolute;
  left: 25%;
  top: 60%;
  transform: translate(-50%, -50%);
  .mid {
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
  .left {
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
  .right {
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
  height: 300px;
  background-image: url("../assets/imagebox/login5.png");
  border-radius: 3px;
  position: absolute;
  left: 75%;
  top: 60%;
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
  .line {
    display: inline-block;
    width: 1px;
    height: 55em;
    margin: 0 8px;
    vertical-align: middle;
    position: relative;
  }
}
</style>

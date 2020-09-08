<template>
  <div>
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{ path: '/home' }">首页</el-breadcrumb-item>
      <el-breadcrumb-item>老师我的</el-breadcrumb-item>
      <el-breadcrumb-item>个人信息</el-breadcrumb-item>
    </el-breadcrumb>

    <!--个人资料卡片-->
    <el-card class="infoCard">
      <el-row>
        <el-col :span="5">
          <p class="title">个人信息</p>
        </el-col>
      </el-row>
      <!--头像区域-->
      <div class="box" id="box1"></div>
      <div class="box" id="box2"></div>
      <el-row class="avaRow">
        <el-avatar shape="circle" :size="100" @error="errorHandler">
          <img id="userAva" @click="drawer = true" :src="userava" />
        </el-avatar>
        <div class="box" id="box3"></div>
        <div class="box" id="box4"></div>
      </el-row>
      <!--用户名区域-->
      <el-row class="name_Row">
        <p class="stuID">{{UserInfo.user_name}}</p>
      </el-row>
      <!--信息标题-->
      <el-row class="section">
        <p class="subTitle">基本信息</p>
        <hr class="sectionHr" align="center" />
        <!--表单-->
        <el-form :model="UserInfo" label-width="0px" class="infoForm">
          <el-form-item class="itemRow">
            <div class="itemNameContainer">
              <p class="itemName">
                <i class="el-icon-key"></i> SYA-ID ：
              </p>
            </div>
            <el-input class="infoItem" readonly="readonly"  v-model="UserInfo.user_id" disabled></el-input>
          </el-form-item>
          <el-form-item class="itemRow">
            <div class="itemNameContainer">
              <p class="itemName">
                <i class="el-icon-user"></i> 用户名 ：
              </p>
            </div>
            <el-input class="infoItem" readonly="readonly" v-model="UserInfo.user_name" disabled></el-input>
          </el-form-item>
          <el-form-item class="itemRow" prop="gender">
            <div class="itemNameContainer">
              <p class="itemName">
                <i class="el-icon-user"></i> 性别 ：
              </p>
            </div>
            <el-radio-group class="infoItem2" v-model="radio">
              <el-radio label="0">男</el-radio>
              <el-radio label="1">女</el-radio>
            </el-radio-group>
          </el-form-item>
        </el-form>
      </el-row>
      <!--信息-->
      <el-row class="section">
        <p class="subTitle">账号绑定</p>
        <hr class="sectionHr" align="center" />
        <!--表单-->
        <el-form :model="UserInfo" label-width="0px" class="infoForm">
          <el-form-item class="itemRow">
            <div class="itemNameContainer">
              <p class="itemName">
                <i class="el-icon-phone-outline"></i> 绑定手机 ：
              </p>
            </div>
            <el-input id="phone" class="infoItem" :placeholder="UserInfo.tel" v-model="phone"></el-input>
          </el-form-item>
          <el-form-item class="itemRow">
            <div class="itemNameContainer">
              <p class="itemName">
                <i class="el-icon-message"></i> 邮箱 :
              </p>
            </div>
            <el-input class="infoItem" readonly="readonly" v-model="UserInfo.email" disabled></el-input>
          </el-form-item>
          <el-form-item class="itemRow">
            <div class="itemNameContainer">
              <p class="itemName">
                <i class="el-icon-bank-card"></i> 银行卡号 :
              </p>
            </div>
            <el-input class="infoItem" :placeholder="UserInfo.bank" v-model="bank"></el-input>
          </el-form-item>
        </el-form>
      </el-row>
      <!-- 右侧弹窗 -->

      
      <el-drawer title="我是标题" :visible.sync="drawer" :with-header="false" :before-close="handleClose">
        <p>选择你的新头像</p>
        <el-card class="box-card">
          <div>
            <img class="image" src="https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1599575737996&di=22f8c24ab5079385e3ff45d8576179c0&imgtype=0&src=http%3A%2F%2Fb-ssl.duitang.com%2Fuploads%2Fitem%2F201809%2F15%2F20180915192557_JSFLK.jpeg"/>
          </div>
          <el-radio class="radio" v-model="radio2" label="1">简约男生</el-radio>
          </el-card>
          <el-card class="box-card">
          <div>
            <img class="image" src="https://ss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=3076245205,742402026&fm=26&gp=0.jpg"/>
          </div>
          <el-radio class="radio" v-model="radio2" label="2">简约女生</el-radio>
          </el-card>
          <el-card class="box-card">
          <div>
            <img class="image" src="https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1599575929381&di=3bc01b19ce6059ac83dc1679c83c1829&imgtype=0&src=http%3A%2F%2Fb-ssl.duitang.com%2Fuploads%2Fitem%2F201811%2F05%2F20181105142239_uiyqs.jpg"/>
          </div>
          <el-radio class="radio" v-model="radio2" label="3">文艺男生</el-radio>
          </el-card>
          
          <el-card class="box-card">
          <div>
            <img class="image" src="https://ss0.bdstatic.com/70cFuHSh_Q1YnxGkpoWK1HF6hhy/it/u=1972236278,461088590&fm=26&gp=0.jpg"/>
          </div>
          <el-radio class="radio" v-model="radio2" label="4">文艺女生</el-radio>
        </el-card>
        
        <div style="margin-bottom: 20px">
        <p>
          <el-button type="primary" icon="el-icon-edit" @click="UpdateUser()">点击修改</el-button>
        </p>
        </div>
      </el-drawer>

      <div style="text-align:center">
        <p class="syaSup">
          <el-button type="primary" icon="el-icon-edit" @click="UpdateUser()">点击修改</el-button>
        </p>
        <p class="syaSup">SYA——Show Your Ability</p>
      </div>
      <div class="topMask square"></div>
      <div class="topMask circular"></div>
      <div class="topMask square2"></div>
      <div class="topMask square3"></div>
    </el-card>
  </div>
</template>

<script>
import axios from "axios";
export default {
  data() {
    return {
      UserInfo: [],
      phone: 0,
      bank: 0,
      radio: "",
      radio2:"",
      regender: true,
      userava: "",
      //newuserava: "",
      drawer: false,
      // userAva:{
      //   src:""
      //   }
    };
  },
  created() {
    this.getUserInfo();
    this.UpdateUser();
  },
  mounted() {
    //this.init();
    this.getUserInfo();
  },
  methods: {
    handleClose(done){
    this.$router.go(0);
    },
    async getUserInfo() {
      const { data: res } = await axios.post(
        "http://localhost:5000/User/GetUserInfo",
        {},
        { withCredentials: true }
      );

      this.UserInfo = res;
      this.radio2=this.UserInfo.avatar;
      console.log(this.radio2);
      if (this.UserInfo.gender == true) {
        this.radio = "0";
      } else if (this.UserInfo.gender == false) {
        this.radio = "1";
      }
      if (this.UserInfo.avatar == "1") {
        this.userava =
          "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1599575737996&di=22f8c24ab5079385e3ff45d8576179c0&imgtype=0&src=http%3A%2F%2Fb-ssl.duitang.com%2Fuploads%2Fitem%2F201809%2F15%2F20180915192557_JSFLK.jpeg";
      }
      if (this.UserInfo.avatar == "2") {
        this.userava =
          "https://ss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=3076245205,742402026&fm=26&gp=0.jpg";
      }
      if (this.UserInfo.avatar == "3") {
        this.userava =
          "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1599575929381&di=3bc01b19ce6059ac83dc1679c83c1829&imgtype=0&src=http%3A%2F%2Fb-ssl.duitang.com%2Fuploads%2Fitem%2F201811%2F05%2F20181105142239_uiyqs.jpg";
      }
      if (this.UserInfo.avatar == "4") {
        this.userava =
          "https://ss0.bdstatic.com/70cFuHSh_Q1YnxGkpoWK1HF6hhy/it/u=1972236278,461088590&fm=26&gp=0.jpg";
      }

      //console.log(this.newuserava);
    },
    // async init(){
    //      var i=await this.getUserInfo();
    //      console.log(this.UserInfo.avatar);
    //      //userAva.src=this.UserInfo.avatar;
    // },
    async UpdateUser() {
      const { data: ccc } = await axios.post(
        "http://localhost:5000/User/GetUserInfo",
        {},
        { withCredentials: true }
      );
      this.UserInfo = ccc;
      //console.log(this.UserInfo);
      //console.log(this.phone);
      if (this.phone == 0) {
        this.phone = this.UserInfo.tel;
      }
      if (this.bank == 0) {
        this.bank = this.UserInfo.bank;
      }
      if (this.radio == "0") {
        this.regender = true;
      }
      if (this.radio == "1") {
        this.regender = false;
      }
      
      const { data: res } = await axios.post(
        "http://localhost:5000/User/UpdateUser",
        {
          gender: this.regender,
          avatar: this.radio2,
          tel: this.phone,
          bank: this.bank,
        },
        { withCredentials: true }
      );
      //console.log(a);
      //console.log(this.UserInfo2);
      //this.UserInfo=res;
      //console.log(res);
    },
    errorHandler() {
      return true;
    },
  },
};
</script>

<style>
@import url("https://fonts.googleapis.com/css?family=Montserrat");

* {
  font-family: "Montserrat", sans-serif, "微软雅黑";
}

.syaSup {
  font-size: 20px;
  margin-top: 2cm;
}
</style>

<style scoped>
el-card {
  margin-top: 1cm;
}

.infoCard {
  margin-top: 1cm;
  height: 1500px;
}

.title {
  font-size: 25px;
  font-weight: bold;
}
.avaRow {
  margin-top: 1cm;
}
.stuID {
  font-size: 20px;
  font-weight: lighter;
}
.subTitle {
  font-size: 22px;
}
.sectionHr {
  margin-top: 0.5cm;
  margin-left: auto;
  margin-right: auto;
  width: 50%;
  height: 4px;
  background-color: gainsboro;
  border: none;
  border-radius: 2px;
}
.infoForm {
  margin-top: 1cm;
}

.itemRow {
  overflow: hidden;
}
.infoItem {
  width: 30%;
  float: left;
}
.infoItem2 {
  margin-top: 20px;
  width: 20%;
  float: left;
}
.itemName {
  font-weight: bold;
  line-height: 40px;
  margin-left: 70%;
  margin-top: 0;
  margin-bottom: 0;
}
.itemNameContainer {
  float: left;
  width: 40%;
  text-align: left;
  height: 40px;
}
.section {
  margin-top: 2cm;
}
.topMask {
  background-color: rgba(68, 138, 255, 0.2);
  position: relative;
}
.square {
  transform: rotate(45deg);
  width: 200px;
  height: 200px;
  border-radius: 5px;
  top: -43cm;
  left: 20%;
}
.circular {
  width: 150px;
  height: 150px;
  border-radius: 50%;
  top: -44cm;
  left: 76%;
  background-color: rgba(255, 208, 75, 0.2);
}
.square2 {
  transform: rotate(30deg);
  width: 200px;
  height: 200px;
  border-radius: 5px;
  top: -42cm;
  left: 0;
  background-color: rgb(0, 255, 208, 0.2);
}
.square3 {
  transform: rotate(45deg);
  width: 200px;
  height: 200px;
  border-radius: 5px;
  top: -47cm;
  left: 90%;
  background-color: rgb(255, 0, 208, 0.1);
}
  .text {
    font-size: 14px;
  }

  .item {
    padding: 18px 0;
  }

  .box-card {
    width: 50%;
    float: left;
    height: 40%;
  }
  .image {
    width: 100%;
    display: block;
  }
  .radio{
    margin-top: 30px;
  }
  /* .syaSup2{
    margin-top: 20px;
  } */
</style>

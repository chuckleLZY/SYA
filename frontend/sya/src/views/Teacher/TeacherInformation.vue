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
        <el-avatar shape="circle" :size="100" :src="UserInfo.avatar" @error="errorHandler">
          <img id="userAva" :src="UserInfo.avatar" />
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
            <el-input class="infoItem" readonly="readonly" v-model="UserInfo.user_id"></el-input>
          </el-form-item>
          <el-form-item class="itemRow">
            <div class="itemNameContainer">
              <p class="itemName">
                <i class="el-icon-user"></i> 用户名 ：
              </p>
            </div>
            <el-input class="infoItem" readonly="readonly" v-model="UserInfo.user_name"></el-input>
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
            <el-input class="infoItem" readonly="readonly" v-model="UserInfo.email"></el-input>
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
      radio:"",
      regender:true,
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
    async getUserInfo() {
      const { data: res } = await axios.post(
        "http://localhost:5000/User/GetUserInfo",
        {},
        { withCredentials: true }
      );

      this.UserInfo = res;
      console.log(this.UserInfo.gender);
      if(this.UserInfo.gender==true)
      {
        this.radio="0";
      }
      else if(this.UserInfo.gender==false)
      {
        this.radio="1";
      }

      console.log(this.radio);
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
        this.regender=true;
      }
      if (this.radio == "1") {
        this.regender=false;
      }
      const { data: res } = await axios.post(
        "http://localhost:5000/User/UpdateUser",
        {
          gender: this.regender,
          avatar: "a.png",
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
</style>

<template>
  <div> 
    <el-breadcrumb separator-class="el-icon-arrow-right">
        <el-breadcrumb-item :to="{ path: '/home' }">首页</el-breadcrumb-item>
        <el-breadcrumb-item>老师我的</el-breadcrumb-item>
        <el-breadcrumb-item>资格验证</el-breadcrumb-item>
    </el-breadcrumb>
    
    <!--个人资料卡片-->
    <el-card class="infoCard">
        <el-row>
            <el-col :span="5">
                <p class="title">资格验证</p>
            </el-col>
        </el-row>
        <!--头像区域-->
        <div class='box' id='box1'></div>
        <div class='box' id='box2'> </div>
            <el-row class="avaRow">
                <el-avatar shape="circle" :size="100" src="https://empty" @error="errorHandler">
                        <img id="userAva" src="aaa" />
                </el-avatar>
        <div class='box' id='box3'></div>
        <div class='box' id='box4'></div>
            </el-row>
        <!--用户名区域-->
        <el-row class="name_Row">
            <p class="stuID">{{UserInfo.user_name}}</p>
        </el-row>
        <!--信息标题-->
        <el-row class="section">
                <p class="subTitle">基本信息</p>
                <hr class="sectionHr" align="center">
        <!--表单-->
            <el-form :model="UserInfo" label-width="0px" class="infoForm">
                <el-form-item class="itemRow">
                    <div class="itemNameContainer">
                        <p class="itemName"><i class="el-icon-key"></i> SYA-ID ：</p>
                    </div>
                    <el-input class="infoItem" readonly="true" v-model="UserInfo.user_id" ></el-input>
                </el-form-item>
                <el-form-item class="itemRow">
                    <div class="itemNameContainer">
                        <p class="itemName"><i class="el-icon-user"></i> 用户名 ：</p>
                    </div>
                    <el-input class="infoItem" readonly="true" v-model="UserInfo.user_name" ></el-input>
                </el-form-item>
            </el-form>
         </el-row>
         <!--信息-->
        <el-row class="section">
                <p class="subTitle">资格验证</p>
                <hr class="sectionHr" align="center">
        <!--表单-->
            <el-form :model="UserInfo" label-width="0px" class="infoForm">
                <el-form-item class="itemRow">
                    <div class="itemNameContainer">
                        <p class="itemName"><i class="el-icon-phone-outline"></i> 工号 ：</p>
                    </div>
                    <el-input class="infoItem" readonly="true" placeholder="请输入工号"></el-input>
                </el-form-item>
            </el-form>
        </el-row>
        <!--信息标题-->

        <div style="text-align:center">
            <p class="syaSup"><el-button type="primary" icon="el-icon-edit" @click="">点击验证</el-button></p>
            <p class="syaSup">SYA——Show Your Ability</p>
        </div>
        <div class='topMask square'></div>
        <div class='topMask circular'></div>
        <div class='topMask square2'></div>
        <div class='topMask square3'></div>
    </el-card>
  </div>
</template>

<script>


import axios from 'axios';
export default {
    data(){
        return{
            UserInfo:[]
        }
    },
    created(){
        this.getUserInfo();
    },
    mounted(){
        this.init();
    },
    methods:{
        async getUserInfo(){
            const {data: res} = await axios.post('http://localhost:5000/User/GetUserInfo',{},{ withCredentials: true });

            this.UserInfo=res;
            console.log(res);
        },
        async init(){
             var i=await this.getUserInfo();
             console.log(this.UserInfo.avatar);
             userAva.src=this.UserInfo.avatar;
        },
        errorHandler(){
            return true;
        }
    }
}
</script>


<style scoped>
.syaSup{
    font-size:20px;
    margin-top: 2cm;
}
el-card{
    margin-top:1cm;
}

.infoCard{
    margin-top:1cm;
    height: 1500px;
}

.title{
    font-size: 25px;
    font-weight: bold;
}
.avaRow{
    margin-top: 1cm;
}
.stuID{
    font-size:20px;
    font-weight: lighter;
}
.subTitle{
    font-size:22px;
}
.sectionHr{
    margin-top: 0.5cm;
    margin-left:auto;
    margin-right: auto;
    width: 50%;
    height:4px;
    background-color:gainsboro;
    border: none;
    border-radius: 2px;
}
.infoForm{
    margin-top:1cm;
}

.itemRow{
    overflow: hidden;
}
.infoItem{
    width: 30%;
    float: left;
}
.itemName{
    font-weight:bold;
    line-height: 40px;
    margin-left:70%;
    margin-top:0;
    margin-bottom: 0;
}
.itemNameContainer{
    float: left;
    width: 40%;
    text-align: left;
    height:40px;
}
.section{
    margin-top:2cm;
}
.topMask{
 background-color: rgba(68,138,255,0.2);
 position: relative;
}
.square{
    transform: rotate(45deg);
    width: 200px;
    height: 200px;
    border-radius: 5px;
    top: -43cm;
    left:20%;
}
.circular{
 width: 150px;
 height: 150px;
 border-radius: 50%;
 top:-44cm;
 left: 76%;
 background-color: rgba(255, 208, 75,0.2);
 }
 .square2{
     transform: rotate(30deg);
     width: 200px;
    height: 200px;
    border-radius: 5px;
    top: -42cm;
    left:0;
    background-color: rgb(0, 255, 208,0.2);
 }
 .square3{
     transform: rotate(45deg);
     width: 200px;
    height: 200px;
    border-radius: 5px;
    top: -47cm;
    left:90%;
    background-color: rgb(255, 0, 208,0.1);
 }
</style>

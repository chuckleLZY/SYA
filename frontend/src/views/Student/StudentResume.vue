<template>
    <div>
        <el-breadcrumb separator-class="el-icon-arrow-right">
            <el-breadcrumb-item :to="{ path: '/home' }">首页</el-breadcrumb-item>
            <el-breadcrumb-item>学生我的</el-breadcrumb-item>
            <el-breadcrumb-item>在线简历</el-breadcrumb-item>
        </el-breadcrumb>
        <!--简历卡片-->
        <el-card class="resumeCard">
            <el-row class="header">
                 <el-col class="titlecol" :span="5">
                    <p class="title">个人简历</p>
                </el-col>
                <el-col :span="2" :offset="14">
                    <el-button class="edit" type="primary" icon="el-icon-edit">编辑简历信息</el-button>
                </el-col>
            </el-row>
            <el-row >
                 <el-col :span="24">
                    <p class="name"><i class="el-icon-user-solid"></i> {{Resume.student_name}} </p>
                </el-col>
            </el-row>
            <el-row class="section">
                <p class="sectionTitle">简历信息</p>
                <hr class="sectionHr" align="center">
            </el-row>
            <el-collapse v-model="activeNames" class="Aresume">
                <el-collapse-item title="  教育经历" name="1">
                    <div class="resumeItem">{{Resume.education}}</div>
                </el-collapse-item>
                <el-collapse-item title="主要技能" name="2">
                     <div>{{Resume.skill}}</div>
                </el-collapse-item>
                <el-collapse-item title="学术成就" name="3">
                     <div>{{Resume.academic}}</div>
                </el-collapse-item>
                <el-collapse-item title="项目经历" name="4">
                    <div>{{Resume.project}}</div>
                </el-collapse-item>
                 <el-collapse-item title="社团/活动经历" name="5">
                    <div>{{Resume.community}}</div>
                </el-collapse-item>
                <el-collapse-item title="个人简介" name="5">
                    <div>{{Resume.introduction}}</div>
                </el-collapse-item>
            </el-collapse>
            <el-row class="section">
                <p class="sectionTitle">联系方式</p>
                <hr class="sectionHr" align="center">
            </el-row>
            <el-row >
                 <el-col :span="24">
                    <p class="tips" ><i class="el-icon-info"></i> 以下信息与您的个人信息绑定，如需修改请修改个人信息</p>
                </el-col>
            </el-row>
            <div>
                <div class="contactContainer">
                    <div>
                        <h3 class="contactInfoTitle"><i class="el-icon-phone-outline"></i> 电话 : <span>{{UserInfo.tel}}</span></h3>
                    </div>
                </div>
                <div class="contactContainer">
                    <div>
                        <h3 class="contactInfoTitle"><i class="el-icon-message"></i> 邮箱 : <span>{{UserInfo.email}}</span></h3>
                    </div>
                </div>
            </div>
            <p class="syaSup">SYA——Show Your Ability</p>
        </el-card>

        <el-card class="editCard">
            <el-row class="header">
                 <el-col class="titlecol" :span="5">
                    <p class="title">在线简历编辑</p>
                </el-col>
                <el-col :span="2" :offset="14">
                    <el-button class="edit"  icon="el-icon-edit" @click="save">提交</el-button>
                </el-col>
            </el-row>
           <el-form class="editForm" :model="Resume" label-width="120px">
                <el-form-item label="教育经历:">
                    <el-input v-model="Resume.education" type="textarea" :rows="5" maxlength="300" show-word-limit ></el-input>
                </el-form-item>
                <el-form-item label="主要技能:">
                    <el-input v-model="Resume.skill" type="textarea" :rows="5" maxlength="300" show-word-limit ></el-input>
                </el-form-item>
                <el-form-item label="学术成就:">
                    <el-input v-model="Resume.academic" type="textarea" :rows="5" maxlength="300" show-word-limit ></el-input>
                </el-form-item>
                 <el-form-item label="项目经历:">
                    <el-input v-model="Resume.project" type="textarea" :rows="5" maxlength="300" show-word-limit ></el-input>
                </el-form-item>
                 <el-form-item label="社团/活动经历:">
                    <el-input v-model="Resume.community" type="textarea" :rows="5" maxlength="300" show-word-limit ></el-input>
                </el-form-item>
                <el-form-item label="个人简介:">
                    <el-input v-model="Resume.introduction" type="textarea" :rows="6" maxlength="400" show-word-limit ></el-input>
                </el-form-item>
           </el-form>
        </el-card>
    </div>
</template>


<script>

var backup;

import axios from 'axios';
export default {
    data(){
        return{
            Resume:[],
            UserInfo:[],
            backup:[]
        }
    },
     created(){
        this.getResume();
        this.getUserInfo();
    },
     methods:{
          async getUserInfo(){
            const {data: res} = await axios.post('http://localhost:5000/User/GetUserInfo',{},{ withCredentials: true });

            this.UserInfo=res;
            console.log(res);
        },
        async getResume(){
            const {data: res} = await axios.post('http://localhost:5000/Resume/GetResume',{},{ withCredentials: true });

            this.Resume=res;
            console.log(res);
        },
        async save(){
            const {data: res} = await axios.put('http://localhost:5000/Resume/UpdateResume',this.Resume,{ withCredentials: true });
            console.log(res);
            this.$message.success('保存成功！');
        }
     }
}
</script>


<style scoped>
.resumeCard{
    padding:0;
}
.resumeCard{
    margin-top:1cm;
}
.title{
    font-size: 25px;
    font-weight: bold;
}
.subtitle{
    font-weight:normal;
    font-size:20px;
}
.titlecol{
    margin-bottom:1cm;
}
.name{
    font-family:"Montserrat",sans-serif,"楷体" ;
    font-weight:bold;
    font-size: 30px;
}
.sectionTitle{
    font-size: 20px;
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
.Aresume{
    width:40%;
    margin-left:30%;
    margin-top:1cm;
}
.section{
    margin-top:2cm;
}
.tips{
    font-size:13px;
    color: gray;
}
.contactInfoTitle{
    font-size:15px;
    font-weight: normal;
}
.syaSup{
    margin-top:4cm;
}
.edit{
    margin-top:20%;
}
.editCard{
    padding:0;
    margin-top:1cm;
}
.editForm{
    width:50%;
    margin-left:25%;
}
</style>

<template>
    <div>
        <el-breadcrumb separator-class="el-icon-arrow-right">
            <el-breadcrumb-item :to="{ path: '/home' }">学生</el-breadcrumb-item>
            <el-breadcrumb-item>我的</el-breadcrumb-item>
            <el-breadcrumb-item>个人简历</el-breadcrumb-item>
        </el-breadcrumb>
        <!--简历卡片-->
        <el-card class="resumeCard" v-if="editResume==false && resumeExsit==true" v-loading="loading">
            <el-row class="header">
                 <el-col class="titlecol" :span="5">
                    <p class="title">个人简历预览</p>
                </el-col>
                <el-col :span="2" :offset="14">
                    <el-button class="edit" type="success" icon="el-icon-edit" @click="edit()">编辑简历信息</el-button>
                </el-col>
            </el-row>
            <el-row >
                 <el-col :span="24">
                    <p class="name"><i class="el-icon-user-solid"></i> {{Resume.student_name}} </p>
                    <p class="resumeInfo" v-if="UserInfo.gender==true">男</p>
                    <p class="resumeInfo" v-if="UserInfo.gender==false">女</p>
                    <p class="resumeInfo" ><span> {{Resume.age}} </span> 岁 </p>
                    <p class="resumeInfo" ><span style="font-family:'楷体';"> {{Resume.city}} </span> </p>
                </el-col>
            </el-row>
            <el-row class="section">
                <p class="sectionTitle">简历信息</p>
                <hr class="sectionHr" align="center">
            </el-row>
            <el-collapse class="Aresume">
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
                <el-collapse-item title="个人简介" name="6">
                    <div>{{Resume.introduction}}</div>
                </el-collapse-item>
            </el-collapse>
            <el-row class="section">
                <p class="sectionTitle">联系方式</p>
                <hr class="sectionHr" align="center">
            </el-row>
            <el-row >
                 <el-col :span="24">
                    <p class="tips" ><i class="el-icon-info"></i> 为保证信息真实性，以下信息与您的个人信息绑定，如需修改请修改个人信息</p>
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
        <!--简历编辑卡片-->
        <el-card class="editCard" v-if=" editResume==true && resumeExsit==true " >
            <el-row class="header">
                 <el-col class="titlecol" :span="5">
                    <p class="title">在线简历编辑</p>
                </el-col>
                <el-col :span="2" :offset="12" >
                    <el-button class="edit"  icon="el-icon-edit" @click="save();" type="success">保存修改</el-button>
                </el-col>
                 <el-col :span="2" :offset="1">
                    <el-button class="edit"  icon="el-icon-close" @click="dialogVisible=true" type="danger">退出编辑</el-button>
                </el-col>
            </el-row>
           <el-form class="editForm" :model="addResume" :rules="addResumeRules" label-width="120px" ref="editFormRef">
                <el-form-item label="姓名:" style="width:40%" prop="student_name">
                    <el-input placeholder="请输入姓名" v-model="addResume.student_name" maxlength="5" show-word-limit ></el-input>
                </el-form-item >
                 <el-form-item label="年龄:" style="width:40%" prop="age">
                    <el-input-number v-model="addResume.age" :min="1" :max="100"></el-input-number>
                </el-form-item>
                <el-form-item label="所在城市:" prop="city">
                    <el-col :span="10">
                     <el-cascader
                        :placeholder="Resume.city"
                        size="large"
                        :options="options"
                        v-model="selectedOptions"
                        @change="handleChange"
                    >
                    </el-cascader>
                    </el-col>
                </el-form-item>
                <el-form-item label="教育经历:" prop="education">
                    <el-input placeholder="请输入您的教育经历" v-model="addResume.education" type="textarea" :rows="5" maxlength="300" show-word-limit ></el-input>
                </el-form-item>
                <el-form-item label="主要技能:" prop="skill">
                    <el-input placeholder="请输入您掌握的主要技能" v-model="addResume.skill" type="textarea" :rows="5" maxlength="300" show-word-limit ></el-input>
                </el-form-item>
                <el-form-item label="学术成就:" prop="academic">
                    <el-input placeholder="请输入您取得的学术成就" v-model="addResume.academic" type="textarea" :rows="5" maxlength="300" show-word-limit></el-input>
                </el-form-item>
                 <el-form-item label="项目经历:" prop="project">
                    <el-input placeholder="请输入您参加的项目经历" v-model="addResume.project" type="textarea" :rows="5" maxlength="300" show-word-limit></el-input>
                </el-form-item>
                 <el-form-item label="社团/活动经历:" prop="project">
                    <el-input placeholder="请输入您的社团经历或者参与活动经历" v-model="addResume.community" type="textarea" :rows="5" maxlength="300" show-word-limit></el-input>
                </el-form-item>
                <el-form-item label="个人简介:" prop="introduction">
                    <el-input placeholder="在这里简单地介绍您自己" v-model="addResume.introduction" type="textarea" :rows="6" maxlength="400" show-word-limit></el-input>
                </el-form-item>
           </el-form>
            <p class="syaSup">SYA——Show Your Ability</p>
        </el-card>

        <!--简历创建卡片-->
        <el-card class="editCard" v-if="resumeExsit==false" >
            <el-row class="header">
                 <el-col class="titlecol" :span="5">
                    <p class="title">个人简历创建</p>
                </el-col>
                <el-col :span="2" :offset="14" >
                    <el-button class="edit"  icon="el-icon-check" @click="createResume" type="success">创建在线简历</el-button>
                </el-col>
            </el-row>
            <el-row>
                 <el-col :span="24" >
                    <h3><i class="el-icon-s-release"></i> SYA检测到您还没有创建在线简历哦，马上创建一个吧！</h3>
                </el-col>
            </el-row>
           <el-form class="editForm" :model="addResume" :rules="addResumeRules" label-width="120px" ref="createFormRef">
                <el-form-item label="姓名:" style="width:40%" prop="student_name">
                    <el-input placeholder="请输入姓名" v-model="addResume.student_name" maxlength="5" show-word-limit ></el-input>
                </el-form-item >
                <el-form-item label="年龄:" style="width:40%" prop="age">
                    <el-input-number v-model="addResume.age" :min="1" :max="100"></el-input-number>
                </el-form-item>
                <el-form-item label="所在城市:" prop="city">
                      <el-col :span="10">
                     <el-cascader
                        size="large"
                        :options="options"
                        v-model="selectedOptions"
                        @change="handleChange"
                    >
                    </el-cascader>
                    </el-col>
                </el-form-item>
                <el-form-item label="教育经历:" prop="education">
                    <el-input placeholder="请输入您的教育经历" v-model="addResume.education" type="textarea" :rows="5" maxlength="300" show-word-limit ></el-input>
                </el-form-item>
                <el-form-item  label="主要技能:" prop="skill">
                    <el-input placeholder="请输入您掌握的主要技能" v-model="addResume.skill" type="textarea" :rows="5" maxlength="300" show-word-limit ></el-input>
                </el-form-item>
                <el-form-item  label="学术成就:" prop="academic">
                    <el-input placeholder="请输入您取得的学术成就" v-model="addResume.academic" type="textarea" :rows="5" maxlength="300" show-word-limit></el-input>
                </el-form-item>
                 <el-form-item label="项目经历:" prop="project">
                    <el-input placeholder="请输入您参加的项目经历" v-model="addResume.project" type="textarea" :rows="5" maxlength="300" show-word-limit></el-input>
                </el-form-item>
                 <el-form-item label="社团/活动经历:" prop="project">
                    <el-input placeholder="请输入您的社团经历或者参与活动经历" v-model="addResume.community" type="textarea" :rows="5" maxlength="300" show-word-limit></el-input>
                </el-form-item>
                <el-form-item label="个人简介:" prop="introduction">
                    <el-input placeholder="在这里简单地介绍您自己" v-model="addResume.introduction" type="textarea" :rows="6" maxlength="400" show-word-limit></el-input>
                </el-form-item>
           </el-form>
            <p class="syaSup">SYA——Show Your Ability</p>
        </el-card>

        <!--关闭对话框-->
        <el-dialog title="提示" :visible.sync="dialogVisible" width="30%">
            <span>需要保存编辑吗？</span>
                <span slot="footer" class="dialog-footer">
                <el-button @click="cancel();dialogVisible=false" >取 消</el-button>
                <el-button type="primary" @click="save();dialogVisible=false">确 定</el-button>
            </span>
        </el-dialog>
        
    </div>
</template>


<script>
import { regionData, CodeToText } from "element-china-area-data";
import axios from 'axios';
export default {
    data(){
        return{
            Resume:[],
            UserInfo:[],
            dialogVisible:false,
            addResume:{
                age:'',
                student_name:'',
                city:'',
                education:'',
                community:'',
                project:'',
                academic:'',
                skill:'',
                introduction:''
            },
            addResumeRules:{
                student_name:[
                    {required:true, message:"请输入您的姓名" }
                ],
                education:[
                    {required: true, message:'请输入教育经历'}
                ],
                skill:[
                    {required: true, message:'请输入主要技能'}
                ],
                academic:[
                    {required: true, message:'请输入学术成就'}
                ],
                project:[
                    {required: true, message:'请输入项目经历'}
                ],
                community:[
                    {required: true, message:'请输入社团/活动经历'}
                ],
                introduction:[
                    {required: true, message:'请输入个人简介'}
                ],
                city:[
                    {required: true, message:'请选择城市'}
                ],
                age:[
                    {required: true, message:'请选择年龄'}
                ]
                
            },
            options: regionData,
            selectedOptions:[],
            editResume:false,
            resumeExsit:true,
            loading:true
        }
    },
     created(){
        this.getResume();
        this.getUserInfo();
    },
     methods:{
          async getUserInfo(){
            const {data: res} = await axios.post(this.$helper.endpointUrl("/User/GetUserInfo"),{},{ withCredentials: true });

            this.UserInfo=res;
        },
        async getResume(){
            const {data: res} = await axios.post(this.$helper.endpointUrl("/Resume/GetResume"),{},{ withCredentials: true });

            this.Resume=res;
            if(res==-1){
                this.resumeExsit=false;
            }
            this.loading=false;
        },
        save(){
             this.$refs.editFormRef.validate(async valid =>{   
                if(!valid) {
                    this.$message.error("请按照验证提示正确填写在线简历后再进行保存");
                    return;
                }
                const {data: res} = await axios.put(this.$helper.endpointUrl("/Resume/UpdateResume"),this.addResume,{ withCredentials: true });
                if (res.status > 400) {
                this.$message.error("保存失败，请稍后重试");
                return;
                }
                this.$message.success("保存成功！");
                this.getResume();//重新调取用户信息
                this.editResume=false;
            })
        },
        cancel(){
            this.dialogVisible=true;
            this.editResume=false;
            this.$message.warning("您所作的更改并未保存");
        },
        edit(){
            this.addResume.education=this.Resume.education;
            this.addResume.skill=this.Resume.skill;
            this.addResume.academic=this.Resume.academic;
            this.addResume.project=this.Resume.project;
            this.addResume.community=this.Resume.community;
            this.addResume.introduction=this.Resume.introduction;
            this.addResume.student_name=this.Resume.student_name;
            this.addResume.city=this.Resume.city;
            this.addResume.age=this.Resume.age;
            console.log(this.selectedOptions);
            this.editResume=true;
        },
         handleChange() {
            var loc = "";
            for (let i = 0; i < this.selectedOptions.length; i++) {//没过一个区域码，就加一个标识符
                if(i>0){
                    loc+=" ";
                }
                loc += CodeToText[this.selectedOptions[i]];
            }
            this.addResume.city=loc;
        },
        createResume(){
             this.$refs.createFormRef.validate(async valid =>{   
                if(!valid) {
                    this.$message.error("只有按照验证提示完成所有简历信息的正确填写后才能创建简历哦");
                    return;
                }
                const {data: res} = await axios.post(this.$helper.endpointUrl("/Resume/CreateResume"),this.addResume,{ withCredentials: true });
                if (res.status > 400) {
                this.$message.error("创建简历失败，请稍后再试");
                return;
                }
                this.$message.success("简历创建成功！");
                this.getResume();//重新调取用户信息
                this.resumeExsit=true;
            })
        },
     }
}
</script>


<style scoped>
.resumeCard{
    padding:0;
}
.resumeCard{
    margin-top:20px;
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
    font-family:"楷体" ;
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
    margin-top:2cm;
}
.resumeInfo{
    font-family: "楷体";
    font-size:20px;
}
</style>

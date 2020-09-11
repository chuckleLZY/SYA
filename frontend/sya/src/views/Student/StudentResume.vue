<template>
    <div>
        <el-breadcrumb separator-class="el-icon-arrow-right">
            <el-breadcrumb-item :to="{ path: '/home' }">首页</el-breadcrumb-item>
            <el-breadcrumb-item>学生我的</el-breadcrumb-item>
            <el-breadcrumb-item>在线简历</el-breadcrumb-item>
        </el-breadcrumb>
        <!--简历卡片-->
        <el-card class="resumeCard" id="card1">
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
                    <p style="font-size:30px"><i class="el-icon-male" style="color: blue" id="gender"></i>
                    <p><span> {{Resume.age}} </span> 岁 </p>
                    <p><span> {{Resume.city}} </span> </p>
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
        <el-card class="editCard" id="card2">
            <el-row class="header">
                 <el-col class="titlecol" :span="5">
                    <p class="title">在线简历编辑</p>
                </el-col>
                <el-col :span="2" :offset="12" >
                    <el-button class="edit"  icon="el-icon-edit" @click="save()" type="success">保存修改</el-button>
                </el-col>
                 <el-col :span="2" :offset="1">
                    <el-button class="edit"  icon="el-icon-close" @click="dialogVisible=true" type="danger">退出编辑</el-button>
                </el-col>
            </el-row>
           <el-form class="editForm" :model="addResume" :rules="addResumeRules" label-width="120px" ref="editFormRef">
                <el-form-item label="教育经历:" prop="education">
                    <el-input v-model="addResume.education" type="textarea" :rows="5" maxlength="300" show-word-limit ></el-input>
                </el-form-item>
                <el-form-item label="主要技能:" prop="skill">
                    <el-input v-model="addResume.skill" type="textarea" :rows="5" maxlength="300" show-word-limit ></el-input>
                </el-form-item>
                <el-form-item label="学术成就:" prop="academic">
                    <el-input v-model="addResume.academic" type="textarea" :rows="5" maxlength="300" show-word-limit></el-input>
                </el-form-item>
                 <el-form-item label="项目经历:" prop="project">
                    <el-input v-model="addResume.project" type="textarea" :rows="5" maxlength="300" show-word-limit></el-input>
                </el-form-item>
                 <el-form-item label="社团/活动经历:" prop="project">
                    <el-input v-model="addResume.community" type="textarea" :rows="5" maxlength="300" show-word-limit></el-input>
                </el-form-item>
                <el-form-item label="个人简介:" prop="introduction">
                    <el-input v-model="addResume.introduction" type="textarea" :rows="6" maxlength="400" show-word-limit></el-input>
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
                education:[
                    {required: true, message:'请输入教育经历', trigger:'blur'}
                ],
                skill:[
                    {required: true, message:'请输入主要技能', trigger:'blur'}
                ],
                academic:[
                    {required: true, message:'请输入学术成就', trigger:'blur'}
                ],
                project:[
                    {required: true, message:'请输入项目经历', trigger:'blur'}
                ],
                community:[
                    {required: true, message:'请输入社团/活动经历', trigger:'blur'}
                ],
                introduction:[
                    {required: true, message:'请输入个人简介', trigger:'blur'}
                ],
                
            }
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
            if(this.UserInfo.gender==false){//如果是女生更改简历显示
                var genIcon=document.getElementById("gender");
                genIcon.className="el-icon-female";
                genIcon.style.color="pink";
            }
            console.log(res);
        },
        async getResume(){
            const {data: res} = await axios.post('http://localhost:5000/Resume/GetResume',{},{ withCredentials: true });

            this.Resume=res;
            console.log(11);
        },
        save(){
            this.addResume.age=this.Resume.age;
            this.addResume.student_name=this.Resume.student_name;
            this.addResume.city=this.Resume.city
             this.$refs.editFormRef.validate(async valid =>{   
                if(!valid) {
                    this.$message.error("请按照验证提示正确填写在线简历后再进行保存");
                    return;
                }
                const {data: res} = await axios.put('http://localhost:5000/Resume/UpdateResume',this.addResume,{ withCredentials: true });
                if (res.status > 400) {
                this.$message.error("保存失败，请稍后重试");
                return;
                }
                this.$message.success("保存成功！");
                this.getResume();//重新调取用户信息
                var card=document.getElementById("card2");//隐藏编辑卡片
                card.style.display="none";
                var card=document.getElementById("card1");
                card.style.display="block";
            })
        },
        cancel(){
            this.dialogVisible=true;
            var card=document.getElementById("card2");//隐藏编辑卡片
            card.style.display="none";
            var card=document.getElementById("card1");
            card.style.display="block";
            this.$message.warning("您所作的更改并未保存");
        },
        edit(){
            this.addResume.education=this.Resume.education;
            this.addResume.skill=this.Resume.skill;
            this.addResume.academic=this.Resume.academic;
            this.addResume.project=this.Resume.project;
            this.addResume.community=this.Resume.community;
            this.addResume.introduction=this.Resume.introduction;
            var card=document.getElementById("card1");//隐藏编辑卡片
            card.style.display="none";
            var card=document.getElementById("card2");
            card.style.display="block";
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
    display:none;
}
.editForm{
    width:50%;
    margin-left:25%;
    margin-top:2cm;
}
</style>

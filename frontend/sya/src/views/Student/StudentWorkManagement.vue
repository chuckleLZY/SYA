<template>
    <div>
        <el-breadcrumb separator-class="el-icon-arrow-right">
            <el-breadcrumb-item :to="{ path: '/home' }">首页</el-breadcrumb-item>
            <el-breadcrumb-item>学生我的</el-breadcrumb-item>
            <el-breadcrumb-item>工作管理</el-breadcrumb-item>
        </el-breadcrumb>
        
         <el-card class="workCard" >
            <el-row class="header">
                 <el-col class="titlecol" :span="5">
                    <p class="title">工作管理</p>
                </el-col>
            </el-row>
            <!--搜索与添加-->
            <el-row  :gutter="30">
                <el-col :span="7">
                    <el-input placeholder="请输入内容" v-model="queryInfo.query" >
                        <el-button slot="append" icon="el-icon-search" @click="getWorklist()"></el-button>
                    </el-input>
                </el-col>
                <el-col :span="2">
                    <el-button type="success" icon="el-icon-plus" @click="$router.push('/Recruitment');">我要申请</el-button>
                </el-col>
            </el-row>
            <!--表格区域-->
            <el-row style="margin-top:1cm">
                <el-table :data="Worklist" border>
                    <el-table-column type="index" label="#" >
                    </el-table-column>
                    <el-table-column prop="work_name" label="工作名称" >
                    </el-table-column>
                    <el-table-column prop="address" label="工作地址" >
                    </el-table-column>
                    <el-table-column prop="salary" label="工作薪水">
                    </el-table-column>
                    <el-table-column prop="total_time" label="工作时长">
                    </el-table-column>
                    <el-table-column label="工作状态">
                         <template slot-scope="scope">
                             <el-tag type="success" v-if="scope.row.status==1">已读</el-tag>
                             <el-tag type="info" v-if="scope.row.status==0">未读</el-tag>
                        </template>
                    </el-table-column>
                    <el-table-column  label="操作" width="400">
                        <template slot-scope="scope">
                            <el-button type="primary" size="medium" icon="el-icon-document-remove" @click="AbForm.work_id=scope.row.work_id; AbsentVisible=true;">申请请假</el-button>
                            <el-button type="warning" size="medium" icon="el-icon-document-delete">申请辞职</el-button>
                            <el-button type="danger" size="medium" icon="el-icon-circle-close">删除工作</el-button>
                        </template>
                    </el-table-column>
                    <el-table-column type="expand" label="详情" width="75" >
                        <template slot-scope="scope">
                            <el-row>
                                <el-col :span="12">
                                    <el-image :src="scope.row.cover" :fit="fit">
                                        <div slot="error" class="image-slot">
                                           <i class="el-icon-picture-outline" style="font-size: 100px"></i>
                                        </div>
                                    </el-image> 
                                </el-col>
                                <el-col :span="8" class="inlineCol" style="text-align:left" :offset="2">
                                    <p class="inlineTitle">工作描述:</p>
                                    <div class="description" style="text-align:left;display: inline;">
                                         <p>{{scope.row.work_description}}</p>
                                    </div>
                                    <p class="inlineTitle"><i class="el-icon-thumb"></i> 点赞 ： <span class="inlineInfo"> {{scope.row.likes_num}} </span> </p>
                                    <p class="inlineTitle"><i class="el-icon-star-off"></i> 收藏 ： <span class="inlineInfo"> {{scope.row.collect_num}} </span> </p>
                                    <p class="inlineTitle"><i class="el-icon-date"></i> 工作周期 ： <span class="inlineInfo"> {{scope.row.start_day}} </span> ~ <span class="inlineInfo"> {{scope.row.end_day}} </span> </p>
                                     <p class="inlineTitle"><i class="el-icon-time"></i> 工作时间 ：
                                        <span class="inlineInfo" v-if="scope.row.week_day==1"> 星期一 </span>  
                                        <span class="inlineInfo" v-if="scope.row.week_day==2"> 星期二 </span> 
                                        <span class="inlineInfo" v-if="scope.row.week_day==3"> 星期三 </span> 
                                        <span class="inlineInfo" v-if="scope.row.week_day==4"> 星期四 </span> 
                                        <span class="inlineInfo" v-if="scope.row.week_day==5"> 星期五 </span> 
                                        <span class="inlineInfo" v-if="scope.row.week_day==6"> 星期六 </span> 
                                        <span class="inlineInfo" v-if="scope.row.week_day==7"> 星期天 </span> 
                                        <span class="inlineInfo"> {{scope.row.start_time}} </span> ~ <span class="inlineInfo"> {{scope.row.end_time}}  </span>
                                    </p>
                                </el-col>
                            </el-row>    
                        </template>
                    </el-table-column>
                </el-table>
            </el-row>
            <!--分页区域-->
             <el-pagination
            @size-change="handleSizeChange"
            @current-change="handleCurrentChange"
            :current-page="queryInfo.pagenum"
            :page-sizes="[5, 10, 15]"
            :page-size="queryInfo.pagesize"
            layout="total, sizes, prev, pager, next, jumper"
            :total="total" style="margin-top:2cm">
            </el-pagination>
         </el-card>

         <!--请假的信息-->
         <el-dialog title="请假申请" :visible.sync="AbsentVisible" @close="AbFormClose()" width="50%" center >
            <el-form :model="AbForm" :rules="AbFormRules" label-width="100px" ref="AbFormRef" >
                 <el-form-item label="请假时间">
                      <el-date-picker type="date" placeholder="选择日期" style="width: 40%;"></el-date-picker>
                 </el-form-item>
                <el-form-item  label="请假时长 ：" prop="leave_time">
                    <el-input v-model="AbForm.leave_time" placeholder="请输入请假时长" style="width:40%" ></el-input>
                </el-form-item>
                <el-form-item label="请假理由 ：" prop="content">
                    <el-input v-model="AbForm.content" placeholder="请输入请假理由" type="textarea" :rows="3" maxlength="300" show-word-limit style="width:80%"></el-input>
                </el-form-item>
                <el-form-item label="备注 ：" prop="proof">
                    <el-input v-model="AbForm.proof" placeholder="此项选填" type="textarea" :rows="3" maxlength="300" show-word-limit style="width:80%"></el-input>
                </el-form-item>
            </el-form>
            <span slot="footer" class="dialog-footer" >
                <el-button @click="AbsentVisible = false">取 消</el-button>
                <el-button type="primary" @click="addAbsentPre();">提 交</el-button>
            </span>
        </el-dialog>
    </div>
</template>


<script>
import axios from 'axios';
export default {
    data(){
        return{
            Worklist:[],
            queryInfo:{
                pagenum:1,
                pagesize:5,
                query:''
            },
            total:20,
            AbsentVisible:false,
            AbForm:{
                work_id:'',
                content:'',
                proof:''

            },
            AbFormRules:{
                leave_time:[
                    { required:true, message:'请输入请假时长', trigger:'blur'}
                    
                ],
                 content:[
                    { required:true, message:'请输入请假理由', trigger:'blur'}
                ]
            }
        }
    },
    created(){
        this.getWorklist()
    },
    methods:{
         async getWorklist(){
            const {data: res} = await axios.post('http://localhost:5000/Work/FindOwnWork',this.queryInfo,{ withCredentials: true });
            this.Worklist=res.worklist;
            this.total=res.totalpage;
            console.log(this.Worklist);
        },
        handleSizeChange(newSize){
            this.queryInfo.pagesize=newSize;
            this.getWorklist();
        },
        handleCurrentChange(newPage){
            this.queryInfo.pagenum=newPage;
            this.getWorklist();
        },
        AbFormClose(){
            this.$refs.AbFormRef.resetFields();
        },
        addAbsentPre(){
            this.$refs.AbFormRef.validate(async valid =>{
                if(!valid) {
                    this.$message.error("请按照验证提示正确填写请假申请后再进行提交");
                    return;
                }
                if(this.AbForm.proof==''){
                this.AbForm.proof='无备注';
                }
                const {data: res} = await axios.post('http://localhost:5000/Leave/RequestRest',this.AbForm,{ withCredentials: true });
                if (res.error_code == -1) {
                    this.$message.error("请假申请提交失败，请稍后重试");
                    return;
                }
                this.$message.success('请假申请提交成功！');
                this.AbsentVisible=false;
            })
        },
    }
    
}
</script>


<style scoped>
.workCard{
    margin-top:1cm;
    padding:0;
}
.title{
    font-size: 25px;
    font-weight: bold;
}
.titlecol{
    margin-bottom:1cm;
}
.inlineCol{
    text-align: center;
}
.inlineTitle{
    font-weight:bold;
    color: grey;
    margin-top:1cm;
}
.inlineInfo{
    font-weight: normal;
    color: black;
}
</style>

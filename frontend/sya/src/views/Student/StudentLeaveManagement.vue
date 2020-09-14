<template>
    <div>
        <el-breadcrumb separator-class="el-icon-arrow-right">
            <el-breadcrumb-item :to="{ path: '/home' }">首页</el-breadcrumb-item>
            <el-breadcrumb-item>学生我的</el-breadcrumb-item>
            <el-breadcrumb-item>请假管理</el-breadcrumb-item>
        </el-breadcrumb>
        
         <el-card class="workCard" >
            <el-row class="header">
                 <el-col class="titlecol" :span="5">
                    <p class="title">请假管理</p>
                </el-col>
            </el-row>
            <!--表格区域-->
            <el-row>
                <el-table :data="LeaveList" border>
                    <el-table-column type="index" label="#" >
                    </el-table-column>
                    <el-table-column prop="leave_day" label="请假日期" >
                    </el-table-column>
                    <el-table-column prop="address" label="请假时段" >
                    </el-table-column>
                    <el-table-column prop="content" label="请假理由">
                    </el-table-column>
                    <el-table-column prop="proof" label="备注">
                    </el-table-column>
                    <el-table-column  label="操作" width="280">
                        <template slot-scope="scope">
                            <el-button type="primary" size="medium" icon="el-icon-edit" @click="AbForm.work_id=scope.row.work_id; AbsentVisible=true;">修改申请</el-button>
                            <el-button type="danger" size="medium" icon="el-icon-circle-close">撤回申请</el-button>
                        </template>
                    </el-table-column>
                </el-table>
            </el-row>

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
            LeaveList:[],
            queryInfo:{
                pagenum:1,
                pagesize:5,
            },
            total:20,
            AbsentVisible:false,
            AbForm:{
                work_id:'',
                content:'',
                proof:'',
                leave_day:'',
                leave_start:'',
                leave_end:''
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
        this.getLeaveList()
    },
    methods:{
         async getLeaveList(){
            const {data: res} = await axios.post('http://localhost:5000/Leave/ViewLeave',this.queryInfo,{ withCredentials: true });
            this.LeaveList=res.leavelist;
            this.total=res.total;
            console.log(this.LeaveList)
        },
        handleSizeChange(newSize){
            this.queryInfo.pagesize=newSize;
            this.getLeaveList();
        },
        handleCurrentChange(newPage){
            this.queryInfo.pagenum=newPage;
            this.getLeaveList();
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
.description{
    text-align: center;
    width: 60%;
    margin-left: 20%;
}
</style>

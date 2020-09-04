<template>
    <div>
        这里是招聘会
        <el-breadcrumb separator-class="el-icon-arrow-right" class="recruitment_breadcrumb">
            <el-breadcrumb-item :to="{ path: '/home' }">学生</el-breadcrumb-item>
            <el-breadcrumb-item>招聘会</el-breadcrumb-item>
        </el-breadcrumb>

        <el-card class="recruitment_card">
            
            <el-row>
                <el-col :span="7">
                    <el-input placeholder="请输入内容" v-model="queryInfo.query" class="input-with-select" clearable @clear="findWork">
                        <!--<el-select v-model="select" slot="prepend" placeholder="搜索类型">
                        <el-option label="工作类型" value="1"></el-option>
                        <el-option label="工作时间" value="2"></el-option>
                        <el-option label="工作地点" value="3"></el-option>
                        </el-select>-->
                        <el-button slot="append" icon="el-icon-search" @click="findWork"></el-button>
                    </el-input>
                </el-col>
                <el-col :span="4"></el-col>
            </el-row>

            <!-- 列表区域 -->
           <!-- <el-table :data="workList" stripe>
                <el-table-column label="#" type="index"></el-table-column>
                <el-table-column label="工作ID" prop="WORK_ID">

                </el-table-column>

                
                <el-table-column label="工作地点" prop="ADDRESS">

                </el-table-column>

                <el-table-column label="时薪" prop="SALARY">

                </el-table-column>

                <el-table-column label="工作时间" prop="TIME">

                </el-table-column> 

                <el-table-column label="操作">
                    <template v-slot:default="scope"></template>
                            <el-tooltip  effect="dark" content="查看详情" placement="top-start" :enterable="false">
                                <el-button type="primary" icon="el-icon-edit" size="mini" circle></el-button>
                            </el-tooltip>
                            <el-tooltip  effect="dark" content="直接申请" placement="top-start" :enterable="false">
                                <el-button type="primary" icon="el-icon-check" size="mini" circle></el-button>
                            </el-tooltip>
                </el-table-column> 

            </el-table>-->


            <!-- 工作卡片-->
            <el-row>
                <!--<el-col :span="8" v-for="(o, index) in 9" :key="o" :offset="index > 0 ? 0 : 0">-->
                    <div v-for="work in workList" :key="work.work_name" >
                    <el-col :span="8" v-for="(o, index) in 1" :key="o" :offset="index > 0 ? 1 : 0">
                        
                    <el-card :body-style="{ padding: '10px'}" class="recruitment_card2">
                    <img :src= work.cover class="image">
                    <div style="padding: 14px;" > 
                        <p >工作名称: {{work.work_name}}</p>
                        <p>点赞: {{work.likes_num}}</p>
                        <p>收藏: {{work.collect_num}}</p>
                        <p>分享: {{work.share_num}}</p>
                        <!--<el-table :data="workList" stripe>
                                    <el-table-column
                            prop="work_name"
                            label="工作名字"
                            width="180">
                        </el-table-column>
                        <el-table-column
                            prop="salary"
                            label="薪资"
                            width="180">
                        </el-table-column>
                        </el-table>-->
                        <div class="bottom clearfix">
                            <el-card>
                        
                        <el-button type="text" class="button" @click="showDrawer(work.work_id)" >查看详情</el-button>
                        
                        </el-card>
                        </div> 

                        <!--<el-table :data="workList" stripe>
                            <el-table-column label="操作">
                                <template v-slot:default="scope">

                                        <el-button type="text" class="button" @click="showDrawer(scope.row.work_name)" >查看详情</el-button>
                                </template>
                            </el-table-column> 
                        </el-table>-->
                    </div>
                    </el-card>
                    
                </el-col>
                </div>
            </el-row>


          <!-- 右侧弹窗 -->
            <el-drawer
                title="工作详情"
                :visible.sync="drawer"
                
                :before-close="handleClose">
                
                <el-card :body-style="{ padding: '10px'}" class="recruitment_card3">
                    <img :src=workInfo.cover class="image">
                <div slot="header" class="clearfix">
                            
                        <!--<el-button style="float: right; padding: 3px 0" type="text">操作按钮</el-button>-->
                </div>
                      <div>
                          <br/>
                <el-form :model="workInfo" label-width="75px" class="demo-ruleForm">
                <el-form-item label="工作名称:" prop="work_name" class="demo-ruleFormItem">
                    <el-input v-model="workInfo.work_name" disabled></el-input>
                </el-form-item>
                <el-form-item label="工作地点:" prop="address" class="demo-ruleFormItem">
                    <el-input v-model="workInfo.address" disabled></el-input>
                </el-form-item>
                <el-form-item label="薪资:" prop="salary" class="demo-ruleFormItem">
                    <el-input v-model="workInfo.salary" disabled></el-input>
                </el-form-item>
                <el-form-item label="工作时间:" prop="work_time" class="demo-ruleFormItem">
                    <el-input v-model="workInfo.work_time" disabled></el-input>
                </el-form-item>
                <el-form-item label="工作描述:" prop="work_description" class="demo-ruleFormItem">
                    <el-input v-model="workInfo.work_description" disabled></el-input>
                </el-form-item>
                
                </el-form>
                </div>
                 
                </el-card>
                <!--<div class="demo-drawer__footer">
                        
                    <el-button type="primary" @click="$refs.drawer.closeDrawer()" :loading="loading" style="margin-top: 16px;">{{ loading ? '提交中 ...' : '确 定' }}</el-button>
                </div>-->

                <div class="demo-drawer__footer">
                        
                        <el-button type="primary"  @click="appWork()" style="margin-top: 16px;" v-if="this.$store.state.role==1">提交简历</el-button>
                        
                </div>
            </el-drawer>


            <!--  分页区域  -->
            <el-pagination
                @size-change="handleSizeChange"
                @current-change="handleCurrentChange"
                :current-page="queryInfo.pagenum"
                :page-sizes="[3, 6, 9]"
                :page-size="3"
                layout="total, sizes, prev, pager, next, jumper"
                :total="total">
            </el-pagination>
        </el-card>
    </div>
</template>


<script>
import axios from 'axios';
export default {
  data() {
    return {
      input1: '',
      input2: '',
      input3: '',
      select: '',

    //获取工作列表的参数对象
      queryInfo:{
          
          pagenum: 1,
          pagesize: 3,
          query: ''
      },
      workList:[],
      total:0,
      drawer: false,
      workInfo:{}
    }
  },
  created(){
      //调用获取发布的工作的API函数
    //  this.getWorkInfo()
      this.getWorkList()
      
  },
  methods:{
      //监听每页条数选项改变的事件
      handleSizeChange(newSize){
        console.log(newSize)
        this.queryInfo.pagesize=newSize
        this.getWorkList()
      },
      //监听页码改变的事件
      handleCurrentChange(newPage){
        //console.log(newPage)
        this.queryInfo.pagenum=newPage
        this.getWorkList()
      },
      
        handleClose(done) {
            this.$confirm('确认关闭？')
            .then(_ => {
                done();
            })
            .catch(_ => {})
        },
     //展示右侧弹窗
    async showDrawer(workid){
        //console.log(workid)
        const res = await axios.post(
        "http://localhost:5000/Work/ViewWorkInfo",
        {work_id:workid},
        {withCredentials: true}
        );
        if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
        }

        this.workInfo=res.data

      //console.log(res);
     this.drawer=true
    },
  //获取工作列表的函数
    async getWorkList(){
      /*  const res= await axios.post("http://localhost:5000/Work/ViewOwnWork",{pagenum: 1,
          pagesize: 2},{withCredetials: true });
        if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
        }
        this.workList=res.data.worklist;
        this.total=res.data.totalpage;
        console.log(res);*/

        const res = await axios.post(
        "http://localhost:5000/Work/ViewAllWork",
        {
          pagenum: this.queryInfo.pagenum,
          pagesize: this.queryInfo.pagesize
        },
        {
          withCredentials: true
        }
      );
      if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
        }
        this.workList=res.data.worklist;
        this.total=res.data.totalpage;
        this.pagesize=res.data.totalpage/res.data.pagenum;
        this.pagenum=res.data.pagenum;
       // console.log(this.pagesize);
      //  console.log(res);
      },

    async findWork(){
        const res=await axios.post('http://localhost:5000/Work/FindWork',{
                    pagenum: this.queryInfo.pagenum,
                     pagesize: this.queryInfo.pagesize,
                    query: this.queryInfo.query
            },{withCredentials: true});
        if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
        }
        this.workList=res.data.worklist;
        this.total=res.data.totalpage;
        console.log(res);
    },
    async appWork(){
        const res=await axios.post('http://localhost:5000/Apply/CreateApply',{work_id:this.workInfo.work_id},{
          withCredentials: true
        });
        if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
        }
        //console.log(res);
        this.$message.success('申请此工作成功');
    }
     

    
     
      
        
  }
}
</script>

<style scoped>
  .recruitment_breadcrumb{
  position: relative;
  left:20px;
  margin-bottom: 15px;
  font-size:12px;
  width:100% !important;
}

.recruitment_card{
  box-shadow: 0 10px 10px rgba(0, 0, 0, 0.15) !important;
  position: relative;
  left:10px;
  width:100% !important;
}

.recruitment_card2{
  box-shadow: 0 10px 10px rgba(0, 0.25, 0, 0.25) !important;
  /*margin-left:70px;
  margin-top:30px;*/
  width:400px !important;
  margin: auto;
  margin-top:30px;
  
}

.recruitment_card3{
  box-shadow: 0 10px 10px rgba(0, 0.25, 0, 0.25) !important;
  margin: auto;
  width:350px !important;
  height:600px !important;
  overflow: auto;
}


.el-select .el-input {
  width: 130px;
}
.input-with-select .el-input-group__prepend {
  background-color: #fff;
}

.el-pagination{
  margin-top: 15px;
}


.time {
  font-size: 13px;
  color: #999;
}

.bottom {
  margin-top: 13px;
  line-height: 8px;
}

.button {
  padding: 0;
  float: right;
}

.image {
  width: 100%;
  display: block;
}

.clearfix:before,
.clearfix:after {
    display: table;
    content: "";
}

.clearfix:after {
    clear: both
}

.demo-ruleFormItem{
  margin-left: -10px;
}
</style>

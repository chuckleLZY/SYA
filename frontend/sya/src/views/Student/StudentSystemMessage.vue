<template>
    <div>
        这里是学生系统通知
        <el-breadcrumb separator-class="el-icon-arrow-right" class="recruitment_breadcrumb">
            <el-breadcrumb-item :to="{ path: '/home' }">学生</el-breadcrumb-item>
            <el-breadcrumb-item>消息</el-breadcrumb-item>
            <el-breadcrumb-item>系统通知</el-breadcrumb-item>
        </el-breadcrumb>


        <el-card class="student_card">
            
            <el-row>
                <el-col :span="7">
                    <el-input placeholder="请输入内容" v-model="queryInfo.query" class="input-with-select">
                        
                        <el-button slot="append" icon="el-icon-search" @click="getWorkList"></el-button>
                    </el-input>
                </el-col>
                <el-col :span="4"></el-col>
            </el-row>
            
            <!-- 列表区域 -->
            <el-table :data="workList" stripe>
                <el-table-column label="#" type="index"></el-table-column>
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
                <el-table-column
                    prop="address"
                    label="地址">
                </el-table-column>
                <el-table-column label="操作">
                    <template v-slot:default="scope">
                       

                    
                            <el-tooltip  effect="dark" content="查看详情" placement="top-start" :enterable="false">
                                <el-button type="primary" icon="el-icon-edit" size="mini" circle></el-button>
                            </el-tooltip>
                            <el-tooltip  effect="dark" content="删除" placement="top-start" :enterable="false">
                                <el-button type="primary" icon="el-icon-delete" size="mini" circle></el-button>
                            </el-tooltip>
                    </template>

                            
                </el-table-column> 

            </el-table>

            <!--  分页区域  -->
            <el-pagination
                @size-change="handleSizeChange"
                @current-change="handleCurrentChange"
                :current-page="queryInfo.pagenum"
                :page-sizes="[1, 2, 5, 10]"
                :page-size="queryInfo.pagesize"
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
          query: '',
          pagenum: 1,
          pagesize: 2
      },
      workList:[],

     /* workList: [{
            date: '2016-05-02',
            name: '王小虎',
            address: '上海市普陀区金沙江路 1518 弄'
          }, {
            date: '2016-05-04',
            name: '王小虎',
            address: '上海市普陀区金沙江路 1517 弄'
          }, {
            date: '2016-05-01',
            name: '王小虎',
            address: '上海市普陀区金沙江路 1519 弄'
          }, {
            date: '2016-05-03',
            name: '王小虎',
            address: '上海市普陀区金沙江路 1516 弄'
          }]*/
    }
  },
  created(){
      //调用获取发布的工作的API函数
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
  //获取工作列表的函数
   async getWorkList(){
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
        //console.log(this.pagesize);
      //  console.log(res);
      },
  }
}
</script>


<style scoped>

</style>

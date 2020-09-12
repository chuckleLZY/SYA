<template>
    <div>


        <el-breadcrumb separator-class="el-icon-arrow-right" class="recruitment_breadcrumb">
            <el-breadcrumb-item :to="{ path: '/home' }">老师</el-breadcrumb-item>
            <el-breadcrumb-item>消息</el-breadcrumb-item>
            <el-breadcrumb-item>系统通知</el-breadcrumb-item>
        </el-breadcrumb>


        <el-card class="teacher_card" style="margin-top:20px;">
            
           <!-- <el-row>
                <el-col :span="7">
                    <el-input placeholder="请输入内容" v-model="queryInfo.query" class="input-with-select">
                        
                        <el-button slot="append" icon="el-icon-search" @click="getSysMessageList"></el-button>
                    </el-input>
                </el-col>
                <el-col :span="4"></el-col>
            </el-row>-->
            
            <!-- 列表区域 -->
            <el-table :data="sysMessageList"  v-loading="loading">
                <el-table-column label="#" type="index"></el-table-column>
                <el-table-column
                    prop="title"
                    label="公告标题"
                    width="280">
                </el-table-column>
                <el-table-column
                    prop="send_time"
                    label="发送时间"
                    width="280">
                </el-table-column>
                <el-table-column
                    prop="status"
                    label="状态"
                    width="280">
                    <template slot-scope="scope">
                    <el-tag type="success" v-if="scope.row.status==1">已读</el-tag>
                    <el-tag type="info" v-if="scope.row.status==0">未读</el-tag>
                  </template>
                </el-table-column>
               <!-- <el-table-column
                    prop="address"
                    label="地址">
                </el-table-column>-->
                <el-table-column label="操作">
                    <template v-slot:default="scope">
                       

                    
                            <el-tooltip  effect="dark" content="查看详情" placement="top-start" :enterable="false">
                                <el-button type="success" icon="el-icon-edit" size="mini" @click="viewMesInfo(scope.row)" ></el-button>
                            </el-tooltip>
                            <el-tooltip  effect="dark" content="删除" placement="top-start" :enterable="false">
                                <el-button type="danger" icon="el-icon-delete" size="mini" @click="removeMesById(scope.row.announcement_id)" ></el-button>
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
                :total="total"
                style="margin-top:10px">
            </el-pagination>
        </el-card>

        <el-dialog title="详情" :visible.sync="checkDialogVisible" width="35%">
      <div style="width:400px;margin:auto;">
        <el-form status-icon label-width="auto" :model="messageData">
          
          <el-form-item label="内容">
            <el-input  disabled v-model="messageData.content"></el-input>
          </el-form-item>
          <el-form-item label="发送时间">
            <el-input  disabled v-model="messageData.send_time"></el-input>
          </el-form-item>
          
          
        </el-form>
        
      </div>
    </el-dialog>


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
      checkDialogVisible: false,
      messageData:{},
      loading: true,
    //获取工作列表的参数对象
      queryInfo:{
          query: '',
          pagenum: 1,
          pagesize: 10
      },
      sysMessageList:[],
      total:0,
      

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
      this.getSysMessageList()
  },
  methods:{
   
      //监听每页条数选项改变的事件
      handleSizeChange(newSize){
        //console.log(newSize)
        this.loading = true;
        this.queryInfo.pagesize=newSize
        this.getSysMessageList()
        this.loading = false;
      },
      //监听页码改变的事件
      handleCurrentChange(newPage){
       // console.log(newPage)
       this.loading = true;
        this.queryInfo.pagenum=newPage
        this.getSysMessageList()
        this.loading = false;
      },
  //获取工作列表的函数
   async getSysMessageList(){
         const res = await axios.post(
        "http://localhost:5000/Announce/GetAnnounce",
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
        this.sysMessageList=res.data.announceItem;
        this.total=res.data.totalpage;
        this.pagesize=res.data.totalpage/res.data.pagenum;
        this.pagenum=res.data.pagenum;
        //console.log(this.pagesize);
      //  console.log(this.sysMessageList);
        this.loading = false;
      },

      async removeMesById(id){
        const confirmResulte=await this.$confirm('此操作将永久删除该系统消息, 是否继续?', '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        }).catch(err=>err)
        //console.log(confirmResulte);
        if(confirmResulte!=='confirm'){
          return this.$message.info("已取消删除");
        }
        const res = await axios.post(
        "http://localhost:5000/Announce/DeleteAnnounce",
        {
          announcement_id: id
          
        },
        {
          withCredentials: true
        }
      );

      if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
        }
      this.$message.info("已成功删除");
      console.log(res);
      this.getSysMessageList();

      },

      async viewMesInfo(row){
        this.messageData = row;
        this.checkDialogVisible = true;
        const res = await axios.post(
        "http://localhost:5000/Announce/GetAnnounceContent",
        {
          announcement_id: row.announcement_id
        },
        {
          withCredentials: true
        }
      );
      this.getSysMessageList();
    //  console.log(res);
      },
  }
}
</script>


<style scoped>

</style>


<template>
    <div>
        这里是学生工作通知

        <el-breadcrumb separator-class="el-icon-arrow-right" class="recruitment_breadcrumb">
            <el-breadcrumb-item :to="{ path: '/home' }">学生</el-breadcrumb-item>
            <el-breadcrumb-item>消息</el-breadcrumb-item>
            <el-breadcrumb-item>工作通知</el-breadcrumb-item>
        </el-breadcrumb>


        <el-card class="student_card">
            <!--
            <el-row>
                <el-col :span="7">
                    <el-input placeholder="请输入内容" v-model="input3" class="input-with-select">
                        <el-select v-model="select" slot="prepend" placeholder="搜索类型">
                        <el-option label="工作类型" value="1"></el-option>
                        <el-option label="工作时间" value="2"></el-option>
                        <el-option label="工作地点" value="3"></el-option>
                        </el-select>
                        <el-button slot="append" icon="el-icon-search"></el-button>
                    </el-input>
                </el-col>
                <el-col :span="4"></el-col>
            </el-row>
            -->

                  <el-row>
                <el-col :span="7">
                    <el-input placeholder="请输入内容" v-model="queryInfo.query" class="input-with-select" clearable @clear="getWorkMessageList">
                        
                        <el-button slot="append" icon="el-icon-search" @click="findWorkMes"></el-button>
                    </el-input>
                </el-col>
                <el-col :span="4"></el-col>
            </el-row>

            <!-- 列表区域 -->
            <el-table :data="workMessageList" :row-class-name="tableRowClassName">
                <el-table-column label="#" type="index"></el-table-column>
                <el-table-column label="发件人" prop="sender_name">

                </el-table-column>

                
                <el-table-column label="消息内容" prop="content">

                </el-table-column>

                <el-table-column label="发送时间" prop="message_time">

                </el-table-column>
                <el-table-column label="状态" prop="status">

                </el-table-column>

                <el-table-column label="操作">
                    <template v-slot:default="scope">
                       

                    
                            <el-tooltip  effect="dark" content="查看详情" placement="top-start" :enterable="false">
                                <el-button type="primary" icon="el-icon-edit" size="mini" @click="viewMesInfo(scope.row)" circle></el-button>
                            </el-tooltip>
                            <el-tooltip  effect="dark" content="删除" placement="top-start" :enterable="false">
                                <el-button type="primary" icon="el-icon-delete" size="mini" @click="removeMesById(scope.row.message_id)"></el-button>
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
        
        <el-dialog title="详情" :visible.sync="checkDialogVisible" width="35%">
      <div style="width:400px;margin:auto;">
        <el-form status-icon label-width="auto" :model="messageData">
          
          <el-form-item label="内容">
            <el-input type="textarea" disabled v-model="messageData.content"></el-input>
          </el-form-item>
          <el-form-item label="发送时间">
            <el-input type="textarea" disabled v-model="messageData.message_time"></el-input>
          </el-form-item>
          <el-form-item label="发送人">
            <el-input type="textarea" disabled v-model="messageData.sender_name"></el-input>
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
    //获取工作列表的参数对象
      queryInfo:{
          
          pagenum: 1,
          pagesize: 5,
          query: ''
      },
      workMessageList:[],
      total:0,
    }
  },
  created(){
      //调用获取发布的工作的API函数
     this.getWorkMessageList()
  },
  methods:{
    tableRowClassName({ row,rowIndex }) {
      // console.log(row);
      if (row.status === 0) {
        return "havenot-row";
      } else if (row.status === 1) {
        return "have-row";
      }
      return "";
    },

      //监听每页条数选项改变的事件
      handleSizeChange(newSize){
        //console.log(newSize)
        this.queryInfo.pagesize=newSize
        this.getWorkMessageList()
      },
      //监听页码改变的事件
      handleCurrentChange(newPage){
       // console.log(newPage)
        this.queryInfo.pagenum=newPage
        this.getWorkMessageList()
      },
  //获取工作列表的函数
  //   async getWorkList(){
  //      const {data:res}= await this.$http.get('URL',{params:this.queryInfo})
   //   }
   async getWorkMessageList(){
         const res = await axios.post(
        "http://localhost:5000/Message/FindReceiveMessage",
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
        this.workMessageList=res.data.messageItem;
        this.total=res.data.totalpage;
        this.pagesize=res.data.totalpage/res.data.pagenum;
        this.pagenum=res.data.pagenum;
      //  console.log(this.pagesize);
       // console.log(res);
      },
      async viewMesInfo(row){
        this.messageData = row;
        this.checkDialogVisible = true;
        const res = await axios.post(
        "http://localhost:5000/Message/ViewedMessage",
        {
          message_id: row.message_id
        },
        {
          withCredentials: true
        }
      );
      this.getWorkMessageList();
      console.log(res);
      },

      async findWorkMes(){
        const res=await axios.post('http://localhost:5000/Message/FindSendMessage',{
                    pagenum: this.queryInfo.pagenum,
                     pagesize: this.queryInfo.pagesize,
                    query: this.queryInfo.query
            },{withCredentials: true});
        if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
        }
        this.workMessageList=res.data.messageItem;
        this.total=res.data.totalpage;
        console.log(res);
    },


      async removeMesById(id){
        const confirmResulte=await this.$confirm('此操作将永久删除该工作消息, 是否继续?', '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        }).catch(err=>err)
        //console.log(confirmResulte);
        if(confirmResulte!=='confirm'){
          return this.$message.info("已取消删除");
        }
        const res = await axios.post(
        "http://localhost:5000/Message/DeleteMessage",
        {
          message_id: id
          
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
      this.getWorkMessageList();

      }
  }
}
</script>


<style scoped>

</style>

<style>

.el-table .havenot-row {
  background: oldlace;
}

.el-table .have-row {
  background: #f0f9eb;
}
</style>


<template>
  <div>
        <el-breadcrumb separator-class="el-icon-arrow-right" class="recruitment_breadcrumb">
            <el-breadcrumb-item :to="{ path: '/home' }">教师</el-breadcrumb-item>
            <el-breadcrumb-item>消息</el-breadcrumb-item>
            <el-breadcrumb-item>请假管理</el-breadcrumb-item>
        </el-breadcrumb>


        <el-card class="teacher_card" style="margin-top:20px;">
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

      <!--    <el-row>
                <el-col :span="7">
                    <el-input placeholder="请输入内容" v-model="queryInfo.query" class="input-with-select">
                        
                        <el-button slot="append" icon="el-icon-search" @click="getWorkList"></el-button>
                    </el-input>
                </el-col>
                <el-col :span="4"></el-col>
      </el-row>-->

      <!-- 列表区域 -->
      <el-table :data="workMessageList" v-loading="loading">
        <el-table-column label="#" type="index"></el-table-column>
        <el-table-column label="请假人" prop="student_name"></el-table-column>

        <el-table-column label="工作名称" prop="work_name"></el-table-column>

        <el-table-column label="请假时间" prop="leave_time"></el-table-column>
        <el-table-column label="状态" prop="status">
          <template slot-scope="scope">
            <el-tag type="success" v-if="scope.row.status==1">已同意</el-tag>
            <el-tag type="danger" v-if="scope.row.status==2">已拒绝</el-tag>
            <el-tag type="info" v-if="scope.row.status==0">未处理</el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作">
          <template v-slot:default="scope">
            <el-tooltip effect="dark" content="进行处理" placement="top-start" :enterable="false">
              <el-button
                type="primary"
                icon="el-icon-edit"
                size="mini"
                @click="viewMesInfo(scope.row)"
              ></el-button>
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
        style="margin-top:10px"
      ></el-pagination>
    </el-card>

    <el-dialog title="详情" :visible.sync="checkDialogVisible" width="35%">
      <div style="width:400px;margin:auto;">
        <el-form status-icon label-width="auto" :model="messageData">
          <el-form-item label="请假原因">
            <el-input disabled v-model="messageData.content"></el-input>
          </el-form-item>

          <el-form-item label="请假人">
            <el-input disabled v-model="messageData.student_name"></el-input>
          </el-form-item>
          <el-form-item label="请假工作">
            <el-input disabled v-model="messageData.work_name"></el-input>
          </el-form-item>
          <el-form-item label="证明">
            <el-input disabled v-model="messageData.proof"></el-input>
          </el-form-item>

          <div class="demo-drawer__footer" style="margin-top:10px">
            <el-tag type="success" v-if="messageData.status==1">已同意</el-tag>
            <el-tag type="danger" v-if="messageData.status==2">已拒绝</el-tag>
            <el-button
              type="info"
              icon="el-icon-star-on"
              plain
              @click="AgreeLea()"
              v-if="!messageData.status"
            >同意请假</el-button>
            <el-button
              type="info"
              icon="el-icon-star-on"
              plain
              @click="RefuseLea()"
              v-if="!messageData.status"
            >拒绝请假</el-button>
          </div>
        </el-form>
      </div>
    </el-dialog>
  </div>
</template>


<script>
import axios from "axios";
export default {
  data() {
    return {
      input1: "",
      input2: "",
      input3: "",
      select: "",
      checkDialogVisible: false,
      messageData: {},
      loading: true,
      //获取工作列表的参数对象
      queryInfo: {
        pagenum: 1,
        pagesize: 10,
        query: ""
      },
      workMessageList: [],
      total: 0
    };
  },
  created() {
    //调用获取发布的工作的API函数
    this.getWorkMessageList();
  },
  methods: {
    //监听每页条数选项改变的事件
    handleSizeChange(newSize) {
      //console.log(newSize)
      this.loading = true;
      this.queryInfo.pagesize = newSize;
      this.getWorkMessageList();
      // this.loading = false;
    },
    //监听页码改变的事件
    handleCurrentChange(newPage) {
      // console.log(newPage)
      this.loading = true;
      this.queryInfo.pagenum = newPage;
      this.getWorkMessageList();
      // this.loading = false;
    },
    //获取工作列表的函数
    //   async getWorkList(){
    //      const {data:res}= await this.$http.get('URL',{params:this.queryInfo})
    //   }
    async getWorkMessageList() {
      // this.loading=true;
      const res = await axios.post(
        "http://localhost:5000/Leave/ProViewLeaves",
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
      this.workMessageList = res.data.leavelist;
      this.total = res.data.total;
      this.pagesize = res.data.totalpage / res.data.pagenum;
      this.pagenum = res.data.pagenum;
      this.loading = false;
      //  console.log(this.pagesize);
      // console.log(res);
      // this.loading=false;
    },
    async viewMesInfo(row) {
      this.messageData = row;
      console.log(this.messageData);
      this.checkDialogVisible = true;
      const res = await axios.post(
        "http://localhost:5000/Leave/ProViewLeaves",
        {},
        {
          withCredentials: true
        }
      );
      this.getWorkMessageList();
      // console.log(res);
    },

    async AgreeLea() {
      //console.log(confirmResulte);

      const res = await axios.post(
        "http://localhost:5000/Leave/ProManageLeave",
        {
          leave_id: this.messageData.leave_id,
          status: 1
        },
        {
          withCredentials: true
        }
      );
      // console.log("aggr", this.messageData);
      if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
      }
      const result = await axios.post(
        "http://localhost:5000/Message/CreateMessage",
        {
          message_type: 0,
          content: this.messageData.work_name + "的请假已被接受",
          receiver_id: this.messageData.student_id
        },
        {
          withCredentials: true
        }
      );
      this.loading=true;
      //this.$message.info("已同意");
      this.$message({
          message: '已同意',
          type: 'info',
          duration:1000
        });
      this.checkDialogVisible = false;
      await this.getWorkMessageList();
      // console.log(res);
    },
    async RefuseLea() {
      //console.log(confirmResulte);

      const res = await axios.post(
        "http://localhost:5000/Leave/ProManageLeave",
        {
          leave_id: this.messageData.leave_id,
          status: 2
        },
        {
          withCredentials: true
        }
      );
      // console.log("refuse", this.messageData);
      if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
      }
      const result = await axios.post(
        "http://localhost:5000/Message/CreateMessage",
        {
          message_type: 0,
          content: this.messageData.work_name + "的请假已被拒绝",
          receiver_id: this.messageData.student_id
        },
        {
          withCredentials: true
        }
      );
      this.loading=true;
      //this.$message.info("已拒绝");
      this.$message({
          message: '已拒绝',
          type: 'info',
          duration:1000
        });
      this.checkDialogVisible = false;
      await this.getWorkMessageList();
      // console.log(res);
    }
  }
};
</script>


<style scoped>
</style>


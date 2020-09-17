<template>
  <div>
    这里是管理员系统通知 --已作废
    <el-card class="student_card">
      <!-- <el-row>
                <el-col :span="7">
                    <el-input placeholder="请输入内容" v-model="queryInfo.query" class="input-with-select">
                        
                        <el-button slot="append" icon="el-icon-search" @click="getSysMessageList"></el-button>
                    </el-input>
                </el-col>
                <el-col :span="4"></el-col>
      </el-row>-->

      <!-- 列表区域 -->
      <el-table :data="sysMessageList" :row-class-name="tableRowClassName" v-loading="loading">
        <el-table-column label="#" type="index"></el-table-column>
        <el-table-column prop="title" label="公告标题" width="280"></el-table-column>
        <el-table-column prop="send_time" label="发送时间" width="280"></el-table-column>
        <el-table-column prop="status" label="状态" width="280"></el-table-column>
        <!-- <el-table-column
                    prop="address"
                    label="地址">
        </el-table-column>-->
        <el-table-column label="操作">
          <template v-slot:default="scope">
            <el-tooltip effect="dark" content="查看详情" placement="top-start" :enterable="false">
              <el-button
                type="primary"
                icon="el-icon-edit"
                size="mini"
                @click="viewMesInfo(scope.row)"
                circle
              ></el-button>
            </el-tooltip>
            <el-tooltip effect="dark" content="删除" placement="top-start" :enterable="false">
              <el-button
                type="primary"
                icon="el-icon-delete"
                size="mini"
                @click="removeMesById(scope.row.announcement_id)"
                circle
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
      ></el-pagination>
    </el-card>

    <el-dialog title="详情" :visible.sync="checkDialogVisible" width="35%">
      <div style="width:400px;margin:auto;">
        <el-form status-icon label-width="auto" :model="messageData">
          <el-form-item label="内容">
            <el-input disabled v-model="messageData.content"></el-input>
          </el-form-item>
          <el-form-item label="发送时间">
            <el-input disabled v-model="messageData.send_time"></el-input>
          </el-form-item>
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
        query: "",
        pagenum: 1,
        pagesize: 5
      },
      sysMessageList: [],
      total: 0

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
    };
  },
  created() {
    //调用获取发布的工作的API函数
    this.getSysMessageList();
  },
  methods: {
    tableRowClassName({ row, rowIndex }) {
      // console.log(row);
      if (row.status === 0) {
        return "havenot-row";
      } else if (row.status === 1) {
        return "have-row";
      }
      return "";
    },
    //监听每页条数选项改变的事件
    handleSizeChange(newSize) {
      //console.log(newSize)
      this.loading = true;
      this.queryInfo.pagesize = newSize;
      this.getSysMessageList();
      this.loading = false;
    },
    //监听页码改变的事件
    handleCurrentChange(newPage) {
      // console.log(newPage)
      this.loading = true;
      this.queryInfo.pagenum = newPage;
      this.getSysMessageList();
      this.loading = false;
    },
    //获取工作列表的函数
    async getSysMessageList() {
      const res = await axios.post(
        this.$helper.endpointUrl("/Announce/GetAnnounce"),
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
      this.sysMessageList = res.data.announceItem;
      this.total = res.data.totalpage;
      this.pagesize = res.data.totalpage / res.data.pagenum;
      this.pagenum = res.data.pagenum;
      this.loading = false;
    },

    async removeMesById(id) {
      const confirmResulte = await this.$confirm(
        "此操作将永久删除该工作消息, 是否继续?",
        "提示",
        {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning"
        }
      ).catch(err => err);
      //console.log(confirmResulte);
      if (confirmResulte !== "confirm") {
        //return this.$message.info("已取消删除");
        return this.$message({
          message: '已取消删除',
          type: 'info',
          duration:1000
        });
      }
      const res = await axios.post(
        this.$helper.endpointUrl("/Announce/DeleteAnnounce"),
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
      //this.$message.info("已成功删除");
      this.$message({
          message: '已成功删除',
          type: 'info',
          duration:1000
        });

      this.getSysMessageList();
    },

    async viewMesInfo(row) {
      this.messageData = row;
      this.checkDialogVisible = true;
      const res = await axios.post(
        this.$helper.endpointUrl("/Announce/GetAnnounceContent"),
        {
          announcement_id: row.announcement_id
        },
        {
          withCredentials: true
        }
      );
    }
  }
};
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

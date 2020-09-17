<template>
  <div v-loading="loading">
    <el-card>
      <el-row>
        <el-col :span="6" style="width: 0;margin-bottom:10px">
          <div>
            <el-button type="primary" @click="dialogVisible=true">发布公告</el-button>
          </div>
        </el-col>
      </el-row>
      <template>
        <el-table :data="tableData" style="width: 100%">
          <el-table-column label="#" type="index" width="180px"></el-table-column>
          <el-table-column prop="title" label="标题" width="280px"></el-table-column>
          <el-table-column prop="send_time" label="时间" width="280px"></el-table-column>
          <!-- <el-table-column prop="edit" label="编辑" width="280px">
            <template>
              <el-tooltip effect="dark" content="编辑" placement="top">
                <el-button type="primary" icon="el-icon-edit" size="mini"></el-button>
              </el-tooltip>
              <el-tooltip effect="dark" content="删除" placement="top">
                <el-button type="danger" icon="el-icon-delete" size="mini"></el-button>
              </el-tooltip>
            </template>
          </el-table-column>-->
          <el-table-column label width="280px">
            <template slot-scope="scope">
              <el-button type="text" size="mini" @click="getMoreInfo(scope.row)">查看详情</el-button>
            </template>
          </el-table-column>
        </el-table>
      </template>
      <!-- 分页 -->
      <div class="block">
        <el-pagination
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange"
          :current-page="pageInfo.pagenum"
          :page-sizes="[5, 10,15,20]"
          :page-size="pageInfo.pagesize"
          layout="total, sizes, prev, pager, next, jumper"
          :total="this.total"
        ></el-pagination>
      </div>
    </el-card>

    <!-- 发布公告弹窗 -->
    <el-dialog title="发布公告" :visible.sync="dialogVisible" width="30%">
      <el-form :model="addForm" status-icon label-width="90px">
        <el-form-item label="标题">
          <el-input v-model="addForm.title" autocomplete="off"></el-input>
        </el-form-item>
        <el-form-item label="内容" autosize>
          <el-input type="textarea" v-model="addForm.content" autocomplete="off"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="dialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="addAnnounceClick()">确 定</el-button>
      </span>
    </el-dialog>

    <!-- 查看公告详情弹窗 -->
    <el-dialog title="查看详情" :visible.sync="dialogSystemMessageVisible" width="30%">
      <el-form :model="systemDataData" status-icon label-width="90px">
        <el-form-item label="标题">
          <el-input v-model="systemDataData.title" autocomplete="off" disabled></el-input>
        </el-form-item>
        <el-form-item label="内容" autosize>
          <el-input type="textarea" v-model="systemDataData.content" autocomplete="off" disabled></el-input>
        </el-form-item>
        <el-form-item label="时间" autosize>
          <el-input type="textarea" v-model="systemDataData.send_time" autocomplete="off" disabled></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button type="danger" @click="deleteMessage()">删除公告</el-button>
      </span>
    </el-dialog>
  </div>
</template>


<script>
import axios from "axios";
export default {
  data() {
    return {
      tableData: [],
      dialogVisible: false,
      dialogSystemMessageVisible: false,
      //表单数据
      addForm: {
        title: "",
        content: ""
      },
      //转圈圈
      loading: true,
      total: 0,
      pageInfo: {
        pagenum: 1,
        pagesize: 10
      },
      //查看详情的data
      systemDataData: {}
    };
  },
  methods: {
    async addAnnounceClick() {
      if (this.addForm.title == "" && this.addForm.content == "") {
        this.$message.warning("请输入公告标题与内容");
        return;
      } else if (this.addForm.title == "") {
        this.$message.warning("请输入公告标题");
        return;
      } else if (this.addForm.content == "") {
        this.$message.warning("请输入公告内容");
        return;
      }
      const result = await axios.post(
        this.$helper.endpointUrl("/Announce/CreateAnnounce"),
        this.addForm,
        { withCredentials: true }
      );
      console.log(result);
      if (result.status == 200) {
        this.$message.success("公告发布成功");
        this.dialogVisible = false;
        this.addForm = {};
        await this.getAllAnnounce();
      }
    },
    async getAllAnnounce() {
      this.loading = true;
      const result = await axios.post(
        this.$helper.endpointUrl("/Announce/GetSendAnnounce"),
        this.pageInfo,
        { withCredentials: true }
      );
      console.log(result);
      if (result.status == 200) {
        this.tableData = result.data.announceItem;
        this.total = result.data.totalpage;
        this.loading = false;
      }
    },
    async handleSizeChange(val) {
      // console.log(`每页 ${val} 条`);
      this.loading = true;
      this.pageInfo.pagesize = val;
      await this.getAllAnnounce();
      this.loading = false;
    },
    async handleCurrentChange(val) {
      this.loading = true;
      this.pageInfo.pagenum = val;
      // console.log(`当前页: ${val}`);
      await this.getAllAnnounce();
      this.loading = false;
    },
    getMoreInfo(row) {
      // console.log(row);
      this.systemDataData = row;
      this.dialogSystemMessageVisible = true;
    },
    async deleteMessage() {
      // console.log(this.systemDataData.announcement_id);
      const result = await axios.post(
        this.$helper.endpointUrl("/Announce/DeleteAnnounceAll"),
        {
          announcement_id: this.systemDataData.announcement_id
        },
        {
          withCredentials: true
        }
      );
      // console.log(result);
      if (result.status == 200) {
        this.$message.success("删除成功");
        this.dialogSystemMessageVisible = false;
        await this.getAllAnnounce();
      }
    }
  },

  async mounted() {
    await this.getAllAnnounce();
  }
};
</script>


<style scoped>
.el-card {
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.15) !important;
}
</style>

<template>
  <div>
    <!-- <el-breadcrumb
      separator-class="el-icon-arrow-right"
      style="margin-bottom: 15px;font-size: 12px;"
    >
      <el-breadcrumb-item>首页</el-breadcrumb-item>
      <el-breadcrumb-item>工作台</el-breadcrumb-item>
      <el-breadcrumb-item>查看收到的简历</el-breadcrumb-item>
    </el-breadcrumb>-->

    <el-card>
      <template>
        <el-table :data="tableData" style="width: 100%">
          <el-table-column label="#" type="index" width="180px"></el-table-column>
          <el-table-column prop="work_name" label="工作名称" width="280px"></el-table-column>
          <el-table-column prop="student_name" label="学生姓名" width="280px"></el-table-column>
          <el-table-column prop="edit" label="状态" width="280px">
            <template slot-scope="scope">
              <!-- <el-tooltip effect="dark" content="查看" placement="top"> -->
              <el-button type="text" size="mini" @click="getMoreInfo(scope.row)">查看详情</el-button>
              <!-- </el-tooltip> -->
            </template>
          </el-table-column>
        </el-table>
      </template>
      <!-- <template>
        <el-table :data="tableData" style="width: 100%">
          <el-table-column type="expand">
            <template slot-scope="props">
              <el-form label-position="left" inline class="demo-table-expand">
                <el-form-item label="商品名称">
                  <span>{{ props.row.name }}</span>
                </el-form-item>
                <el-form-item label="所属店铺">
                  <span>{{ props.row.shop }}</span>
                </el-form-item>
                <el-form-item label="商品 ID">
                  <span>{{ props.row.id }}</span>
                </el-form-item>
                <el-form-item label="店铺 ID">
                  <span>{{ props.row.shopId }}</span>
                </el-form-item>
                <el-form-item label="商品分类">
                  <span>{{ props.row.category }}</span>
                </el-form-item>
                <el-form-item label="店铺地址">
                  <span>{{ props.row.address }}</span>
                </el-form-item>
                <el-form-item label="商品描述">
                  <span>{{ props.row.desc }}</span>
                </el-form-item>
              </el-form>
            </template>
          </el-table-column>
          <el-table-column label="#" type="index" width="180px"></el-table-column>
          <el-table-column label="工作名称" prop="work_name" width="280px"></el-table-column>
          <el-table-column label="学生姓名" prop="student_name" width="280px"></el-table-column>
        </el-table>
      </template>-->
    </el-card>

    <el-dialog title="详情" :visible.sync="checkDialogVisible" width="30%" center>
      <!-- <span>这是一段信息</span> -->
      <el-form status-icon label-width="100px">
        <el-form-item label="姓名" prop>
          <el-input disabled style="width:300px;height:500px">范德萨打发但是幅度萨芬的撒阿飞的萨芬的萨芬的撒f'd's</el-input>
        </el-form-item>
        <el-form-item label="城市" prop>
          <el-input disabled style="width:300px"></el-input>
        </el-form-item>
        <el-form-item label="学历" prop>
          <el-input disabled style="width:300px"></el-input>
        </el-form-item>
        <el-form-item label="简介" prop>
          <el-input disabled style="width:300px"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button type="primary" @click="checkDialogVisible = false">确 定</el-button>
        <el-button @click="checkDialogVisible = false">取 消</el-button>
      </span>
    </el-dialog>

    <!-- <el-button type="primary" @click="login()">登录</el-button> -->
  </div>
</template>


<script>
import axios from "axios";
// axios.defaults.headers.get["Content-Type"] = "application/json;charset=UTF-8";
export default {
  data() {
    return {
      tableData: [],
      checkDialogVisible: false
    };
  },
  methods: {
    getMoreInfo(row) {
      console.log(row.name);
      this.editForm = row;
      this.checkDialogVisible = true;
    },
    editClickTrue() {
      this.checkDialogVisible = false;
      this.$message.success("成功");
    },

    // async login() {
    //   const result = await axios.post(
    //     "http://localhost:27682/Account/Login",
    //     {
    //       username: "tn@qq.com",
    //       Password:
    //         "ba3253876aed6bc22d4a6ff53d8406c6ad864195ed144ab5c87621b6c233b548baeae6956df346ec8c17f5ea10f35ee3cbc514797ed7ddd3145464e2a0bab413"
    //     },
    //     { withCredentials: true }
    //   );
    //   console.log(result);
    // },

    async getResume(resumeId) {
      const result = await axios.post(
        "http://localhost:27682/Resume/GetResumeInfo",
        {
          resume_id: 51
        }
      );
      return result;
    },

    async getReceivedResume() {
      const result = await axios.get(
        "http://localhost:27682/Apply/ProViewApps",
        {
          withCredentials: true
        }
      );
      this.tableData = result.data.applist;
      for (var i = 0; i < this.tableData.length; i++) {
        console.log(this.tableData[i].resume_id);
        const resume = await this.getResume(this.tableData[i].resume_id);
        this.tableData[i]["city"] = resume.data.city;
        console.log(this.tableData[i]);
      }
    }
  },

  async mounted() {
    // await this.getReceivedResume();
  }
};
</script>


<style scoped>
.el-card {
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.15) !important;
}
.demo-table-expand {
  font-size: 0;
}
.demo-table-expand label {
  width: 90px;
  color: #99a9bf;
}
.demo-table-expand .el-form-item {
  margin-right: 0;
  margin-bottom: 0;
  width: 50%;
}
</style>

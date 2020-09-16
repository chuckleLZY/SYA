<template>
  <div>
      
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{ path: '/home' }">教师</el-breadcrumb-item>
      <el-breadcrumb-item>工作台</el-breadcrumb-item>
      <el-breadcrumb-item>查看申请</el-breadcrumb-item>
    </el-breadcrumb>
    <el-card style="margin-top:20px;width:100%">
      <el-table :data="tableData" style="width: 100%" v-loading="loading">
        <el-table-column label="#" type="index" width="180px"></el-table-column>
        <el-table-column prop="work_name" label="工作名称" width="280px"></el-table-column>
        <el-table-column prop="student_name" label="学生姓名" width="280px"></el-table-column>
        <el-table-column prop="edit" label="状态" width="280px">
          <template slot-scope="scope">
            <el-tag type="success" v-if="scope.row.status==1">已接受</el-tag>
            <el-tag type="danger" v-if="scope.row.status==2">已拒绝</el-tag>
            <el-tag type="info" v-if="scope.row.status==0">待操作</el-tag>
          </template>
        </el-table-column>
        <el-table-column label width="280px">
          <template slot-scope="scope">
            <el-button type="text" size="mini" @click="getMoreInfo(scope.row)">查看详情</el-button>
          </template>
        </el-table-column>
      </el-table>
      <!-- 分页 -->
      <div class="block">
        <el-pagination
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange"
          :current-page="pageInfo.pagenum"
          :page-sizes="[1, 2, 5, 10]"
          :page-size="pageInfo.pagesize"
          layout="total, sizes, prev, pager, next, jumper"
          :total="this.total"
        ></el-pagination>
      </div>
    </el-card>

    <el-dialog title="详情" :visible.sync="checkDialogVisible" width="35%">
      <div style="width:400px;margin:auto;">
        <el-form status-icon label-width="auto" :model="reSumeData">
          <div style="width:300px;margin:auto;">
            <el-form-item label="姓名">
              <el-input disabled v-model="reSumeData.student_name"></el-input>
            </el-form-item>
          </div>
          <div style="width:300px;margin:auto;">
            <el-form-item label="年龄">
              <el-input disabled v-model="reSumeData.age"></el-input>
            </el-form-item>
          </div>
          <div style="width:300px;margin:auto;">
            <el-form-item label="城市">
              <el-input disabled v-model="reSumeData.city"></el-input>
            </el-form-item>
          </div>
          <div style="width:300px;margin:auto;">
            <el-form-item label="学历">
              <el-input disabled v-model="reSumeData.education"></el-input>
            </el-form-item>
          </div>
          <el-form-item label="技能" autosize>
            <el-input type="textarea" disabled v-model="reSumeData.skill"></el-input>
          </el-form-item>
          <el-form-item label="简介" autosize>
            <el-input type="textarea" disabled v-model="reSumeData.introduction"></el-input>
          </el-form-item>
          <el-form-item label="社区经历" autosize>
            <el-input type="textarea" disabled v-model="reSumeData.community"></el-input>
          </el-form-item>
          <el-form-item label="项目经历" autosize>
            <el-input type="textarea" disabled v-model="reSumeData.project"></el-input>
          </el-form-item>
          <el-form-item label="学术经历" autosize>
            <el-input type="textarea" disabled v-model="reSumeData.academic"></el-input>
          </el-form-item>
        </el-form>
        <span slot="footer" class="dialog-footer">
          <el-tag type="success" v-if="reSumeData.status==1">已接受</el-tag>
          <el-tag type="danger" v-if="reSumeData.status==2">已拒绝</el-tag>
          <el-button type="primary" @click="accept()" v-if="!reSumeData.status">接 受</el-button>
          <el-button @click="refuse()" v-if="!reSumeData.status">拒 绝</el-button>
        </span>
      </div>
    </el-dialog>
  </div>
</template>


<script>
import axios from "axios";
import { thistle } from "color-name";
export default {
  data() {
    return {
      tableData: [],
      checkDialogVisible: false,
      reSumeData: {},
      loading: true,
      total: 0,
      pageInfo: {
        pagenum: 1,
        pagesize: 10
      }
    };
  },
  methods: {
    //接收申请
    async accept() {
      this.checkDialogVisible = false;
      console.log("resumeData",this.reSumeData)
      const result = await axios.post(
        this.$appconfig.toAbsUrl("/Apply/ProManageApp"),
        {
          apply_id: this.reSumeData.apply_id,
          status: 1 //同意
        },
        {
          withCredentials: true
        }
      );
      // console.log(result);
      // console.log(this.reSumeData);

      //发消息
      const res = await axios.post(
        this.$appconfig.toAbsUrl("/Message/CreateMessage"),
        {
          message_type: 0,
          content: "Congratulations!"+this.reSumeData.teacher_name+" 接受了 "+this.reSumeData.work_name+" 的工作申请",
          receiver_id: this.reSumeData.student_id
        },
        {
          withCredentials: true
        }
      );
      console.log(result);

      //刷新
      this.$message.success("接受成功");
      this.loading = true;
      await this.getReceivedResume();
    },
    //拒绝申请
    async refuse() {
      this.checkDialogVisible = false;
      this.checkDialogVisible = false;
      const result = await axios.post(
        this.$appconfig.toAbsUrl("/Apply/ProManageApp"),
        {
          apply_id: this.reSumeData.apply_id,
          status: 2 //拒绝
        },
        {
          withCredentials: true
        }
      );
      // console.log(result);
      // console.log(this.reSumeData);

      //发消息
      const res = await axios.post(
        this.$appconfig.toAbsUrl("/Message/CreateMessage"),
        {
          message_type: 0,
          content: this.reSumeData.teacher_name+" 拒绝了 "+this.reSumeData.work_name+" 的工作申请",
          receiver_id: this.reSumeData.student_id
        },
        {
          withCredentials: true
        }
      );
      // console.log(result);
      //刷新
      this.$message.success("拒绝成功");
      this.loading = true;
      await this.getReceivedResume();
    },
    //查看简历详情
    getMoreInfo(row) {
      // console.log("row",row);
      this.reSumeData = row;
      this.checkDialogVisible = true;
    },
    //获得简历信息
    async getResume(resumeId) {
      const result = await axios.post(
        this.$appconfig.toAbsUrl("/Resume/GetResumeInfo"),
        {
          resume_id: resumeId
        },
        {
          withCredentials: true
        }
      );
      return result;
    },
    //获得所有申请信息
    async getReceivedResume() {
      const result = await axios.post(
        this.$appconfig.toAbsUrl("/Apply/ProViewApps"),
        this.pageInfo,
        {
          withCredentials: true
        }
      );
      this.total = result.data.totalpage;
      // console.log(result);
      this.tableData = result.data.applist;
      for (var i = 0; i < this.tableData.length; i++) {
        // console.log(this.tableData[i].resume_id);
        const resume = await this.getResume(this.tableData[i].resume_id);
        this.tableData[i]["student_name"] = resume.data.student_name;
        this.tableData[i]["age"] = resume.data.age;
        this.tableData[i]["city"] = resume.data.city;
        this.tableData[i]["education"] = resume.data.education;
        this.tableData[i]["community"] = resume.data.community;
        this.tableData[i]["project"] = resume.data.project;
        this.tableData[i]["academic"] = resume.data.academic;
        this.tableData[i]["skill"] = resume.data.skill;
        this.tableData[i]["introduction"] = resume.data.introduction;
        this.tableData[i]["student_id"]=resume.data.student_id;
        // console.log(this.tableData[i]);
      }
      // console.log("table",this.tableData);

      //取消加载的转圈圈
      this.loading = false;
    },
    //获取一页的申请信息
    async getOnePageReceivedResume() {
      const result = await axios.post(
        this.$appconfig.toAbsUrl("/Apply/ProViewApps"),
        this.pageInfo,
        {
          withCredentials: true
        }
      );
      console.log(result);
      this.tableData = result.data.applist;
      for (var i = 0; i < this.tableData.length; i++) {
        // console.log(this.tableData[i].resume_id);
        const resume = await this.getResume(this.tableData[i].resume_id);
        this.tableData[i]["student_name"] = resume.data.student_name;
        this.tableData[i]["age"] = resume.data.age;
        this.tableData[i]["city"] = resume.data.city;
        this.tableData[i]["education"] = resume.data.education;
        this.tableData[i]["community"] = resume.data.community;
        this.tableData[i]["project"] = resume.data.project;
        this.tableData[i]["academic"] = resume.data.academic;
        this.tableData[i]["skill"] = resume.data.skill;
        this.tableData[i]["introduction"] = resume.data.introduction;
        // console.log(this.tableData[i]);
      }
      //取消加载的转圈圈
      this.loading = false;
    },
    // //每一行的颜色
    // tableRowClassName({ row, rowIndex }) {
    //   // console.log(row);
    //   if (row.status === 2) {
    //     return "warning-row";
    //   } else if (row.status === 1) {
    //     return "success-row";
    //   }
    //   return "";
    // },
    async handleSizeChange(val) {
      this.loading = true;
      this.pageInfo.pagesize = val;
      await this.getOnePageReceivedResume();
      // console.log(`每页 ${val} 条`);
      this.loading = false;
    },
    async handleCurrentChange(val) {
      this.loading = true;
      this.pageInfo.pagenum = val;
      await this.getOnePageReceivedResume();
      // console.log(`当前页: ${val}`);
      this.loading = false;
    }
  },

  async mounted() {
    await this.getReceivedResume();
    // console.log(this.tableData);
  }
};
</script>


<style>
/* .el-card {
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.15) !important;
} */
/* .demo-table-expand {
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
} */
/* .el-table .warning-row {
  background: oldlace;
}

.el-table .success-row {
  background: #f0f9eb;
} */
</style>

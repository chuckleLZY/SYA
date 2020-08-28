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
      <el-table
        :data="tableData"
        style="width: 100%"
        v-loading="loading"
        :row-class-name="tableRowClassName"
      >
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
      <el-form status-icon label-width="100px" :model="reSumeData">
        <el-form-item label="姓名">
          <el-input disabled style="width:300px" v-model="reSumeData.student_name"></el-input>
        </el-form-item>
        <el-form-item label="年龄">
          <el-input disabled style="width:300px" v-model="reSumeData.age"></el-input>
        </el-form-item>
        <el-form-item label="城市">
          <el-input disabled style="width:300px" v-model="reSumeData.city"></el-input>
        </el-form-item>
        <el-form-item label="学历">
          <el-input disabled style="width:300px" v-model="reSumeData.education"></el-input>
        </el-form-item>
        <el-form-item label="community">
          <el-input disabled style="width:300px" v-model="reSumeData.community"></el-input>
        </el-form-item>
        <el-form-item label="project">
          <el-input disabled style="width:300px" v-model="reSumeData.project"></el-input>
        </el-form-item>
        <el-form-item label="academic">
          <el-input disabled style="width:300px" v-model="reSumeData.academic"></el-input>
        </el-form-item>
        <el-form-item label="skill">
          <el-input disabled style="width:300px" v-model="reSumeData.skill"></el-input>
        </el-form-item>
        <el-form-item label="简介">
          <el-input disabled style="width:300px" v-model="reSumeData.introduction"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-tag type="success" v-if="reSumeData.status==1">已同意</el-tag>
        <el-tag type="danger" v-if="reSumeData.status==2">已拒绝</el-tag>
        <el-button type="primary" @click="accept()" v-if="!reSumeData.status">同 意</el-button>
        <el-button @click="refuse()" v-if="!reSumeData.status">拒绝</el-button>
      </span>
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
      loading: true
    };
  },
  methods: {
    //接收申请
    async accept() {
      this.checkDialogVisible = false;
      const result = await axios.post(
        "http://localhost:5000/Apply/ProManageApp",
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

      //刷新
      this.loading = true;
      await this.getReceivedResume();
    },
    //拒绝申请
    async refuse() {
      this.checkDialogVisible = false;
      this.checkDialogVisible = false;
      const result = await axios.post(
        "http://localhost:5000/Apply/ProManageApp",
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

      //刷新
      this.loading = true;
      await this.getReceivedResume();
    },
    //查看简历详情
    getMoreInfo(row) {
      // console.log(row);
      this.reSumeData = row;
      this.checkDialogVisible = true;
    },
    //获得简历信息
    async getResume(resumeId) {
      const result = await axios.post(
        "http://localhost:5000/Resume/GetResumeInfo",
        {
          resume_id: resumeId
        },
        {
          withCredentials: true
        }
      );
      return result;
    },

    //resume
    // {
    //     "student_id": 3,
    //     "age": 1,
    //     "student_name": "lzy",
    //     "city": "shanghai",
    //     "education": "none",
    //     "community": "none",
    //     "project": "none",
    //     "academic": "none",
    //     "skill": "none",
    //     "introduction": "none"
    // }

    //获得申请信息
    async getReceivedResume() {
      const result = await axios.post(
        "http://localhost:5000/Apply/ProViewApps",{},
        {
          withCredentials: true
        }
      );
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

    //每一行的颜色
    tableRowClassName({ row, rowIndex }) {
      // console.log(row);
      if (row.status === 2) {
        return "warning-row";
      } else if (row.status === 1) {
        return "success-row";
      }
      return "";
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
.el-table .warning-row {
  background: oldlace;
}

.el-table .success-row {
  background: #f0f9eb;
}
</style>

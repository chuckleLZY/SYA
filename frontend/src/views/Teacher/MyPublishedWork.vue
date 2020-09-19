<template>
  <div>
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{ path: '/home' }">教师</el-breadcrumb-item>
      <el-breadcrumb-item>工作台</el-breadcrumb-item>
      <el-breadcrumb-item>发布记录</el-breadcrumb-item>
    </el-breadcrumb>
    <el-card style="margin-top:20px;width: 98%" v-loading="loading">
      <el-table :data="tableData" style="width: 100%" stripe>
        <el-table-column type="expand">
          <template slot-scope="props">
            <el-form
              label-position="left"
              inline
              class="demo-table-expand"
              style="margin-left:240px;"
            >
              <el-form-item label="工作名称">
                <span>{{ props.row.work_name }}</span>
              </el-form-item>
              <el-form-item label="工作地点">
                <span>{{ props.row.address }}</span>
              </el-form-item>
              <el-form-item label="工薪薪资">
                <span>{{ props.row.salary }}</span>
              </el-form-item>
              <el-form-item label="工作日">
                <span>{{ props.row.week_day }}</span>
              </el-form-item>
              <el-form-item label="开始日期">
                <span>{{ props.row.start_day }}</span>
              </el-form-item>
              <el-form-item label="结束日期">
                <span>{{ props.row.end_day }}</span>
              </el-form-item>
              <el-form-item label="开始时间">
                <span>{{ props.row.start_time }}</span>
              </el-form-item>
              <el-form-item label="结束时间">
                <span>{{ props.row.end_time }}</span>
              </el-form-item>
              <el-form-item label="工作描述">
                <span>{{ props.row.work_description }}</span>
              </el-form-item>
            </el-form>
          </template>
        </el-table-column>
        <el-table-column label="#" type="index"></el-table-column>
        <el-table-column label="工作名称" prop="work_name"></el-table-column>
        <el-table-column prop="likes_num" label="点赞"></el-table-column>
        <el-table-column prop="collect_num" label="收藏"></el-table-column>
        <!-- <el-table-column prop="share_num" label="分享" width="160"></el-table-column> -->
        <el-table-column fixed="right" label="操作">
          <template slot-scope="scope">
            <el-button
              @click="editInfo(scope.row)"
              type="success"
              icon="el-icon-edit"
              size="medium"
              >编辑</el-button
            >
            <el-button
              @click="deleteWork(scope.row.work_id)"
              type="danger"
              icon="el-icon-delete"
              size="medium"
            >
              删除
            </el-button>
          </template>
        </el-table-column>
      </el-table>

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

    <el-dialog title="修改" :visible.sync="DialogVisible" width="50%" center>
      <el-form
        :model="editdata"
        :rules="formrules2"
        style="width:500px;margin:auto;"
        label-position="left"
        ref="thisform"
      >
        <el-form-item label="工作名称" prop="name">
          <el-input
            v-model="editdata.work_name"
            maxlength="15"
            show-word-limit
          ></el-input>
        </el-form-item>
        <el-form-item label="工作图像" required> </el-form-item>
        <!-- 上传 -->
        <el-tooltip
          class="item"
          effect="dark"
          content="点击上传需要替换的工作封面"
          placement="top"
        >
          <el-upload
            class="avatar-uploader"
            action
            :show-file-list="false"
            :http-request="Upload"
            :before-upload="beforeAvatarUpload"
          >
            <img v-if="uploadImg" :src="uploadImg" class="avatar" />
            <i v-else class="el-icon-plus avatar-uploader-icon"></i>
          </el-upload>
        </el-tooltip>
        <el-form-item label="工作地点" prop="new_address">
          <el-input width="300px" v-model="editdata.address"></el-input>
        </el-form-item>
        <el-form-item label="工作薪资" prop="salary">
          <el-input v-model="editdata.salary"></el-input>
        </el-form-item>
        <el-form-item label="工作日期" required>
          <el-col :span="9">
            <el-form-item prop="start_day">
              <el-date-picker
                v-model="editdata.start_day"
                type="date"
                placeholder="开始日期"
                format="yyyy-MM-dd"
                value-format="yyyy-MM-dd"
                style="width: 100%;"
                :picker-options="pickerOptions2"
              ></el-date-picker>
            </el-form-item>
          </el-col>
          <el-col class="line" :span="2">-</el-col>
          <el-col :span="9">
            <el-form-item prop="end_day">
              <el-date-picker
                v-model="editdata.end_day"
                type="date"
                placeholder="结束日期"
                format="yyyy-MM-dd"
                value-format="yyyy-MM-dd"
                style="width: 100%;"
                :picker-options="pickerOptions3"
              ></el-date-picker>
            </el-form-item>
          </el-col>
        </el-form-item>
        <el-form-item label="工作时间" required>
          <el-col :span="9">
            <el-form-item prop="start_time">
              <el-time-select
                placeholder="起始时间"
                v-model="editdata.start_time"
                :picker-options="{
                  start: '08:00',
                  step: '01:00',
                  end: '20:00',
                }"
                style="width: 100%;"
              ></el-time-select>
            </el-form-item>
          </el-col>
          <el-col class="line" :span="2">-</el-col>
          <el-col :span="9">
            <el-form-item prop="end_time">
              <el-time-select
                placeholder="结束时间"
                v-model="editdata.end_time"
                :picker-options="{
                  start: '08:00',
                  step: '01:00',
                  end: '21:00',
                  minTime: editdata.start_time,
                }"
                style="width: 100%;"
              ></el-time-select>
            </el-form-item>
          </el-col>
        </el-form-item>
        <el-form-item label="工作日" prop="week_day">
          <el-select v-model="editdata.week_day" placeholder="请选择工作日">
            <el-option label="星期一" value="1"></el-option>
            <el-option label="星期二" value="2"></el-option>
            <el-option label="星期三" value="3"></el-option>
            <el-option label="星期四" value="4"></el-option>
            <el-option label="星期五" value="5"></el-option>
            <el-option label="星期六" value="6"></el-option>
            <el-option label="星期日" value="7"></el-option>
          </el-select>
        </el-form-item>
        <!-- <el-form-item label="工作薪资（元/天）" prop="new_salary">
          <el-input v-model="editdata.new_salary"></el-input>
        </el-form-item>-->
        <el-form-item label="工作描述" prop="work_description">
          <el-input
            type="textarea"
            v-model="editdata.work_description"
            maxlength="100"
            show-word-limit
            :autosize="{ minRows: 3, maxRows: 6 }"
          ></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="DialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="Editework('editdate')"
          >确 认</el-button
        >
      </div>
    </el-dialog>
  </div>
</template>

<script>
import axios from "axios";
import { client } from "../../oss.js";
export default {
  data() {
    return {
      loading: true,
      uploadImg: "",
      DialogVisible: false,
      pageInfo: {
        pagenum: 1,
        pagesize: 10,
      },
      total: 0,
      editdata: {
        work_id: "",
        work_name: "",
        cover: "",
        work_description: "",
        address: "",
        salary: "",
        start_day: "",
        end_day: "",
        start_time: "",
        end_time: "",
        week_day: "",
      },
      tableData: [],
      form: {
        new_name: "",
        new_cover:
          "http://photo.tongji.edu.cn/__local/8/E4/D1/653D3735DC6F0691C6B2C1D4089_8C3254E4_59616.jpg",
        new_address: "",
        new_start_day: "",
        new_end_day: "",
        new_start_time: "",
        new_end_time: "",
        new_week_day: "",
        new_time: "",
        new_salary: "",
        new_desc: "",
      },
      formrules2: {
        start_day: [
          {
            type: "string",
            required: true,
            message: "请选择工作开始日期",
            trigger: "change",
          },
        ],
        end_day: [
          {
            type: "string",
            required: true,
            message: "请选择工作结束日期",
            trigger: "change",
          },
        ],
        start_time: [
          {
            type: "string",
            required: true,
            message: "请选择工作开始时间",
            trigger: "change",
          },
        ],
        end_time: [
          {
            type: "string",
            required: true,
            message: "请选择工作结束时间",
            trigger: "change",
          },
        ],
        week_day: [
          { required: true, message: "请选择工作日", trigger: "change" },
        ],
        address: [
          { required: true, message: "请填写工作地址", trigger: "change" },
        ],
        salary: [
          { required: true, message: "请填写工作薪资", trigger: "change" },
        ],
        work_description: [
          { required: true, message: "请填写工作描述", trigger: "blur" },
        ],
      },
      //设置选择日期
      pickerOptions2: {
        //结束时间不能大于开始时间
        disabledDate: (time) => {
          if (this.editdata.new_end_day) {
            return (
              time.getTime() > new Date(this.editdata.new_end_day).getTime() ||
              time.getTime() < Date.now()
            );
          } else {
            //没有选择结束时间，只能选择今天之后的时间不包括今天
            return time.getTime() < Date.now();
          }
        },
      },
      pickerOptions3: {
        //结束时间小于开始时间
        disabledDate: (time) => {
          if (this.editdata.new_start_day) {
            return (
              time.getTime() < new Date(this.editdata.new_start_day).getTime()
            );
          } else {
            //若未输入开始时间则默认为今天后时间不包括今天
            return time.getTime() < Date.now();
          }
        },
      },
    };
  },
  methods: {
    async getWorkList() {
      const result = await axios.post(
        this.$helper.endpointUrl("/Work/ViewHistoryWork"),
        this.pageInfo,
        {
          withCredentials: true,
        }
      );
      this.total = result.data.totalpage;
      // console.log(result);
      this.tableData = result.data.worklist;

      //取消加载的转圈圈
      this.loading = false;
    },

    //获取一页的申请信息
    async getOnePageworklist() {
      this.Dialogvisible = false;
      this.loading = true;
      const result = await axios.post(
        this.$helper.endpointUrl("/Work/ViewHistoryWork"),
        this.pageInfo,
        {
          withCredentials: true,
        }
      );
      // console.log(result);
      this.tableData = result.data.worklist;

      //取消加载的转圈圈
      this.loading = false;
    },

    //编辑信息
    editInfo(row) {
      console.log(this.editdata);
      this.editdata.work_id = row.work_id;
      this.editdata.work_name = row.work_name;
      this.editdata.cover = row.cover;
      this.editdata.work_description = row.work_description;
      this.editdata.address = row.address;
      this.editdata.salary = row.salary;
      this.editdata.start_day = row.start_day;
      this.editdata.end_day = row.end_day;
      this.editdata.start_time = row.start_time;
      this.editdata.end_time = row.end_time;
      if (row.week_day == 1) {
        this.editdata.week_day = "星期一";
      }
      if (row.week_day == 2) {
        this.editdata.week_day = "星期二";
      }
      if (row.week_day == 3) {
        this.editdata.week_day = "星期三";
      }
      if (row.week_day == 4) {
        this.editdata.week_day = "星期四";
      }
      if (row.week_day == 5) {
        this.editdata.week_day = "星期五";
      }
      if (row.week_day == 6) {
        this.editdata.week_day = "星期六";
      }
      if (row.week_day == 7) {
        this.editdata.week_day = "星期日";
      }
      this.DialogVisible = true;
    },
    //删除工作
    async deleteWork(id) {
      //console.log(id);
      const confirmResulte = await this.$confirm(
        "此操作将永久删除该工作, 是否继续?",
        "提示",
        {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning",
        }
      ).catch((err) => err);

      if (confirmResulte !== "confirm") {
        // return this.$message.info("已取消删除");
        return this.$message({
          message: "已取消删除",
          type: "info",
          duration: 1000,
        });
      }

      const result = await axios.post(
        this.$helper.endpointUrl("/Work/DeleteWork"),
        { work_id: id },
        {
          withCredentials: true,
        }
      );
      console.log(result);

      if (result.data == -1) {
        this.$message.error("删除失败");
      }
      this.$message({
        message: "已成功删除",
        type: "info",
        duration: 1000,
      });
      this.getWorkList();
    },

    // 编辑工作
    async Editework() {
      if (this.editdata.week_day == "星期一") {
        this.editdata.week_day = 1;
      }
      if (this.editdata.week_day == "星期二") {
        this.editdata.week_day = 2;
      }
      if (this.editdata.week_day == "星期三") {
        this.editdata.week_day = 3;
      }
      if (this.editdata.week_day == "星期四") {
        this.editdata.week_day = 4;
      }
      if (this.editdata.week_day == "星期五") {
        this.editdata.week_day = 5;
      }
      if (this.editdata.week_day == "星期六") {
        this.editdata.week_day = 6;
      }
      if (this.editdata.week_day == "星期日") {
        this.editdata.week_day = 7;
      }
      this.editdata.week_day = parseInt(this.editdata.week_day);
      console.log(this.editdata);
      this.$refs.thisform.validate(async (valid) => {
        if (!valid) {
          this.$message.error("请按照提示正确填写工作内容");
          return;
        }
        // console.log(this.form);
        const result = await axios.post(
          this.$helper.endpointUrl("/Work/ChangeWorkInfo"),
          this.editdata,
          { withCredentials: true }
        );
        // console.log(result);
        if (result.status == 200) {
          //console.log("edit success");
          this.$message.success("修改成功");
          this.DialogVisible = false;
          //console.log("edit success222");
          this.getOnePageworklist();
          //console.log("edit success333");
          /*setTimeout(() => {
           this.$router.go(0);
          }, 500);
          this.$router.push("MyPublishedWork");*/
        } else {
          this.$message.error("发生了一些错误");
        }
      });
    },

    async handleSizeChange(val) {
      this.loading = true;
      this.pageInfo.pagesize = val;
      await this.getOnePageworklist();
      // console.log(`每页 ${val} 条`);
      this.loading = false;
    },
    async handleCurrentChange(val) {
      this.loading = true;
      this.pageInfo.pagenum = val;
      await this.getOnePageworklist();
      // console.log(`当前页: ${val}`);
      this.loading = false;
    },
    Upload(file) {
      var fileName = "banner" + `${Date.parse(new Date())}` + ".jpg"; //定义唯一的文件名
      //定义唯一的文件名，打印出来的uid其实就是时间戳
      client()
        .multipartUpload(fileName, file.file)
        .then((result) => {
          // console.log(result);
          this.uploadImg = this.$helper.ossFileUrl(result.name);
        });
    },
    // 图片限制
    beforeAvatarUpload(file) {
      const isJPEG = file.name.split(".")[1] === "jpeg";
      const isJPG = file.name.split(".")[1] === "jpg";
      const isPNG = file.name.split(".")[1] === "png";
      const isLt1000K = file.size / 1024 / 1000 < 2;
      if (!isJPG && !isJPEG && !isPNG) {
        this.$message.error("上传图片只能是 JPEG/JPG/PNG 格式!");
      }
      if (!isLt1000K) {
        this.$message.error("单张图片大小不能超过 1000KB!");
      }

      return (isJPEG || isJPG || isPNG) && isLt1000K;
    },
  },
  async mounted() {
    await this.getWorkList();
    // console.log(this.tableData);
  },
};
</script>

<style scoped>
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
/* 上传 */
.avatar-uploader .el-upload {
  border: 10px dashed #af2a2a;
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
}
.avatar-uploader .el-upload:hover {
  border-color: #409eff;
}
.avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 178px;
  height: 178px;
  line-height: 178px;
  text-align: center;
}
.avatar {
  width: 400px;
  height: 239px;
  display: block;
}
</style>

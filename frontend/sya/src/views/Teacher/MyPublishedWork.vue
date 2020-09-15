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
        <el-table-column label="#" type="index" width="130"></el-table-column>

        <el-table-column label="工作名称" prop="work_name" width="430"></el-table-column>
        <el-table-column prop="likes_num" label="点赞" width="120"></el-table-column>
        <el-table-column prop="collect_num" label="收藏" width="120"></el-table-column>
        <el-table-column prop="share_num" label="分享" width="160"></el-table-column>
        <el-table-column fixed="right" label="操作">
          <template slot-scope="scope">
            <el-button
              @click="editInfo(scope.row)"
              type="danger"
              icon="el-icon-edit"
              size="medium"
            >编辑</el-button>
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

    <el-dialog title="修改" :visible.sync="DialogVisible" width="50%">
      <el-form :model="editdata" :rules="formrules2" style="width:500px;margin:auto;" label-position="left" ref="thisform">
        <el-form-item label="工作ID" prop="work_id">
          <el-input type="textarea" disabled v-model="editdata.work_id"></el-input>
        </el-form-item>
        <el-form-item label="工作名称" prop="name">
          <el-input
            type="textarea"
            disabled
            v-model="editdata.work_name"
            maxlength="15"
            show-word-limit
          ></el-input>
        </el-form-item>
        <el-form-item label="工作图像"  required>
          <!-- <el-row>
          <el-col :span="24">
            <el-image :src="editdata.new_cover" :fit="fit">
              <div slot="error" class="image-slot">
                   <i class="el-icon-picture-outline" style="font-size: 100px"></i>
                </div>
              </el-image> 
            </el-col>
    </el-row>
    <el-radio-group v-model="editdata.new_cover">
      <el-radio border label="http://photo.tongji.edu.cn/__local/8/E4/D1/653D3735DC6F0691C6B2C1D4089_8C3254E4_59616.jpg" >封面一</el-radio>
      <el-radio border label="http://photo.tongji.edu.cn/__local/4/34/82/827E6293ACF9838C9C6F4D455B6_C35BBE06_74B1C.jpg" >封面二</el-radio>
      <el-radio border label="http://photo.tongji.edu.cn/__local/F/94/46/A22A1DF1FEF48DCCD579B1121F9_A1090C4F_69757.jpg" >封面三</el-radio>
          </el-radio-group>-->
        </el-form-item>
        <!-- 上传 -->
        <el-tooltip class="item" effect="dark" content="点击上传图片" placement="top">
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
          <el-input width="300px" v-model="editdata.new_address"></el-input>
        </el-form-item>
        <el-form-item label="工作薪资" prop="new_salary">
          <el-input v-model="editdata.new_salary"></el-input>
        </el-form-item>
        <el-form-item label="工作日期"  required>
          <el-col :span="9">
<el-form-item prop="new_start_day">
            <el-date-picker
              v-model="editdata.new_start_day"
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
            <el-form-item prop="new_end_day">
            <el-date-picker
              v-model="editdata.new_end_day"
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
            <el-form-item prop="new_start_time">
            <el-time-select
              placeholder="起始时间"
              v-model="editdata.new_start_time"
              :picker-options="{
      start: '08:00',
      step: '01:00',
      end: '20:00'
    }"
              style="width: 100%;"
            ></el-time-select>
            </el-form-item>
          </el-col>
          <el-col class="line" :span="2">-</el-col>
          <el-col :span="9">
            <el-form-item prop="new_end_time">
            <el-time-select
              placeholder="结束时间"
              v-model="editdata.new_end_time"
              :picker-options="{
      start: '08:00',
      step: '01:00',
      end: '21:00',
      minTime: editdata.new_start_time
    }"
              style="width: 100%;"
            ></el-time-select>
            </el-form-item>
          </el-col>
        </el-form-item>
        <el-form-item label="工作日" prop="new_week_day">
          <el-select v-model="editdata.new_week_day" placeholder="请选择工作日">
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
        <el-form-item label="工作描述" prop="new_desc">
          <el-input
            type="textarea"
            v-model="editdata.new_desc"
            maxlength="100"
            show-word-limit
            :autosize="{ minRows: 3, maxRows: 6}"
          ></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="cancel2('thisform')">取 消</el-button>
        <el-button type="primary" @click="Editework('editdate')">编 辑</el-button>
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
        pagesize: 10
      },
      total: 0,
      editdata: {},
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
        new_desc: ""
      },
      formrules2: {
          
          new_start_day: [
            { type: 'string', required: true, message: '请选择工作开始日期', trigger: 'change' }
          ],
          new_end_day: [
            { type: 'string', required: true, message: '请选择工作结束日期', trigger: 'change' }
          ],
          new_start_time: [
            { type: 'string', required: true, message: '请选择工作开始时间', trigger: 'change' }
          ],
          new_end_time: [
            { type: 'string', required: true, message: '请选择工作结束时间', trigger: 'change' }
          ],
          new_week_day: [
           { required: true, message: '请选择工作日', trigger: 'change' }
          ],
          new_address: [
            { required: true, message: '请填写工作地址', trigger: 'change' }
          ],
          new_salary: [
            { required: true, message: '请填写工作薪资', trigger: 'change' }
          ],
          new_desc: [
            { required: true, message: '请填写工作描述', trigger: 'blur' }
          ]
        },
      //设置选择日期
      pickerOptions2: {
        //结束时间不能大于开始时间
        disabledDate: time => {
          if (this.editdata.new_end_day) {
            return (
              time.getTime() > new Date(this.editdata.new_end_day).getTime() ||
              time.getTime() < Date.now()
            );
          } else {
            //没有选择结束时间，只能选择今天之后的时间不包括今天
            return time.getTime() < Date.now();
          }
        }
      },
      pickerOptions3: {
        //结束时间小于开始时间
        disabledDate: time => {
          if (this.editdata.new_start_day) {
            return (
              time.getTime() < new Date(this.editdata.new_start_day).getTime()
            );
          } else {
            //若未输入开始时间则默认为今天后时间不包括今天
            return time.getTime() < Date.now();
          }
        }
      }
    };
  },
  methods: {
    //重置
    cancel2(formName) {

        this.$confirm('是否清除表格内容', '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
          this.$refs[formName].resetFields();
          this.DialogVisible = false;
          this.$message({
            type: 'success',
            message: '已清除!'
          });
        }).catch(() => {
          this.DialogVisible = false;
          this.$message({
            type: 'info',
            message: '未清除'
          });          
        });
      },
    async getWorkList() {
      const result = await axios.post(
        "http://localhost:5000/Work/ViewHistoryWork",
        this.pageInfo,
        {
          withCredentials: true
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
        "http://localhost:5000/Work/ViewHistoryWork",
        this.pageInfo,
        {
          withCredentials: true
        }
      );
      // console.log(result);
      this.tableData = result.data.worklist;

      //取消加载的转圈圈
      this.loading = false;
    },
    //编辑信息
    editInfo(row) {
      // console.log(row);
      this.editdata = row;
      this.DialogVisible = true;
    },

    // 编辑工作
    async Editework() {
       this.$refs.thisform.validate(async valid => {
          if (!valid) {
            this.$message.error("请按照提示正确填写工作内容");
          return;
          } 
      // console.log(this.form);
      const result = await axios.post(
        "http://localhost:5000/Work/ChangeWorkInfo",
        {
          work_id: this.editdata.work_id,
          work_name: this.editdata.work_name,
          cover: this.uploadImg, //上传的封面
          work_description: this.editdata.new_desc,
          address: this.editdata.new_address,
          salary: parseInt(this.editdata.new_salary),
          start_day: this.editdata.new_start_day,
          end_day: this.editdata.new_end_day,
          start_time: this.editdata.new_start_time,
          end_time: this.editdata.new_end_time,
          week_day: parseInt(this.editdata.new_week_day)
        },
        { withCredentials: true }
      );
      // console.log(result);
      if (result.status == 200) {
        console.log("edit success");
        this.$message.success("修改成功");
        this.Dialogvisible = false;
        console.log("edit success222");
        await this.getOnePageworklist();
        console.log("edit success333");
        setTimeout(() => {
          this.$router.go(0);
        }, 500);
        this.$router.push("MyPublishedWork");
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
        .then(result => {
          // console.log(result);
          this.uploadImg =
            "http://fccimg.oss-cn-beijing.aliyuncs.com/" + result.name;
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
    }
  },
  async mounted() {
    await this.getWorkList();
    // console.log(this.tableData);
  }
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

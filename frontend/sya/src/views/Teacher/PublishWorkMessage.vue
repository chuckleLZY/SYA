<template>
  <div>
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{ path: '/home' }">教师</el-breadcrumb-item>
      <el-breadcrumb-item>工作台</el-breadcrumb-item>
      <el-breadcrumb-item>发布工作</el-breadcrumb-item>
    </el-breadcrumb>

    <el-card style="margin-top:20px;width: 100%">
      <el-row class="section">
        <p class="subTitle">创建工作</p>
        <hr class="sectionHr" align="center" />
        <el-form
          ref="form"
          :model="form"
          label-width="150px"
          style="width: 550px;margin-left:280px;margin-top:40px"
        >
          <el-form-item label="工作名称" prop="name">
            <el-input v-model="form.name" maxlength="20" show-word-limit></el-input>
          </el-form-item>
          <el-form-item label="工作图像" prop="cover">
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
            <!-- <el-row>
              <el-col :span="24">
                <el-image :src="form.cover" :fit="fit">
                  <div slot="error" class="image-slot">
                    <i class="el-icon-picture-outline" style="font-size: 100px"></i>
                  </div>
                </el-image>
              </el-col>
            </el-row>
            <el-radio-group v-model="form.cover">
              <el-radio
                border
                label="http://photo.tongji.edu.cn/__local/8/E4/D1/653D3735DC6F0691C6B2C1D4089_8C3254E4_59616.jpg"
              >封面一</el-radio>
              <el-radio
                border
                label="http://photo.tongji.edu.cn/__local/4/34/82/827E6293ACF9838C9C6F4D455B6_C35BBE06_74B1C.jpg"
              >封面二</el-radio>
              <el-radio
                border
                label="http://photo.tongji.edu.cn/__local/F/94/46/A22A1DF1FEF48DCCD579B1121F9_A1090C4F_69757.jpg"
              >封面三</el-radio>
            </el-radio-group>-->
          </el-form-item>
          <el-form-item label="工作地点" prop="address">
            <el-input v-model="form.address"></el-input>
          </el-form-item>
          <el-form-item label="工作薪资" prop="salary">
            <el-input v-model="form.salary"></el-input>
          </el-form-item>
          <el-form-item label="工作日期" prop="date">
            <el-col :span="11">
              <el-date-picker
                v-model="form.start_day"
                type="date"
                placeholder="开始日期"
                format="yyyy-MM-dd"
                value-format="yyyy-MM-dd"
                style="width: 100%;"
                :picker-options="pickerOptions0"
              ></el-date-picker>
            </el-col>
            <el-col class="line" :span="2">-</el-col>
            <el-col :span="11">
              <el-date-picker
                v-model="form.end_day"
                type="date"
                placeholder="结束日期"
                format="yyyy-MM-dd"
                value-format="yyyy-MM-dd"
                style="width: 100%;"
                :picker-options="pickerOptions1"
              ></el-date-picker>
            </el-col>
          </el-form-item>
          <el-form-item label="工作时间" prop="time">
            <el-col :span="11">
              <el-time-select
                placeholder="起始时间"
                v-model="form.start_time"
                :picker-options="{
      start: '08:00',
      step: '01:00',
      end: '20:00'
    }"
                style="width: 100%;"
              ></el-time-select>
            </el-col>
            <el-col class="line" :span="2">-</el-col>
            <el-col :span="11">
              <el-time-select
                placeholder="结束时间"
                v-model="form.end_time"
                :picker-options="{
      start: '08:00',
      step: '01:00',
      end: '21:00',
      minTime: form.start_time
    }"
                style="width: 100%;"
              ></el-time-select>
            </el-col>
          </el-form-item>
          <el-form-item label="工作日" prop="week_day">
            <el-select v-model="form.week_day" placeholder="请选择工作日">
              <el-option label="星期一" value="1"></el-option>
              <el-option label="星期二" value="2"></el-option>
              <el-option label="星期三" value="3"></el-option>
              <el-option label="星期四" value="4"></el-option>
              <el-option label="星期五" value="5"></el-option>
              <el-option label="星期六" value="6"></el-option>
              <el-option label="星期日" value="7"></el-option>
            </el-select>
          </el-form-item>

          <el-form-item label="工作描述" prop="desc">
            <el-input
              type="textarea"
              v-model="form.desc"
              maxlength="100"
              show-word-limit
              :autosize="{ minRows: 3, maxRows: 6}"
            ></el-input>
          </el-form-item>
        </el-form>
      </el-row>

      <div class="choose_btm">
        <el-button type="success" @click="Creatework()">新建</el-button>
        <el-button @click="cancelForm()">取 消</el-button>
      </div>
    </el-card>
  </div>
</template>




<script>
import axios from "axios";
import { client } from "../../oss.js";
export default {
  data() {
    return {
      form: {
        name: "",
        cover:
          "http://photo.tongji.edu.cn/__local/8/E4/D1/653D3735DC6F0691C6B2C1D4089_8C3254E4_59616.jpg",
        address: "",
        start_day: "",
        end_day: "",
        start_time: "",
        end_time: "",
        week_day: "",
        time: "",
        salary: "",
        desc: ""
      },
      uploadImg: "",
      //设置选择日期
      pickerOptions0: {
        //结束时间不能大于开始时间
        disabledDate: time => {
          if (this.form.end_day) {
            return (
              time.getTime() > new Date(this.form.end_day).getTime() ||
              time.getTime() < Date.now()
            );
          } else {
            //没有选择结束时间，只能选择今天之后的时间不包括今天
            return time.getTime() < Date.now();
          }
        }
      },
      pickerOptions1: {
        //结束时间小于开始时间
        disabledDate: time => {
          if (this.form.start_day) {
            return time.getTime() < new Date(this.form.start_day).getTime();
          } else {
            //若未输入开始时间则默认为今天后时间不包括今天
            return time.getTime() < Date.now();
          }
        }
      }
    };
  },
  methods: {
    onSubmit() {
      console.log("submit!");
    },
    //创建工作
    async Creatework() {
      console.log(this.form);
      const result = await axios.post(
        "http://localhost:5000/Work/CreateWork",
        {
          work_name: this.form.name,
          cover: this.uploadImg,//上传的封面
          work_description: this.form.desc,
          address: this.form.address,
          salary: parseInt(this.form.salary),
          start_day: this.form.start_day,
          end_day: this.form.end_day,
          start_time: this.form.start_time,
          end_time: this.form.end_time,
          week_day: parseInt(this.form.week_day)
        },
        {
          withCredentials: true
        }
      );
      if (result.status == 200) {
        this.$message.success("创建成功");
        this.$router.push("/MyPublishedWork");
      } else {
        this.$message.error("发生了一些错误");
      }
    },
    Upload(file) {
      var fileName = "banner" + `${Date.parse(new Date())}` + ".jpg"; //定义唯一的文件名
      //定义唯一的文件名，打印出来的uid其实就是时间戳
      client()
        .multipartUpload(fileName, file.file)
        .then(result => {
          console.log(result);
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
  }
};
</script>


<style scoped>
.subTitle {
  margin-top: 40px;
  font-size: 22px;
}
.sectionHr {
  margin-top: 0.8cm;
  margin-left: auto;
  margin-right: auto;
  width: 65%;
  height: 4px;
  background-color: gainsboro;
  border: none;
  border-radius: 2px;
}
.outside {
  margin-left: 150px;
  margin-right: 600px;
  margin-top: 40px;
}
.choose {
  margin-left: 400px;
  margin-right: 200px;
  margin-top: 160px;
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

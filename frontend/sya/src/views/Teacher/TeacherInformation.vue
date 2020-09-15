<template>
  <div>
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{ path: '/home' }">教师</el-breadcrumb-item>
      <el-breadcrumb-item>我的</el-breadcrumb-item>
      <el-breadcrumb-item>个人资料</el-breadcrumb-item>
    </el-breadcrumb>

    <!--个人资料卡片-->
    <el-card class="infoCard" v-if="cardVisible==true" v-loading="loading">
      <el-row>
        <el-col :span="5">
          <p class="title">我的资料</p>
        </el-col>
        <el-col :span="2" :offset="14" style="margin-top:0.7cm">
          <el-button type="success" icon="el-icon-edit" @click="edit()">编辑个人资料</el-button>
        </el-col>
      </el-row>
      <!--头像区域-->
      <div class="box" id="box1"></div>
      <div class="box" id="box2"></div>
      <el-row class="avaRow">
        <div class="block">
          <!-- <el-avatar shape="circle" :size="100" :src="UserInfo.avatar" fit="fill"> -->
          <!-- <img id="userAva" /> -->
          <!-- </el-avatar> -->
          <el-avatar shape="circle" style="height:100px;width:100px">
            <el-image :src="UserInfo.avatar" fit="fill" style="height:100%;width:100%"></el-image>
          </el-avatar>
          <!-- <el-image :src="UserInfo.avatar" fit="fill" style="height:100px;width:100px"></el-image> -->
        </div>
        <div class="box" id="box3"></div>
        <div class="box" id="box4"></div>
      </el-row>
      <!--用户名区域-->
      <el-row class="name_Row">
        <p class="stuID">{{UserInfo.user_name}}</p>
      </el-row>
      <el-row>
        <p v-if="UserInfo.user_role==1" style="font-weight:bold; font-size:20px;">
          <i class="el-icon-s-custom"></i> 学生
        </p>
      </el-row>
      <el-tag type="primary" style="zoom:150%" effect="dark" v-if="UserInfo.gender==true">
        <i class="el-icon-male"></i>
      </el-tag>
      <el-tag type="danger" style="zoom:150%" effect="dark" v-if="UserInfo.gender==false">
        <i class="el-icon-female"></i>
      </el-tag>
      <!--信息-->
      <el-row class="section">
        <p class="subTitle">账号绑定</p>
        <hr class="sectionHr" align="center" />
        <!--表单-->
        <el-form label-width="0px" class="infoForm">
          <el-form-item class="itemRow">
            <div class="itemNameContainer">
              <p class="itemName">
                <i class="el-icon-phone-outline"></i> 绑定手机 ：
              </p>
            </div>
            <el-input class="infoItem" readonly v-model="UserInfo.tel"></el-input>
          </el-form-item>
          <el-form-item class="itemRow">
            <div class="itemNameContainer">
              <p class="itemName">
                <i class="el-icon-message"></i> 邮箱 :
              </p>
            </div>
            <el-input class="infoItem" readonly v-model="UserInfo.email"></el-input>
          </el-form-item>
          <el-form-item class="itemRow">
            <div class="itemNameContainer">
              <p class="itemName">
                <i class="el-icon-bank-card"></i> 银行卡号 :
              </p>
            </div>
            <el-input class="infoItem" readonly v-model="UserInfo.bank"></el-input>
          </el-form-item>
        </el-form>
      </el-row>
      <!--信息标题-->
      <el-row class="section">
        <p class="subTitle">工作情况</p>
        <hr class="sectionHr" align="center" />
        <!--表单-->
        <el-form label-width="0px" class="infoForm">
          <el-form-item class="itemRow">
            <div class="itemNameContainer">
              <p class="itemName">
                <i class="el-icon-date"></i> 工作总时长 ：
              </p>
            </div>
            <el-input class="infoItem" readonly v-model="UserInfo.work_time"></el-input>
          </el-form-item>
          <el-form-item class="itemRow">
            <div class="itemNameContainer">
              <p class="itemName">
                <i class="el-icon-document"></i> 申请总次数 :
              </p>
            </div>
            <el-input class="infoItem" readonly v-model="UserInfo.nof_apply"></el-input>
          </el-form-item>
          <el-form-item class="itemRow">
            <div class="itemNameContainer">
              <p class="itemName">
                <i class="el-icon-document-delete"></i> 请假总次数 :
              </p>
            </div>
            <el-input class="infoItem" readonly v-model="UserInfo.nof_absent"></el-input>
          </el-form-item>
          <el-form-item class="itemRow">
            <div class="itemNameContainer">
              <p class="itemName">
                <i class="el-icon-help"></i> 累积总收入 :
              </p>
            </div>
            <el-input class="infoItem" readonly v-model="UserInfo.income"></el-input>
          </el-form-item>
        </el-form>
      </el-row>
      <div style="text-align:center">
        <p class="syaSup">SYA——Show Your Ability</p>
      </div>
    </el-card>

    <!--资料编辑卡片-->
    <el-card v-if="cardVisible==false" style="margin-top:1cm">
      <el-row>
        <el-col :span="5">
          <p class="title">个人资料编辑</p>
        </el-col>
        <el-col :span="2" :offset="12" style="margin-top:0.7cm">
          <el-button type="success" icon="el-icon-edit" @click="saveedit()">保存修改</el-button>
        </el-col>
        <el-col :span="2" :offset="1" style="margin-top:0.7cm">
          <el-button type="danger" icon="el-icon-edit" @click="dialogVisible=true">退出编辑</el-button>
        </el-col>
      </el-row>
      <!-- 头像 -->
      <el-upload
        class="avatar-uploader"
        action
        :show-file-list="false"
        :http-request="Upload"
        :before-upload="beforeAvatarUpload"
      >
        <img v-if="editForm.avatar" :src="editForm.avatar" class="avatar" />
        <i v-else class="el-icon-plus avatar-uploader-icon"></i>
      </el-upload>
      <div class="imgTitle">点击上传头像</div>
      <!-- 表单 -->
      <el-form
        class="editForm"
        ref="editRef"
        :model="editForm"
        :rules="editFormRules"
        label-width="120px"
      >
        <el-form-item style="width:50%" label="性别 : ">
          <el-switch
            style="display: block; margin-top:4%;"
            v-model="editForm.gender"
            active-color="#409EFF"
            inactive-color="#ff4949"
            active-text="男"
            inactive-text="女"
          ></el-switch>
        </el-form-item>
        <el-form-item style="width:80%" label="手机号码 : " prop="tel">
          <el-input v-model="editForm.tel"></el-input>
        </el-form-item>
        <el-form-item style="width:100%" label="银行卡号 : " prop="bank">
          <el-input v-model="editForm.bank"></el-input>
        </el-form-item>
      </el-form>
    </el-card>
    <!--关闭对话框-->
    <el-dialog title="提示" :visible.sync="dialogVisible" width="30%" center>
      <span>需要保存编辑吗？</span>
      <span slot="footer" class="dialog-footer">
        <el-button @click="cancel();dialogVisible=false">取 消</el-button>
        <el-button type="primary" @click="saveedit();dialogVisible=false">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>
 



<script>
import axios from "axios";
import { client } from "../../oss.js";
export default {
  data() {
    var checkMobile = (rule, value, cb) => {
      //验证手机号正则表达式
      const regMobile = /^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/;
      if (regMobile.test(value)) {
        return cb();
      }
      cb(new Error("请输入合法的手机号"));
    };
    return {
      UserInfo: [],
      cardVisible: true, //true显示资料卡，false显示编辑卡
      editForm: {
        gender: true,
        avatar: "A.png",
        tel: "",
        bank: ""
      },
      editFormRules: {
        tel: [
          { required: true, message: "请输入手机号码", trigger: "blur" },
          { validator: checkMobile, trigger: "blur" }
        ],
        bank: [{ required: true, message: "请输入银行卡号", trigger: "blur" }]
      },
      dialogVisible: false,
      loading: false
    };
  },
  async created() {
    await this.getUserInfo();
  },
  methods: {
    async getUserInfo() {
      this.loading = true;
      const { data: res } = await axios.post(
        "http://localhost:5000/User/GetUserInfo",
        {},
        { withCredentials: true }
      );
      this.UserInfo = res;
      if (!res.avatar) {
        this.UserInfo.avatar =
          "https://cube.elemecdn.com/9/c2/f0ee8a3c7c9638a54940382568c9dpng.png";
      } else {
        this.UserInfo.avatar = res.avatar;
        console.log("img", typeof res.avatar, res.avatar);
      }
      console.log(res);
      this.loading = false;
    },
    errorHandler() {
      return true;
    },
    edit() {
      //进入编辑模式
      this.cardVisible = false;
      this.editForm.gender = this.UserInfo.gender;
      this.editForm.tel = this.UserInfo.tel;
      this.editForm.bank = this.UserInfo.bank;
      this.editForm.avatar = this.UserInfo.avatar;
    },
    saveedit() {
      this.$refs.editRef.validate(async valid => {
        if (!valid) {
          this.$message.error("请按照验证提示正确填写个人资料后后再进行保存");
          return;
        }
        // 没做修改，直接返回
        if (
          this.editForm.gender == this.UserInfo.gender &&
          this.editForm.tel == this.UserInfo.tel &&
          this.editForm.bank == this.UserInfo.bank &&
          this.editForm.avatar == this.UserInfo.avatar
        ) {
          this.$message.info("个人信息未作修改");
          this.getUserInfo(); //重新调取用户信息
          this.cardVisible = true;
          return;
        }
        const { data: res } = await axios.post(
          "http://localhost:5000/User/UpdateUser",
          {
            gender: this.editForm.gender,
            avatar: this.editForm.avatar,
            tel: this.editForm.tel,
            bank: this.editForm.bank
          },
          { withCredentials: true }
        );
        if (res.status > 400) {
          this.$message.error("保存失败，请稍后重试");
          return;
        }
        // this.getUserInfo(); //重新调取用户信息
        setTimeout(() => {
          this.$router.go(0);
        }, 500);
        this.$router.push("TeacherInformation");
        this.cardVisible = true;
        this.$message.success("保存成功！");
      });
    },
    cancel() {
      this.dialogVisible = true;
      this.cardVisible = true;
      this.$message.warning("您所作的更改并未保存");
    },

    //头像
    // handleAvatarSuccess(res, file) {
    //   this.imageUrl = URL.createObjectURL(file.raw);
    // },
    Upload(file) {
      var fileName = "banner" + `${Date.parse(new Date())}` + ".jpg"; //定义唯一的文件名
      //定义唯一的文件名，打印出来的uid其实就是时间戳
      client()
        .multipartUpload(fileName, file.file)
        .then(result => {
          console.log(result);
          this.editForm.avatar =
            "http://fccimg.oss-cn-beijing.aliyuncs.com/" + result.name;
        });
    },
    // 图片限制
    beforeAvatarUpload(file) {
      const isJPEG = file.name.split(".")[1] === "jpeg";
      const isJPG = file.name.split(".")[1] === "jpg";
      const isPNG = file.name.split(".")[1] === "png";
      const isLt500K = file.size / 1024 / 500 < 2;
      if (!isJPG && !isJPEG && !isPNG) {
        this.$message.error("上传图片只能是 JPEG/JPG/PNG 格式!");
      }
      if (!isLt500K) {
        this.$message.error("单张图片大小不能超过 500KB!");
      }

      return (isJPEG || isJPG || isPNG) && isLt500K;
    }
  }
};
</script>



<style scoped>
.syaSup {
  font-size: 20px;
  margin-top: 2cm;
}

.infoCard {
  margin-top: 20px;
}

.title {
  font-size: 25px;
  font-weight: bold;
}
.avaRow {
  margin-top: 1cm;
}
.stuID {
  font-size: 20px;
  font-weight: normal;
}
.subTitle {
  font-size: 22px;
}
.sectionHr {
  margin-top: 0.5cm;
  margin-left: auto;
  margin-right: auto;
  width: 50%;
  height: 4px;
  background-color: gainsboro;
  border: none;
  border-radius: 2px;
}
.infoForm {
  margin-top: 1cm;
}

.itemRow {
  overflow: hidden;
}
.infoItem {
  width: 30%;
  float: left;
}
.itemName {
  font-weight: bold;
  line-height: 40px;
  margin-left: 70%;
  margin-top: 0;
  margin-bottom: 0;
}
.itemNameContainer {
  float: left;
  width: 40%;
  text-align: left;
  height: 40px;
}
.section {
  margin-top: 2cm;
}
.editForm {
  width: 50%;
  margin-left: 25%;
  margin-top: 2cm;
}

/* 头像 */
.avatar-uploader .el-upload {
  border: 1px dashed #d9d9d9;
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
  width: 100px;
  height: 100px;
  line-height: 178px;
  text-align: center;
}
.avatar {
  width: 100px;
  height: 100px;
  display: block;
}
.imgTitle {
  font-size: 15px;
  font-weight: bold;
}
</style>

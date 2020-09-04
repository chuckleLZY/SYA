<template>
  <div class="outside">
    <span>
      <i class="el-icon-s-tools"></i>&emsp;个人信息
    </span>
    <el-divider></el-divider>
    <el-form
      :model="ruleForm"
      :rules="rules"
      ref="ruleForm"
      label-width="100px"
      class="demo-ruleForm"
    >
      <el-form-item label="昵称" prop="name">
        <el-input v-bind:placeholder="Infolist.user_name" v-model="ruleForm.name"></el-input>
      </el-form-item>
      <el-form-item label="密码" prop="password">
        <el-input placeholder="请输入新密码" v-model="ruleForm.password" show-password></el-input>
      </el-form-item>
      <!-- <el-form-item label="所在地" prop="region">
        <el-select v-model="ruleForm.region" v-bind:placeholder="Infolist.user_name">
          <el-option label="四平路校区" value="siping"></el-option>
          <el-option label="嘉定校区" value="jiading"></el-option>
        </el-select>
      </el-form-item> -->
      <el-form-item label="性别" prop="gender">
        <el-radio-group v-model="radio">
          <el-radio label="0">男</el-radio>
          <el-radio label="1">女</el-radio>
        </el-radio-group>
      </el-form-item>
      <el-form-item label="手机号" prop="phonenumber">
        <el-input v-bind:placeholder="Infolist.tel" v-model="ruleForm.phonenumber"></el-input>
      </el-form-item>
      <el-form-item label="邮箱" prop="email">
        <el-input v-bind:placeholder="Infolist.email" v-model="ruleForm.email"></el-input>
      </el-form-item>
      <el-form-item label="银行" prop="bank">
        <el-input v-bind:placeholder="Infolist.bank" v-model="ruleForm.bank"></el-input>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="UpdateUser()">修改</el-button>
        <el-button @click="resetForm('ruleForm')">重置</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>


<script>
import axios from "axios";
export default {
  data() {
    return {
      ruleForm: {
        name: "",
        password: "",
        region: "",
        gender: "",
        phonenumber: "",
        email: "",
        bank:"",
      },
      radio: "0",
      Infolist:[] ,
      rules: {
        name: [
          { required: true, message: "请输入昵称", trigger: "blur" },
          { min: 3, max: 8, message: "长度在 3 到 8 个字符", trigger: "blur" },
        ],
        password: [
          { required: true, message: "请输入密码", trigger: "blur" },
          { min: 3, max: 8, message: "长度在 3 到 8 个字符", trigger: "blur" },
        ],
        phonenumber: [
          { required: true, message: "请输入手机号码", trigger: "blur" },
          { max: 11, message: "长度为11个字符", trigger: "blur" },
        ],
        email: [
          { required: true, message: "请输入邮箱", trigger: "blur" },
          {
            min: 3,
            max: 15,
            message: "长度在 3 到 15 个字符",
            trigger: "blur",
          },
        ],
        // region: [
        //   { required: true, message: "请选择所在地", trigger: "change" },
        // ],
        gender: [
          { required: true, message: "请选择性别", trigger: "change" },
        ],
        bank: [
          { required: true, message: "请输入银行卡号", trigger: "blur" },
          { max: 16, message: "长度为16个字符", trigger: "blur" },
        ],
      }, 
    };
  },
  created(){
      //调用获取个人信息的API函数
      this.GetUserInfo()
      
  },
  methods: {
    // submitForm(
    //   formName //不能为空
    // ) {
    //   this.$refs[formName].validate((valid) => {
    //     if (valid) {
    //       alert("submit!");
    //     } else {
    //       console.log("error submit!!");
    //       return false;
    //     }
    //   });
    // },

    resetForm(
      formName //重置表格
    ) {
      this.$refs[formName].resetFields();
    },
    async GetUserInfo() {
      const res = await axios.post("http://localhost:5000/User/GetUserInfo",
      {},
        {
          withCredentials: true,
        }
      );
      // console.log(res);
      if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
      }
      this.Infolist=res.data;
      console.log(this.Infolist);
    },
    // buttonclick(formName)
    // {
    //     UpdateUser();
    //     submitForm(formName) //不能为空
    // },
    async UpdateUser(){
        const res=await axios.post('http://localhost:5000/User/UpdateUser',
        {
          gender:this.Infolist.gender,
          avatar: "../assets/imagebox/1.jpeg",
          tel:this.Infolist.tel,
          bank:this.Infolist.bank,
        },
        {
          withCredentials: true
        });
        if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
        }
        this.Infolist.tel=this.ruleForm.phonenumber;
        this.Infolist.bank=this.ruleForm.bank;
        this.Infolist.gender=this.ruleForm.gender;
        // this.Infolist.tel=this.ruleForm.phonenumber;
        console.log(this.Infolist);
        console.log(res);
        this.$message.success('修改信息成功');
        GetUserInfo();
    }
     
  },
};
</script>


<style scoped>
.outside {
  margin-left: 200px;
  margin-right: 500px;
  margin-top: 20px;
}
</style>

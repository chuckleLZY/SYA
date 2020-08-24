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
        <el-input placeholder="请输入昵称" v-model="ruleForm.name"></el-input>
      </el-form-item>
      <el-form-item label="密码" prop="password">
        <el-input placeholder="请输入密码" v-model="ruleForm.password" show-password></el-input>
      </el-form-item>
      <el-form-item label="所在地" prop="region">
        <el-select v-model="ruleForm.region" placeholder="请选择所在地">
          <el-option label="四平路校区" value="siping"></el-option>
          <el-option label="嘉定校区" value="jiading"></el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="手机号" prop="phonenumber">
        <el-input placeholder="请输入手机号" v-model="ruleForm.phonenumber"></el-input>
      </el-form-item>
      <el-form-item label="邮箱" prop="email">
        <el-input placeholder="请输入邮箱" v-model="ruleForm.email"></el-input>
      </el-form-item>
      <el-form-item label="性别" prop="gender">
        <el-radio-group v-model="ruleForm.gender">
          <el-radio label="男"></el-radio>
          <el-radio label="女"></el-radio>
        </el-radio-group>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submitForm('ruleForm')">修改</el-button>
        <el-button @click="resetForm('ruleForm')">重置</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>


<script>
export default {
  data() {
    return {
      ruleForm: {
        name: "",
        password: "",
        region: "",
        resource: "",
        phonenumber: "",
        email: "",
      },
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
        region: [
          { required: true, message: "请选择所在地", trigger: "change" },
        ],
        resource: [
          { required: true, message: "请选择性别", trigger: "change" },
        ],
      },
    };
  },
  methods: {
    submitForm(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          alert("submit!");
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    resetForm(formName) {
      this.$refs[formName].resetFields();
    },
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

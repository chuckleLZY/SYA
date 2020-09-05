<template>
  <div>
    <span>
      <i class="el-icon-s-tools"></i>&emsp;管理员资格验证
    </span>
    <el-divider></el-divider>工号：
    <el-input v-model="input" placeholder="请输入您的教师工号"></el-input>
    <el-row>
      <el-button type="primary">提交</el-button>
    </el-row>
  </div>
</template>


<script>
export default {
  data() {
    return {
      input: ""
    };
  }
};
</script>


<style scoped>
.el-input {
  width: 20%;
  margin-bottom: 20px;
}
</style>

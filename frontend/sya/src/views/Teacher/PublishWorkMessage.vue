<template>
    <div >
<div class="outside">
 <el-form ref="form" :model="form" label-width="150px">
  <el-form-item label="工作名称" prop="name">
    <el-input v-model="form.name" maxlength="15" show-word-limit ></el-input>
  </el-form-item>
  <el-form-item label="工作图像" prop="cover">
    <el-select v-model="form.cover" placeholder="请选择工作封面">
      <el-option label="封面一" value="https://bkzs.tongji.edu.cn/themes/136/userfiles/images/2015/10/26/source/wo1k84x2rc6equf.jpg"></el-option>
      <el-option label="封面二" value="https://ss1.bdstatic.com/70cFuXSh_Q1YnxGkpoWK1HF6hhy/it/u=208279108,1011135259&fm=26&gp=0.jpg"></el-option>
      <el-option label="封面三" value="http://photo.tongji.edu.cn/__local/F/94/46/A22A1DF1FEF48DCCD579B1121F9_A1090C4F_69757.jpg"></el-option>
    </el-select>
  </el-form-item>
  <el-form-item label="工作地点" prop="address">
  <el-input v-model="form.address" ></el-input>
  </el-form-item>
  <el-form-item label="工作时间" prop="time">
  <el-input v-model="form.time" ></el-input>
  </el-form-item>
  <el-form-item label="工作薪资（元/天）" prop="salary">
    <el-input v-model="form.salary" ></el-input>
  </el-form-item>
  <el-form-item label="工作描述" prop="desc">
    <el-input type="textarea" v-model="form.desc" maxlength="100" show-word-limit :autosize="{ minRows: 3, maxRows: 6}"></el-input>
  </el-form-item>
</el-form>
    </div>
    
    <span class="choose">
        <el-button type="success" @click="Creatework()">新 建</el-button>
    <el-button @click="cancelForm">取 消</el-button>
</span>
    </div>
    
</template>


<script>
import axios from "axios";
export default {
        data() {
      return {
        form: {
          name: '',
          cover:'',
          address:'',
          time:'',
          salary:'',
          desc: ''
        }
      }
    },
    methods: {
      onSubmit() {
        console.log('submit!');
      },

      // 创建新的工作
      async Creatework() {
      console.log(this.form);
      const result = await axios.post(
        "http://localhost:5000/Work/CreateWork",
        {
          work_name: this.form.name,
          cover: this.form.cover,
          work_description: this.form.desc,
          address: this.form.address,
          salary: parseInt(this.form.salary),
          work_time: this.form.time,
        },
        { withCredentials: true }
      );
      console.log(result);
      if (result.status == 200) {
        this.$message.success("创建成功");
      }
      else{
        this.$message.error("发生了一些错误");
      }
    },
    //this.$refs.formname.resetFields();

    }
    
}
</script>


<style scoped>
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
</style>

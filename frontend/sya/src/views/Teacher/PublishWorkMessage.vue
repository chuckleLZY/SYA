<template>
    <div >
<div class="outside">
 <el-form ref="form" :model="form" label-width="150px">
  <el-form-item label="工作名称" prop="name">
    <el-input v-model="form.name" maxlength="20" show-word-limit ></el-input>
  </el-form-item>
  <el-form-item label="工作图像" prop="cover">
    <el-select v-model="form.cover" placeholder="请选择工作封面">
      <el-option label="封面一" value="http://photo.tongji.edu.cn/__local/8/E4/D1/653D3735DC6F0691C6B2C1D4089_8C3254E4_59616.jpg"></el-option>
      <el-option label="封面二" value="http://photo.tongji.edu.cn/__local/4/34/82/827E6293ACF9838C9C6F4D455B6_C35BBE06_74B1C.jpg"></el-option>
      <el-option label="封面三" value="http://photo.tongji.edu.cn/__local/F/94/46/A22A1DF1FEF48DCCD579B1121F9_A1090C4F_69757.jpg"></el-option>
    </el-select>
  </el-form-item>
  <el-form-item label="工作地点" prop="address">
  <el-input v-model="form.address" ></el-input>
  </el-form-item>
  <el-form-item label="工作日期" prop="date">
   <el-col :span="11">
      <el-date-picker
      v-model="form.start_day"
      type="date"
      placeholder="开始日期"
      format="yyyy-MM-dd"
      value-format="yyyy-MM-dd"
      style="width: 100%;">
    </el-date-picker>
    </el-col>
    <el-col class="line" :span="2">-</el-col>
    <el-col :span="11">
      <el-date-picker
      v-model="form.end_day"
      type="date"
      placeholder="结束日期"
      format="yyyy-MM-dd"
      value-format="yyyy-MM-dd"
      style="width: 100%;">
    </el-date-picker>
    
    </el-col>
  </el-form-item>
  <el-form-item label="工作时间" prop="time">
  <el-col :span="11">
      <el-time-picker
    v-model="form.start_time"
    :picker-options="{
      selectableRange: '08:30:00 - 19:30:00'
    }"
    placeholder="开始时间"
    value-format="HH:mm:ss"
    style="width: 100%;">
  </el-time-picker>
    </el-col>
    <el-col class="line" :span="2">-</el-col>
    <el-col :span="11">
      <el-time-picker
    v-model="form.end_time"
    :picker-options="{
      selectableRange: '09:30:00 - 20:30:00'
    }"
    placeholder="结束时间"
    value-format="HH:mm:ss"
    style="width: 100%;">
  </el-time-picker>
   
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
  <el-form-item label="工作薪资（元/天）" prop="salary">
    <el-input v-model="form.salary" ></el-input>
  </el-form-item>
  <el-form-item label="工作描述" prop="desc">
    <el-input type="textarea" v-model="form.desc" maxlength="100" show-word-limit :autosize="{ minRows: 3, maxRows: 6}"></el-input>
  </el-form-item>
</el-form>
    </div>
    
    <div class="choose_btm">
        <el-button type="success" @click="Creatework()">新建</el-button>
    <el-button @click="cancelForm()">取 消</el-button>
</div>
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
          start_day:'',
          end_day:'',
          start_time:'',
          end_time:'',
          week_day:'',
          time:'',
          salary:'',
          desc: ''
        }
      }
    },
   methods:{
       onSubmit() {
        console.log('submit!');
      },
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
      }
      else{
        this.$message.error("发生了一些错误");
      }
    },

      
     

    
      },
    
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

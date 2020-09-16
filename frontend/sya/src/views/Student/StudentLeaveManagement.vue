<template>
  <div>
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{ path: '/home' }">学生</el-breadcrumb-item>
      <el-breadcrumb-item>我的</el-breadcrumb-item>
      <el-breadcrumb-item>请假管理</el-breadcrumb-item>
    </el-breadcrumb>

    <el-card class="workCard" v-loading="loading">
      <el-row class="header">
        <el-col class="titlecol" :span="5">
          <p class="title">请假管理</p>
        </el-col>
      </el-row>
      <!--表格区域-->
      <el-row>
        <el-table :data="LeaveList" border>
          <el-table-column type="index" label="#"> </el-table-column>
          <el-table-column prop="leave_day" label="请假日期"> </el-table-column>
          <el-table-column prop="leave_start" label="起始时间">
          </el-table-column>
          <el-table-column prop="leave_end" label="结束时间"> </el-table-column>
          <el-table-column prop="content" label="请假理由"> </el-table-column>
          <el-table-column prop="proof" label="备注"> </el-table-column>
          <el-table-column prop="status" label="状态" width="80">
            <template slot-scope="scope">
              <el-tag type="primary" v-if="scope.row.status == 0"
                >申请中</el-tag
              >
              <el-tag type="success" v-if="scope.row.status == 1"
                >已通过</el-tag
              >
              <el-tag type="danger" v-if="scope.row.status == 2">已拒绝</el-tag>
            </template>
          </el-table-column>
          <el-table-column label="操作" width="270">
            <template slot-scope="scope">
              <el-button
                type="primary"
                size="medium"
                icon="el-icon-search"
                @click="getWorkInfo(scope.row.work_id);workVisible=true"
                >工作详情</el-button
              >
              <el-button
                type="success"
                size="medium"
                icon="el-icon-edit"
                :disabled="scope.row.status!=0"
                @click="getWorkInfo(scope.row.work_id);AbForm.leave_id=scope.row.leave_id;AbForm.work_id=scope.row.work_id;AbsentVisible=true"
                >修改申请</el-button
              >
            </template>
          </el-table-column>
        </el-table>
      </el-row>

      <el-pagination
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
        :current-page="queryInfo.pagenum"
        :page-sizes="[5, 10, 15]"
        :page-size="queryInfo.pagesize"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
        style="margin-top:2cm"
      >
      </el-pagination>
    </el-card>

    <!--更新请假的信息-->
    <el-dialog
      title="修改申请"
      :visible.sync="AbsentVisible"
      @close="AbFormClose()"
      width="50%"
      center
    >
      <el-form
        :model="AbForm"
        :rules="AbFormRules"
        label-width="100px"
        ref="AbFormRef"
      >
        <el-form-item label="请假时长 ：" prop="leave_time">
          <el-input
            v-model="AbForm.leave_time"
            placeholder="请输入请假时长"
            style="width:40%"
          ></el-input>
        </el-form-item>
        <el-form-item label="请假理由 ：" prop="content">
          <el-input
            v-model="AbForm.content"
            placeholder="请输入请假理由"
            type="textarea"
            :rows="3"
            maxlength="300"
            show-word-limit
            style="width:80%"
          ></el-input>
        </el-form-item>
        <el-form-item label="备注 ：" prop="proof">
          <el-input
            v-model="AbForm.proof"
            placeholder="此项选填"
            type="textarea"
            :rows="3"
            maxlength="300"
            show-word-limit
            style="width:80%"
          ></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="AbsentVisible = false">取 消</el-button>
        <el-button type="primary" @click="updateAbsent()">提 交</el-button>
      </span>
    </el-dialog>

    <!--工作详情-->
    <el-dialog title="工作详情 " :visible.sync="workVisible" width="50%" center>
      <el-row style="text-align:center">
        <el-image :src="WorkList.cover" fit="fit">
          <div slot="error" class="image-slot">
            <i class="el-icon-picture-outline" style="font-size: 100px"></i>
          </div>
        </el-image>
      </el-row>
      <el-row>
        <el-col :offset="7">
          <p class="inlineTitle"><i class="el-icon-paperclip"></i> 工作名称 ：
            <span class="inlineInfo"> {{ WorkList.work_name }} </span>
          </p>
          <p class="inlineTitle"><i class="el-icon-tickets "></i> 工作描述 ：
            <span class="inlineInfo"> {{ WorkList.work_description }} </span>
          </p>
          <p class="inlineTitle">
            <i class="el-icon-thumb"></i> 点赞 ：
            <span class="inlineInfo"> {{ WorkList.likes_num }} </span>
          </p>
          <p class="inlineTitle">
            <i class="el-icon-star-off"></i> 收藏 ：
            <span class="inlineInfo"> {{ WorkList.collect_num }} </span>
          </p>
          <p class="inlineTitle">
            <i class="el-icon-date"></i> 工作周期 ：
            <span class="inlineInfo"> {{ WorkList.start_day }} </span> ~
            <span class="inlineInfo"> {{ WorkList.end_day }} </span>
          </p>
          <p class="inlineTitle">
            <i class="el-icon-time"></i> 工作时间 ：
            <span class="inlineInfo" v-if="WorkList.week_day == 1">
              星期一
            </span>
            <span class="inlineInfo" v-if="WorkList.week_day == 2">
              星期二
            </span>
            <span class="inlineInfo" v-if="WorkList.week_day == 3">
              星期三
            </span>
            <span class="inlineInfo" v-if="WorkList.week_day == 4">
              星期四
            </span>
            <span class="inlineInfo" v-if="WorkList.week_day == 5">
              星期五
            </span>
            <span class="inlineInfo" v-if="WorkList.week_day == 6">
              星期六
            </span>
            <span class="inlineInfo" v-if="WorkList.week_day == 7">
              星期天
            </span>
            <span class="inlineInfo"> {{ WorkList.start_time }} </span> ~
            <span class="inlineInfo"> {{ WorkList.end_time }} </span>
          </p>
        </el-col>
      </el-row>
      <el-row style="text-align:left"> </el-row>
      <span slot="footer" class="dialog-footer">
        <el-button type="primary" @click="workVisible = false">关 闭</el-button>
      </span>
    </el-dialog>

    <!--请假的信息-->
         <el-dialog title="请假申请" :visible.sync="AbsentVisible" @close="AbFormClose();getLeaveList();" width="50%" center >
            <el-form :model="AbForm" :rules="AbFormRules" label-width="100px" ref="AbFormRef" >
                <el-form-item label="请假日期:" prop="leave_day">
                    <el-col :span="8">
                        <el-date-picker
                        v-model="AbForm.leave_day"
                        type="date"
                        placeholder="开始日期"
                        format="yyyy-MM-dd"
                        value-format="yyyy-MM-dd"
                        style="width: 100%;"
                        :picker-options="pickerOptions0">
                        </el-date-picker>
                    </el-col>
                </el-form-item>
                <el-form-item label="工作时间" required>
                <el-col :span="8">
                    <el-form-item prop="leave_start">
                        <el-time-select
                        placeholder="起始时间"
                        v-model="AbForm.leave_start"
                        :picker-options="{
                        start: this.WorkList.start_time,
                        step: '00:15',
                        end: this.WorkList.end_time,
                        }"
                        style="width: 100%;">
                    </el-time-select>
                    </el-form-item>
                </el-col>
                 <el-col class="line" :span="2" style="text-align:center;">---</el-col>
                <el-col :span="8">
                    <el-form-item prop="leave_end">
                        <el-time-select
                        placeholder="结束时间"
                        v-model="AbForm.leave_end"
                        :picker-options="{
                        start: this.WorkList.start_time,
                        step: '00:15',
                        end:this.WorkList.end_time,
                        minTime:AbForm.leave_start
                        }"
                        style="width: 100%;">
                       </el-time-select>
                    </el-form-item>
                </el-col>
                </el-form-item>

                <el-form-item label="请假理由 ：" prop="content">
                    <el-input v-model="AbForm.content" placeholder="请输入请假理由" type="textarea" :rows="3" maxlength="300" show-word-limit style="width:80%"></el-input>
                </el-form-item>

                <el-form-item label="备注 ：" prop="proof">
                    <el-input v-model="AbForm.proof" placeholder="此项选填" type="textarea" :rows="3" maxlength="300" show-word-limit style="width:80%"></el-input>
                </el-form-item>

            </el-form>

            <span slot="footer" class="dialog-footer" >
                <el-button @click="AbsentVisible = false">取 消</el-button>
                <el-button type="primary" @click="updateAbsent();">确 认</el-button>
            </span>
        </el-dialog>
        
    
  </div>
</template>

<script>
import axios from "axios";
export default {
  data() {
    return {
      loading:true,
      WorkList: [],
      LeaveList: [],
      workVisible: false,
      total: 20,
      AbsentVisible: false,
      queryInfo:{
                pagenum:1,
                pagesize:5,
      },
      AbForm: {
        leave_id:"",
        work_id: "",
        content: "",
        proof: "",
        leave_day: "",
        leave_start: "",
        leave_end: "",
      },
      AbFormRules:{
                leave_day:[
                    {required:true, message:"请选择时间"},
                ],
                content:[
                    {required:true, message:"请输入请假理由"}
                ],
                 leave_start:[
                    {required:true, message:"请选择请假的起始时间"},
                ],
                leave_end:[
                    {required:true, message:"请选择请假的结束时间"},
                ]

      },
      pickerOptions0: { //设置日期必须在工作时间之间
      	    disabledDate: (time) => {
                return  new Date(this.WorkList.start_day).getTime()> time.getTime() || new Date(this.WorkList.end_day).getTime()< time.getTime() || time.getDay()!=(this.WorkList.week_day%7)
            }
      },
    };
  },
  created() {
    this.getLeaveList();
  },
  methods: {
    async getLeaveList() {
      const {
        data: res,
      } = await axios.post(
        "http://localhost:5000/Leave/ViewLeave",
        this.queryInfo,
        { withCredentials: true }
      );
      this.LeaveList = res.leavelist;
      this.total = res.total;
      this.loading=false;
    },
    handleSizeChange(newSize) {
      this.queryInfo.pagesize = newSize;
      this.getLeaveList();
    },
    handleCurrentChange(newPage) {
      this.queryInfo.pagenum = newPage;
      this.getLeaveList();
    },
    async getWorkInfo(workID) {
      const { data: res } = await axios.post(
        "http://localhost:5000/Work/ViewWorkInfo",
        { work_id: workID },
        { withCredentials: true }
      );
      this.WorkList = res;
      console.log(this.WorkList);
    },
    AbFormClose() {
      this.$refs.AbFormRef.resetFields();
    },
    updateAbsent() {
      this.$refs.AbFormRef.validate(async (valid) => {
        if (!valid) {
          //this.$message.error("请按照验证提示正确填写请假申请后再进行");
          this.$message({
          message: '请按照验证提示正确填写请假申请后再进行',
          type: 'error',
          duration:1000
        });
          return;
        }
        if (this.AbForm.proof == "") {
          this.AbForm.proof = "无备注";
        }
        const {
          data: res,
        } = await axios.post(
          "http://localhost:5000/Leave/UpdateLeave",
          this.AbForm,
          { withCredentials: true }
        );
        //this.$message.success("请假申请更新成功!");
        this.$message({
          message: '请假申请更新成功',
          type: 'success',
          duration:1000
        });
        this.AbsentVisible = false;
        this.loading=false;
      });
    },
  },
};
</script>

<style scoped>
.workCard {
  margin-top: 20px;
  padding: 0;
}
.title {
  font-size: 25px;
  font-weight: bold;
}
.titlecol {
  margin-bottom: 1cm;
}
.inlineCol {
  text-align: center;
}
.inlineTitle {
  font-weight: bold;
  color: grey;
  margin-top: 1cm;
}
.inlineInfo {
  font-weight: normal;
  color: black;
}
.description {
  text-align: center;
  width: 60%;
  margin-left: 20%;
}
.inlineTitle{
    font-weight:bold;
    color: grey;
    margin-top:1cm;
}
.inlineInfo{
    font-weight: normal;
    color: black;
}
</style>

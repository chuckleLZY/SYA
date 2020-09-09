<template>
<div>
  <el-card>
  <el-table
    :data="tableData"
    style="width: 100%">
    <el-table-column type="expand">
      <template slot-scope="props">
        <el-form label-position="left" inline class="demo-table-expand">
          <el-form-item label="工作名称">
            <span>{{ props.row.work_name }}</span>
          </el-form-item>
          <el-form-item label="工作地点">
            <span>{{ props.row.address }}</span>
          </el-form-item>
          <el-form-item label="工薪薪资">
            <span>{{ props.row.salary }}</span>
          </el-form-item>
          <el-form-item label="工作时间">
            <span>{{ props.row.work_time }}</span>
          </el-form-item>
          <el-form-item label="工作描述">
            <span>{{ props.row.work_description }}</span>
          </el-form-item>
        </el-form>
      </template>
    </el-table-column>
    <el-table-column label="" type="index">
    </el-table-column>
    
    <el-table-column
      label="工作名称"
      prop="work_name">
    </el-table-column>
    <el-table-column
      prop="likes_num"
      label="点赞">
    </el-table-column>
    <el-table-column
      prop="collect_num"
      label="收藏">
    </el-table-column>
    <el-table-column
      prop="share_num"
      label="分享">
    </el-table-column>
    <el-table-column
      fixed="right"
      label="操作">
     
      <template slot-scope="scope">
        <el-button @click="editInfo(scope.row)" type="danger" size="small">编 辑</el-button>
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
  
<el-dialog title="修改" :visible.sync="DialogVisible">
  <el-form :model="editdata">
    <el-form-item label="工作ID" prop="work_id">
    <el-input type="textarea" disabled v-model="editdata.work_id" ></el-input>
  </el-form-item>
     <el-form-item label="工作名称" prop="work_name">
    <el-input type="textarea" disabled v-model="editdata.work_name" maxlength="15" show-word-limit ></el-input>
  </el-form-item>
  <el-form-item label="工作图像" prop="new_cover">
     <el-select v-model="editdata.new_cover" placeholder="请选择工作封面">
      <el-option label="封面一" value="http://photo.tongji.edu.cn/__local/8/E4/D1/653D3735DC6F0691C6B2C1D4089_8C3254E4_59616.jpg"></el-option>
      <el-option label="封面二" value="http://photo.tongji.edu.cn/__local/4/34/82/827E6293ACF9838C9C6F4D455B6_C35BBE06_74B1C.jpg"></el-option>
      <el-option label="封面三" value="http://photo.tongji.edu.cn/__local/F/94/46/A22A1DF1FEF48DCCD579B1121F9_A1090C4F_69757.jpg"></el-option>
    </el-select>
  </el-form-item>
  <el-form-item label="工作地点" prop="new_address">
  <el-input v-model="editdata.new_address" ></el-input>
  </el-form-item>
  <el-form-item label="工作时间" prop="new_time">
  <el-input v-model="editdata.new_time" ></el-input>
  </el-form-item>
  <el-form-item label="工作薪资（元/天）" prop="new_salary">
    <el-input v-model="editdata.new_salary" ></el-input>
  </el-form-item>
  <el-form-item label="工作描述" prop="new_desc">
    <el-input type="textarea" v-model="editdata.new_desc" maxlength="100" show-word-limit :autosize="{ minRows: 3, maxRows: 6}"></el-input>
  </el-form-item>
  </el-form>
  <div slot="footer" class="dialog-footer">
    <el-button @click="DialogVisible = false">取 消</el-button>
    <el-button type="primary" @click="Editework()">编 辑</el-button>
  </div>
</el-dialog>

  </div>
</template>


<script>
import axios from "axios";
export default {
        data() {
          return {
           DialogVisible: false,
           pageInfo: {
        pagenum: 1,
        pagesize: 10
      },
      total: 0,
     editdata:{},
        tableData: [
          {
            "work_id": 5,
            "work_name": "run",
            "cover": "http://photo.tongji.edu.cn/__local/F/94/46/A22A1DF1FEF48DCCD579B1121F9_A1090C4F_69757.jpg",
            "work_description": "GOOD",
            "address": "TJU",
            "salary": 50,
            "work_time": "MON",
            "likes_num": 1,
            "collect_num": 1,
            "share_num": 1
        },
        ],
         form: {
          name: '',
          cover:'',
          address:'',
          time:'',
          salary:'',
          desc: ''
        },
      }
    },
    methods:{
    async getWorkList() {
      const result = await axios.post(
        "http://localhost:5000/Work/ViewHistoryWork",
        this.pageInfo,
        {
          withCredentials: true
        }
      );
      this.total = result.data.totalpage;
      console.log(result);
      this.tableData = result.data.worklist;
      
      //取消加载的转圈圈
      this.loading = false;
    },

      //获取一页的申请信息
    async getOnePageworklist() {
      const result = await axios.post(
        "http://localhost:5000/Work/ViewHistoryWork",
        this.pageInfo,
        {
          withCredentials: true
        }
      );
      console.log(result);
      this.tableData = result.data.worklist;
    
      //取消加载的转圈圈
      this.loading = false;
    },
//编辑信息
    editInfo(row){
      this.editdata = row;
      this.DialogVisible = true;
     
    },

      // 编辑工作
      async Editework() {
      console.log(this.form);
      const result = await axios.post(
        "http://localhost:5000/Work/ChangeWorkInfo",
        {
          work_id:this.editdata.work_id,
          work_name: this.editdata.work_name,
          cover: this.editdata.new_cover,
          work_description: this.editdata.new_desc,
          address: this.editdata.new_address,
          salary: parseInt(this.editdata.new_salary),
          work_time: this.editdata.new_time,
        },
        { withCredentials: true }
      );
      console.log(result);
      if (result.status == 200) {
        this.$message.success("修改成功");
        this.Dialogvisible = false
      }
      else{
        this.$message.error("发生了一些错误");
      }
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
      },
        async mounted() {
    await this.getWorkList();
    // console.log(this.tableData);
  }

    
    
}
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
</style>

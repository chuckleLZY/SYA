<template>
  <div>
    <!-- 卡片视图 -->
    <el-card>
    <el-button class="newfav" type="success" size="mini" icon="el-icon-s-order" @click="dialog = true">收藏夹的编辑</el-button>
      <!-- <el-button type="text" @click="Create()">新建收藏夹</el-button> -->
      <!-- <el-button type="text" @click="dialog = true">新建收藏夹</el-button> -->
      <br><br>
      <el-row :gutter="20">
        <el-col :span="6">
          <el-input placeholder="请输入内容" v-model="queryInfo.query" clearable @clear="getGoodsList">
            <el-button slot="append" icon="el-icon-search" @click="getGoodsList"></el-button>
          </el-input>
        </el-col>
      </el-row>
      <!-- 表格数据 -->
      <el-table :data="FavoriteList" border stripe>
        <el-table-column type="index"></el-table-column>
        <el-table-column label="收藏夹名称" prop="work_name" width="400px"></el-table-column>
        <el-table-column label="工作数量" prop="salary" width="400px"></el-table-column>

        <el-table-column label="操作" width="295px">
          <template slot-scope="scope">
            <el-button type="primary" icon="el-icon-edit" size="mini" @click="dialog = true">查看收藏夹具体工作</el-button>
          </template>
        </el-table-column>
        <!-- <el-table-column label="操作" width="340px">
                    <template slot-scope="scope">
                          <el-button type="primary" size="mini" icon="el-icon-search">查看详情</el-button>
                          <el-button type="warning" size="mini" icon="el-icon-s-order" @click="showAbsent">申请请假</el-button>
                          <el-button type="danger" size="mini" icon="el-icon-s-release">我要辞职</el-button>
                    </template>
        </el-table-column> -->
                

      </el-table>

<el-drawer
  title="收藏夹的编辑"
  :before-close="handleClose"
  :visible.sync="dialog"
  direction="rtl"
  custom-class="demo-drawer"
  ref="drawer"
  >
  <div class="demo-drawer__content" size=40%>
    <el-form :model="form"  >
      <!-- 收藏夹id -->
    <el-form-item label="id" :label-width="formLabelWidth" prop="favid">
        <el-input v-model="form.favid" autocomplete="off" placeholder="请输入收藏夹id，为一个数字"></el-input>
    </el-form-item>
    <!-- 收藏夹名字 -->
    <el-form-item label="名字" :label-width="formLabelWidth" prop="favname">
        <el-input v-model="form.favname" autocomplete="off" placeholder="请输入收藏夹名字，长度为3-10字符"></el-input>
      </el-form-item>
      <!-- 工作数量 -->
      <el-form-item label="工作数量" :label-width="formLabelWidth" prop="favworknum">
        <el-input v-model="form.favworknum" autocomplete="off" placeholder="请输入工作数量，为一个数字"></el-input>
      </el-form-item>
    </el-form>
    <div class="demo-drawer__footer">
      <el-button @click="cancelForm">取消</el-button>
      <el-button type="info" @click="Update()">更新</el-button>
      <el-button type="success" @click="Create()">新建</el-button>
      <el-button type="danger" @click="Delete()">删除</el-button>
    </div>
  </div>
</el-drawer>
      <el-pagination
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
        :current-page="queryInfo.pagenum"
        :page-sizes="[5, 10, 15, 20]"
        :page-size="queryInfo.pagesize"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
        background
      ></el-pagination>
    </el-card>
  </div>
</template>

<script>
import axios from "axios";
export default {
  data () {
    return {
      queryInfo: {
        query: '',
        pagenum: 1,
        pagesize: 10
      },
       //收藏夹列表
      FavoriteList:[],

      dialog: false,
      loading: false,
      form: {
            favid:"51",
            favname:"pp",
            favworknum:"0"
      },
      formLabelWidth: '80px',
      timer: null,
    }
  },
  created(){
      this.getfavoritelist();
  },
  methods: {
    //获取favorite列表内容
    async getfavoritelist()
    {
        const res = await axios.post(
        "http://localhost:5000/Work/ViewOwnWork",
        {
          pagenum: this.queryInfo.pagenum,
          pagesize: this.queryInfo.pagesize
        },
        {
          withCredentials: true
        }
      );
       if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
        }
        this.FavoriteList=res.data.worklist;
        this.total=res.data.totalpage;
        this.pagesize=res.data.totalpage/res.data.pagenum;
        this.pagenum=res.data.pagenum;
    },
    // 删除收藏夹
    async Delete(){
       const result = await axios.put(
        "http://localhost:5000/Favorite/UpdateFavorite",
        {
          favorite_id: parseInt(this.form.favid),
          favorite_name: this.form.favname,
          work_num: parseInt(this.form.favworknum)
        },
        { withCredentials: true }
      );
      console.log(result);
      if (result.status == 200) {
        this.$message.success("删除成功");
      }
      else{
        this.$message.error("发生了一些错误");
      }
    },
    // 创建新的收藏夹
      async Create() {
      console.log(this.form);
      const result = await axios.post(
        "http://localhost:5000/Favorite/CreateFavorite",
        {
          favorite_id: parseInt(this.form.favid),
          favorite_name: this.form.favname,
          work_num: parseInt(this.form.favworknum)
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

    //更新收藏夹名字
    async Update(){
       const result = await axios.put(
        "http://localhost:5000/Favorite/UpdateFavorite",
        {
          favorite_id: parseInt(this.form.favid),
          favorite_name: this.form.favname,
          work_num: parseInt(this.form.favworknum)
        },
        { withCredentials: true }
      );
      console.log(result);
      if (result.status == 200) {
        this.$message.success("更新成功");
      }
      else{
        this.$message.error("发生了一些错误");
      }
    },
    cancelForm() {
      this.loading = false;
      this.dialog = false;
      clearTimeout(this.timer);
    }
  }
}
</script>

<style lang="less" scoped>
.newfav{
  float: left;
}
</style>

<template>
  <div>
    <!-- 卡片视图 -->
    <el-card>
    <el-button class="newfav" type="success" size="mini" icon="el-icon-s-order" @click="dialogCreateVisible = true">新建收藏夹</el-button>
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
        <el-table-column label="收藏夹名称" prop="favorite_name" width="400px"></el-table-column>
        <el-table-column label="工作数量" prop="work_num" width="400px"></el-table-column>
        <el-table-column label="操作" width="435px">
          <template slot-scope="scope_favorite">
            <el-button type="primary" icon="el-icon-edit" size="mini" @click="showDrawer(scope_favorite.row.favorite_id)">查看收藏夹具体工作</el-button>
            <el-button type="success" icon="el-icon-edit" size="mini" @click="showUpdateDialog(scope_favorite.row.favorite_id)">编辑</el-button>
            <el-button type="danger" icon="el-icon-edit" size="mini" @click="Delete(scope_favorite.row.favorite_id)">删除</el-button>
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
  title="收藏夹的工作内容!"
  :visible.sync="table"
  direction="rtl"
  size="50%">
   <el-table :data="Favorite_work" :model="Favorite_work_table">
      <el-table-column property="work_name" label="工作名字" v-model="Favorite_work_table.work_name" width="120"></el-table-column>
      <el-table-column property="address" label="工作地址" v-model="Favorite_work_table.address" width="120"></el-table-column>
      <el-table-column property="salary" label="工作薪水" v-model="Favorite_work_table.salary"  width="120"></el-table-column>
      <el-table-column property="work_time" label="工作时间" v-model="Favorite_work_table.work_time"  width="120"></el-table-column>
       <el-table-column label="操作" width="165px">
          <template slot-scope="scope_work">
            <el-button type="danger" icon="el-icon-edit" size="mini" @click="DeleteWork(scope_work.row.work_id)">从收藏夹中删除工作</el-button>
          </template>
        </el-table-column>
    </el-table>
    <!-- <el-pagination
        @size-change="handleSizeChange1"
        @current-change="handleCurrentChange1"
        :current-page="queryInfo.pagenum1"
        :page-sizes="[5, 10, 15, 20]"
        :page-size="queryInfo.pagesize1"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
        background
      ></el-pagination> -->
</el-drawer>

<el-dialog
  title="新建一个收藏夹"
  :before-close="handleClose"
  :visible.sync="dialogCreateVisible"
  direction="rtl"
  custom-class="demo-drawer"
  ref="drawer"
  >
  <div class="demo-drawer__content" size=40%>
    <el-form :model="form"  >
      <!-- 收藏夹id -->
    <!-- <el-form-item label="id" :label-width="formLabelWidth" prop="favid">
        <el-input v-model="form.favid" autocomplete="off" placeholder="请输入收藏夹id，为一个数字"></el-input>
    </el-form-item> -->
    <!-- 收藏夹名字 -->
    <el-form-item label="名字" :label-width="formLabelWidth" prop="favname">
        <el-input v-model="form.favname" autocomplete="off" placeholder="请输入收藏夹名字，长度为3-10字符"></el-input>
      </el-form-item>
      <!-- 工作数量 -->
      <!-- <el-form-item label="工作数量" :label-width="formLabelWidth" prop="favworknum">
        <el-input v-model="form.favworknum" autocomplete="off" placeholder="请输入工作数量，为一个数字"></el-input>
      </el-form-item> -->
    </el-form>
    <div class="demo-drawer__footer">
      <el-button @click="dialogCreateVisible = false">取消</el-button>
      <!-- <el-button type="info" @click="Update()">更新</el-button> -->
      <el-button type="success" @click="Create(scope)">新建</el-button>
      <!-- <el-button type="danger" @click="Delete()">删除</el-button> -->
    </div>
  </div>
</el-dialog>
<!-- 更新收藏夹的对话框 -->
<el-dialog
  title="更新这个收藏夹"
  :before-close="handleClose"
  :visible.sync="dialogUpdateVisible"
  width="50%"
  >
  <div class="demo-drawer__content" size=40%>
    <el-form :model="formUpdate"  >
    <!-- 收藏夹名字 -->
    <el-form-item label="名字" :label-width="formLabelWidth" prop="favname">
        <el-input v-model="formUpdate.favname" autocomplete="off" placeholder="请输入收藏夹名字，长度为3-10字符"></el-input>
      </el-form-item>
    </el-form>
    <div class="demo-drawer__footer">
      <el-button @click="dialogUpdateVisible = false">取消</el-button>
      <!-- <el-button type="info" @click="Update()">更新</el-button> -->
      <el-button type="success" @click="Update()">更新</el-button>
      <!-- <el-button type="danger" @click="Delete()">删除</el-button> -->
    </div>
  </div>
</el-dialog>
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
      Favorite_work_table:{
        id:Number
      },
       //收藏夹列表
      FavoriteList:[],
      dialogUpdateVisible: false,
      dialogCreateVisible: false,
      favoriteIdWork:Number,
      table: false,
      dialog: false,
      loading: false,
      form: {
            favname:"pp"
      },
      formUpdate:{
        id:Number,
        favname:"ppc"
      },
      formLabelWidth: '80px',
      timer: null,
    }
  },
  created(){
      this.getfavoritelist();
  },
  methods: {
      //监听每页条数选项改变的事
      handleSizeChange(newSize){
        console.log(newSize)
        this.queryInfo.pagesize=newSize
        this.getfavoritelist()
      },
      //   handleSizeChange1(newSize){
      //   console.log(newSize)
      //   this.queryInfo.pagesize1=newSize
      // },
      //监听页码改变的事件
      handleCurrentChange(newPage){
        //console.log(newPage)
        this.queryInfo.pagenum=newPage
        this.getfavoritelist()
      },
      // handleCurrentChange1(newPage){
      //   //console.log(newPage)
      //   this.queryInfo.pagenum1=newPage
      // },
    async showUpdateDialog(id){
      this.dialogUpdateVisible=true;
      this.formUpdate.id = id;
    },
    async showDrawer(favoriteid)
    {
      
     const res = await axios.post(
        "http://localhost:5000/Favorite/GetFavoriteInfo",
        {favorite_id:favoriteid},
        {withCredentials: true}
        );
        if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
        }

        this.Favorite_work=res.data.worklist;
        this.table =true;
        this.Favorite_work_table.id = favoriteid;
        // this.total=res.data.totalpage;
        // this.pagesize=res.data.totalpage/res.data.pagenum;
        // this.pagenum=res.data.pagenum;
    }, 
    //获取favorite列表内容
    async getfavoritelist()
    {
        const res = await axios.post(
        "http://localhost:5000/Favorite/GetFavorite",
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
        this.FavoriteList=res.data.favoriteItem;
        this.total=res.data.totalpage;
        this.pagesize=res.data.totalpage/res.data.pagenum;
        this.pagenum=res.data.pagenum;
    },
    // 删除收藏夹中的工作
    async DeleteWork(workIdDeleteWork){
         const result = await axios.post(
        "http://localhost:5000/Favorite/DeleteFavoriteWork",
        {
          favorite_id:this.Favorite_work_table.id ,
          work_id:workIdDeleteWork
        },
        { withCredentials: true }
      );
      console.log(result);
      if (result.status == 200) {
        this.$message.success("删除成功");
        getfavoritelist();
        showDrawer();
      }
      else{
        this.$message.error("发生了一些错误");
      }
    },
    // 删除收藏夹
    async Delete(favoriteIdDelete){
       const result = await axios.post(
        "http://localhost:5000/Favorite/DeleteFavorite",
        {
          favorite_id:favoriteIdDelete
        },
        { withCredentials: true }
      );
      console.log(result);
      if (result.status == 200) {
        this.$message.success("删除成功");
        getfavoritelist();
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
          // favorite_id: parseInt(this.form.favid),
          favorite_name: this.form.favname,
          // work_num: parseInt(this.form.favworknum)
        },
        { withCredentials: true }
      );
      console.log(result);
      if (result.status == 200) {
        this.$message.success("创建成功");
        cancelForm();
        getfavoritelist();
      }
      else{
        this.$message.error("发生了一些错误");
      }
    },
    //更新收藏夹名字函数1
    //更新收藏夹名字
    async Update(){
       const result = await axios.put(
        "http://localhost:5000/Favorite/UpdateFavorite",
        {
          favorite_id: this.formUpdate.id,
          favorite_name: this.formUpdate.favname
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

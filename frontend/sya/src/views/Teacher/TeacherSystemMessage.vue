<template>
    <div>
        这里是老师系统通知

        <el-breadcrumb separator-class="el-icon-arrow-right" class="recruitment_breadcrumb">
            <el-breadcrumb-item :to="{ path: '/home' }">老师</el-breadcrumb-item>
            <el-breadcrumb-item>消息</el-breadcrumb-item>
            <el-breadcrumb-item>系统通知</el-breadcrumb-item>
        </el-breadcrumb>


        <el-card class="student_card">
            <!--
            <el-row>
                <el-col :span="7">
                    <el-input placeholder="请输入内容" v-model="input3" class="input-with-select">
                        <el-select v-model="select" slot="prepend" placeholder="搜索类型">
                        <el-option label="工作类型" value="1"></el-option>
                        <el-option label="工作时间" value="2"></el-option>
                        <el-option label="工作地点" value="3"></el-option>
                        </el-select>
                        <el-button slot="append" icon="el-icon-search"></el-button>
                    </el-input>
                </el-col>
                <el-col :span="4"></el-col>
            </el-row>
            -->
            <!-- 列表区域 -->
            <el-table :data="workList" stripe>
                <el-table-column label="#" type="index"></el-table-column>
                <el-table-column label="发件人" prop="sender_id">

                </el-table-column>

                
                <el-table-column label="消息内容" prop="content">

                </el-table-column>

                <el-table-column label="发送时间" prop="message_time">

                </el-table-column>
                <el-table-column label="操作">
                    <template v-slot:default="scope"></template>
                            <el-tooltip  effect="dark" content="查看详情" placement="top-start" :enterable="false">
                                <el-button type="primary" icon="el-icon-edit" size="mini" circle></el-button>
                            </el-tooltip>
                            <el-tooltip  effect="dark" content="删除" placement="top-start" :enterable="false">
                                <el-button type="primary" icon="el-icon-delete" size="mini" circle></el-button>
                            </el-tooltip>
                </el-table-column> 

            </el-table>

            <!--  分页区域  -->
            <el-pagination
                @size-change="handleSizeChange"
                @current-change="handleCurrentChange"
                :current-page="queryInfo.pagenum"
                :page-sizes="[1, 2, 5, 10]"
                :page-size="queryInfo.pagesize"
                layout="total, sizes, prev, pager, next, jumper"
                :total="total">
            </el-pagination>
        </el-card>
        
    </div>
</template>


<script>
export default {
    data() {
    return {
      input1: '',
      input2: '',
      input3: '',
      select: '',

    //获取工作列表的参数对象
      queryInfo:{
          query: '',
          pagenum: 1,
          pagesize: 2
      },
      workList:[],
    }
  },
  created(){
      //调用获取发布的工作的API函数
     // this.getWorkList()
  },
  methods:{
      //监听每页条数选项改变的事件
      handleSizeChange(newSize){
        console.log(newSize)
        this.queryInfo.pagesize=newSize
      },
      //监听页码改变的事件
      handleCurrentChange(newPage){
        console.log(newPage)
        this.queryInfo.pagenum=newPage
      }
  //获取工作列表的函数
  //   async getWorkList(){
  //      const {data:res}= await this.$http.get('URL',{params:this.queryInfo})
   //   }
  }
}
</script>


<style scoped>

</style>

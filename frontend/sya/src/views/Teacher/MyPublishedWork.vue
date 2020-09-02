<template>
    <div>
        这里是我发布过的工作
    <el-table
    :data="tableData"
    border
    style="width: 90%">
    <el-table-column
      fixed
      prop="number"
      label="序号"
      width="70">
    </el-table-column>
    <el-table-column
      fixed
      prop="work_name"
      label="工作名称"
      width="170">
    </el-table-column>
    <el-table-column
      prop="address"
      label="工作地址"
      width="140">
    </el-table-column>
     <el-table-column
      prop="work_description"
      label="工作描述"
      width="230">
    </el-table-column>
    <el-table-column
      prop="likes_num"
      label="点赞"
      width="120">
    </el-table-column>
    <el-table-column
      prop="collect_num"
      label="收藏"
      width="120">
    </el-table-column>
    <el-table-column
      prop="share_num"
      label="分享"
      width="120">
    </el-table-column>
    <el-table-column
      fixed="right"
      label="操作"
      width="150">
      <template slot-scope="scope">
        <el-button @click="handleClick(scope.row)" type="text" size="small">查看</el-button>
        <el-button type="text" size="small">编辑</el-button>
      </template>
    </el-table-column>
  </el-table>
    </div>
</template>


<script>
export default {
       
    
}
</script>


<style scoped>


</style>

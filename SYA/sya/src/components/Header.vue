<template>
    <div>
        <el-menu
        class="el-menu-demo"
        mode="horizontal"
        background-color="#545c64"
        text-color="#fff"
        active-text-color="#ffd04b"
        router>
        <el-menu-item index="/Home">首页</el-menu-item>
        <el-menu-item index="/Recruitment">招聘会</el-menu-item>
        <el-menu-item index="/TeacherTable">老师工作台</el-menu-item>
        <el-menu-item index="/AdminTable">管理员工作台</el-menu-item>
        <el-menu-item index="/StudentMessage">学生消息</el-menu-item>
        <el-menu-item index="/TeacherMessage">老师消息</el-menu-item>
        <el-menu-item index="/AdminMessage">管理员消息</el-menu-item>
        <el-menu-item index="/StudentMine">学生我的</el-menu-item>
        <el-menu-item index="/TeacherMine">老师我的</el-menu-item>
        <el-menu-item index="/AdminMine">管理员我的</el-menu-item>
        <el-menu-item index="/StudentFavorites">学生收藏夹</el-menu-item>
        <el-menu-item index="/TeacherFavorites">老师收藏夹</el-menu-item>
        </el-menu>
    </div>
</template>


<script>
  export default {
    data() {
      return {

      };
    },
    methods: {

    }
  }
</script>
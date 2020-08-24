<template>
  <div>
    <el-breadcrumb
      separator-class="el-icon-arrow-right"
      style="margin-bottom: 15px;font-size: 12px;"
    >
      <el-breadcrumb-item>首页</el-breadcrumb-item>
      <el-breadcrumb-item>工作台</el-breadcrumb-item>
      <el-breadcrumb-item>查看收到的简历</el-breadcrumb-item>
    </el-breadcrumb>

    <el-card>
      <template>
        <el-table :data="tableData" style="width: 100%">
          <el-table-column label="#" type="index" width="180px"></el-table-column>
          <el-table-column prop="name" label="姓名" width="280px"></el-table-column>
          <el-table-column prop="age" label="年龄" width="280px"></el-table-column>
          <el-table-column prop="edit" label="查看详情" width="280px">
            <template slot-scope="scope">
              <el-tooltip effect="dark" content="查看" placement="top">
                <el-button type="primary" icon="el-icon-edit" size="mini" @click="check(scope.row)"></el-button>
              </el-tooltip>
            </template>
          </el-table-column>
        </el-table>
      </template>
    </el-card>

    <el-dialog title="详情" :visible.sync="checkDialogVisible" width="30%">
      <!-- <span>这是一段信息</span> -->
      <el-form label-position="left" label-width="80px" :model="editForm">
        <el-form-item label="姓名">
          <el-input v-model="editForm.name" disabled></el-input>
        </el-form-item>
        <el-form-item label="年龄">
          <el-input v-model="editForm.age" disabled></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="checkDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="editClickTrue()">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>


<script>
import { METHODS } from "http";
export default {
  data() {
    return {
      tableData: [
        {
          name: "fc1",
          age: "18"
        },
        {
          name: "fc2",
          age: "19"
        },
        {
          name: "fc3",
          age: "20"
        }
      ],
      checkDialogVisible: false,
      editForm: {},
    };
  },
  methods: {
    check(row) {
      console.log(row.name);
      this.editForm = row;
      this.checkDialogVisible = true;
    },
    editClickTrue() {
      this.checkDialogVisible = false;
      this.$message.success("成功");
    }
  }
};
</script>


<style scoped>
.el-card {
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.15) !important;
}
</style>

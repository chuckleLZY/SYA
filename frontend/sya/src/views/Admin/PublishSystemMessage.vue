<template>
    <div>
        发布系统公告
        <el-breadcrumb separator-class="el-icon-arrow-right" style="margin-bottom: 15px;font-size: 12px;">
        <el-breadcrumb-item >首页</el-breadcrumb-item>
        <el-breadcrumb-item>管理员工作台</el-breadcrumb-item>
        <el-breadcrumb-item>发布系统公告</el-breadcrumb-item>
        </el-breadcrumb>

        <el-card>
            <el-row >
            <el-col :span="6" style="width: 0;margin-bottom:10px"><div>
                <el-button type="primary" @click="dialogVisible=true">发布公告</el-button>
            </div></el-col>
            </el-row>
            <template>
                <el-table
                :data="tableData"
                style="width: 100%">
                <el-table-column label="#" type="index" width="180px"></el-table-column>
                <el-table-column prop="title" label="标题" width="280px"></el-table-column>
                <el-table-column prop="date" label="时间" width="280px"></el-table-column>
                <el-table-column prop="edit" label="编辑" width="280px">
                    <template>
                        <el-tooltip effect="dark" content="编辑" placement="top">
                        <el-button type="primary" icon="el-icon-edit" size="mini"></el-button>
                        </el-tooltip>
                        <el-tooltip effect="dark" content="删除" placement="top">
                        <el-button type="danger" icon="el-icon-delete" size="mini"></el-button>
                        </el-tooltip>
                    </template>
                </el-table-column>
                </el-table>
            </template>
        </el-card>


        <!-- 弹窗 -->
        <el-dialog
        title="发布公告"
        :visible.sync="dialogVisible"
        width="30%">
        <!-- <span>这是一段信息</span> -->
        <el-form :model="addForm" status-icon :rules="addFormRules" addFormRef="ruleForm" label-width="90px" label-position="left">
        <el-form-item label="标题">
            <el-input v-model="addForm.title" autocomplete="off"></el-input>
        </el-form-item>
        <el-form-item label="内容">
            <el-input v-model="addForm.message" autocomplete="off"></el-input>
        </el-form-item>
        </el-form>

        
        <span slot="footer" class="dialog-footer">
            <el-button @click="dialogVisible = false">取 消</el-button>
            <el-button type="primary" @click="dialogVisible = false">确 定</el-button>
        </span>
        </el-dialog>
    </div>
</template>


<script>
export default {
        data() {
        return {
          tableData: [{
            title: "标题1",
            date: '2020-01-01',
          },
          {
            title: "标题2",
            date: '2020-01-02',
          },
          {
            title: "标题3",
            date: '2020-01-03',
          },
          ],
          dialogVisible:false,
          //表单数据
          addForm:{
              title:'',
              message:'',
          },
          //规则
          addFormRules:{

          },
          //引用
          addFormRef:{

          }
        }
      }
}
</script>


<style scoped>

.el-card{
    box-shadow: 0 1px 1px rgba(0, 0, 0, 0.15) !important;
}

</style>

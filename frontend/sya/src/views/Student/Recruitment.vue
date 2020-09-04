<template>
    <div>
        这里是招聘会
    </div>
</template>


<script>
export default {
<<<<<<< Updated upstream
=======
  data() 
  {
    return {
      input1: '',
      input2: '',
      input3: '',
      select: '',

    //获取工作列表的参数对象
      queryInfo:{
          
          pagenum: 1,
          pagesize: 3,
          query: ''
      },
      workList:[],
      total:0,
      drawer: false,
      workInfo:{}
    }
  },
  created(){
      //调用获取发布的工作的API函数
    //  this.getWorkInfo()
      this.getWorkList()
      
  },
  methods:{
      //监听每页条数选项改变的事件
      handleSizeChange(newSize){
        console.log(newSize)
        this.queryInfo.pagesize=newSize
        this.getWorkList()
      },
      //监听页码改变的事件
      handleCurrentChange(newPage){
        //console.log(newPage)
        this.queryInfo.pagenum=newPage
        this.getWorkList()
      },
      
        handleClose(done) {
            this.$confirm('确认关闭？')
            .then(_ => {
                done();
            })
            .catch(_ => {})
        },
     //展示右侧弹窗
    async showDrawer(workid){
        //console.log(workid)
        const res = await axios.post(
        "http://localhost:5000/Work/ViewWorkInfo",
        {work_id:workid},
        {withCredentials: true}
        );
        if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
        }

        this.workInfo=res.data

      //console.log(res);
     this.drawer=true
    },
  //获取工作列表的函数
    async getWorkList(){
      /*  const res= await axios.post("http://localhost:5000/Work/ViewOwnWork",{pagenum: 1,
          pagesize: 2},{withCredetials: true });
        if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
        }
        this.workList=res.data.worklist;
        this.total=res.data.totalpage;
        console.log(res);*/

        const res = await axios.post(
        "http://localhost:5000/Work/ViewAllWork",
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
        this.workList=res.data.worklist;
        this.total=res.data.totalpage;
        this.pagesize=res.data.totalpage/res.data.pagenum;
        this.pagenum=res.data.pagenum;
       // console.log(this.pagesize);
      //  console.log(res);
      },

    async findWork(){
        const res=await axios.post('http://localhost:5000/Work/FindWork',{
                    pagenum: this.queryInfo.pagenum,
                     pagesize: this.queryInfo.pagesize,
                    query: this.queryInfo.query
            },{withCredentials: true});
        if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
        }
        this.workList=res.data.worklist;
        this.total=res.data.totalpage;
       // console.log(res);
    },
    async appWork(){
        const res=await axios.post('http://localhost:5000/Apply/CreateApply',{work_id:this.workInfo.work_id},{
          withCredentials: true
        });
        if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
        }
        console.log(res);
        this.$message.success('申请此工作成功');
    }
     

>>>>>>> Stashed changes
    
}
</script>


<style scoped>
.outside {
  margin-left: 200px;
  margin-right: 500px;
  margin-top: 20px;
}
</style>

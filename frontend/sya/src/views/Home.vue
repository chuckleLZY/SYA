<template>
  <div class="cha_img">
    <!--动态将图片轮播图的容器高度设置成与图片一致-->
    <el-carousel :interval="4000" type="card" height="400px">
      <el-carousel-item v-for="item in imagebox" :key="item.id">
        <img :src="item.idView" class="image" />
      </el-carousel-item>
    </el-carousel>
    <el-container>
      <el-header>
        <el-row :gutter="40" type="flex" class="row-bg" justify="center">
          <el-col :span="10">
            <div class="grid-content bg-purple-light">最新工作招聘信息</div>
          </el-col>
          <el-col :span="10">
            <div class="grid-content bg-purple-light">系统公告</div>
          </el-col>
        </el-row>
      </el-header>
      <el-divider></el-divider>
      <el-main>
        <el-row :gutter="40" type="flex" class="row-bg1" justify="center">
          <el-col :span="10">
            <div class="grid-content bg-purple-light">
              <div>
                <span v-if="workList[0]">
                  <i class="el-icon-wallet"></i>
                  &emsp;{{workList[0].address}}发布新工作:{{workList[0].work_name}}
                </span>
                <!-- <p>
                  {{workList[0].work_description}}aaaaa
                </p>-->
                <el-divider></el-divider>
                <span v-if="workList[1]">
                  <i class="el-icon-wallet"></i>
                  &emsp;{{workList[1].address}}发布新工作:{{workList[1].work_name}}
                </span>
                <el-divider></el-divider>
                <span v-if="workList[2]">
                  <i class="el-icon-wallet"></i>
                  &emsp;{{workList[2].address}}发布新工作:{{workList[2].work_name}}
                </span>
                <el-divider></el-divider>
                <span v-if="workList[3]">
                  <i class="el-icon-wallet"></i>
                  &emsp;{{workList[3].address}}发布新工作:{{workList[3].work_name}}
                </span>
                <el-divider></el-divider>
                <span>
                  <el-link type="primary" @click="moreRecruitment()">点击查看更多>></el-link>
                </span>
              </div>
            </div>
          </el-col>
          <el-col :span="10">
            <div class="grid-content bg-purple-light">
              <div>
                <span v-if="annoList[0]">
                  <i class="el-icon-message-solid"></i>
                  &emsp;{{annoList[0].send_time}}新公告:{{annoList[0].title}}
                </span>
                <el-divider></el-divider>
                <span v-if="annoList[1]">
                  <i class="el-icon-message-solid"></i>
                  &emsp;{{annoList[1].send_time}}新公告:{{annoList[1].title}}
                </span>
                <el-divider></el-divider>
                <span v-if="annoList[2]">
                  <i class="el-icon-message-solid"></i>
                  &emsp;{{annoList[2].send_time}}新公告:{{annoList[2].title}}
                </span>
                <el-divider></el-divider>
                <span v-if="annoList[3]">
                  <i class="el-icon-message-solid"></i>
                  &emsp;{{annoList[3].send_time}}新公告:{{annoList[3].title}}
                </span>
                <el-divider></el-divider>
                <span>
                  <el-link type="primary" @click="moreStudentSystemMessage()">点击查看更多>></el-link>
                </span>
              </div>
            </div>
          </el-col>
        </el-row>
        <el-divider></el-divider>
        <div style="text-align:center">
            <p class="syaSup">SYA——Show Your Ability</p>
        </div>
      </el-main>
    </el-container>
  </div>
</template>


<script>
import axios from "axios";
export default {
  // name: "zmd",
  data() {
    return {
      imagebox: [
        { id: 0, idView: require("../assets/imagebox/1.jpeg") },
        { id: 1, idView: require("../assets/imagebox/2.jpeg") },
        { id: 2, idView: require("../assets/imagebox/3.png") },
        { id: 3, idView: require("../assets/imagebox/4.jpg") }
        //imagebox是assets下一个放图片的文件夹
      ],
      screenWidth: 0,
      workList: [],
      annoList: [],
      totalpage : 0,
      totalpage2: 0,
      count : 0,
    };
  },
  created() {
    //调用获取发布的工作的API函数
    //  this.getWorkInfo()
    this.getWorkList(), this.getAnnounce();
  },
  methods: {
    setSize: function() {
      // 通过浏览器宽度(图片宽度)计算高度
      this.bannerHeight = (400 / 1920) * this.screenWidth;
    },
    async getWorkList() {
      var aaa = await axios.post(
        this.$appconfig.toAbsUrl("/Work/ViewAllWork"),
        {
          pagenum: 1,
          pagesize: 1
        },
        {
          withCredentials: true
        }
      );
      // console.log(res);
      if (aaa.status !== 200) {
        this.$message.error("Unexpected response");
        return;
      }
      this.totalpage=aaa.data.totalpage;
      //console.log(aaa.data.totalpage);
      //console.log(this.totalpage);
      const res = await axios.post(
        this.$appconfig.toAbsUrl("/Work/ViewAllWork"),
        {
          pagenum: 1,
          pagesize: this.totalpage
        },
        {
          withCredentials: true
        }
      );
      //console.log(res);
      if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
      }
        this.workList[0] = res.data.worklist[this.totalpage-1];
        this.workList[1] = res.data.worklist[this.totalpage-2];
        this.workList[2] = res.data.worklist[this.totalpage-3];
        this.workList[3] = res.data.worklist[this.totalpage-4];
      //console.log(res);
      this.$forceUpdate();
      //console.log(this.workList);
    },
    async getAnnounce() {
      if (this.$store.state.role == 0) {
        var bbb;
        bbb = await axios.post(
          this.$appconfig.toAbsUrl("/Announce/GetSendAnnounce"),
          {
            pagenum: 1,
            pagesize: 1
          },
          {
            withCredentials: true
          }
        );
        // console.log("res111", res);
      } else {
        bbb = await axios.post(
          this.$appconfig.toAbsUrl("/Announce/GetAnnounce"),
          {
            pagenum: 1,
            pagesize: 1
          },
          {
            withCredentials: true
          }
        );
        // console.log("res222", res);
      }
      //console.log("res",res);
      if (bbb.status !== 200) {
        this.$message.error("Unexpected response");
        return;
      }
      this.totalpage2=bbb.data.totalpage;
      //console.log(this.totalpage2);
      if (this.$store.state.role == 0) {
        var res;
        res = await axios.post(
          this.$appconfig.toAbsUrl("/Announce/GetSendAnnounce"),
          {
            pagenum: 1,
            pagesize: this.totalpage2
          },
          {
            withCredentials: true
          }
        );
        // console.log("res111", res);
      } else {
        res = await axios.post(
          this.$appconfig.toAbsUrl("/Announce/GetAnnounce"),
          {
            pagenum: 1,
            pagesize: this.totalpage2
          },
          {
            withCredentials: true
          }
        );
        // console.log("res222", res);
      }
      //console.log("res",res);
      if (res.status !== 200) {
        this.$message.error("Unexpected response");
        return;
      }
      //this.annoList = res.data.announceItem;
      this.annoList[0] = res.data.announceItem[this.totalpage2-1];
      this.annoList[1] = res.data.announceItem[this.totalpage2-2];
      this.annoList[2] = res.data.announceItem[this.totalpage2-3];
      this.annoList[3] = res.data.announceItem[this.totalpage2-4];
      this.$forceUpdate();
      // this.pagesize=res.data.totalpage/res.data.pagenum;
      // this.pagenum=res.data.pagenum;
      // console.log("annolist", this.annoList);
      // console.log(res);
    },
    moreRecruitment() {
      this.$router.push("/Recruitment");
    },
    moreStudentSystemMessage() {
      if (this.$store.state.role == 1) {
        this.$router.push("/StudentSystemMessage");
      } else if (this.$store.state.role == 2) {
        this.$router.push("/TeacherSystemMessage");
      }else{
        this.$router.push("/PublishSystemMessage");
      }
    }
  },
  mounted() {
    // 首次加载时,需要调用一次
    this.screenWidth = window.innerWidth;
    this.setSize();
    // 窗口大小发生改变时,调用一次
    window.onresize = () => {
      this.screenWidth = window.innerWidth;
      this.setSize();
    };
  }
};
</script>


<style scoped>
.el-carousel__item h3 {
  color: #475669;
  font-size: 14px;
  opacity: 0.75;
  line-height: 200px;
  margin: 0;
}

.el-carousel__item:nth-child(2n) {
  background-color: #99a9bf;
}

.el-carousel__item:nth-child(2n + 1) {
  background-color: #d3dce6;
}
img {
  /*设置图片宽度和浏览器宽度一致*/
  width: 100%;
  height: inherit;
}
.cha_img {
  margin-top: 50px;
  margin-left: 100px;
  margin-right: 100px;
}
.el-row {
  margin-bottom: 20px;
  margin-top: 20px;
  /* &:last-child 
  {
    margin-bottom: 0;
  } */
}
.el-col {
  border-radius: 4px;
}
.bg-purple {
  background: #d3dce6;
}
.grid-content {
  border-radius: 4px;
  min-height: 36px;
}
.el-header {
  color: #333;
  text-align: center;
  line-height: 60px;
  margin-top: 50px;
  font-size: 20px;
}
.el-main {
  background-color: #e9eef3;
  color: #333;
  text-align: center;
  line-height: 50px;
  height: 1000px;
}
</style>

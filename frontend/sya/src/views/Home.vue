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
                <span>
                  <i class="el-icon-wallet"></i>&emsp;青春是一个短暂的美梦, 当你醒来时, 它早已消失无踪
                </span>
                <el-divider></el-divider>
                <span>
                  <i class="el-icon-wallet"></i>&emsp;少量的邪恶足以抵消全部高贵的品质, 害得人声名狼藉
                </span>
                <el-divider></el-divider>
                <span>
                  <i class="el-icon-wallet"></i>&emsp;青春是一个短暂的美梦, 当你醒来时, 它早已消失无踪
                </span>
                <el-divider></el-divider>
                <span>
                  <i class="el-icon-wallet"></i>&emsp;少量的邪恶足以抵消全部高贵的品质, 害得人声名狼藉
                </span>
                <el-divider></el-divider>
                <span>
                  <el-link type="primary">点击查看更多>></el-link>
                </span>
              </div>
            </div>
          </el-col>
          <el-col :span="10">
            <div class="grid-content bg-purple-light">
              <span>
                <i class="el-icon-message-solid"></i>&emsp;青春是一个短暂的美梦, 当你醒来时, 它早已消失无踪
              </span>
              <el-divider></el-divider>
              <span>
                <i class="el-icon-message-solid"></i>&emsp;少量的邪恶足以抵消全部高贵的品质, 害得人声名狼藉
              </span>
              <el-divider></el-divider>
              <span>
                <i class="el-icon-message-solid"></i>&emsp;青春是一个短暂的美梦, 当你醒来时, 它早已消失无踪
              </span>
              <el-divider></el-divider>
              <span>
                <i class="el-icon-message-solid"></i>&emsp;少量的邪恶足以抵消全部高贵的品质, 害得人声名狼藉
              </span>
              <el-divider></el-divider>
              <span>
                <el-link type="primary">点击查看更多>></el-link>
              </span>
            </div>
          </el-col>
        </el-row>
        <el-divider></el-divider>
      </el-main>
    </el-container>
  </div>
</template>


<script>
export default {
  name: "zmd",
  data() {
    return {
      imagebox: [
        { id: 0, idView: require("../assets/imagebox/1.jpeg") },
        { id: 1, idView: require("../assets/imagebox/2.jpeg") },
        { id: 2, idView: require("../assets/imagebox/3.png") },
        { id: 3, idView: require("../assets/imagebox/4.jpg") },
        //imagebox是assets下一个放图片的文件夹
      ],
      screenWidth: 0,
    };
  },
  methods: {
    setSize: function () {
      // 通过浏览器宽度(图片宽度)计算高度
      this.bannerHeight = (400 / 1920) * this.screenWidth;
    },
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
  },
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
  margin-top: 20px;
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
  margin-top: 20px;
  font-size: 20px;
}
.el-main {
  background-color: #e9eef3;
  color: #333;
  text-align: center;
  line-height: 50px;
  height: 800px;
}
</style>

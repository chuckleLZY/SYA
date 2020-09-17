import Vue from "vue";
import config from "./config.json";

const helperFunctions = {
  endpointUrl: (endpoint) => (config.API_URL + endpoint),
  ossFileUrl: (filename) => (config.OSS_URL + filename)
}

const helper = {
  install: (Vue) => {
    Vue.helper = helperFunctions;
    Vue.prototype.$helper = helperFunctions;
  }
};

Vue.use(helper);

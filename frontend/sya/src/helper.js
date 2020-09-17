import Vue from "vue";
import config from "./config.json";

const helperFunctions = {
  endpointUrl: (relative) => {
    return config.API_URL + relative;
  },
}

const helper = {
  install: (Vue) => {
    Vue.helper = helperFunctions;
    Vue.prototype.$helper = helperFunctions;
  }
};

Vue.use(helper);

import Vue from "vue";

const configEntries = {
  API_URL: "http://localhost:5001",
};

const configFunctions = {
  toAbsUrl: (relative) => {
    return configEntries.API_URL + relative;
  },
}

const appconfig = {
  install: (Vue) => {
    Vue.appconfig = configFunctions;
    Vue.prototype.$appconfig = configFunctions;
  }
};

Vue.use(appconfig);

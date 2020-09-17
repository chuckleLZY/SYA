import OSS from "ali-oss";
import config from "./config.json"

export function client() {
  var client = new OSS(config.OSS_CONF);
  return client;
}

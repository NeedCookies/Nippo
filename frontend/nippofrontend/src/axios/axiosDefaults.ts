import axios from "axios";

axios.defaults.baseURL = "http://localhost:5042";

axios.interceptors.request.use(config => {
    config.withCredentials = true;
    return config;
  }, error => {
    return Promise.reject(error);
  });
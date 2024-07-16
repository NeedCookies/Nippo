import axios from "axios";

axios.defaults.baseURL = "http://localhost/api";

axios.interceptors.request.use(function (config) {
        config.withCredentials = true; 

        return config;
    }, function(error){
        return Promise.reject(error);
    });

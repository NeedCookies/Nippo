import axios from "axios";

axios.defaults.baseURL = "http://localhost:8080";

axios.interceptors.request.use(function (config) {
        config.withCredentials = true; 

        return config;
    }, function(error){
        return Promise.reject(error);
    });

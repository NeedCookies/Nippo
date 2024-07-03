import axios from "axios";

axios.defaults.baseURL = "http://localhost:8080";

axios.interceptors.request.use(function (config) {
        const accessToken = sessionStorage.getItem('accessToken');
        if (accessToken && accessToken.length != 0){
            config.headers.Authorization = `Bearer ${accessToken}`;
        }
        return config;
    }, function(error){
        return Promise.reject(error);
    });


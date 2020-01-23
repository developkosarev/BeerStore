import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store/store'

import BootstrapVue from 'bootstrap-vue'
import "font-awesome/css/font-awesome.min.css"

import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';

import axios from 'axios';

Vue.use(BootstrapVue);

// filters
import { currency, date } from "./filters";

Vue.filter("currency", currency);
Vue.filter("date", date);

const debug = process.env.NODE_ENV !== 'production' 
//const baseURL = debug ? 'http://localhost:50202/api/' : 'http://newbeerstore.chance-ltd.ru/api/';
const baseURL = 'http://localhost:50202/api/';

Vue.http = axios.create({
  baseURL: baseURL,
  timeout: 10000,
});

Vue.http.interceptors.response.use(undefined, function(error) {
   const originalRequest = error.config;

   if (error.response.status === 401 && !originalRequest._retry && store.getters['auth/refreshToken']) {
     originalRequest._retry = true;

     const payload = {
       token: store.getters['auth/token'],
       refreshToken: store.getters['auth/refreshToken']
     };

      return Vue.http
        .post("/tokenapi/refresh", payload)
        .then(response => {
          const auth = response.data;
          Vue.http.defaults.headers.common["Authorization"] = `Bearer ${
            auth.token
          }`;
          originalRequest.headers["Authorization"] = `Bearer ${
            auth.token
          }`;
          store.commit("auth/loginSuccess", auth);
          localStorage.setItem("auth", JSON.stringify(auth));

          return Vue.http(originalRequest);
        })
        .catch(error => {
          store.commit("auth/logout");
          router.push({ path: "/" });
          delete Vue.http.defaults.headers.common["Authorization"];
          return Promise.reject(error);
        });

   }

  return Promise.reject(error);
});

const initialAuth = localStorage.getItem("auth");
if (initialAuth) {
  store.dispatch("auth/initializeAuth");
}

Vue.config.productionTip = true

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')

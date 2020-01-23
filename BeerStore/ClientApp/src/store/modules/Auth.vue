<script>
import Vue from 'vue';

export default {
    namespaced: true,
    state: {
        loading: false,
        showAuthModal: false,
        auth: null
    },
    getters: {
        loading(state){
          return state.loading;
        },
        showAuthModal(state) {
          return state.showAuthModal;
        },
        isAuthenticated(state) {
          return state.auth !== null && state.auth.token !== null;

          //return state.auth !== null && state.auth.access_token !== null;
        },
        isInRole: (state, getters) => (role) => {		  
          return getters.isAuthenticated && state.auth.roles.indexOf(role) > -1;

          //const result = state.getters.isAuthenticated;
          //return result;
        },
        token(state){
            return state.auth.token;
        },
        refreshToken(state){
          return state.auth.refreshToken;
        }
    },
    mutations: {
        showAuthModal(state){
          state.showAuthModal = true;
        },
        hideAuthModal(state){
          state.showAuthModal = false;
        },
        loginRequest(state) {
            state.loading = true;
        },
        loginSuccess(state, payload) {
          state.auth = payload;
          state.loading = false;
        },
        loginError(state) {
          state.loading = false;
        },
        logout(state) {
          state.auth = null;
        },
        registerRequest(state) {
            state.loading = true;
        },
        registerSuccess(state) {
            state.loading = false;
        },
        registerError(state) {
            state.loading = false;
        },
        forgotRequest(state) {
            state.loading = true;
        },
        forgotSuccess(state) {
            state.loading = false;
        },
        forgotError(state) {
            state.loading = false;
        },
        resetRequest(state) {
          state.loading = true;
        },
        resetSuccess(state) {
          state.loading = false;
        },
        resetError(state) {
          state.loading = false;
        },
        setAuthData(state, payload){
          state.auth = payload;
        }
    },
    actions: {
      login(store, payload) {
        return new Promise((resolve, reject) => {
          store.commit("loginRequest");
          Vue.http
            .post("/tokenapi/create?includeRefreshToken=true", payload)
            .then(response => {
              const auth = response.data;
              Vue.http.defaults.headers.common["Authorization"] = `Bearer ${auth.token}`;
              store.commit("loginSuccess", auth)
              store.commit("hideAuthModal")
              store.dispatch("storeAuthData", auth)
              resolve(response);
            })
            .catch(error => {
              // eslint-disable-next-line
              console.log(error)
              store.commit("loginError")
              delete Vue.http.defaults.headers.common["Authorization"];
              reject(error.response);
            });
        });
      },
      logout(store) {
        store.commit("logout");
        delete Vue.http.defaults.headers.common["Authorization"];
        store.dispatch("clearAuthData")
      },
      register(store, payload) {
        return new Promise((resolve, reject) => {
          store.commit("registerRequest");
          Vue.http
            .post("/auth/register", payload)
            .then(response => {
              store.commit("registerSuccess");
              resolve(response);
            })
            .catch(error => {
              store.commit("registerError");
              reject(error.response);
            });
        });
      },
      forgotPassword(store, payload) {
        return new Promise((resolve, reject) => {
          store.commit("forgotRequest");
          Vue.http
            .post("/auth/forgot", payload)
            .then(response => {
              store.commit("forgotSuccess");
              resolve(response);
            })
            .catch(error => {
              // eslint-disable-next-line
              store.commit("forgotError");
              reject(error.response);
            });

        });
      },
      resetPassword(store, payload) {
        return new Promise((resolve, reject) => {
          store.commit("resetRequest");
          Vue.http
            .post("/auth/reset", payload)
            .then(response => {
              store.commit("resetSuccess");
              resolve(response);
            })
            .catch(error => {
              // eslint-disable-next-line
              //console.log(1);
              //console.log(error.response);
              store.commit("resetError");
              reject(error.response);
            });

        });
      },

      loadAuthData(store) {
        let data = localStorage.getItem("auth");
        if (data != null) {
          store.commit("setAuthData", JSON.parse(data));

          if (store.getters.isAuthenticated) {
            Vue.http.defaults.headers.common["Authorization"] = `Bearer ${store.state.auth.token}`;
          }
        }
      },
      storeAuthData(store, auth) {
        localStorage.setItem("auth", JSON.stringify(auth));
      },
      clearAuthData() {
        localStorage.removeItem('auth')
      },
      initializeAuth(store) {
        store.dispatch("loadAuthData");
      }
    }
}

</script>
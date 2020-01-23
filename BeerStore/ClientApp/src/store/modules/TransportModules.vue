<script>
import Vue from 'vue';

export default {
  namespaced: true,
  state: {
    itemsTransportModule: {}
  },
  getters: {
    items(state) {
      return state.itemsTransportModule;
    }
  },
  mutations: {
    setItems(state, data) {
      state.itemsTransportModule = data
    }
  },
  actions: {
    loadItems(store) {
      return new Promise((resolve, reject) => {
        /*
        let newParams = {pageNumber: params.page, shoppingArea: params.shoppingArea, filter: params.filter};
        if (newParams.filter === '') {
          delete newParams.filter;
        }
        */

        Vue.http
          .get('transportmodules')
          .then((response) => {
            store.commit('setItems', response.data);
            resolve(response);
          }).catch(error => {
          reject(error.response);
        });
      });
    }
  }
}

</script>
<script>
import Vue from 'vue';

export default {
  namespaced: true,
  state: {
    itemsCustomer: {}
  },
  getters: {
    items(state) {
      return state.itemsCustomer.items;
    }
  },
  mutations: {
    setItems(state, data) {
      state.itemsCustomer = data
    },
    deleteCustomer(state, customerToRemove){
      let index  = state.itemsCustomer.items.findIndex(line => line == customerToRemove);
      if (index > -1) {
        state.itemsCustomer.items.splice(index, 1);
      }
    }
  },
  actions: {
    loadItems(store, params) {
      return new Promise((resolve, reject) => {
        let newParams = {pageNumber: params.page, shoppingArea: params.shoppingArea, filter: params.filter};
        if (newParams.filter === '') {
          delete newParams.filter;
        }

        Vue.http
          .get('customers', {params: newParams})
          .then((response) => {
            store.commit('setItems', response.data);
            resolve(response);
          }).catch(error => {
          reject(error.response);
        });
      });
    },
    addToShoppingAria(store, customer){
      return new Promise((resolve, reject) => {
        Vue.http
          .post('CustomerShoppingAreas/addcustomer', customer)
          .then((response) => {
            resolve(response);
          }).catch(error => {
            reject(error.response);
        });
      });
    },
    deleteToShoppingAria(store, customer){
      return new Promise((resolve, reject) => {
        Vue.http
          .post('CustomerShoppingAreas/deletecustomer', customer)
          .then((response) => {
            store.commit('deleteCustomer', customer);
            resolve(response);
          }).catch(error => {
          reject(error.response);
        });
      });
    }
  }
}

</script>
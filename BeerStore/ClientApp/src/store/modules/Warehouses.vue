<script>
import Vue from 'vue';

export default {
  namespaced: true,
  state: {
    itemsWarehouse: {}
  },
  getters: {
    items(state) {
      return state.itemsWarehouse.items;
    }
  },
  mutations: {
    setItems(state, data) {
      state.itemsWarehouse = data
    },
    deleteWarehouse(state, warehouseToRemove) {
      let index = state.itemsWarehouse.items.findIndex(line => line == warehouseToRemove);
      if (index > -1) {
        state.itemsWarehouse.items.splice(index, 1);
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
          .get('warehouses', {params: newParams})
          .then((response) => {
            store.commit('setItems', response.data);
            resolve(response);
          }).catch(error => {
          reject(error.response);
        });
      });
    },
    addToShoppingAria(store, warehouse) {
      return new Promise((resolve, reject) => {
        Vue.http
          .post('WarehouseShoppingAreas/addwarehouse', warehouse)
          .then((response) => {
            resolve(response);
          }).catch(error => {
          reject(error.response);
        });
      });
    },
    deleteToShoppingAria(store, warehouse) {
      return new Promise((resolve, reject) => {
        Vue.http
          .post('WarehouseShoppingAreas/deletewarehouse', warehouse)
          .then((response) => {
            store.commit('deleteWarehouse', warehouse);
            resolve(response);
          }).catch(error => {
          reject(error.response);
        });
      });
    }
  }
}

</script>
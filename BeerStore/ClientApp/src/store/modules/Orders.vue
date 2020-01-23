<script>
import Vue from "vue";

export default {
  namespaced: true,
  state: {
    itemsOrder: {},
    itemsLoaded: false,
    orders: []
  },
  getters: {
    items(state){
      return state.itemsOrder.items;
    },
    itemsLoaded(state){
      return state.itemsLoaded;
    }
  },
  mutations: {
    setOrder(state, order) {
      let index  = state.itemsOrder.items.findIndex(item => item.orderId == order.orderId);
      if (index > -1) {
        state.itemsOrder.items.splice( index, 1, order );
      }
    },
    // changeOrderShipped(state, order) {
    //     Vue.set(order, "shipped",
    //         order.shipped == null || !order.shipped ? true : false);
    // }
    setItems(state, data) {
      state.itemsOrder = data
      state.itemsLoaded = true
    }
  },
  actions: {
    async storeOrder(context, order) {
      order.orderId = 0;
      order.customer = context.rootState.cart.customer;
      order.outlet = context.rootState.cart.outlet;
      order.warehouse = context.rootState.cart.warehouse;
      order.lines = context.rootState.cart.lines;

      let lines = order.lines.map(function (item) {
        return {
          cartLineId: 0,
          product: item.product,
          quantity: item.quantity,
          marketingEvent: item.marketingEvent === null ? null : {marketingEventId: item.marketingEvent}
        };
      });
      order.lines = lines;

      return (await Vue.http.post('orders', order)).data.orderId;
    },
    loadItems(store, params) {
      return new Promise((resolve, reject) => {
        Vue.http
          .get('orders', {params: {pageNumber: params.page, pageSize: 10}})
          .then((response) => {
            store.commit('setItems', response.data);
            resolve(response);
          }).catch(error => {
          reject(error.response);
        });
      });
    },
    async getOrder(store, orderId) {
      store.commit('setOrder',
        (await Vue.http.get('orders/'+orderId)).data);
    }

    // async getOrders(context) {
    //      context.commit("setOrders",
    //          (await context.rootGetters.authenticatedAxios.get(ORDERS_URL)).data);
    // },
    // async updateOrder(context, order) {
    //     context.commit("changeOrderShipped", order);
    //     await context.rootGetters.authenticatedAxios
    //         .put(`${ORDERS_URL}/${order.id}`, order);
    // }
  }
}
</script>
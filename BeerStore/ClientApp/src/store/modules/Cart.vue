<script>
  import Vue from 'vue';

  export default {
    namespaced: true,
    state: {
      customer:{},
      outlet:{},
      warehouse:{},
      lines: []
    },
    getters: {
      itemCount: state => state.lines.reduce((total, line) => total + line.quantity, 0),
      totalPrice: state => state.lines.reduce((total, line) => total + (line.quantity * line.price), 0),
      customer(state){
        return state.customer;
      },
      customerId(state){
        return state.customer.customerId;
      },
      outlet(state){
        return state.outlet;
      },
      outletId(state){
        return state.outlet.outletId;
      },
      warehouse(state){
        return state.warehouse;
      },
      warehouseId(state){
        return state.warehouse.warehouseId;
      },
      warehouseDepartmentCode(state){
        return state.warehouse.departmentCode;
      }
    },
    mutations: {
      addProduct(state, productQuantity) {
        let product = productQuantity.product;
        let quantity = productQuantity.quantity;

        let line  = state.lines.find(line => line.product.productId == product.productId);
        if (line != null) {
          line.quantity = line.quantity + quantity;
        } else {
          state.lines.push({ product: product,
                             quantity: quantity,
                             price: product.price,
                             marketingEvent: null,
                             marketingEventLoad: false,
                             marketingEventItems: [{value: null, text: 'Без скидки'}]
          });
        }
      },
      changeQuantity(state, update) {
        update.line.quantity =  update.quantity;
      },
      changeMarketingEvent(state, update) {
        update.line.marketingEvent =  update.marketingEventId;
      },
      removeProduct(state, lineToRemove) {
        let index  = state.lines.findIndex(line => line == lineToRemove);
        if (index > -1) {
          state.lines.splice(index, 1);
        }
      },

      setMarketingEvent(state, data){
        let index  = state.lines.findIndex(line => line == data.line);
        if (index > -1) {
          let items = data.items.map(function(item){
            return {value: item.id, text: item.descr};
          });
          state.lines[index].marketingEventItems = [...state.lines[index].marketingEventItems, ...items];
          state.lines[index].marketingEventLoad = true;
        }
      },
      setCartData(state, data) {
        state.lines = data;
      },
      addCustomer(state, customer) {
        state.customer = customer;

        if (state.customer.customerId != state.outlet.outletId) {
          state.outlet = {};
        }
      },
      addOutlet(state, outlet){
        state.outlet = outlet;
      },
      addWarehouse(state, warehouse) {
        state.warehouse = warehouse;
      }
    },
    actions: {
      loadMarketingEvent(store, lineMarketingEvent) {
        let productId = lineMarketingEvent.product.productId;

        Vue.http.get('marketingEventProducts/'+productId)
          .then((response) => {
            store.commit('setMarketingEvent', {line: lineMarketingEvent,
                                               items: response.data});
          })

      },
      loadCartData(context) {
        //localStorage.removeItem("cart")
        let data = localStorage.getItem("cart");
        if (data != null) {
          let localCart = JSON.parse(data);
          context.commit("setCartData",  localCart.lines);
          context.commit("addCustomer",  localCart.customer);
          context.commit("addOutlet",    localCart.outlet);
          context.commit("addWarehouse", localCart.warehouse);

          //context.commit("setCartData", JSON.parse(data));
        }
      },
      storeCartData(context) {
        let localCart = {lines:     context.state.lines,
                         customer:  context.state.customer,
                         outlet:    context.state.outlet,
                         warehouse: context.state.warehouse}

        //localStorage.setItem("cart", JSON.stringify(context.state.lines));
        localStorage.setItem("cart", JSON.stringify(localCart));
      },
      clearCartData(context) {
        context.commit("addCustomer",{});
        context.commit("addOutlet",{});
        context.commit("setCartData", []);
      },
      initializeCart(context, store) {
        context.dispatch("loadCartData");
        store.watch(state => state.cart.lines,
          () => context.dispatch("storeCartData"), { deep: true});
        store.watch(state => state.cart.customer,
          () => context.dispatch("storeCartData"), { deep: true});
        store.watch(state => state.cart.outlet,
          () => context.dispatch("storeCartData"), { deep: true});
        store.watch(state => state.cart.warehouse,
          () => context.dispatch("storeCartData"), { deep: true});
      }
    }
  }
</script>
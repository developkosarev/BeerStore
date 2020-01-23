import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

import auth from './modules/Auth';
import counterTest from './modules/CounterTest';
import products from './modules/Products';
import categories from './modules/Categories';
import customers from './modules/Customers';
import outlets from './modules/Outlets';
import warehouses from './modules/Warehouses';
import cart from './modules/Cart';
import orders from './modules/Orders';
import transportModules from './modules/TransportModules';

export default new Vuex.Store({
  modules:{
    auth,
    counterTest,
    products,
    categories,
    customers,
    outlets,
    warehouses,
    cart,
    orders,
    transportModules
  },

})

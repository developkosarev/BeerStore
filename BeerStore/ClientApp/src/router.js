import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router);

//import Login from './components/Login.vue'
//import Register from './components/Register.vue';
//import Dashboard from './components/Dashboard.vue';

import Home from './views/Home.vue'
import ProductList from './components/ProductList.vue';
import Categories from './components/Catalog/Categories.vue';
import CustomerList from './components/CustomerList.vue';
import OutletList from './components/OutletList.vue';
import WarehouseList from './components/WarehouseList.vue';
import TransportModuleList from './components/TransportModuleList.vue';
import ShoppingCart from './components/Cart/ShoppingCart.vue';
import Checkout from './components/Cart/Checkout.vue';
import OrderThanks from './components/Cart/OrderThanks.vue';
import NeedRole from './components/Auth/NeedRole.vue';
import OrderList from './components/Order/OrderList.vue';
import ResetPasswordForm from './components/Auth/ResetPasswordForm.vue';
import ResetPasswordConfirmation from './components/Auth/ResetPasswordConfirmation.vue';

import store from './store/store'

const router = new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {path: '/', name: 'home', component: Home},
    //{path: '/login', name: 'login', component: Login},
    //{path: '/register', name: 'register', component: Register},
    //{path: '/dashboard', name: 'dashboard', component: Dashboard},

    {path: '/password/reset', name: 'resetPasswordForm', component: ResetPasswordForm,
      props: (route) => ({ userId: route.query.userId, code:  route.query.code})
    },
    {path: '/password/confirmation', name: 'resetPasswordConfirmation', component: ResetPasswordConfirmation},
    {path: '/needrole', name: 'needrole', component: NeedRole},
    {path: '/categories/:id?', name: 'categories', component: Categories,
      props: (route) => ({id: route.params.id}),
      meta: { requiresAuth: true, role: "Users" }
    },
    {path: '/customers', name: 'customers', component: CustomerList,
      meta: { requiresAuth: true, role: "Users" } },
    {path: '/outlets', name: 'outlets', component: OutletList,
      meta: { requiresAuth: true, role: "Users" } },
    {path: '/warehouses', name: 'warehouses', component: WarehouseList,
      meta: { requiresAuth: true, role: "Users" } },
    {path: '/transportModules', name: 'transportModules', component: TransportModuleList,
      meta: { requiresAuth: true, role: "Users" } },
    {path: '/cart', component: ShoppingCart,
      meta: { requiresAuth: true, role: "Users" } },
    {path: '/checkout', component: Checkout,
      meta: { requiresAuth: true, role: "Users" } },
    {path: '/orders', name: 'orders', component: OrderList,
      meta: { requiresAuth: true, role: "Users" } },
    {path: "/thanks/:id", component: OrderThanks},
    {path: '/about', name: 'about',
      // route level code-splitting
      // this generates a separate chunk (about.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import(/* webpackChunkName: "about" */ './views/About.vue'),
        meta: { requiresAuth: true, role: "Users" }
    },
    {
      path: '/products', name: 'productlist', component: ProductList,
      beforeEnter(from, to, next){
        //store.dispatch('products/loadItems');
        next();
      }
    },
    {path: '/products/:id', name: 'product', component: ProductList}
    //{
    //  path: '/categories/:id?',
    //  name: 'categories',
    //  component: Categories,
    //  props: (route) => ({
    //    id: route.params.id
    //  }),
    //  beforeEnter(from, to, next){
    //    store.dispatch('categories/loadItems');
    //
    //    let id = from.params.id;
    //    id = id == undefined ? 0: id;
    //
    //    store.dispatch('categories/setCategoryId', id);
    //    next();
    //  }
    //}

  ]
})

router.beforeEach((to, from, next) => {
  if (to.matched.some(route => route.meta.requiresAuth)) {

    if (!store.getters['auth/isAuthenticated']) {
      store.commit("auth/showAuthModal");
      next({ path: from.path, query: { redirect: to.path } });
    } else {
      if (
        to.matched.some(
          route => route.meta.role && store.getters['auth/isInRole'](route.meta.role)
        )
      ) {
        next();
      } else if (!to.matched.some(route => route.meta.role)) {
        next();
      } else {
        next({ path: "/needrole" });
      }
    }
  } else {
    if (
      to.matched.some(
        route =>
          route.meta.role &&
          (!store.getters['auth/isAuthenticated'] ||
            store.getters['auth/isInRole'](route.meta.role))
      )
    ) {
      next();
    } else {
      if (to.matched.some(route => route.meta.role)) {
        next({ path: "/" });
      }

      next();
     }
  }
});

export default router;
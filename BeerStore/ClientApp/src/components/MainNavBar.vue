<template>          
  <!--  fixed="top" v-bind:sticky="true" -->
  <b-navbar toggleable="md" type="light" variant="light">
    <b-navbar-toggle target="nav_collapse"></b-navbar-toggle>
    <b-navbar-brand class="col-auto mr-auto" to="/">Beer Store</b-navbar-brand>

    <b-collapse is-nav id="nav_collapse">

        <b-navbar-nav>

          <!--
          <b-nav-item-dropdown text="Подразделение" right>
            <b-dropdown-item href="#">Саратов</b-dropdown-item>
            <b-dropdown-item href="#">Волжский</b-dropdown-item>
          </b-nav-item-dropdown>
          -->

          <b-nav-item to="/categories">Каталог</b-nav-item>
          <b-nav-item to="/customers">Покупатели</b-nav-item>
          <b-nav-item to="/orders">Заказы</b-nav-item>
          
          <!--
          <b-nav-item to="/transportModules">Транспорт</b-nav-item>

          <b-nav-item to="/outlets">Грузополучатели</b-nav-item>
          <b-nav-item to="/warehouses">Склады</b-nav-item>
          <b-nav-item to="/about">О программе</b-nav-item>
          <b-nav-item to="/">Home</b-nav-item>
          <b-nav-item to="/login">Login</b-nav-item>
          -->
          <auth-nav-item/>
        </b-navbar-nav>

        <!-- Right aligned nav items -->
        <!--
        <b-navbar-nav class="ml-auto">
                    
          <b-nav-item-dropdown text="User" right>                        
            <b-dropdown-item href="#">Profile</b-dropdown-item>
            <b-dropdown-item href="#">Signout</b-dropdown-item>
          </b-nav-item-dropdown>

        </b-navbar-nav>
      -->

      </b-collapse>

      <cart-summary/>

  </b-navbar>
</template>

<script>
  import cartSummary from '@/components/Cart/CartSummary.vue';
  import authNavItem from '@/components/Auth/AuthNavItem.vue';

  export default {
    name: 'MainNavBar',
    components: {
      cartSummary,
      authNavItem
    },
    data(){
      return {            
      }
    }
  }
</script> 
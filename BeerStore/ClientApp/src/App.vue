<template>      
  <div id="app">    
    <main-nav-bar></main-nav-bar>
    <router-view/>
    <auth-modal :show="showAuthModal" />
  </div>
</template>

<script>
import {mapGetters} from 'vuex';
import {mapActions} from 'vuex';

import mainNavBar from '@/components/MainNavBar.vue';
import AuthNavItem from "@/components/Auth/AuthNavItem.vue";
import AuthModal from "@/components/Auth/AuthModal.vue";

export default {
  name: 'App',
  components: {
    mainNavBar,
    AuthNavItem,
    AuthModal,
  },
  data () {
    return {      
      title: 'BeerStore'
    }
  },
  computed: {
    ...mapGetters({
      showAuthModal: 'auth/showAuthModal'
    })
  },
  methods: {
    ...mapActions({
      initializeCart: "cart/initializeCart"
      //initializeAuth: "auth/initializeAuth"
    })
  },
  created() {
    this.initializeCart(this.$store);
    //this.initializeAuth();
  }
}
</script> 

<style lang="scss">
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
#nav {
  padding: 30px;
  a {
    font-weight: bold;
    color: #2c3e50;
    &.router-link-exact-active {
      color: #42b983;
    }
  }
}
</style>

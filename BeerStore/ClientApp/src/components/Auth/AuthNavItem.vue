<template>
  <b-nav-item-dropdown v-if="isAuthenticated" left>
    <template slot="button-content">
      <i class="fa fa-user"></i>
      Выход
    </template>
    <b-dropdown-item v-if="isCustomer" to="/account">
      <i class="fa fa-user"></i>
      My Account
    </b-dropdown-item>
    <b-dropdown-item @click="logout">
      <i class="fa fa-sign-out-alt"></i>
      Выход
    </b-dropdown-item>
  </b-nav-item-dropdown>
  <b-nav-item v-else @click="login">
    <i class="fa fa-user"></i>
    Вход / регистрация
  </b-nav-item>
</template>

<script>
import {mapGetters} from 'vuex';
import {mapMutations} from 'vuex';

export default {
  name: "AuthNavItem",
  computed: {
    ...mapGetters({
      isAuthenticated:     'auth/isAuthenticated'}),
    isCustomer() {
      return true;
      //return this.$store.getters.isInRole("Customer");
    }
  },
  methods: {
    ...mapMutations({
      showAuthModal: "auth/showAuthModal"}),
    login() {
      this.showAuthModal();
    },
    logout() {
      this.$store.dispatch("auth/logout").then(() => {
        if (this.$route.matched.some(route => route.meta.requiresAuth)) {
          this.$router.push("/");
        }
      });

      // this.$store.dispatch("logout").then(() => {
      //   if (this.$route.matched.some(route => route.meta.requiresAuth)) {
      //     this.$router.push("/");
      //   }
      // });
    }
  }
};
</script>

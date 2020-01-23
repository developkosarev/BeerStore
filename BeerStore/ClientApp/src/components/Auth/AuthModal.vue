<template>
  <b-modal v-model="show" hide-header hide-footer no-close-on-backdrop no-close-on-esc>

    <template v-if="activePage === 0">
      <b-tabs v-model="index">
        <b-tab title="Войти" active>
          <login-form :registered="registered" @close="close" v-on:forgotPassword="forgotPassword" /> <!-- :=v-bind краткая форма приявязка в атрибуту -->
        </b-tab>
        <b-tab title="Регистрация" >
          <register-form @success="success" @close="close" /><!--@=v-on краткая форма события-->
        </b-tab>
      </b-tabs>
    </template>

    <template v-else-if="activePage === 1">
      <forgot-password-form @forgotPasswordConfirmation="forgotPasswordConfirmation" @close="close" />
    </template>

    <template v-else-if="activePage === 2">
      <forgot-password-confirmation @close="close" />
    </template>

  </b-modal>
</template>

<script>
import {mapMutations} from 'vuex';
import LoginForm from './LoginForm.vue';
import RegisterForm from './RegisterForm.vue';
import ForgotPasswordForm from './ForgotPasswordForm.vue';
import ForgotPasswordConfirmation from './ForgotPasswordConfirmation.vue';

export default {
  name: "auth-modal",
  components: {
    LoginForm,
    RegisterForm,
    ForgotPasswordForm,
    ForgotPasswordConfirmation,
  },
  props: {
    show: {
      type: Boolean,
      required: true
    }
  },
  data() {
    return {
      index: 0,
      registered: false,
      activePage: 0
    };
  },
  methods: {
    ...mapMutations({ hideAuthModal: "auth/hideAuthModal" }),
    success() {
      this.registered = true;
      this.index = 0;
    },
    close() {
      this.activePage = 0;
      this.hideAuthModal();
      let query = Object.assign({}, this.$route.query);
      delete query.redirect;
      this.$router.push({ query: query });
    },
    forgotPassword(){
      this.activePage = 1;
    },
    forgotPasswordConfirmation(){
      this.activePage = 2;
    }
  }
};
</script>


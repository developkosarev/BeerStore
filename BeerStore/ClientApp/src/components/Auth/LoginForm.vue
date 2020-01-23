<template>
  <form @submit.prevent="login" class="p-2">
    <b-alert variant="danger" :show="error !== null" dismissible @dismissed="error = null">
      {{ error }}
    </b-alert>
    <b-alert variant="success" :show="registered && error === null">
      Регистрация прошла успешно. Водите под своим логином.
    </b-alert>

    <p>Войдите со своим E-mail и паролем.</p>
    <b-form-group label="E-mail">
      <b-form-input v-model.trim="email" />
    </b-form-group>
    <b-form-group label="Пароль">
      <b-form-input v-model.trim="password" type="password" />
    </b-form-group>

    <b-form-group>
      <b-button variant="primary" type="submit" :disabled="loading">Войти</b-button>
      <b-button variant="default" @click="close" :disabled="loading">Отмена</b-button>
    </b-form-group>

    <div class="m-3 text-left">
      <b-link href="#" @click="forgotPassword">Забыли пароль?</b-link>
    </div>

  </form>
</template>

<script>
import {mapGetters} from 'vuex';

export default {
  name: "login-form",
  props: {
    registered: {
      type: Boolean,
      required: false
    }
  },
  data() {
    return {
      email: "",
      password: "",
      error: null
    };
  },
  computed: {
    ...mapGetters({
      loading:     'auth/loading'
    }),
    loading1() {
      return false;
      //return this.$store.state.loading;
    }
  },
  methods: {
    login() {
       const payload = {
         Username: this.email,
         Password: this.password
       };

       this.$store
         .dispatch("auth/login", payload)
         .then(() => {   //.then(response => {
           this.error = null;
           this.email = "";
           this.password = "";

           if (this.$route.query.redirect) {
             this.$router.push(this.$route.query.redirect);
           }
         })
         .catch(error => {
           if (error === undefined) {
            this.error = 'Нет связи c API сервером';
           } else {
            this.error = error.data.error;
           }
         });
    },
    close() {
      this.$emit("close");
    },
    forgotPassword(){
      this.$emit("forgotPassword");
    }
  }
};
</script>


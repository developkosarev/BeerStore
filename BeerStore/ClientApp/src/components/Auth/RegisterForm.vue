<template>
  <form @submit.prevent="submit" class="p-2">
    <b-alert variant="danger" :show="regErrors !== null" dismissible @dismissed="regErrors = null">
      <div v-for="(error, index) in regErrors" :key="index">{{ error[0] }}</div>
    </b-alert>

    <b-form-group label="E-mail">
      <b-form-input v-model.trim="email" />
    </b-form-group>
    <b-form-group label="Имя">
      <b-form-input v-model.trim="firstName" />
    </b-form-group>
    <b-form-group label="Фамилия">
      <b-form-input v-model.trim="lastName" />
    </b-form-group>
    <b-form-group label="Пароль">
      <b-form-input v-model.trim="password" type="password" />
    </b-form-group>
    <b-form-group label="Подтверждения пароля">
      <b-form-input v-model.trim="confirmPassword" type="password" />
    </b-form-group>
    <b-form-group>
      <b-button variant="primary" type="submit" :disabled="loading">Регистрация</b-button>
      <b-button variant="default" @click="close" :disabled="loading">Отмена</b-button>
    </b-form-group>
  </form>
</template>

<script>
import {mapGetters} from 'vuex';

export default {
  name: "register-form",
  data() {
    return {
      email: "",
      firstName: "",
      lastName: "",
      password: "",
      confirmPassword: "",
      regErrors: null
    };
  },
  computed: {
    ...mapGetters({
      loading:     'auth/loading'
    }),
  },
  methods: {
    submit() {
        const payload = {
          Name: this.email,
          FirstName: this.firstName,
          LastName: this.lastName,
          Password: this.password,
          ConfirmPassword: this.confirmPassword
        };

       this.$store
         .dispatch("auth/register", payload)
         // eslint-disable-next-line
         .then(response => {
           this.regErrors = null;
           this.email = "";
           this.firstName = "";
           this.lastName = "";
           this.password = "";
           this.confirmPassword = "";

           this.$emit("success");
         })
         .catch(error => {
           if (typeof error.data === "string" || error.data instanceof String) {
             this.regErrors = { error: [error.data] };
           } else {
             this.regErrors = error.data;
           }
         });
    },
    close() {
      this.regErrors = null;
      this.$emit("close");
    }
  }
};
</script>


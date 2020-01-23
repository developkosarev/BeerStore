<template>
  <div class="container">

    <form @submit.prevent="resetPassword" class="p-2">

        <b-alert variant="danger" :show="errors.length>0" dismissible @dismissed="errors = []">
            <div v-for="(error, index) in errors" :key="index">{{ error }}</div>
        </b-alert>

        <p>Введите новый пароль</p>

        <b-form-group label="E-mail">
            <b-form-input v-model.trim="email" />
        </b-form-group>
        <b-form-group label="Пароль">
          <b-form-input v-model.trim="password" type="password" />
        </b-form-group>
        <b-form-group label="Подтверждения пароля">
          <b-form-input v-model.trim="confirmPassword" type="password" />
        </b-form-group>

        <b-form-group>
          <b-button variant="primary" type="submit" :disabled="loading">Отправить</b-button>
          <b-button variant="default" @click="close" :disabled="loading">Отмена</b-button>
        </b-form-group>

    </form>
  </div>
</template>

<script>
import {mapGetters} from 'vuex';

export default {
  name: "ResetPasswordForm",
  props: {
    userId: {
      type: String,
      required: false
    },
    code: {
      type: String,
      required: false
    },
  },
  data() {
    return {
      email: "",
      password: "",
      confirmPassword: "",
      errors: []
    };
  },
  computed: {
    ...mapGetters({
      loading:     'auth/loading'
    })
  },
  methods: {
    validEmail(email) {
      // eslint-disable-next-line
      var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      return re.test(email);
    },
    checkForm() {
      this.errors = [];

      if (!this.email) {
        this.errors.push("Требуется e-mail.");
      } else if (!this.validEmail(this.email)) {
        this.errors.push("Укажите корректный адрес электронной почты.");
      }
      if (!this.password) {
        this.errors.push("Требуется пароль.");
      }
      if (!this.confirmPassword) {
        this.errors.push("Требуется подтверждение пароля.");
      }
      if (this.password !== this.confirmPassword) {
        this.errors.push("Пароли не совпадают.");
      }

    },
    resetPassword() {
      this.checkForm();

      if (this.errors.length) {
        return false;
      }

      const payload = {
        Email: this.email,
        Password: this.password,
        ConfirmPassword: this.confirmPassword,
        Code: this.code
      };

      this.$store
        .dispatch("auth/resetPassword", payload)
        .then(() => {   //.then(response => {
          this.errors = [];
          this.email = "";
          this.password = "";
          this.confirmPassword = "";

          this.$router.push("/password/confirmation");
        })
        .catch(error => {
          if (error === undefined) {
            this.errors = { error: ['Нет связи c API сервером'] };
          } else {
            // typeof тип перемееной строка
            // typeof undefined // "undefined"
            // typeof 0 // "number"
            // typeof true // "boolean"
            // typeof "foo" // "string"
            // typeof {} // "object"
            // typeof null // "object"  (1)
            // typeof function(){} // "function"  (2)

            //

            //console.log(2);
            //console.log(error);

            if (typeof error.data === "string" || error.data instanceof String) {
              this.errors = { error: [error.data] };
            } else {
              this.errors = error.data;
            }
          }
        });

    },
    close() {
      this.$router.replace('/')
    }
  }
};
</script>


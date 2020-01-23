<template>
  <form @submit.prevent="forgotPassword" class="p-2">
    <b-alert variant="danger" :show="error !== null" dismissible @dismissed="error = null">
      {{ error }}
    </b-alert>

    <p>Восстановить пароль</p>
    <b-form-group label="E-mail">
      <b-form-input v-model.trim="email" />
    </b-form-group>

    <b-form-group>
      <b-button variant="primary" type="submit" :disabled="loading">Отправить</b-button>
      <b-button variant="default" @click="close" :disabled="loading">Отмена</b-button>
    </b-form-group>
  </form>

</template>

<script>
import {mapGetters} from 'vuex';

export default {
  name: "ForgotPasswordForm",
  data() {
    return {
      email: "",
      error: null
    };
  },
  computed: {
    ...mapGetters({
      loading:     'auth/loading'
    })
  },
  methods: {
    forgotPassword() {
      const payload = {
        Email: this.email,
        Host: document.location.host
      };

      this.$store
        .dispatch("auth/forgotPassword", payload)
        .then(() => {   //.then(response => {
          this.error = null;
          this.email = "";

          this.$emit("forgotPasswordConfirmation");
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
    }
  }
};
</script>


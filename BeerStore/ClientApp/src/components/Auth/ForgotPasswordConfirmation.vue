<template>

    <div>
        <p>Восстановление пароля</p>
        <p>Проверьте ваш E-mail для восстановления пароля.</p>

        <b-button variant="default" @click="close">ОК</b-button>
    </div>

</template>

<script>
    export default {
      name: "ForgotPasswordConfirmation",
      /*
      data() {
        return {}
      },
      */
      methods: {
        close() {
          this.$emit("close");
        }
      }
    }
</script>

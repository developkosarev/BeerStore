<template>

    <div>
        <p>Изменение пароля выполнено</p>

        <!--
        <b-button variant="default" @click="close">ОК</b-button>
        -->
    </div>

</template>

<script>
    export default {
      name: "ResetPasswordConfirmation",
      methods: {
        close() {
          //this.$emit("close");
        }
      }
    }
</script>

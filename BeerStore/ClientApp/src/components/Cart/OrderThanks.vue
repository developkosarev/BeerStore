<template>

    <div class="m-2 text-center">
        <h2>Спасибо!</h2>
        <p>Ваш заказ сохранен, с номером №{{orderId}}.</p>
        <router-link to="/" class="btn btn-primary">Вернуться к покупкам</router-link>
    </div>

</template>

<script>
    export default {
        computed: {
            orderId() {
                return this.$route.params.id;
            }
        }
    }
</script>

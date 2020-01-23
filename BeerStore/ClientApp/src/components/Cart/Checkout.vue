<template>
    <div class="container">
        <b-alert variant="danger" :show="error !== null" dismissible @dismissed="error = null">
            {{ error }}
        </b-alert>

        <h5 class="text-center">Оформление заказа</h5>

        <b-form-checkbox id="checkbox1"
                         v-model="order.thisReturn"
                         value=true
                         unchecked-value=false
                         class="col-12"
        >
            Возврат от покупателя
        </b-form-checkbox>
        <div></div>
        <b-form-checkbox id="checkbox2"
                         v-model="order.finance"
                         value=true
                         unchecked-value=false
                         class="col-12"
        >
            Фин
        </b-form-checkbox>

        <b-form-group
                id="fieldset1"
                label="Вид взаиморасчетов"
                class="m-1"
        >
            <b-form-select v-model="order.payType" :options="payTypeOptions" class="mb-3" />
        </b-form-group>


        <b-form-group
                id="fieldset2"
                label="Комментарий"
                label-for="input1"
                class="m-1"
        >
            <b-form-input id="input1" v-model.trim="order.comment"></b-form-input>
        </b-form-group>


        <div class="text-center">
            <router-link to="/cart" class="btn btn-secondary m-1">
                Назад
            </router-link>
            <button class="btn btn-primary m-1" v-on:click="submitOrder">
                Отправить заказ
            </button>
        </div>
    </div>
</template>

<script>

import { mapActions } from "vuex";

export default {
    components: {

    },
    data: function() {
        return {
            order: {
                thisReturn: false,
                finance: false,
                payType: 0,
                comment: ''
            },
            payTypeOptions: [
                { value: 0, text: 'Наличные' },
                { value: 1, text: 'Безналичные' },
                { value: 2, text: 'Отсрочка платежа' },
            ],
            error: null
        }
    },
    methods: {
        ...mapActions({
            "storeOrder": "orders/storeOrder",
            "clearCart": "cart/clearCartData"
        }),
        async submitOrder(){
          try {
            let order = await this.storeOrder(this.order);
            this.clearCart();
            this.$router.push(`/thanks/${order}`);
          } catch(e) {
            this.error = 'Ошибка сохранения заказа! Зайдите в корзину и повторите выбор склада.';
          }
        }

        // async submitOrder() {
        //     this.$v.$touch();
        //     if (!this.$v.$invalid) {
        //         let order = await this.storeOrder(this.order);
        //         this.clearCart();
        //         this.$router.push(`/thanks/${order}`);
        //     }
        // }
    }
}
</script>

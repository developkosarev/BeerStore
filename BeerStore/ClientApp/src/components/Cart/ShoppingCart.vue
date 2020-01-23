<template>
    <div class="container-fluid">
        <div class="row">
            <div class="col mt-2">
                <h5 class="text-center">Ваша корзина</h5>

                <div v-if="error" class="col">
                    <div class="alert alert-danger">
                        Заполните контрагента, грузополучателя и склад
                    </div>
                </div>

                <div class="row no-gutters">
                <div class="col-9">
                    <div class="text-left">
                        Контрагент:
                        {{customer.descr}}
                    </div>
                </div>
                <div class="col-3">
                    <div class="text-right">
                        <router-link to="/customers" class="btn btn-sm btn-info m-1">
                            Выбрать
                        </router-link>
                    </div>
                </div>
            </div>

                <div class="row no-gutters">
                <div class="col-9">
                    <div class="text-left">
                        Грузополучатель:
                        {{outlet.descr}}
                    </div>
                </div>
                <div class="col-3">
                    <div class="text-right">
                        <router-link to="/outlets" class="btn btn-sm btn-info m-1">
                            Выбрать
                        </router-link>
                    </div>
                </div>
            </div>

                <div class="row no-gutters">
                <div class="col-9">
                    <div class="text-left">
                        Склад:
                        {{warehouse.descr}}
                    </div>
                </div>
                <div class="col-3">
                    <div class="text-right">
                        <router-link to="/warehouses" class="btn btn-sm btn-info m-1">
                            Выбрать
                        </router-link>
                    </div>
                </div>
            </div>

                <cart-line v-for="line in lines" v-bind:key="line.product.productId"
                           v-bind:line="line"
                           v-on:quantity="handleQuantityChange(line, $event)"
                           v-on:remove="remove"
                           v-on:marketingEvent="handleMarketingEventChange(line, $event)"
                           v-on:getMarketingEvent="loadMarketingEvent" />
            </div>
        </div>

        <div class="row">
            <div class="col">
                <strong>
                    Итого
                </strong>
            </div>
            <div class="col">
                <strong>
                    {{totalPrice.toFixed(2)}}
                </strong>
            </div>
        </div>

        <div class="row">
            <div class="col">
                <div class="text-center">
                    <router-link to="/categories" class="btn btn-secondary btn-sm m-1">
                        Продолжить покупки
                   </router-link>
                   <button class="btn btn-primary btn-sm m-1"
                            v-on:click="onCheckout">
                        Заказать >
                   </button>
                </div>
            </div>
        </div>
    </div>
</template>
    
<script>

import { mapState, mapMutations, mapGetters } from "vuex";
import CartLine from "./ShoppingCartLine";

export default {
    components: { CartLine },
    data() {
        return {
          error: false
        }
    },
    computed: {
        ...mapState({ lines: state => state.cart.lines }),
        ...mapGetters({ totalPrice : "cart/totalPrice",
                          customer : "cart/customer",
                        customerId : "cart/customerId",
                            outlet : "cart/outlet",
                          outletId : "cart/outletId",
                         warehouse : "cart/warehouse",
                       warehouseId : "cart/warehouseId",})
    },
    methods: {
        ...mapMutations({
            change: "cart/changeQuantity",
            changeMarketingEvent: "cart/changeMarketingEvent",
            remove: "cart/removeProduct"
        }),
        handleQuantityChange(line, $event) {
            this.change({ line, quantity: $event});
        },
        handleMarketingEventChange(line, $event) {
          this.changeMarketingEvent({ line, marketingEventId: $event});
        },
        loadMarketingEvent(line){
          this.$store.dispatch('cart/loadMarketingEvent', line)
        },
        onCheckout(){
          this.error = false;
          if ( this.customerId === undefined ||
               this.outletId === undefined ||
               this.warehouseId === undefined ){
            this.error = true;
          }

          if (!this.error) {
            this.$router.push('/checkout');
          }
        }
    }
}
</script>

<template>
    <div class="card m-1 p-0 bg-light" >
        <div class="card-body bg-white p-1">
            <div class="row no-gutters">
                <div class="col-12 col-md-8 text-left">
                    {{product.descr}}
                </div>
                <div class="col-12 col-md-4">
                    <div class="form-row pt-1 pt-md-0">
                        <div class="col-4">

                            <template v-if="warehouseDepartmentCode == 'S00000098' ">
                                <strong> {{product.price1}}</strong>
                                {{product.quantity1}}
                            </template>
                            <template v-else>
                                <strong> {{product.price}}</strong>
                                {{product.quantity}}
                            </template>

                        </div>

                        <div class="col-5 input-group">
                            <div class="input-group-prepend">
                                <button class="btn btn-primary btn-sm m-0"
                                        v-on:click="minus">
                                    <i class="fa fa-minus" aria-hidden="true"></i>
                                </button>
                            </div>

                            <b-form-input size="sm"
                                          type="number"
                                          placeholder="Кол-во"
                                          v-model="qvalue"
                                          class="text-center"
                                          v-on:input="sendChangeEvent"
                                          v-on:click.native="onClick"
                            >
                            </b-form-input>

                            <div class="input-group-prepend">
                                <button class="btn btn-primary btn-sm"
                                        v-on:click="plus">
                                    <i class="fa fa-plus" aria-hidden="true"></i>
                                </button>
                                <button class="btn btn-info btn-sm"
                                        v-on:click="plusPack">
                                    уп
                                </button>
                            </div>
                        </div>

                        <div class="col-3">
                            <button class="btn btn-success btn-sm pull-right"
                                    v-on:click="onAddToCart">
                                Заказ
                            </button>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>

import {mapMutations} from 'vuex';
import {mapGetters} from 'vuex';

export default {
    props: ["product"],
    data: function() {
        return {
          //qvalue: this.product.pack === 0 ? 1 : this.product.pack,
          qvalue: 0
        }
    },
    computed: {
        ...mapGetters({ warehouseDepartmentCode : "cart/warehouseDepartmentCode" })
    },
    methods: {
      ...mapMutations({ addProduct: "cart/addProduct" }),
      sendChangeEvent() {
        if (Number(this.qvalue) < 0) {
          this.qvalue = 0;
        }
      },
      onClick(event){
        event.target.select();
      },
      minus(){
        if (Number(this.qvalue) > 1){
            this.qvalue = Number(this.qvalue) - 1;
        }
      },
      plus(){
        this.qvalue = Number(this.qvalue) + 1;
      },
      plusPack(){
        let quantity = this.product.pack === 0 ? 1 : this.product.pack;
        this.qvalue = Number(this.qvalue) + quantity;
      },
      onAddToCart(){
        this.addProduct({ product: this.product, quantity: Number(this.qvalue)});
      },
      onAddToCartPack(){
        //let quantity = this.product.pack === 0 ? 1 : this.product.pack;
        //this.addProduct({ product: this.product, quantity: quantity});
      }
    }
}
</script>

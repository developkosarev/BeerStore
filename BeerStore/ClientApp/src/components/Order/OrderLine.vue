<template>

    <div class="card m-1 p-1 bg-light">
        <div class="card-header text-white bg-primary text-left p-1">
            № {{order.orderId}} от {{order.date | date}}
        </div>

        <h6 class="card-title text-left m-1">
            {{order.customer.descr}} {{order.customer.inn}}
        </h6>
        <small class="card-title text-left m-1">{{order.outlet.descr}}</small>
        <small class="card-title text-left m-1">{{order.warehouse.descr}}</small>

        <button type="button" class="btn btn-outline-secondary btn-sm text-left"
                v-on:click="showLine"
        >
            <i class="fa fa-list" aria-hidden="true"></i>
            Список товаров
        </button>

        <template v-if="showLines">
            <ul v-for="(line, index) in order.lines"
                :key="line.cartLineId + 'L'"
                class="list-group list-group-flush">
                <li class="list-group-item text-left text-white bg-success">
                    <div class="row">
                        <div class="col-2">
                            {{index + 1}}
                        </div>
                        <div class="col-8">
                            <small>{{line.product.descr}}</small>
                        </div>
                        <div class="col-2">
                            {{line.quantity}}
                        </div>
                    </div>
                </li>
            </ul>
        </template>

    </div>

</template>

<script>

export default {
    props: ["order"],
    data: function() {
        return {
          showLines: false
        }
    },
    methods: {
      showLine(){
        if (this.order.lines == null) {
            this.$store.dispatch('orders/getOrder', this.order.orderId).then(() => {
              this.showLines = true;
            });
        } else {
          this.showLines = !this.showLines;
        }
      }
    }
}
</script>

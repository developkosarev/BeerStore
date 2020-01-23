<template>
    <div class="float-right">
        <small>
            <span v-if="itemCount > 0">
                {{ itemCount }} шт <!-- {{ totalPrice }} руб.-->
            </span>
            <span v-else>
                (пусто)
            </span>
        </small>
        <router-link to="/cart" class="btn btn-sm text-dark"
                     v-bind:disabled="itemCount == 0">
            <i class="fa fa-shopping-cart"></i>
        </router-link>
    </div>
</template>

<script>
  import { mapGetters } from 'vuex';

  export default {
    computed: {
      ...mapGetters({
        itemCount: "cart/itemCount",
        totalPrice: "cart/totalPrice"
      })
    }
  }
</script>
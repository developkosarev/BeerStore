<template>
    <div class="content">
        <div v-if="!loaded" class="col">
            <div class="alert alert-success">
                Загрузка...
            </div>
        </div>
        <template v-else>

            <order-line v-for="order in orders"
                 :key="order.orderId"
                 v-bind:order="order">
            </order-line>

            <template v-if="totalPages>1">
                <b-pagination align="center" size="md" v-model="currentPage" :total-rows="totalCount" :per-page="pageSize" :limit=10>
                </b-pagination>
            </template>
        </template>
    </div>
</template>

<script>  
  import {mapGetters} from 'vuex';
  import OrderLine from "./OrderLine";

  export default {
    name: 'OrderList',
    components: {
      OrderLine,
    },
    created(){
      this.fetchData();
    },
    data(){
      return {
        filter: '',
        totalCount: 0,
        totalPages: 0,
        currentPage: 1,
        pageSize: 1
      }
    },
    computed: {    	
		...mapGetters({
          orders:    'orders/items',
          loaded:    'orders/itemsLoaded'
		})
	},
    watch: {
      'currentPage': 'fetchData'
    },
    methods:{
      fetchData () {
        this.$store
          .dispatch('orders/loadItems',
            {page:       this.currentPage})
          .then((response ) => {
            this.totalCount = response.data.totalCount;
            this.totalPages = response.data.totalPages;
            this.pageSize = response.data.pageSize;
        });
      }
    }
  }
</script>
<template>
    <div class="content">
        <div v-if="!loaded" class="col">
            <div class="alert alert-success">
                Загрузка...
            </div>
        </div>
        <template v-else>

            <form @submit.prevent="filterData">
                <b-input-group-append>
                    <b-form-input type="text" placeholder="Отбор" class="m-1 p-1" v-model="filter" size="sm"/>
                    <b-btn type="submit" text="Button" variant="outline-secondary" size="sm"
                           class="m-1 p-1"
                            v-on:click="fetchData">
                        Найти
                    </b-btn>
                </b-input-group-append>
            </form>

            <div v-for="outlet in outlets"
                 :key="outlet.outletId"
                 class="card m-1 p-1 bg-light">
                {{outlet.descr}}
                <div class="card-text bg-white p-1">
                    <button class="btn btn-success btn-sm float-right"
                            v-on:click="onAddToCart(outlet)">

                        Выбрать
                    </button>
                </div>
            </div>

            <template v-if="totalPages>1">
                <b-pagination align="center" size="md" v-model="currentPage" :total-rows="totalCount" :per-page="pageSize" :limit=10>
                </b-pagination>
            </template>
        </template>
    </div>
</template>

<script>  
  import {mapGetters} from 'vuex';
  import {mapMutations} from 'vuex';

  export default {
    name: 'OutletList',
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
          outlets:    'outlets/items',
          loaded:     'outlets/itemsLoaded',
          customerId: 'cart/customerId'
		})
	},
    watch: {
      'currentPage': 'fetchData'
    },
    methods:{
      ...mapMutations({ addOutlet: "cart/addOutlet" }),
      fetchData () {
        this.$store
          .dispatch('outlets/loadItems',
            {customerId: this.customerId,
             filter:     this.filter,
             page:       this.currentPage})
          .then((response ) => {
            this.totalCount = response.data.totalCount;
            this.totalPages = response.data.totalPages;
            this.pageSize = response.data.pageSize;
        });
      },
      filterData(){
        this.currentPage = 1;
        this.fetchData();
      },
      onAddToCart(outlet){
        this.addOutlet(outlet);
        this.$router.push("/cart");
      }
    }
  }
</script>
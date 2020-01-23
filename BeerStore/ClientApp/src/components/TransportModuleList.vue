<template>
    <div class="content">
        <div v-if="!itemsLoaded" class="col">
            <div class="alert alert-success">
                Загрузка...
            </div>
        </div>
        <template v-else>
            <b-alert variant="danger" :show="error !== null" dismissible @dismissed="error = null">
                {{ error }}
            </b-alert>

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

            <div v-for="transportModule in transportModules"
                 :key="transportModule.Id"
                 class="card m-1 p-1 bg-light">
                {{transportModule.totalOut}}
                {{transportModule.descr}}

                <!--
                <div class="card-text bg-white p-1">
                    <template v-if="!shoppingArea">
                        <button class="btn btn-primary btn-sm float-left"
                                v-on:click="onAddToShoppingArea(warehouse)"
                        >
                            Добавить
                        </button>
                    </template>
                    <template v-else>
                        <button class="btn btn-danger btn-sm float-left"
                                v-on:click="onDeleteToShoppingArea(warehouse)"
                        >
                            Удалить
                        </button>
                    </template>

                    <button class="btn btn-success btn-sm float-right"
                            v-on:click="onAddToCart(warehouse)">

                        Выбрать
                    </button>
                </div>
                -->

            </div>

        </template>
    </div>
</template>

<script>  
  import {mapGetters} from 'vuex';
  /*import {mapMutations} from 'vuex';*/

  export default {
    name: 'TransportModuleList',
    created(){
      this.fetchData();
    },
    data(){
      return {
        filter: '',
        totalCount: 0,
        totalPages: 0,
        currentPage: 1,
        pageSize: 1,
        itemsLoaded: false,
        error: null
      }
    },
    computed: {    	
		...mapGetters({
          transportModules: 'transportModules/items'
		})
	},
    watch: {
      'currentPage': 'fetchData'
    },
    methods:{
      fetchData () {
        this.itemsLoaded = false;
        this.$store
          .dispatch('transportModules/loadItems')
          .then((response) => {
            //this.totalCount = response.data.totalCount;
            //this.totalPages = response.data.totalPages;
            //this.pageSize = response.data.pageSize;
            this.itemsLoaded = true;
            this.error = null;
            // eslint-disable-next-line
          }).catch(error => {
            this.itemsLoaded = true;
              if (error === undefined) {
            this.error = 'Нет связи c API сервером';
            } else {
              this.error = error.data.error;
            }
        });
      },
      filterData(){
        this.currentPage = 1;
        this.fetchData();
      }
    }
  }
</script>
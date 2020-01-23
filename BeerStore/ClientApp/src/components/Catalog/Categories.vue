<template>
  <div class="content">    
    <div v-if="!loaded" class="col">
        <div class="alert alert-success">
          Загрузка...
        </div>
    </div>
    <template v-else>
      <b-breadcrumb :items="itemsBreadCrumb" class="mb-0 p-2"/>
            
      <ul class="list-group">
        <li v-for="categorie in categories"
                   :key="categorie.categoryId"
                   class="list-group-item list-group-item-action" 
                   v-on:click="onClick(categorie)"
        >
          {{ categorie.descr }}
        </li>
      </ul>

      <!--
      Вспомогательные классы bootstrap
      http://getbootstrap.ru/docs/v4-alpha/components/utilities/
      Класс card
      https://itchief.ru/bootstrap/card-v4
      -->

      <template v-if="totalCountProduct>0 || filter !=='' ">
        <form @submit.prevent="filterData">
          <b-input-group-append>
            <b-form-input type="text" placeholder="Отбор" class="m-1 p-1" v-model="filter" size="sm"/>
            <b-btn type="submit" text="Button" variant="outline-secondary" size="sm"
                   class="m-1 p-1"
                   v-on:click="filterData">
              Найти
            </b-btn>
          </b-input-group-append>
        </form>

        <product-line v-for="product in products"
                      :key="product.productId"
                      v-bind:product="product"
        >
        </product-line>

        <template v-if="totalPagesProduct>1">
          <b-pagination align="center" size="md" v-model="currentPageProduct" :total-rows="totalCountProduct" :per-page="pageSizeProduct" :limit=10>
          </b-pagination>
        </template>

      </template>

    </template>
  </div>  
</template>

<script>

import {mapGetters} from 'vuex';
import ProductLine from "./ProductLine";

export default {
  name: 'Categories',
  components: {
     ProductLine,
  },
  created () {
    this.$store.dispatch('categories/loadItems', this.$route.params.id)
      // eslint-disable-next-line
      .then(response => {
        //this.error = null;
        //this.email = "";
        //this.password = "";

        //if (this.$route.query.redirect) {
        //  this.$router.push(this.$route.query.redirect);
        //}
      })
      // eslint-disable-next-line
      .catch(error => {
        //console.log(error);
        //if (error === undefined) {
        //  this.error = 'Нет связи c API сервером';
        //} else {
        //  this.error = error.data.error;
        //}
      });

    this.$store.dispatch('categories/setCategoryId', this.$route.params.id)
  },

  beforeRouteUpdate (to, from, next) {    
    this.$store.dispatch('categories/setCategoryId', to.params.id)
    next()
  },
  
  data() {
    return {
      filter: '',
      currentPageProduct: 1
    }
  },
  computed: {
    ...mapGetters({
      categories:     'categories/items',
      loaded:         'categories/itemsLoaded',
      itemsParent:    'categories/itemsParent',
      products:       'categories/itemsProduct',
      totalCountProduct: 'categories/totalCountProduct',
      totalPagesProduct: 'categories/totalPagesProduct',
      pageSizeProduct:   'categories/pageSizeProduct'
	}),
    itemsBreadCrumb(){
        var result = [{text: 'Категории', to: {name: 'categories'}}];
        this.itemsParent.forEach(function (item) {
            let crumb = {
                text: item.descr,
                to: {
                    name: 'categories',
                    params: {id: item.categoryId}
                }
            }

            result.push(crumb);
        });
        return result;
    }
  },
  watch: {
    'currentPageProduct': 'fetchDataProduct'
  },
  methods: {
    fetchDataProduct() {
      this.$store.dispatch('categories/loadItemsProduct',
        {filter: this.filter,
         page:   this.currentPageProduct}
      );

    },
    filterData(){
      this.currentPageProduct = 1;
      this.fetchDataProduct();
    },
    onClick(item) {
      this.filter = '';
      this.currentPageProduct = 1;
      this.$router.push({ name: 'categories', params: { id: item.categoryId } });

      //if (item.parentCategoryId == 0) {        
      //  this.$router.push({ name: 'categories', params: { id: item.categoryId } })
      //} else {
      //  this.$store.dispatch('categories/setCategoryId', item.categoryId);
      //
      //  // со строкой запроса, получится /register?plan=private
      //  this.$router.push({ name: 'productlist',  query: { categoryid: item.categoryId } })
      //}            
    }
  }
}
</script>
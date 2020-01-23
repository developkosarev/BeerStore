<template>
  <div class="content">    
    <h1>Products</h1>



    <div class="card" v-for="product in products" v-bind:key="product.id">
      <div class="card-body">
        <div class="row no-gutters">

          <div class="col-10 col-md-7 text-left">
            Product name
            {{product.descr}}


            <!--
            <h4 class="product-name">
              <strong>Product name</strong>
            </h4>
            <h4><small>Product description</small></h4>
            -->
          </div>

          <div class="col-2 col-md-1 order-md-2">
            <button type="button" class="btn btn-outline-danger btn-xs">
              <i class="fa fa-trash" aria-hidden="true"></i>
            </button>
          </div>

          <div class="col-12 col-md-4 order-md-1">
            <form class="form-row">
              <div class="col-left p-2">
                <strong>25.00
                  <span class="text-muted m-1">x</span>
                </strong>
              </div>
              <div class="col-left">
                <input type="text" class="form-control input-sm" value="20" size="1">
              </div>
              <div class="col-left p-2">
                <strong>
                  <span class="text-muted m-1">=</span>
                  2000.00
                </strong>
              </div>
            </form>
          </div>

          <!--
          <select class="form-control-sm">
            <option>Выберите маркетинговое мероприятие</option>
            <option>Выберите маркетинговое мероприятие</option>
            <option>Выберите маркетинговое мероприятие</option>
            <option>Выберите маркетинговое мероприятие</option>
          </select>

          <div class="col-4 form-inline text-right">
              <strong>25.00
                <span class="text-muted m-1">x</span>
              </strong>
              <input type="text" class="form-control input-sm" value="20" size="1">
              <button type="button" class="btn btn-outline-danger btn-xs">
                <i class="fa fa-trash" aria-hidden="true"></i>
              </button>
          </div>
          -->



          <!--
          <div class="col-xs-2 col-md-2">
            <img class="img-responsive" src="http://placehold.it/120x80" alt="prewiew">
          </div>

          <div class="col-xs-4 col-md-6">
            <h4 class="product-name"><strong>Product name</strong></h4><h4><small>Product description</small></h4>
          </div>

          <div class="col-xs-6 col-md-4 row">
            <div class="col-xs-6 col-md-6 text-right" style="padding-top: 5px">
              <h6><strong>25.00 <span class="text-muted">x</span></strong></h6>
            </div>
            <div class="col-xs-4 col-md-4">
              <input type="text" class="form-control input-sm" value="1">
            </div>
            <div class="col-xs-2 col-md-2">
              <button type="button" class="btn btn-outline-danger btn-xs">
                <i class="fa fa-trash" aria-hidden="true"></i>
              </button>
            </div>
          </div>
          -->


        </div>
      </div>
    </div>

    <!--
    <b-breadcrumb :items="items"/>        
        
    <ul class="list-group">      
      <router-link v-for="product in products"
                 :key="product.categoryId"
                 :to="{name: 'product',
                         params: {
                           id: product.categoryId
                         }
                 }"
                 class="list-group-item list-group-item-action">        
        {{ product.descr }}
        {{ product.categoryId }}
      </router-link>        
    </ul>         
    -->

  </div>  
</template>

<script>

//дизайн корзины
//  https://bootsnipp.com/snippets/K012l

/*import {mapGetters} from 'vuex';
import {mapActions} from 'vuex';*/

export default {
  name: 'ProductList',
  components: {

  },
  data() {
	return {
      items: [{text: 'Товары'},
              {text: 'Pr2'},
              {text: 'Pr3'}],
      products: [{id:1, descr: 'dddddddddddddddddddddddddddddddddddddd11111111111111111111111111111111111111111111122222222222222222222222222222'},
                 {id:2, descr: '2'},
                 {id:3, descr: '3'}]
    }
  },
  methods: {
    fetchData () {
    }
  }
}
</script>
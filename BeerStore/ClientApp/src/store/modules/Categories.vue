<script>
import Vue from 'vue';

export default {
	namespaced: true,
	state: {
		itemsLoaded: false,
		items: [],
		itemsParent: [],
		parentCategoryId: 0,

		itemsProduct: {}
	},
	getters: {		
		itemsLoaded(state){
			return state.itemsLoaded;
		},
		items(state){
			let newitems = state.items.filter(function(item) {
				return item.parentCategoryId == state.parentCategoryId;
			});
			return newitems;			
		},
		itemsParent(state){
			return state.itemsParent;
		},
		parentCategory(state){
			return state.parentCategoryId
		},

		/*Products*/
		itemsProduct(state){
			return state.itemsProduct.items;
		},
        currentPageProduct(state){
            if (state.itemsProduct.currentPage === undefined){
              return 0;
            } else {
              return state.itemsProduct.currentPage;
            }
        },
        totalCountProduct(state){
            if (state.itemsProduct.totalCount === undefined){
              return 0;
            } else {
              return state.itemsProduct.totalCount;
            }
        },
        totalPagesProduct(state){
          if (state.itemsProduct.totalPages === undefined){
            return 0;
          } else {
            return state.itemsProduct.totalPages;
          }
        },
        pageSizeProduct(state){
          if (state.itemsProduct.pageSize === undefined){
            return 0;
          } else {
            return state.itemsProduct.pageSize;
          }
        }
	},
	mutations: {
		setItems(state, items){
			items.sort(function (a, b) {            	
				if (a.descr > b.descr) {
					return 1;
				}
				if (a.descr < b.descr) {
					return -1;
				}
                // a должно быть равным b
                return 0;
            });

			state.items = items;
			state.itemsLoaded = true;
		},
		setParentCategoryId(state, categoryId){
			let id = categoryId === undefined ? 0: parseInt(categoryId)
			state.parentCategoryId = id
            state.itemsParent = []

            if (id != 0) {
                state.itemsParent = buildItemsParent(id, state.items, [])
            }
		},
        setItemsProduct(state, items){
            state.itemsProduct = items;
        },
	},
	actions: {
		loadItems(store, id){
          return new Promise((resolve, reject) => {
			if(!store.state.itemsLoaded){
				Vue.http
                    .get('categories')
                    .then((response) => {
						store.commit('setItems', response.data)
						store.commit('setParentCategoryId', id)
                        resolve(response);
                    })
                    .catch(error => {
                        // eslint-disable-next-line
                        console.log(error);
                        reject(error.response);
                    });
			}
          });
		},
        loadItemsProduct(store, params){
            Vue.http.get('products', {params: {categoryId: store.getters.parentCategory,
                                                pageNumber: params.page,
                                                filter: params.filter}})
              .then((response) => {
                store.commit('setItemsProduct', response.data);
            })
        },
		setCategoryId(store, categoryId){
			store.commit('setParentCategoryId', categoryId)
            store.commit('setItemsProduct', {});

            if (categoryId !== undefined) {
                Vue.http.get('products', {params: {categoryId: categoryId, pageNumber: 1}})
                    .then((response) => {
                        store.commit('setItemsProduct', response.data);
                    })
            } else {
                store.commit('setItemsProduct', {});
            }
		}
    }
}

/*Построения дерева родителей*/
function buildItemsParent(categoryId, items, itemsResult){
    items.forEach(function(item) {
        if (item.categoryId === categoryId) {
            //itemsResult.push(item.categoryId)
            itemsResult.unshift(item);

            if (item.parentCategoryId !== 0) {
                return buildItemsParent(item.parentCategoryId, items, itemsResult)
            }
        }
    });
    return itemsResult
}

</script>
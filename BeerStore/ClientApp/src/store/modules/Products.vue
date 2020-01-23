<script>
import Vue from 'vue';

export default {
	namespaced: true,
	state: {
		parentCategoryId: 0,
		items: {},
		itemsLoaded: false
	},
	getters: {
		items(state){			
			return state.items;
		},
		itemsLoaded(state){
			return state.itemsLoaded;
		},
		product: (state, getters) => (id) => {
			return getters.items[id];
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
			state.parentCategoryId = categoryId;
		}
	},
	actions: {
		loadItems(store){
			if(!store.state.itemsLoaded){
				Vue.http.get('categories')
					.then((response) => {
						store.commit('setItems', response.data);
				});
			}
		}
	}
}

</script>
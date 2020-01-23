<script>
import Vue from 'vue';

export default {
	namespaced: true,
	state: {
		itemsOutlet: {},
		itemsLoaded: false
	},
	getters: {
		items(state){
			return state.itemsOutlet.items;
		},
		itemsLoaded(state){
			return state.itemsLoaded;
		}		
	},
	mutations: {
		setItems(state, data){
			state.itemsOutlet = data
			state.itemsLoaded = true
		}
	},
	actions: {
		loadItems(store, params){
          return new Promise((resolve, reject) => {
                Vue.http
                  .get('outlets',{params: {customerId: params.customerId, pageNumber: params.page, filter: params.filter} })
                  .then((response) => {
                    store.commit('setItems', response.data);
                    resolve(response);
				}).catch(error => {
                  reject(error.response);
                });
          });
		}
	}
}

</script>
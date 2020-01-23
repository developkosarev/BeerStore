<script>
export default {
	namespaced: true,
	state: {
		cnt: 0
	},
	getters: {
		cnt(state){
			return state.cnt;
		},
		total(state, getters){
			return getters.cnt * 3;
		}
	},
	mutations: {
		minus(state){
			if (state.cnt > 1) {
				state.cnt--;
			}			
		},
		plus(state){
			state.cnt++;
		},		
		start(state){			
			state.cnt = 0;

			// var str = "Ослик Иа-Иа посмотрел на Иа виадук"; // ищем в этой строке
			// var target = "Иа"; // цель поиска

			// var pos = -1;
			// while ((pos = str.indexOf(target, pos + 1)) != -1) {
			// 	state.cnt++;
			// }

			for(let i=1; i<10000; i++) {
				var str = String(i); // ищем в этой строке
				var target = "3"; // цель поиска

				var pos = -1;
				while ((pos = str.indexOf(target, pos + 1)) != -1) {
					state.cnt++;
				}			
			}
		},
		setCount(state, value){
			state.cnt = 0;
			
			let num = parseInt(value);

			if (isNaN(num) || num < 1) {				
				num = 1;
			} 
			
			state.cnt = num;
		}
	}
}
</script>  
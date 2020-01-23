<template>
    <div>
        <h1>Laravel 5 – Our Cool Dashboard</h1>
        <h5 id="test1c">{{msg}}</h5>
        <button class="btn btn-success btn-sm"
                v-on:click="onClick">
            Нажми
        </button>
        <button class="btn btn-success btn-sm"
                v-on:click="onClickClear">
            Стереть
        </button>
    </div>
</template>

<script>
  export default {
    name: 'Dashboard',
    data(){
      return {
        msg: ''
      }
    },
    methods: {
      onClick() {
        //window.aaaaaaa = 'test';
        //this.msg = window.aaaaaaa;
        //console.log(window);
      },
      onClickClear() {
        this.msg = '';
      }
    }
  }

  //var externalForm = null;

</script>
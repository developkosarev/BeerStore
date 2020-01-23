<template>
  <div class="hello">
    <div class="row">
      <div class="col">
        <h4>{{ msg }}</h4>
      </div>
    </div>
    <div class="row">
      <div class="col">
        <router-link to="/categories" class="btn btn-outline-dark">
          Оформить заявку на доставку >
        </router-link>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'HelloWorld',
  props: {
    msg: String
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
/*
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
*/
</style>

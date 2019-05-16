<template>
  <nav class="navbar navbar-expand-md navbar-dark bg-dark sticky-top">
    <div class="container">
      <a id="brand" class="navbar-brand" href="/Home">NewsBlog</a>
      <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarsExample04" aria-controls="navbarsExample04" aria-expanded="true" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>

      <div class="navbar-collapse collapse show" id="navbarsExample04" style="">
        <ul class="navbar-nav mr-auto">
          <li class="nav-item active">
            <a class="nav-link" href="#"> <span class="sr-only">(current)</span></a>
          </li>
          <li class="nav-item pr-2">
            <select id="sort" class="form-control" v-model="sort_selected">
              <option value="0" selected>Сортировать по дате</option>
              <option value="1">Сортировать по источнику</option>
            </select>
          </li>
          <li class="nav-item pr-2">
            <select id="sources" class="form-control" v-model="source_selected">
              <option>Все источники</option>
            </select>
          </li>
          <li class="nav-item pr-2">
            <select id="categories" class="form-control" v-model="category_selected">
              <option>Все категории</option>
            </select>
          </li>
        </ul>
        <div>
          <input value="" name="query" class="form-control" type="text" v-model="query" placeholder="Поиск">
        </div>
      </div>
    </div>
  </nav>
</template>

<script>

export default {
  name: 'Navbar',
  data () {
    return {
      query: '',
      source_selected: 'Все источники',
      category_selected: 'Все категории',
      sort_selected: 0
    }
  },
  mounted() {
    
  }
}
</script>

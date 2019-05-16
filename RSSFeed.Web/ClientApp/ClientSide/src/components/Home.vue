<template>
  <div class='row d-flex justify-content-center' v-if='seen'>
    <input id='pageNumber' type='hidden' name='name' v-bind:value='page' />
    <div class='card' v-for='post in items' :key='post.id'>
      <a :href='post.postUrl' id='getVideo' target='_blank' v-on:click='postSeen(post.id)'>
        <input type='hidden' id='postId' v-model='post.id' />
        <img class='card-img-top image img-logo img-thumbnail' :src='post.channel.image'>
        <img :class='post.imageClasses' :src='post.imageUrl'>
        <div class='card-body'>
          <h5 class='card-title'>{{ post.title }}</h5>
          <p class='card-description'>{{ post.body }}</p>
        </div>
      </a>
    </div>
  </div>
  <div class='row d-flex justify-content-center' id='notFoundOrNoContent' v-else-if='notFound'>
    <div class='content-fluid text-center vertical-align-div'>
      <span class='vertical-align-span'>Post not found</span>
    </div>
  </div>
</template>

<script>
import Service from '@/api-services/news.service.js'

export default {
  name: 'Home',
  data () {
    return {
      posts: false,
      seen: true,
      show: true,
      loader: false,
      notFound: false,
      page: 1,
      bottom: false,
      displayBlock: '',
      source_selected: 'Все источники',
      query: '',
      sort_selected: 0,
      category_selected: 'Все категории',
      items: []
    }
  },
  mounted () {
    var _this = this
    var query = ''
    var source = document.getElementById('sources').value
    var sort = document.getElementById('sort').value
    var categories = document.getElementById('categories').value
    console.log(_this.items)
    Service.getAll(this.page, query, source, sort, categories)
      .then((response) => {
        if (response.data.data.length > 0) {
          console.log(response)
          this.notFound = false
          this.seen = true
          response.data.data.forEach(function (item) {
            if (item.imageUrl === 'https://knews.kg/wp-content/uploads/2016/02/logo.png') {
              item.imageClasses = 'card-img-top image img-fluid img-thumbnail img-background'
            } else {
              item.imageClasses = 'card-img-top image img-fluid img-thumbnail'
            }
            if (item.body.length > 100) {
              item.body = item.body.substring(0, 100) + '...'
            }
            _this.items.push(item)
          })
          this.page++
          console.log(_this.items)
        }
        if (response.data.filtered === 0 && response.data.total === 0) {
          this.seen = false
          this.notFound = true
        }
        this.posts = true
      },
      () => {
        this.posts = true
        this.seen = false
        this.notFound = true
      })
  }
}
</script>

<style>
  .card {
    width: 15rem;
    margin: 1rem;
}

.img-background {
    background-color: #212529;
    border-color: #212529;
}

.img-logo {
    position: absolute;
    top: 6px;
    left: 6px;
    background-color: #212529;
    border-color: #212529;
    width: 25%;
}
.img-thumbnail {
    /*height: 25em;*/
    -o-object-fit: cover;
    /*object-fit: cover;*/
}
a {
    color: black;
}

    a:hover {
        color: black;
        text-decoration: none;
    }

</style>

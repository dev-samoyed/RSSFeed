<template>
  <footer class="footer">
    <div class="container">
      <span class="text-muted">&copy; 2019 - NewsBlog</span>
    </div>
  </footer>
</template>

<script>
export default {
  name: 'HelloWorld'
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
.footer {
    /*position: absolute;*/
    width: 100%;
    bottom: 0;
    height: 4em;
    line-height: 60px;
    background-color: #f5f5f5;
}

    .footer > .container {
        padding-right: 15px;
        padding-left: 15px;
    }

</style>

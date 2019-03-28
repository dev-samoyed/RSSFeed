// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    new Vue({
        el: '#posts',
        data: {
            posts: false,
            seen: true,
            show: true,
            loader: false,
            notFound: false,
            page: 1,
            bottom: false,
            displayBlock: '',
            source_selected: 'Все источники',
            sort_selected: 0,
            category_selected: 'Все категории',
            items: []
        },
        methods: {
            // this method need for hide and show search input (toogle method), sets the value of show
            getWindowWidth(event) {
                var windowWidth = document.documentElement.clientWidth;
                if (windowWidth >= 1050) {
                    this.show = false;
                    this.displayBlock = 'block';
                }
                else {
                    this.show = true;
                    this.displayBlock = '';
                }
            },
            toggle: function () {
                this.show = !this.show;
                return this.displayBlock = !this.show === true ? 'block' : '';
            },
            getQuery: function () {
                return document.querySelector("input[name=query]").value;
            },
            getSource: function () {
                return document.getElementById("sources").value;
            },
            getSort: function () {
                return document.getElementById("sort").value;
            },
            getCategory: function () {
                return document.getElementById("categories").value;
            },
            isLoading(state) {
                return this.loader = state;
            },
            bottomVisible() {
                const scrollY = window.scrollY;
                const visible = document.documentElement.clientHeight;
                const pageHeight = document.documentElement.scrollHeight;
                const bottomOfPage = visible + scrollY >= pageHeight;
                return bottomOfPage || pageHeight < visible;
            },
            addPosts() {
                // without _this variable push method don't work
                var _this = this;
                this.isLoading(true);
                axios.get(`/Home/GetData/?pageNumber=${this.page}&query=${this.getQuery()}&source=${this.getSource()}&sort=${this.getSort()}&category=${this.getCategory()}`)
                    .then((response) => {
                        if (response.data.data.length > 0) {
                            this.notFound = false;
                            this.seen = true;

                            response.data.data.forEach(function (item) {
                                if (item.imageUrl == "https://knews.kg/wp-content/uploads/2016/02/logo.png") {
                                    item.imageClasses = "card-img-top image img-fluid img-thumbnail img-background";
                                }
                                else {
                                    item.imageClasses = 'card-img-top image img-fluid img-thumbnail';
                                }

                                if (item.body.length > 170) {
                                    item.body = item.body.substring(0, 170) + "...";
                                }
                                _this.items.push(item);
                            });
                            this.page++;
                        }
                        if (response.data.filtered === 0 && response.data.total === 0) {
                            this.seen = false;
                            this.notFound = true;
                        }
                        this.isLoading(false);
                        this.posts = true;
                    },
                        (error) => {
                            this.posts = true;
                            this.seen = false;
                            this.notFound = true;
                            this.isLoading(false);
                        });
            },
            postSeen(postId) {
                var id = document.getElementById('postId').value;
                axios.get(`/Home/PostSeen/?postId=${id}`);
            },
            saveCategories() {
                var categories = '';
                var categoryList = $('#categories');
                
                axios.get(`/Home/GetCategoriesBySource/?sourceId=${this.getSource()}`)
                    .then((response) => {
                        response.data.forEach(function (item) {
                            categories += `<option value="${item.value}">${item.value}</option>`;
                        });
                        this.category_selected = 'Все категории';
                        categoryList.html(categories);
                    },
                        (error) => {
                            this.posts = true;
                            this.seen = false;
                            this.notFound = true;
                            this.isLoading(false);
                        });
            }
        },
        watch: {
            source_selected: function (val, oldval) {
                this.page = 1;
                this.saveCategories();
                this.items.length = 0;
                this.addPosts();
            },
            sort_selected: function () {
                this.items.length = 0;
                this.page = 1;
                this.addPosts();
            },
            category_selected: function (val) {
                this.page = 1;
                this.items.length = 0;
                this.addPosts();
            },
            bottom(bottom) {
                if (bottom) {
                    this.addPosts();
                }
            }
        },
        mounted() {
            this.$nextTick(function () {
                this.getWindowWidth();
            });
        },
        created() {
            window.addEventListener('scroll', () => {
                this.bottom = this.bottomVisible();
            });
            this.addPosts();
        },
        beforeDestroy() {
            window.removeEventListener('resize', this.getWindowWidth);
            window.removeEventListener('scroll');
        }
    });
});
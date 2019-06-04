Vue.use(VueTables.ClientTable);

$(document).ready(function () {
    new Vue({
        el: "#app",
        data: {
            createChannel: false,
            title: "",
            url: "",
            imageUrl: "",
            message: '',
            columns: ['title'],
            data: [],
            options: {
                sortable: ['title'],
                filterable: ['title']
            }
        },
        methods: {
            sub: function (event) {
                if (this.title == "" || this.url == "" || this.imageUrl == "") {
                    this.message = "Заполните поля";
                    event.preventDefault();
                } else {
                    this.message = '';
                    axios.get(`/Admin/Channels/Create/?imageUrl=${this.imageUrl}&&title=${this.title}&&url=${this.url}`)
                        .then(() => {
                            alert("Канал сохранен");
                            this.createChannel = false;
                        });
                }   
            },
            getChannels() {
                // without _this variable push method don't work
                var _this = this;
                axios.get(`/Admin/Channels/GetData/`)
                    .then((response) => {
                        if (response.data.data.length > 0) {
                            console.log(response);
                            response.data.data.forEach(function (item) {
                                _this.data.push(item);
                            });
                        }
                    });
            }
        },
        created() {
            this.getChannels();
        }
    });
});

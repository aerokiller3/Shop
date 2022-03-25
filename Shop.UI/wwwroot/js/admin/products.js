var url = '/products';

var app = new Vue({
    el: '#app',
    data: {
        loading: false,
        objectIndex: 0,
        form: null,
        products: [],
        images: []
    },
    mounted() {
        this.getProducts();
    },
    methods: {
        getProducts() {
            this.loading = true;
            axios.get(url)
                .then(res => {
                    console.log(res);
                    this.products = res.data;
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        createProduct() {
            this.loading = true;
            const form = new FormData();

            form.append('request.name', this.form.name);
            form.append('request.description', this.form.description);
            form.append('request.value', parseFloat(this.form.value));

            if (this.form.categories != null)
            {
                for (let i = 0; i < this.form.categories.length; i++) {
                    form.append('request.categoriesId', this.form.categories[i]);
                }
            }

            for (let i = 0; i < this.images.length; i++) {
                form.append('request.images', this.images[i]);
            }

            axios.post('/products',
                form,
                {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                })
                .then(() => this.getProducts())
                .catch(err => {
                    console.log(err);
                })
                .finally(this.resetForm);
        },
        updateProduct() {
            this.loading = true;

            const form = new FormData();

            form.append('request.id', this.form.id);
            form.append('request.name', this.form.name);
            form.append('request.description', this.form.description);
            form.append('request.value', parseFloat(this.form.value));

            for (let i = 0; i < this.images.length; i++) {
                form.append('request.images', this.images[i]);
            }

            for (let i = 0; i < this.form.categories.length; i++) {
                form.append('request.categoriesId', this.form.categories[i]);
            }

            axios.put(url, form,
                {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                })
                .then(() => this.getProducts())
                .catch(err => {
                    console.log(err);
                })
                .finally(this.resetForm);
        },
        deleteProduct(id, index) {
            this.loading = true;
            axios.delete('/products/' + id)
                .then(res => {
                    console.log(res);
                    this.products.splice(index, 1);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        newProduct() {
            this.form = {
                name: "",
                description: ""
            };
        },
        addImage(event) {
            const file = event.target.files[0];
            this.images.push(file);
            //this.form.images.add(file.name);
        },
        moveImageUp(index) {
            const image = this.images[index];
            this.images.splice(index, 1);
            this.images.splice(index - 1, 0, image);
        },
        moveImageDown(index) {
            const image = this.images[index];
            this.images.splice(index, 1);
            this.images.splice(index + 1, 0, image);
        },
        editProduct(id, index) {
            this.loading = true;
            this.objectIndex = index;
            axios.get(url + '/' + id)
                .then(res => this.form = res.data)
                .catch(err => console.log(err))
                .finally(() => this.loading = false);
        },
        resetForm() {
            this.form = null;
            this.loading = false;
            this.images = [];
        }
    },
    computed: {
        fileImages() {
            return this.images.map(x => URL.createObjectURL(x));
        }
    }
});
Vue.config.devtools = true;
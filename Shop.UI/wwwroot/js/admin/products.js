var url = '/products';

var app = new Vue({
    el: '#app',
    data: {
        editing: false,
        loading: false,
        objectIndex: 0,
        productModel: {
            id: 0,
            name: "Product Name",
            description: "Product Description",
            value: 1.99,
            image: null,
            categories: []
        },
        products: []
    },
    mounted() {
        this.getProducts();
    },
    methods: {
        getProduct(id) {
            this.loading = true;
            axios.get(url + '/' + id)
                .then(res => {
                    console.log(res);
                    var product = res.data;
                    this.productModel = {
                        id: product.id,
                        name: product.name,
                        description: product.description,
                        value: product.value,
                        image: product.image,
                        categories: product.categories
                    }
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
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

            form.append('request.name', this.productModel.name);
            form.append('request.description', this.productModel.description);
            form.append('request.value', parseFloat(this.productModel.value));
            form.append('request.image', this.productModel.image);

            for (let i = 0; i < this.productModel.categories.length; i++) {
                form.append('request.categoriesId', this.productModel.categories[i]);
            }

            //for (let i = 0; i < this.images.length; i++) {
            //    form.append('form.images', this.images[i])
            //}

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
        uploadImage(event) {
            this.productModel.image = event.target.files[0];
        },
        updateProduct() {
            this.loading = true;

            const form = new FormData();

            form.append('request.id', this.productModel.id);
            form.append('request.name', this.productModel.name);
            form.append('request.description', this.productModel.description);
            form.append('request.value', parseFloat(this.productModel.value));
            form.append('request.image', this.productModel.image);

            for (let i = 0; i < this.productModel.categories.length; i++)
            {
                form.append('request.categoriesId', this.productModel.categories[i]);
            }

            axios.put(url,
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
            this.editing = true;
            this.productModel.id = 0;
        },
        editProduct(id, index) {
            this.objectIndex = index;
            this.getProduct(id);
            this.editing = true;
        },
        //editProduct(id) {
        //    this.loading = true;
        //    return axios.get(url + '/' + id)
        //        .then(res => this.form = res.data)
        //        .catch(err => console.log(err))
        //        .finally(() => this.loading = false);
        //},
        cancel() {
            this.editing = false;
        },
        resetForm() {
            this.form = null;
            this.loading = false;
            this.editing = false;
            this.image = null;
        }
    },
    computed: {
    }
});
Vue.config.devtools = true;
/*var url = '/products';

var app = new Vue({
    el: '#app',
    data: () => ({
        form: null,
        loading: false,
        products: [],
        images: [],
    }),
    created() {
        return this.getProducts();
    },
    methods: {
        getProducts() {
            this.loading = true;
            return axios.get(url)
                .then(res => this.products = res.data)
                .catch(err => console.log(err))
                .finally(this.resetForm);
        },
        newProduct() {
            this.form = {
                name: "",
                description: "",
                series: "",
                stockDescription: ""
            };
        },
        editProduct(id) {
            this.loading = true;
            return axios.get(url + '/' + id)
                .then(res => this.form = res.data)
                .catch(err => console.log(err))
                .finally(() => this.loading = false);
        },
        addImage(event) {
            const file = event.target.files[0];
            this.images.push(file);
        },
        moveImageUp(index) {
            const image = this.form.images[index];
            this.images.splice(index, 1);
            this.images.splice(index - 1, 0, image);
        },
        moveImageDown(index) {
            const image = this.form.images[index];
            this.images.splice(index, 1);
            this.images.splice(index + 1, 0, image);
        },
        createProduct() {
            this.loading = true;
            const form = new FormData();

            form.append('form.name', this.form.name);
            form.append('form.description', this.form.description);
            form.append('form.series', this.form.series);
            form.append('form.stockDescription', this.form.stockDescription);

            for (let i = 0; i < this.images.length; i++) {
                form.append('form.images', this.images[i]);
            }

            axios.post(url, form, {
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

            form.append('form.id', this.form.id);
            form.append('form.name', this.form.name);
            form.append('form.description', this.form.description);
            form.append('form.series', this.form.series);
            form.append('form.stockDescription', this.form.stockDescription);

            for (let i = 0; i < this.images.length; i++) {
                form.append('form.images', this.images[i]);
            }

            axios.put(url, form, {
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
        resetForm() {
            this.form = null;
            this.images = [];
            this.loading = false;
        }
    },
    computed: {
        fileImages() {
            return this.images.map(x => URL.createObjectURL(x));
        }
    }
});*/
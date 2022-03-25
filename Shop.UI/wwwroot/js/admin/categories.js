var app = new Vue({
    el: '#app',
    data: {
        editing: false,
        loading: false,
        objectIndex: 0,
        categoryModel: {
            id: 0,
            title: "Категория",
            parentCategoryId: null
        },
        categories: []
    },
    mounted() {
        this.getCategories();
    },
    methods: {
        getCategory(id) {
            this.loading = true;
            axios.get('/categories/' + id)
                .then(res => {
                    console.log(res);
                    var category = res.data;
                    this.categoryModel = {
                        id: category.id,
                        title: category.title,
                        parentCategoryId: category.parentCategoryId
                    }
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        getCategories() {
            this.loading = true;
            axios.get('/categories')
                .then(res => {
                    console.log(res);
                    this.categories = res.data;
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        createCategory() {
            this.loading = true;

            const form = new FormData();

            form.append('request.title', this.categoryModel.title);
            form.append('request.parentCategoryId', this.categoryModel.parentCategoryId);

            axios.post('/categories',
                form,
                {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                })
                .then(() => this.getCategories())
                .catch(err => {
                    console.log(err);
                })
                .finally(this.resetForm);
        },
        updateCategory() {
            this.loading = true;

            const form = new FormData();

            form.append('request.id', this.categoryModel.id);
            form.append('request.title', this.categoryModel.title);
            form.append('request.parentCategoryId', this.categoryModel.parentCategoryId);

            axios.put('/categories',
                form,
                {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                })
                .then(() => this.getCategories())
                .catch(err => {
                    console.log(err);
                })
                .finally(this.resetForm);

            //axios.put('/categories', this.categoryModel)
            //    .then(res => {
            //        console.log(res.data);
            //        this.categories.splice(this.objectIndex, 1, res.data);
            //    })
            //    .catch(err => {
            //        console.log(err);
            //    })
            //    .then(() => {
            //        this.loading = false;
            //        this.editing = false;
            //    });
        },
        deleteCategory(id, index) {
            this.loading = true;
            axios.delete('/categories/' + id)
                .then(res => {
                    console.log(res);
                    this.categories.splice(index, 1);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        newCategory() {
            this.editing = true;
            this.categoryModel.id = 0;
        },
        editCategory(id, index) {
            this.objectIndex = index;
            this.getCategory(id);
            this.editing = true;
        },
        cancel() {
            this.editing = false;
        },
        resetForm() {
            this.form = null;
            this.loading = false;
            this.editing = false;
        }
    },
    computed: {
    }
});
Vue.config.devtools = true;
var app = new Vue({
    el: '#app',
    data: {
        products: [],
        search: '',
        categoryIds: [],
        categoryAgeId: 0,
        categoryTypeId: 0,
        categoryWayId: 0
    },
    mounted() {
        this.getProducts();
    },
    computed: {
        filterBroducts: function () {
            return this.products
                .filter(product => {
                    return product.name.match(this.search);
                })
                .filter(product => {
                    product.categories.include(categoryAgeId);
                })
                .filter(product => {
                    product.categories.include(categoryTypeId);
                })
                .filter(product => {
                    product.categories.include(categoryWayId);
                });
        }
    },
    methods: {
        getProducts() {
            axios.get('/')
                .then(res => {
                    console.log(res);
                    this.products = res.data;
                })
                .catch(err => {
                    console.log(err);
                });
        }
    }
})
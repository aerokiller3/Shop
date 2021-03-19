var app = new Vue({
    el: '#app',
    data: {
        username: "",
        users: []
    },
    mounted() {
        this.getUsers();
    },
    methods: {
        createUser() {
            this.loading = true;
            axios.post('/users', { username: this.username })
                .then(res => {
                    console.log(res);
                })
                .catch(err => {
                    console.log(err);
                });
        },
        getUsers() {
            this.loading = true;
            axios.get('/users')
                .then(res => {
                    console.log(res);
                    this.users = res.data;
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        deleteUser(email, index) {
            this.loading = true;
            axios.delete('/users/' + email)
                .then(res => {
                    console.log(res);
                    this.users.splice(index, 1);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        }
    }
});
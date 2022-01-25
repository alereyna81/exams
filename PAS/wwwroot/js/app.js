const routes=[
    {path:'/js/home',component:home},
    //{path:'/js/activity',component:activity},
    {path:'/js/property',component:property}
]

const router=new VueRouter({
    routes
})

const app = new Vue({
    router
}).$mount('#app')
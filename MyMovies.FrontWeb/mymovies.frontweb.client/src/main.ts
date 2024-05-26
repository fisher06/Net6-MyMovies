import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import 'sweetalert2/dist/sweetalert2.min.css';
import './assets/img/background.jpg'
import './assets/img/loading.gif'
import './assets/main.css'


import { createApp } from 'vue'
import App from './App.vue'

import router from "@/router";
import VueSweetalert2 from 'vue-sweetalert2';
import VueCookies from 'vue-cookies'


createApp(App).use(VueCookies).use(VueSweetalert2).use(router).mount('#app')


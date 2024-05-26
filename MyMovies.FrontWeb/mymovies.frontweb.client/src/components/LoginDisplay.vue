<template>
    <ul class="navbar-nav">
        <li class="nav-item" v-if='this.isLoggedIn()'>
            <a id="manage" class="nav-link text-dark" title="Manage">
                {{ 'Hello ' + this.currentUser + " !"}}
            </a>
        </li>
        <li class="nav-item" v-if='this.isLoggedIn() && !registerAccountStatus'>
            <a id="logout" class="btn btn-secondary" @click="logout">Logout</a>
        </li>

        <!--Créer un compte-->
        <li class="nav-item" v-if='!this.isLoggedIn() && !registerAccountStatus && !loginStatus'>
            <a class="btn btn-secondary" id="register" @click="showCreateAccount">Créer un compte</a>
        </li>
        <li class="nav-item" v-if='registerAccountStatus'>
            <label class="nav-link">Email</label>
        </li>
        <li class="nav-item" v-if='registerAccountStatus'>
            <input class="form-control" type="text" id="newEmail" maxlength="1024" min="4" required v-model="registerLogin.email" />
        </li>
        <li class="nav-item" v-if='registerAccountStatus'>
            <label class="nav-link">Mot&nbsp;de&nbsp;Passe</label>
        </li>
        <li class="nav-item" v-if='registerAccountStatus'>
            <input class="form-control" type="password" id="newPassword" maxlength="1024" min="4" required v-model="registerLogin.password" />
        </li>
        <li class="nav-item" v-if='registerAccountStatus'>
            <button class="btn btn-primary" @click="saveCreateAccount">Enregistrer</button>
        </li>
        <li class="nav-item" v-if='registerAccountStatus'>
            <button class="btn btn-secondary" @click="cancelCreateAccount">Annuler</button>
        </li>

        <!--Connexion-->
        <li class="nav-item" v-if='!this.isLoggedIn() && !registerAccountStatus && !loginStatus'>
            <a class="btn btn-primary" id="login" @click="showLogin">Connexion</a>
        </li>
        <li class="nav-item" v-if='loginStatus'>
            <label class="nav-link">Email</label>
        </li>
        <li class="nav-item" v-if='loginStatus'>
            <input class="form-control" type="text" id="email" maxlength="1024" min="4" required v-model="currentLogin.email" />
        </li>
        <li class="nav-item" v-if='loginStatus'>
            <label class="nav-link">Mot&nbsp;de&nbsp;Passe</label>
        </li>
        <li class="nav-item" v-if='loginStatus'>
            <input class="form-control" type="password" id="password" maxlength="1024" min="4" required v-model="currentLogin.password" />
        </li>
        <li class="nav-item" v-if='loginStatus'>
            <button class="btn btn-primary" @click="login">Se connecter </button>
        </li>
        <li class="nav-item" v-if='loginStatus'>
            <button class="btn btn-secondary" @click="cancelLogin">Annuler</button>
        </li>
    </ul>
</template>
<script lang="ts">
    import { defineComponent } from 'vue';
    import Swal from 'sweetalert2';
    import type Login from "@/types/Login";
    import type ResponseData from "@/types/ResponseDate";
    import AccountDataService from "@/services/AccountDataService";

    export default defineComponent({
        name: "login-display",
        data() {
            return {
                currentLogin: { email: "", password: "" } as Login,
                registerLogin: { email: "", password: "" } as Login,
                currentUser: "",
                registerAccountStatus: false,
                loginStatus: false,
            };
        },
        methods: {
            // Show Login
            showLogin() {
                this.loginStatus = true;
            },
            cancelLogin() {
                this.loginStatus = false;
            },

            // Login
            login() {
                let data = {
                    email: this.currentLogin.email,
                    password: this.currentLogin.password,
                };
                // Login - Ajout du Cookie
                AccountDataService.login(data)
                    .then((response: ResponseData) => {
                        console.log("OK ");
                        console.log(response.data.accessToken);
                        this.$cookies.set('.AspNetCore.Cookies', response.data.accessToken, '1', '/', 'localhost');
                        this.loginStatus = false;
                        this.getUserProfile();
                        this.$forceUpdate();
                        this.$router.push({ name: "movies", query: { _r: Date.now() } });
                    })
                    .catch((e: Error) => {
                        console.log(e);
                        Swal.fire({
                            icon: "error", title: "Erreur...", text: "Email et ou mot de passe incorrecte !"
                        });
                    });
            },
            logout() {
                this.$cookies.remove('.AspNetCore.Cookies');
                this.getUserProfile();
                this.$forceUpdate();
                this.$router.push({ name: "movies", query: { _r: Date.now() } });
            },
            isLoggedIn() {
                return this.$cookies.isKey('.AspNetCore.Cookies');
            },
            getUserProfile() {
                if (this.isLoggedIn()) {
                    AccountDataService.token = this.$cookies.get('.AspNetCore.Cookies');
                    AccountDataService.getUserInfo()
                        .then((response: ResponseData) => {
                            this.currentUser = response.data.email;
                        })
                        .catch((e: Error) => {
                            console.log(e);
                        });
                }
            },

            // Show Create account
            showCreateAccount() {
                this.registerAccountStatus = true;
            },
            cancelCreateAccount() {
                this.registerAccountStatus = false;
            },

            // Create account
            saveCreateAccount() {
                let data = {
                    username: this.registerLogin.email,
                    email: this.registerLogin.email,
                    password: this.registerLogin.password,
                };
                AccountDataService.register(data)
                    .then((response: ResponseData) => {
                        Swal.fire("Compte crée  !").then((result) => {
                            this.registerAccountStatus = false;
                            this.$forceUpdate();
                        });
                    })
                    .catch((e: Error) => {
                        console.log(e);
                        Swal.fire({
                            icon: "error", title: "Erreur...", text: "Erreur lors de l'enregistrement !"
                        });
                    });
            }
        },
        mounted() {
            this.getUserProfile();
        },
    });
</script>
<style>
</style>

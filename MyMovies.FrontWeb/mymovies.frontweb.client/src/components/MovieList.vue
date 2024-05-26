<template>
    <div class="row my-2">
        <h3 class="card-title text-success mb-3 ml-3">Liste des films</h3>
        <div class="text-center" v-if='isLoading'>
            <img src="/src/assets/img/loading.gif">
        </div>
        <div class="form-group py-1" v-if='!isLoading && isLoggedIn()'>
            <router-link to="/movie-create" class="btn btn-secondary">Ajouter un nouveau Film</router-link>
        </div>
        <div class="container" v-if='!isLoading'>
            <div class="card" v-for="(movie, index) in movies">
                <div class="card-body">
                    <img v-if="movie.imageUri!=null" :src="'/src/assets/img/movies/' + movie.imageUri" :alt="movie.title">
                    <img v-else :src="('src/assets/img/movies/Default.jpg')" :alt="movie.title">
                    <router-link :to="'/movie-edit/' + movie.id" class="card-title">
                        <h4><b>{{ movie.title }}</b> - {{ movie.releaseDate }}</h4>
                    </router-link>
                </div>
                <div class="card-details">{{ movie.resume }}</div>
            </div>
        </div>
    </div>

</template>

<script lang="ts">
    import { defineComponent } from "vue";
    import type Movie from "@/types/Movie";
    import type ResponseData from "@/types/ResponseDate";
    import MovieDataService from "@/services/MovieDataService";

    export default defineComponent({
        name: "movies-list",
        data() {
            return {
                movies: [] as Movie[],
                isLoading: true,
            };
        },
        methods: {
            retrieveMovies() {
                MovieDataService.getAll()
                    .then((response: ResponseData) => {
                        this.movies = response.data;
                        console.log(response.data);
                        this.isLoading = false;
                    })
                    .catch((e: Error) => {
                        console.log(e);
                    });
            },
            isLoggedIn() {
                return this.$cookies.isKey('.AspNetCore.Cookies');
            }
        },
        mounted() {
            this.retrieveMovies();
        },
    });
</script>

<style>
</style>
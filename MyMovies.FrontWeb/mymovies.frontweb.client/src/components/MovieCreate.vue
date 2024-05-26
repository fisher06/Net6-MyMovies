<template>
    <div class="row my-2">
        <h3 class="card-title text-success mb-3 ml-3">Création d'un nouveau Film</h3>

        <div class="form-group">
            <div class="col-12">
                <div class="form-group">
                    <label>Titre</label>
                    <input class="form-control" type="text" id="title" v-model="currentMovie.title" maxlength="4000" min="2" required />
                </div>
                <div class="form-group">
                    <label>Date de sortie</label>
                    <input class="form-control" type="number" id="releaseDate" v-model="currentMovie.releaseDate" required />
                </div>
                <div class="form-group">
                    <label>Durée du film</label>
                    <input class="form-control" type="text" id="runtime" v-model="currentMovie.runtime" required />
                </div>
                <div class="form-group">
                    <label>Résumé</label>
                    <textarea id="textarea" class="form-control" rows="8" max-rows="6" v-model="currentMovie.resume"></textarea>
                </div>
                <div class="form-group py-2">
                    <button class="btn btn-primary" @click="createMovie">Enregistrer</button>
                </div>
            </div>
        </div>
        <div class="form-group py-1">
            <router-link to="/" class="btn btn-secondary">Retour à la liste</router-link>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import Swal from 'sweetalert2';
    import type Movie from "@/types/Movie";
    import type ResponseData from "@/types/ResponseDate";
    import MovieDataService from "@/services/MovieDataService";

    export default defineComponent({
        name: "movie-edit",
        data() {
            return {
                currentMovie: {} as Movie,
            };
        },
        methods: {
            createMovie() {
                let data = {
                    title: this.currentMovie.title,
                    releaseDate: this.currentMovie.releaseDate,
                    runtime: this.currentMovie.runtime,
                    resume: this.currentMovie.resume,
                };
                MovieDataService.token = this.$cookies.get('.AspNetCore.Cookies');
                MovieDataService.create(data)
                    .then((response: ResponseData) => {
                        console.log(response.data);
                        Swal.fire("Nouveau film crée !").then((result) => {
                            this.$router.push({ name: "movies" });
                        });
                    })
                    .catch((e: Error) => {
                        Swal.fire({
                            icon: "error",
                            title: "Erreur...",
                            text: "Problème lors de la création du nouveau film !"
                        }).then((result) => {

                        });
                    });
            }
        },
        mounted() { },
    });
</script>

<style>
</style>
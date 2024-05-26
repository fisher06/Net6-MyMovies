<template>
    <div class="row my-2">
        <h3 class="card-title text-success mb-3 ml-3">Modification d'un Film</h3>
        <div class="text-center" v-if='isLoading'>
            <img src="/src/assets/img/loading.gif">
        </div>
        <div class="form-group" v-if='!isLoading'>
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
                    <button class="btn btn-primary" @click="updateMovie">Enregistrer</button>
                    <button class="btn btn-danger float-end" @click="deleteMovieConfirm">Supprimer</button>
                </div>
            </div>
        </div>
        <div class="form-group py-1" v-if='!isLoading'>
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
                isLoading: true,
            };
        },
        methods: {
            getMovie(id: any) {
                console.log("id" + id);
                MovieDataService.token = this.$cookies.get('.AspNetCore.Cookies');
                MovieDataService.get(id)
                    .then((response: ResponseData) => {
                        this.currentMovie = response.data;
                        console.log(response.data);
                        this.isLoading = false;
                    })
                    .catch((e: Error) => {
                        Swal.fire({
                            icon: "error",
                            title: "Erreur...",
                            text: "Problème lors de la récupération du film !"
                        }).then((result) => {
                            this.$router.push({ name: "movies" });
                        });
                    });
            },
            updateMovie() {
                let data = {
                    id: this.currentMovie.id,
                    title: this.currentMovie.title,
                    releaseDate: this.currentMovie.releaseDate,
                    runtime: this.currentMovie.runtime,
                    resume: this.currentMovie.resume,
                };
                this.isLoading = true;
                MovieDataService.token = this.$cookies.get('.AspNetCore.Cookies');
                MovieDataService.update(this.currentMovie.id, data)
                    .then((response: ResponseData) => {
                        console.log(response.data);
                        Swal.fire("Film mis à jour !").then(() => {
                            this.$router.push({ name: "movies" });
                        });
                    })
                    .catch((e: Error) => {
                        this.isLoading = false;
                        Swal.fire({
                            icon: "error",
                            title: "Erreur...",
                            text: "Problème lors de mise à jour du film !"
                        });
                    });
            },
            deleteMovieConfirm() {
                this.isLoading = true;
                Swal.fire({
                    title: "Confirmer la suppression du film ?",
                    text: "",
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#3085d6",
                    cancelButtonColor: "#d33",
                    confirmButtonText: "Confirmer",
                    cancelButtonText: "Annuler"
                }).then((result) => {
                    if (result.isConfirmed) {
                        MovieDataService.token = this.$cookies.get('.AspNetCore.Cookies');
                        MovieDataService.delete(this.currentMovie.id)
                            .then((response: ResponseData) => {
                                console.log(response.data);
                                Swal.fire("Film supprimé !").then((result) => {
                                    this.$router.push({ name: "movies" });
                                });
                            })
                            .catch((e: Error) => {
                                console.log(e);
                                Swal.fire({
                                    icon: "error",
                                    title: "Erreur...",
                                    text: "Problème lors de la suppression du film !"
                                });
                            });
                    }
                    this.isLoading = false;
                });
            }
        },
        mounted() {
            this.getMovie(this.$route.params.id);
        },
    });
</script>

<style>
</style>
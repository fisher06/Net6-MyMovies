<script setup lang="ts">
    import LoginDisplay from './components/LoginDisplay.vue'
</script>
<template>
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light box-shadow">
            <div class="container">
                <a class="navbar-brand text-success" href="/">MyMovies</a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                        aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse d-sm-inline-flex flex-sm-row-reverse">
                    <LoginDisplay />
                </div>
            </div>
        </nav>
    </header>

    <div class="container">
        <main>
            <article class="content px-4">
                <router-view />
            </article>
        </main>
    </div>

</template>
<script lang="ts">
    import { defineComponent } from "vue";

    export default defineComponent({
        name: "App",
        methods: {
        }
    });
</script>

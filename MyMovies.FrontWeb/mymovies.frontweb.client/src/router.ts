import { createWebHistory } from "vue-router";
import { createRouter } from "vue-router";
import type { RouteRecordRaw } from "vue-router";

const routes: Array<RouteRecordRaw> = [
    {
        path: "/",
        alias: "/movies",
        name: "movies",
        component: () => import("./components/MovieList.vue"),
    },
    {
        path: "/movie-edit/:id",
        name: "movie-edit",
        component: () => import("./components/MovieEdit.vue"),
    },
    {
        path: "/movie-create",
        name: "movie-create",
        component: () => import("./components/MovieCreate.vue"),
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;
import { createRouter, createWebHistory } from 'vue-router'
import Home from './pages/home.vue'
import Perfil from './pages/perfil.vue'
import Reservas from './pages/reservas.vue'

const routes = [
    { path: '/home', component: Home },
    { path: '/perfil', component: Perfil },
    { path: '/reservas', component: Reservas }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router

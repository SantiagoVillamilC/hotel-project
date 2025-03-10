<script setup>
import { ref, onMounted, onUnmounted } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const nombreUsuario = ref(null);

// Función para obtener el nombre del localStorage
const obtenerNombreUsuario = () => {
  const usuario = JSON.parse(localStorage.getItem("usuario"));
  nombreUsuario.value = usuario?.nombre || null;
};

// Función para cerrar sesión
const cerrarSesion = () => {
  const confirmar = confirm("¿Estás seguro de que quieres cerrar sesión?");
  if (confirmar) {
    localStorage.removeItem("usuario"); // Borra el usuario del localStorage
    obtenerNombreUsuario(); // Actualiza la variable reactiva
  }
};

// Evento para detectar cambios en localStorage
const detectarCambioStorage = (event) => {
  if (event.key === "usuario") {
    obtenerNombreUsuario();
  }
};

// Ejecutar la función al montar el componente
onMounted(() => {
  obtenerNombreUsuario();
  window.addEventListener("storage", detectarCambioStorage);
});

// Remover el eventListener cuando se desmonte el componente
onUnmounted(() => {
  window.removeEventListener("storage", detectarCambioStorage);
});
</script>

<template>
  <div class="barra-navegacion">
    <h3 class="logo">Página de reservas</h3>
    <nav>
      <ul class="lista-navegacion">
        <li class="elemento-navegacion">
          <router-link class="enlace" to="/home">Inicio</router-link>
        </li>
        <li class="elemento-navegacion">
          <router-link class="enlace" to="/reservas">Reservas</router-link>
        </li>
        <li class="elemento-navegacion">
          <router-link class="enlace" to="/perfil">Perfil</router-link>
        </li>
      </ul>
    </nav>
    <div class="boton-sesion" @click="nombreUsuario ? cerrarSesion() : router.push('/login')">
      {{ nombreUsuario ? `Bienvenido, ${nombreUsuario}` : 'Iniciar sesión' }}
    </div>
  </div>
</template>




<style scoped>
.barra-navegacion {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 15px 30px;
  height: 5vh;
}
.logo {
  font-size: 20px;
  font-weight: bold;
  color: #fff;
}

.lista-navegacion {
    display: flex;
  gap: 20px;
  list-style: none;
  padding: 10px 20px;
  margin: 0;
  background: rgba(255, 255, 255, 0.2);
  border-radius: 10px;
  backdrop-filter: blur(10px);
  -webkit-backdrop-filter: blur(10px);
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.enlace {
  text-decoration: none;
  font-size: 16px;
  font-weight: bold;
  color: white;
  padding: 10px 15px;
  transition: 0.3s;
  border-radius: 5px;
}

.enlace:hover {
  background: rgba(255, 255, 255, 0.3);
}

/* Botón de iniciar sesión */
.boton-sesion {
  background: white;
  color: black;
  padding: 10px 20px;
  font-size: 16px;
  font-weight: bold;
  border-radius: 10px;
  cursor: pointer;
  transition: 0.3s;
}

.boton-sesion:hover {
  background: #f0f0f0;
}
</style>

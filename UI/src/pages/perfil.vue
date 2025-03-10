<script setup>
import { ref } from 'vue'

const modoRegistro = ref(true) // true = registro, false = inicio de sesión

// Datos del usuario
const usuario = ref({
    nombre: '',
    correo: '',
    telefono: '',
    contraseña: ''
})

// Alternar entre Registro e Inicio de Sesión
const cambiarModo = () => {
    modoRegistro.value = !modoRegistro.value
}

// envío del formulario
const enviarFormulario = async () => {
    const endpoint = modoRegistro.value 
        ? 'http://localhost:36172/api/Perfil' 
        : 'http://localhost:36172/api/Perfil/login';

    const datos = modoRegistro.value 
        ? { nombre: usuario.value.nombre, correo: usuario.value.correo, telefono: usuario.value.telefono, contraseña: usuario.value.contraseña }
        : { correo: usuario.value.correo, contraseña: usuario.value.contraseña };

    try {
        const response = await fetch(endpoint, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(datos)
        });

        const resultado = await response.json();

        if (response.status === 401) {
            throw new Error(resultado.mensaje || 'Credenciales incorrectas');
        }

        if (!response.ok) {
            throw new Error(resultado.mensaje || 'Error en la solicitud');
        }

        if (!modoRegistro.value) {
            // Guardar el id, nombre y correo en localStorage
            localStorage.setItem("usuario", JSON.stringify({
                id: resultado.id,
                nombre: resultado.nombre,
                correo: resultado.correo
            }));
        }

        alert(modoRegistro.value ? 'Registro exitoso' : `Bienvenido, ${resultado.nombre}!`);
    } catch (error) {
        console.error(error);
        alert(error.message);
    }
};
</script>

<template>
    <div class="auth-container">
        <h1>{{ modoRegistro ? 'Registrarse' : 'Iniciar Sesión' }}</h1>
        
        <form @submit.prevent="enviarFormulario">
            <div v-if="modoRegistro">
                <label>Nombre</label>
                <input type="text" v-model="usuario.nombre" required />

                <label>Teléfono</label>
                <input type="text" v-model="usuario.telefono" required />
            </div>

            <label>Correo Electrónico</label>
            <input type="email" v-model="usuario.correo" required />

            <label>Contraseña</label>
            <input type="password" v-model="usuario.contraseña" required />

            <button type="submit">{{ modoRegistro ? 'Registrarse' : 'Iniciar Sesión' }}</button>
        </form>

        <p @click="cambiarModo" class="toggle">
            {{ modoRegistro ? '¿Ya tienes cuenta? Inicia sesión aquí' : '¿No tienes cuenta? Regístrate aquí' }}
        </p>
    </div>
</template>

<style scoped>
.auth-container {
    max-width: 400px;
    margin: auto;
    padding: 20px;
    border: 1px solid #ddd;
    border-radius: 10px;
    box-shadow: 2px 2px 10px rgba(0, 0, 0, 0.1);
    background: #fff;
    text-align: center;
}

h1 {
    margin-bottom: 20px;
}

form {
    display: flex;
    flex-direction: column;
    gap: 15px;
}

label {
    font-weight: bold;
    text-align: left;
}

input {
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 5px;
    width: 100%;
}

button {
    background: #007bff;
    color: white;
    padding: 10px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
}

button:hover {
    background: #0056b3;
}

.toggle {
    margin-top: 10px;
    color: #007bff;
    cursor: pointer;
}

.toggle:hover {
    text-decoration: underline;
}
</style>

<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import variables from '../variables';

// reactivo para las reservas
const reservas = ref([]);
const reservasSinFiltro = ref([]);

// obtener las reservas del usuario logueado utilizando su id
const refreshData = () => {
  const usuario = JSON.parse(localStorage.getItem("usuario"));
  if (!usuario) {
    reservas.value = [];
    reservasSinFiltro.value = [];
    return;
  }
  axios.get(variables.API_URL + "reserva/usuario/" + usuario.id)
    .then((response) => {
      reservas.value = response.data;
      reservasSinFiltro.value = response.data;
    })
    .catch((error) => {
      console.error(error);
    });
};

const deleteClick = (id) => {
  if (!confirm("¿Estás seguro de eliminar la reserva?")) {
    return;
  }
  axios.delete(variables.API_URL + "reserva/" + id)
    .then((response) => {
      refreshData();
      alert(response.data);
    });
};

onMounted(refreshData);
</script>

<template>
  <div class="contenedor">
    <div v-if="reservas.length > 0">
      <table class="tabla-reservas">
        <thead>
          <tr>
            <th>Hotel</th>
            <th>Fecha de Entrada</th>
            <th>Fecha de Salida</th>
            <th>Cantidad de Personas</th>
            <th>Estado</th>
            <th>Opciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="res in reservas" :key="res.id">
            <td>{{ res.hotel }}</td>
            <td>{{ res.fecha_entrada }}</td>
            <td>{{ res.fecha_salida }}</td>
            <td>{{ res.cantidad_personas }}</td>
            <td>{{ res.estado }}</td>
            <td class="opciones">
              <button class="boton-eliminar" @click="deleteClick(res.id)">🗑️ Eliminar</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <div v-else>
      <p>No tienes reservas</p>
    </div>
  </div>
</template>

<style scoped>
.contenedor {
  padding: 20px;
  text-align: center;
  font-family: Arial, sans-serif;
}

.tabla-reservas {
  width: 100%;
  border-collapse: collapse;
  margin-top: 10px;
}

.tabla-reservas th, .tabla-reservas td {
  border: 1px solid #ddd;
  padding: 10px;
  text-align: center;
}

.tabla-reservas th {
  background-color: #f8f9fa;
  font-weight: bold;
}

.opciones {
  text-align: center;
}

.boton-eliminar {
  background-color: #dc3545;
  color: white;
  padding: 8px 12px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 14px;
  transition: 0.3s;
}

.boton-eliminar:hover {
  background-color: #c82333;
}
</style>

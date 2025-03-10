<script setup>
import { ref, onMounted } from 'vue';

// bbtener hoteles de la API
const hoteles = ref([]);

const fetchHoteles = async () => {
  try {
    const response = await fetch('http://localhost:36172/api/Hotel');
    if (!response.ok) throw new Error('Error al obtener hoteles');
    hoteles.value = await response.json();
  } catch (error) {
    console.error(error);
  }
};

// reserva al hacer clic en una tarjeta
const manejarReserva = (hotel) => {
  const usuario = JSON.parse(localStorage.getItem("usuario"));

  if (usuario && usuario.nombre) {
    const confirmar = confirm(`¿Quieres registrar una reserva en el hotel ${hotel.nombre}?`);
    if (confirmar) {
      // Solicitar datos de la reserva
      let fechaInicio = prompt("Ingrese la fecha de inicio (YYYY-MM-DD):");
      let fechaFin = prompt("Ingrese la fecha final (YYYY-MM-DD):");
      let cantidadPersonas = prompt("Ingrese la cantidad de personas:");

      if (!fechaInicio || !fechaFin || !cantidadPersonas) {
        alert("Todos los datos son obligatorios.");
        return;
      }

      // construir objeto reserva
      const reserva = {
        IdPerfil: usuario.id,   
        Hotel: hotel.nombre,     
        FechaEntrada: fechaInicio,  
        FechaSalida: fechaFin,      
        CantidadPersonas: parseInt(cantidadPersonas),
        Estado: "Pendiente"
      };

      // enviar reserva a la API
      fetch('http://localhost:36172/api/reserva', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(reserva)
      })
      .then(response => response.json())
      .then(data => {
        alert("Reserva registrada con éxito");
      })
      .catch(error => {
        console.error(error);
        alert("Error al registrar la reserva");
      });
    }
  } else {
    alert("Debes iniciar sesión para hacer una reserva.");
  }
};

onMounted(fetchHoteles);
</script>

<template>
  <div>
    <h1>Hoteles Disponibles</h1>
    <div class="hoteles-container">
      <div v-for="hotel in hoteles" 
           :key="hotel.id" 
           class="hotel-card" 
           @click="manejarReserva(hotel)">
        <img :src="hotel.imagenUrl" :alt="hotel.nombre" class="hotel-img" />
        <h2>{{ hotel.nombre }}</h2>
        <p>{{ hotel.ubicacion }}</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.hoteles-container {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 20px;
  padding: 20px;
}

.hotel-card {
  border: 1px solid #ddd;
  border-radius: 10px;
  padding: 15px;
  text-align: center;
  box-shadow: 2px 2px 10px rgba(0, 0, 0, 0.1);
  background: #fff;
  transition: 0.3s;
  cursor: pointer;
}

.hotel-card:hover {
  background: rgba(255, 255, 255, 0.3);
  transform: translateY(-5px);
}

.hotel-img {
  width: 100%;
  height: 150px;
  object-fit: cover;
  border-radius: 10px;
}

@media (max-width: 1200px) {
  .hoteles-container {
    grid-template-columns: repeat(3, 1fr);
  }
}

@media (max-width: 900px) {
  .hoteles-container {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 600px) {
  .hoteles-container {
    grid-template-columns: repeat(1, 1fr);
  }
}
</style>

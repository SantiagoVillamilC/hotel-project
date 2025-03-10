<script setup>
import HelloWorld from './components/HelloWorld.vue'
import barraHeader from './components/barraHeader.vue';
</script>

<template>
    <header>
        <barra-header class="barra"/>
        <div class="contenedorElementosHeader">
            <div>
                <h4 class="lato-bold">Explora, descubre y reserva tu próxima aventura.</h4>
                <h5 class="lato-regular">Haz que cada viaje sea una historia para recordar.</h5>
            </div>
        </div>

        <div class="contenedor-info lato-regular">
            <div class="info-box">
                <h3>10M+</h3>
                <p>Clientes satisfechos</p>
            </div>
            <div class="info-box">
                <h3>09+</h3>
                <p>Años de experiencia</p>
            </div>
            <div class="info-box">
                <h3>12K</h3>
                <p>Destinos disponibles</p>
            </div>
            <div class="info-box">
                <h3>5.0</h3>
                <p>Calificación promedio</p>
            </div>
        </div>
    </header>

    <router-view></router-view>
</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Lato:wght@400;700&display=swap');

.lato-regular {
  font-family: "Lato", sans-serif;
  font-weight: 400;
  font-style: normal;
}

.lato-bold {
  font-family: "Lato", sans-serif;
  font-weight: 700;
  font-style: normal;
}

header {
    height: 90vh;
    background-image: url('../public/lake-8844310_1920.jpg');
    background-size: cover;
    background-position: center;
    background-repeat: no-repeat;
    border-radius: 0 0 15px 15px;
    margin: 0.5%;
    color: white;
    position: relative;
    margin-bottom: 5%;
}

.contenedorElementosHeader {
    height: 75%;
    margin: 0 5%;
    display: flex;
    align-items: center;
    justify-content: center;
    text-align: center;
}

.contenedorElementosHeader h4 {
    font-size: 60px;
    margin: 0;
}

.contenedorElementosHeader h5 {
    font-size: 25px;
    margin: 0;
}

.contenedor-info {
    display: flex;
    justify-content: center;
    gap: 20px;
    position: absolute;
    bottom: -25px;
    width: 100%;
    padding: 0 2%;
}

.info-box {
    background: rgba(255, 255, 255, 0.2);
    padding: 20px 30px;
    border-radius: 15px;
    text-align: center;
    width: 200px;
    backdrop-filter: blur(10px);
    -webkit-backdrop-filter: blur(10px);
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    color: white;
    font-size: 18px;
    font-weight: bold;
    transition: 0.3s;
}

.info-box h3 {
    font-size: 28px;
    margin: 0;
}

.info-box p {
    font-size: 14px;
    margin: 5px 0 0;
    /* color: black; */
}

.info-box:hover {
    background: rgba(255, 255, 255, 0.3);
    transform: translateY(-5px);
}

@media (max-width: 768px) {
    .contenedorElementosHeader h4 {
        font-size: 40px;
    }

    .contenedorElementosHeader h5 {
        font-size: 20px;
    }

    .contenedor-info {
        flex-wrap: wrap;
        gap: 10px;
    }

    .info-box {
        width: 45%;
        padding: 15px;
    }
}

@media (max-width: 480px) {
    .contenedor-info {
        flex-direction: column;
        align-items: center;
    }

    .info-box {
        width: 80%;
    }
}
</style>

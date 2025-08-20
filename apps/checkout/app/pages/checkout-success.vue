<script setup lang="ts">
const route = useRoute()

const email = computed(() => route.query.email)
</script>

<template>
  <div class="order">
    <div class="main">
      <div class="card">
        <h1 class="success-title">
          Seu pedido foi realizado
        </h1>

        <p class="success-message">
          Daqui a pouco você receberá um e-mail no endereço
          <span class="email-highlight">{{ email }}</span>
          com todos os detalhes de sua compra.
        </p>
      </div>
    </div>
  </div>
</template>

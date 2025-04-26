<script setup lang="ts">
import CustomerInfoStep from '~/components/checkout/CustomerInfoStep.vue'
import ShippingInfoStep from '~/components/checkout/ShippingInfoStep.vue'

definePageMeta({
  middleware: ['checkout'],
})

const store = useStoreState()
</script>

<template>
  <header class="bg-white w-full shadow py-4">
    <div class="max-w-7xl mx-auto px-4">
      <h1 class="text-xl font-semibold text-gray-700">
        {{ store.name }}
      </h1>
    </div>
  </header>

  <div class="checkout-container">
    <div class="left-column">
      <div class="form-section">
        <div class="header">
          <div class="header__track">
            <div class="number">
              1
            </div>
          </div>
          <div class="header__text">
            <p class="font-semibold">
              Identifique-se
            </p>
            <p>
              Utilizaremos seu e-mail para: Identificar seu perfil, histórico de compra, notificação de pedidos e carrinho de compras.
            </p>
          </div>
        </div>

        <CustomerInfoStep />
      </div>

      <div class="form-section">
        <div class="header">
          <div class="header__track">
            <div class="number">
              2
            </div>
          </div>
          <div class="header__text">
            <p class="font-semibold">
              Endereço
            </p>
            <p>
              Cadastre ou selecione um endereço
            </p>
          </div>
        </div>

        <ShippingInfoStep />
      </div>
    </div>

    <div class="payment-column">
      <div class="form-section">
        <div class="header">
          <div class="header__track">
            <div class="number">
              3
            </div>
          </div>
          <div class="header__text">
            <p class="font-semibold">
              Pagamento
            </p>
            <p>
              Escolha uma forma de pagamento
            </p>
          </div>
        </div>

        <div class="form-row">
          <label>Número do Cartão</label>
          <input type="text" class="input-field">
        </div>
      </div>
    </div>

    <div class="summary-column">
      <div class="form-section">
        <h2 class="text-xl font-bold mb-4">
          Resumo da Compra
        </h2>
        <div class="summary-item">
          <span>Produtos:</span>
          <span>R$ 999,00</span>
        </div>
      </div>
    </div>
  </div>
</template>

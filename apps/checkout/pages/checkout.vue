<script setup lang="ts">
import type { CustomerInfo } from '#checkout-ui/types'
import { CUSTOMER_INFO_HANDLER_KEY } from '#checkout-blocks/constants'

definePageMeta({
  middleware: ['checkout'],
})

provide(CUSTOMER_INFO_HANDLER_KEY, handleData)

const { submitCustomerInfo } = useCheckout()

async function handleData(data: CustomerInfo) {
  await $fetch('/api/checkout/customer', {
    method: 'PUT',
    body: data,
  })
  submitCustomerInfo(data)
}
</script>

<template>
  <div class="checkout-container">
    <div class="left-column">
      <UCustomerInfo />

      <UCustomerAddress />
    </div>

    <div class="payment-column">
      <UPayment />
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

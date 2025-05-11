<script setup lang="ts">
import type { Cep, CreditCardPayment, CustomerInfo, ShippingAddress } from '#checkout-ui/types'
import {
  CEP_HANDLER_KEY,
  CREDIT_CARD_FORM_KEY,
  CUSTOMER_ADDRESS_HANDLER_KEY,
  CUSTOMER_ADDRESS_SELECT_HANDLER_KEY,
  CUSTOMER_INFO_HANDLER_KEY,
} from '#checkout-blocks/constants'

definePageMeta({
  middleware: ['checkout'],
})

provide(CUSTOMER_INFO_HANDLER_KEY, handleCustomer)
provide(CEP_HANDLER_KEY, handleCep)
provide(CUSTOMER_ADDRESS_HANDLER_KEY, handleAddress)
provide(CUSTOMER_ADDRESS_SELECT_HANDLER_KEY, handleSelectAddress)
provide(CREDIT_CARD_FORM_KEY, handleCreditCard)

const {
  total,
  submitCustomerInfo,
  saveAddress,
  submitShippingAddress,
  setCartItems,
} = useCheckout()

async function sync() {
  const response = await $fetch('/api/checkout/summary', {
    headers: useRequestHeaders(),
  })
  if (response.customer) {
    submitCustomerInfo(response.customer)
    response.customer.addresses.forEach((address) => {
      saveAddress(address)
    })
    if (response.customer.addresses.length) {
      submitShippingAddress()
    }
  }
  if (response.cartItems) {
    setCartItems(response.cartItems)
  }
}

async function handleCustomer(data: CustomerInfo) {
  try {
    await $fetch('/api/checkout/customer', {
      method: 'PUT',
      body: data,
    })
    submitCustomerInfo(data)
  }
  catch (error) {
    return error.data.data
  }
}

async function handleCep(cep: string): Promise<Cep> {
  return await $fetch('/api/cep', { method: 'post', body: { cep } })
}

async function handleAddress(data: ShippingAddress) {
  if ('id' in data && !!data.id) {
    return
  }

  try {
    const response = await $fetch('/api/checkout/customer/addresses', {
      method: 'post',
      body: data,
    })
    saveAddress(response)
  }
  catch (error) {
    return error.data.data
  }
}

async function handleSelectAddress() {
  submitShippingAddress()
}

async function handleCreditCard(data: CreditCardPayment) {
  try {
    await $fetch('/api/checkout/payment', {
      method: 'POST',
      body: data,
    })
  }
  catch (error) {
    return error.data.data
  }
}

await callOnce(async () => sync())
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
          <span>Total:</span>
          <span>R$ {{ total.toFixed(2) }}</span>
        </div>
      </div>
    </div>
  </div>
</template>

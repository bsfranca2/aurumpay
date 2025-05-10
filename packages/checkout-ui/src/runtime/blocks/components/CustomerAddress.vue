<script setup lang="ts">
import { ref } from 'vue'
import Step from '../components/Step.vue'
import { useCheckout } from '../composables/useCheckout'
import { useCheckoutStep } from '../composables/useCheckoutStep'
import { ShippingStep } from '../constants'
import CustomerAddressForm from './CustomerAddressForm.vue'
import CustomerAddressSelectList from './CustomerAddressSelectList.vue'

const { mainAddress, hasAddress } = useCheckout()
const { isActive, isFilled, isAccessible } = useCheckoutStep(ShippingStep)

const showList = ref(hasAddress.value)

function onSubmitted() {
  showList.value = true
}
</script>

<template>
  <Step
    step="2"
    :title="$t('customerAddressTitle')"
    :description="$t('customerAddressDescription')"
    :is-active
    :is-accessible
    :is-filled
  >
    <template v-if="showList">
      <CustomerAddressSelectList />
    </template>
    <template v-else>
      <CustomerAddressForm @submitted="onSubmitted" />
    </template>

    <template #resume>
      <p class="font-medium">
        Endereço selecionado:
      </p>
      <p>{{ `${mainAddress?.addressLine1}, ${mainAddress?.number} - ${mainAddress?.neighborhood}` }}</p>
      <p>{{ `${mainAddress?.city}-${mainAddress?.state} | CEP ${mainAddress?.cep}` }}</p>
    </template>
  </Step>
</template>

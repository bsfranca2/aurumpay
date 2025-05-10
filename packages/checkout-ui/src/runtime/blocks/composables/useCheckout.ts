import type { CustomerAddress, CustomerInfo, ExistingAddress } from '../../types'
import { computed } from 'vue'
import { CustomerStep, ShippingStep } from '../constants'
import { useCheckoutState } from './useCheckoutState'
import { useCheckoutStepper } from './useCheckoutStepper'
import { useCheckoutSubmitted } from './useCheckoutSubmitted'

export function useCheckout() {
  const checkout = useCheckoutState()
  const form = useCheckoutSubmitted()
  const stepper = useCheckoutStepper()

  const hasAddress = computed(() => !!checkout.value.addresses.length)
  const mainAddress = computed(() => checkout.value.addresses.find(address => address.isMain))

  function submitCustomerInfo(customerInfo: CustomerInfo) {
    checkout.value.customerInfo = customerInfo
    form.submit(CustomerStep)
    stepper.goToNext()
  }

  function saveAddress(data: CustomerAddress) {
    const addressIndex = checkout.value.addresses.findIndex(
      address => 'id' in address && address.id === (data as ExistingAddress).id,
    )

    if (addressIndex !== -1) {
      checkout.value.addresses[addressIndex] = { ...data }
    }
    else {
      checkout.value.addresses.push({ ...data })
    }
  }

  function submitShippingAddress() {
    form.submit(ShippingStep)
    stepper.goToNext()
  }

  const getters = {
    ...checkout.value,
    hasAddress,
    mainAddress,
  }

  const actions = {
    submitCustomerInfo,
    saveAddress,
    submitShippingAddress,
  }

  return { ...getters, ...actions }
}

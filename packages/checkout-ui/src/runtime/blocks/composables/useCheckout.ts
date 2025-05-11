import type { CartItem } from '@aurumpay/api-types/checkout'
import type { CustomerAddress, CustomerInfo, ExistingAddress } from '../../types'
import { computed, readonly } from 'vue'
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

  const total = computed(() => {
    return checkout.value.cartItems.reduce((sum, item) => sum + item.product.price * item.quantity, 0)
  })

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

  function setCartItems(cartItems: CartItem[]) {
    checkout.value.cartItems = cartItems
  }

  const getters = {
    ...checkout.value,
    hasAddress,
    mainAddress,
    total: readonly(total),
  }

  const actions = {
    submitCustomerInfo,
    saveAddress,
    submitShippingAddress,
    setCartItems,
  }

  return { ...getters, ...actions }
}

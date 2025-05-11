import type { CartItem } from '@aurumpay/api-types/checkout'
import type { CustomerAddress, CustomerInfo } from '../../types'
import { useState } from '#imports'

interface CheckoutState {
  customerInfo: CustomerInfo
  addresses: CustomerAddress[]
  cartItems: CartItem[]
}

export function useCheckoutState() {
  return useState<CheckoutState>('checkout:data', () => ({
    customerInfo: {
      email: '',
      fullName: '',
      cpf: '',
      phoneNumber: '',
    },
    addresses: [],
    cartItems: [],
  }))
}

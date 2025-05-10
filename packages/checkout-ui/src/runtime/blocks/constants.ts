import type { InjectionKey } from 'vue'
import type {
  Cep,
  CustomerInfo,
  FormHandlerResponse,
  ShippingAddress,
} from '../types'

export const CustomerStep = 'customer-info'
export const ShippingStep = 'shipping-address'
export const PaymentStep = 'payment'
export type CheckoutStep = typeof CustomerStep | typeof ShippingStep | typeof PaymentStep

type CustomerInfoHandler = (data: CustomerInfo) => Promise<FormHandlerResponse>
export const CUSTOMER_INFO_HANDLER_KEY = Symbol('CUSTOMER_INFO_HANDLER_KEY') as InjectionKey<CustomerInfoHandler>

type CustomerAddressHandler = (data: ShippingAddress) => Promise<FormHandlerResponse>
export const CUSTOMER_ADDRESS_HANDLER_KEY = Symbol('CUSTOMER_ADDRESS_HANDLER_KEY') as InjectionKey<CustomerAddressHandler>
export const CEP_HANDLER_KEY = Symbol('CEP_HANDLER_KEY') as InjectionKey<(cep: string) => Promise<Cep>>
export const CUSTOMER_ADDRESS_ID_KEY = Symbol('CUSTOMER_ADDRESS_ID_KEY') as InjectionKey<number | undefined>

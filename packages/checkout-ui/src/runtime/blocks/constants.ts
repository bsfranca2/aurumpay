import type { InjectionKey } from 'vue'
import type { CustomerInfo, FormHandlerResponse } from '../types'

export const CustomerStep = 'customer-info'
export const ShippingStep = 'shipping-address'
export const PaymentStep = 'payment'
export type CheckoutStep = typeof CustomerStep | typeof ShippingStep | typeof PaymentStep

type CustomerInfoHandler = (data: CustomerInfo) => Promise<FormHandlerResponse>
export const CUSTOMER_INFO_HANDLER_KEY = Symbol() as InjectionKey<CustomerInfoHandler>

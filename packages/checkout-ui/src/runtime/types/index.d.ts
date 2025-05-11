export type {
  CustomerAddress,
} from '@aurumpay/api-types/checkout'

export type TranslateFn = (key: string) => string

export type FormHandlerResponse = { validationErrors: Record<string, string[]> } | null | undefined | void

export interface CustomerInfo {
  email: string
  fullName: string
  cpf: string
  phoneNumber: string
}

export interface Cep {
  cep: string
  state: string
  city: string
  street: string
  neighborhood: string
}

export interface Address {
  cep: string
  addressLine1: string
  addressLine2: string | undefined
  number: string
  neighborhood: string
  city: string
  state: string
}

export interface ExistingAddress extends Address {
  id: number
}

export type ShippingAddress = (Address | ExistingAddress) & {
  recipient: string
}

export interface CreditCardPayment {
  token: string
  cardholderName: string
  identificationNumber: string
  identificationType: string
  installments: number
}

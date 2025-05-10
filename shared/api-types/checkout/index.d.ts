// Generated types from OpenAPI schema

export interface API {
  AddCustomerAddress?: AddCustomerAddress
  CartItem?: CartItem
  CheckoutSession?: CheckoutSession
  CreateCheckout?: CreateCheckout
  Customer?: Customer
  CustomerAddress?: CustomerAddress
  HttpValidationProblemDetails?: HttpValidationProblemDetails
  IdentifyCustomer?: IdentifyCustomer
}

export interface AddCustomerAddress {
  addressLine1: string
  addressLine2?: string
  cep: string
  city: string
  neighborhood: string
  number: string
  recipient: string
  state: string
}

export interface CartItem {
  productId: number
  quantity: number
}

export interface CheckoutSession {
  cartItems: CartItem[]
  customer?: Customer
}

export interface Customer {
  addresses: CustomerAddress[]
  cpf: string
  email: string
  fullName: string
  isProspect: boolean
  phoneNumber: string
}

export interface CustomerAddress {
  addressLine1: string
  addressLine2: string
  cep: string
  city: string
  id: number
  isMain: boolean
  neighborhood: string
  number: string
  recipient: string
  state: string
}

export interface CreateCheckout {
  cartItems: { [key: string]: number }
}

export interface HttpValidationProblemDetails {
  detail?: string
  errors?: { [key: string]: string[] }
  instance?: string
  status?: number
  title?: string
  type?: string
}

export interface IdentifyCustomer {
  cpf: string
  email: string
  fullName: string
  phoneNumber: string
}

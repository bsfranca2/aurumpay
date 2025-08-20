// Generated types from OpenAPI schema

export interface API {
  AddCustomerAddress?: AddCustomerAddress
  CartItem?: CartItem
  CartItemProduct?: CartItemProduct
  CheckoutSession?: CheckoutSession
  CheckoutStatus?: number
  CreateCheckout?: CreateCheckout
  Customer?: Customer
  CustomerAddress?: CustomerAddress
  HttpValidationProblemDetails?: HttpValidationProblemDetails
  IdentifyCustomer?: IdentifyCustomer
  NullableOfPaymentGatewayId?: NullableOfPaymentGatewayId
  NullableOfPaymentMethodId?: NullableOfPaymentMethodId
  Order?: Order
  OrderItem?: OrderItem
  OrderPayment?: OrderPayment
  OrderPaymentStatus?: number
  OrderStatus?: number
  PaymentStatus?: number
  ProcessOrderPaymentRequest?: ProcessOrderPaymentRequest
  SelectPaymentMethod?: SelectPaymentMethod
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
  product: CartItemProduct
  quantity: number
}

export interface CartItemProduct {
  id: number
  name: string
  price: number
}

export interface CheckoutSession {
  cartItems: CartItem[]
  customer?: Customer
  paymentGatewayId?: NullableOfPaymentGatewayId
  paymentMethodId?: NullableOfPaymentMethodId
  status: number
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

export interface NullableOfPaymentGatewayId {
  value?: number
}

export interface NullableOfPaymentMethodId {
  value?: number
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

export interface Order {
  createdAt: Date
  customerId: number
  id: number
  items: OrderItem[]
  orderTotal: number
  paymentStatus: number
  status: number
  storeId: number
}

export interface OrderItem {
  productId: number
  productName: string
  quantity: number
  totalPrice: number
  unitPrice: number
}

export interface OrderPayment {
  amount: number
  orderId: number
  orderPaymentStatus: number
  orderStatus: number
  paymentId: string
  paymentInstructions: string
  paymentStatus: number
}

export interface ProcessOrderPaymentRequest {
  paymentData: { [key: string]: any }
}

export interface SelectPaymentMethod {
  paymentMethodType?: string
}

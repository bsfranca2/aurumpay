import type { ProblemDetails, Store } from '#shared/types/api'
import type {
  CheckoutSession,
  CreateCheckout,
  CustomerAddress,
  IdentifyCustomer,
  Order,
  ProcessOrderPaymentRequest,
  SelectPaymentMethod,
} from '@aurumpay/api-types/checkout'
import type { AxiosInstance, AxiosResponse } from 'axios'
import type { FetchOptions } from 'ofetch'
import type { Either } from 'result'
import { left, right } from 'result'

type ApiFn = <Right, Left = ProblemDetails>(url: string, options: FetchOptions) => Promise<Either<Left, Right>>

export function createApiSdk(api: ApiFn) {
  return {
    api,

    // Store
    store: () => api<Store>('/store', { method: 'get' }),

    // Checkout
    checkout: {
      init: (data: CreateCheckout) => api('/checkout/init/product', { method: 'post', body: data }),
      summary: () => api<CheckoutSession>('/checkout/summary', { method: 'get' }),
      identifyCustomer: (data: IdentifyCustomer) => api('/checkout/customer', { method: 'put', body: data }),
      addAddress: (data: CustomerAddress) => api<CustomerAddress>('/checkout/customer/addresses', { method: 'post', body: data }),
      paymentMethod: (data: SelectPaymentMethod) => api('/checkout/payment-method', { method: 'put', body: data }),
      finalize: () => api<Order>('/checkout/finalize', { method: 'post' }),
    },

    orders: {
      payment: (id: number, data: ProcessOrderPaymentRequest) => api(`/orders/${id}/payment`, { method: 'post', body: data }),
    },
  }
}

export function createApiErrorHandler(api: AxiosInstance) {
  async function apiWithErrorHandler<Left, Right>(url: string, options: FetchOptions): Promise<Either<Left, Right>> {
    try {
      const method = (options.method || 'GET').toUpperCase()
      const axiosConfig = {
        params: options.query,
      }

      let response: AxiosResponse<Right>

      if (method === 'POST') {
        response = await api.post(url, options.body, axiosConfig)
      }
      else if (method === 'PUT') {
        response = await api.put(url, options.body, axiosConfig)
      }
      else {
        response = await api.get(url, axiosConfig)
      }

      return right(response.data)
    }
    catch (err: any) {
      console.error('API Error:', err.response?.data || err.message)
      return left(err.response?.data || err.message || err)
    }
  }
  return apiWithErrorHandler
}

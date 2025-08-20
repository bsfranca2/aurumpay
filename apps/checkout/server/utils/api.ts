import type { ProblemDetails } from '#shared/types/api'
import type { H3Event } from 'h3'
import type { Either } from 'result'
import { cartCookieName, cartHeaderName, checkoutSessionCookieName, checkoutSessionHeaderName } from '#shared/lib/cookies'
import axios from 'axios'
import { decodeJwt } from 'jose'
import { createApiErrorHandler, createApiSdk } from '../lib/api'

export function getApiSdk(event: H3Event) {
  const { apiUrl, checkoutSessionExpiration, cartMaxAge } = useRuntimeConfig()
  const cartCookie = getCookie(event, cartCookieName)
  const checkoutSessionCookie = getCookie(event, checkoutSessionCookieName)

  const proxyHeaders = getProxyRequestHeaders(event)
  delete proxyHeaders['content-length']
  delete proxyHeaders['Content-Length']
  delete proxyHeaders['content-type']
  delete proxyHeaders['Content-Type']

  const api = axios.create({
    baseURL: apiUrl,
    headers: {
      ...proxyHeaders,
      ...(checkoutSessionCookie ? { [checkoutSessionHeaderName]: `Bearer ${checkoutSessionCookie}` } : {}),
      ...(cartCookie ? { [cartHeaderName]: cartCookie } : {}),
    },
  })

  api.interceptors.response.use(
    (response) => {
      const bearerToken = response.headers[checkoutSessionHeaderName.toLowerCase()]
      if (bearerToken) {
        const token = bearerToken.split(' ')[1]
        try {
          const payload = decodeJwt(token)
          const expiresAt = payload.exp
            ? new Date(payload.exp * 1000)
            : new Date(Date.now() + checkoutSessionExpiration)
          setCookie(event, checkoutSessionCookieName, token, {
            httpOnly: true,
            secure: true,
            sameSite: 'strict',
            expires: expiresAt,
          })
        }
        catch {
          setCookie(event, checkoutSessionCookieName, token, {
            httpOnly: true,
            secure: true,
            sameSite: 'strict',
            expires: new Date(Date.now() + checkoutSessionExpiration),
          })
        }
      }

      const cartId = response.headers[cartHeaderName.toLowerCase()]
      if (cartId) {
        setCookie(event, cartCookieName, cartId, {
          httpOnly: true,
          secure: true,
          sameSite: 'strict',
          maxAge: cartMaxAge,
        })
      }

      return response
    },
    (error) => {
      console.error('Response Error:', error.response?.status, error.message)
      throw error
    },
  )

  const apiWithErrorHandler = createApiErrorHandler(api)
  return createApiSdk(apiWithErrorHandler)
}

export function mapResponse<Left extends ProblemDetails, Right>(response: Either<Left, Right>) {
  if (response.isLeft()) {
    throw createError({
      statusCode: response.value?.status ?? 500,
      statusMessage: response.value?.title ?? 'Server error',
      data: response.value,
    })
  }
  return response.value
}

import type { H3Event } from 'h3'
import type { Either } from 'result'
import type { ProblemDetails } from '~/server/types/api'
import { decodeJwt } from 'jose'
import { ofetch } from 'ofetch'
import { cartCookieName, cartHeaderName, checkoutSessionCookieName, checkoutSessionHeaderName } from '~/lib/cookies'
import { createApiErrorHandler, createApiSdk } from '~/server/lib/api'

export function getApiSdk(event: H3Event) {
  const { apiUrl, checkoutSessionExpiration, cartMaxAge } = useRuntimeConfig()
  const cartCookie = getCookie(event, cartCookieName)
  const checkoutSessionCookie = getCookie(event, checkoutSessionCookieName)
  const api = ofetch.create({
    baseURL: apiUrl,
    headers: {
      ...getProxyRequestHeaders(event),
      ...(checkoutSessionCookie ? { [checkoutSessionHeaderName]: `Bearer ${checkoutSessionCookie}` } : {}),
      ...(cartCookie ? { [cartHeaderName]: cartCookie } : {}),
    },
    onResponse({ response }) {
      // TODO: Check if exists but empty
      const bearerToken = response.headers.get(checkoutSessionHeaderName)
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

      const cartId = response.headers.get(cartHeaderName)
      if (cartId) {
        setCookie(event, cartCookieName, cartId, {
          httpOnly: true,
          secure: true,
          sameSite: 'strict',
          maxAge: cartMaxAge,
        })
      }
    },
  })
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

import type { H3Event } from 'h3'
import { checkoutSessionCookieName } from '~/lib/cookies'

export function requireCheckoutSession(event: H3Event) {
  const checkoutSessionCookie = getCookie(event, checkoutSessionCookieName)
  if (!checkoutSessionCookie) {
    throw createError({
      statusCode: 401,
    })
  }
}

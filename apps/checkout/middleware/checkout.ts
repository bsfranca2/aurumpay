import { useHost } from '~/composables/useHost.server'
import { checkoutSessionCookieName, cartCookieName } from '~/lib/cookies'

export default defineNuxtRouteMiddleware(() => {
  if (import.meta.browser) {
    return
  }

  const checkoutSession = useCookie(checkoutSessionCookieName)
  if (checkoutSession.value) {
    return
  }
  const lastCart = useCookie(cartCookieName)
  if (!lastCart.value) {
    return navigateTo('/cart')
  }
  const { url } = useHost()
  return navigateTo(`${url}/restore`, { external: true })
})

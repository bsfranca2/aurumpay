import { cartCookieName } from '~/lib/cookies'

export default defineEventHandler(async (event) => {
  const apiSdk = getApiSdk(event)
  const response = await apiSdk.checkout.init({ cartItems: {} })
  if (response.isLeft()) {
    deleteCookie(event, cartCookieName, {
      httpOnly: true,
      secure: true,
      sameSite: 'strict',
    })
    return sendRedirect(event, '/cart')
  }
  return sendRedirect(event, '/checkout')
})

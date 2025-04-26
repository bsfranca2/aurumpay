export default defineEventHandler(async (event) => {
  try {
    const apiSdk = getApiSdk(event)
    const tokens = getRouterParam(event, 'tokens') ?? ''
    // TODO: Handle invalid tokens or quantity
    const cartItems = parseCartItems(tokens)
    const response = await apiSdk.checkout.init({ cartItems })
    if (response.isLeft()) {
      throw new Error('Invalid response')
    }
    return sendRedirect(event, '/checkout')
  }
  catch {
    throw createError({
      statusCode: 404,
      statusMessage: 'Page Not Found',
    })
  }
})

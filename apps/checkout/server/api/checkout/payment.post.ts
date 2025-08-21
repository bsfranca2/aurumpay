import { checkoutSessionCookieName } from '~~/shared/lib/cookies'

export default defineEventHandler(async (event) => {
  requireCheckoutSession(event)

  const body = await readBody(event)

  const apiSdk = getApiSdk(event)

  const paymentMethodResult = await apiSdk.checkout.paymentMethod({
    paymentMethodType: 'CreditCard',
  })
  const _paymentMethod = mapResponse(paymentMethodResult)

  const orderResult = await apiSdk.checkout.finalize()
  const _order = mapResponse(orderResult)

  const paymentResult = await apiSdk.checkout.payment({
    paymentData: body,
  })
  const payment = mapResponse(paymentResult)

  if ([1, 2].includes(payment.paymentStatus)) {
    deleteCookie(event, checkoutSessionCookieName)
  }

  return payment
})

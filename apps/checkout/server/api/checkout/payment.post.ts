export default defineEventHandler(async (event) => {
  requireCheckoutSession(event)

  const body = await readBody(event)

  const apiSdk = getApiSdk(event)

  const paymentMethodResult = await apiSdk.checkout.paymentMethod({
    paymentMethodType: 'CreditCard',
  })
  const _paymentMethod = mapResponse(paymentMethodResult)

  const orderResult = await apiSdk.checkout.finalize()
  const order = mapResponse(orderResult)

  const paymentResult = await apiSdk.orders.payment(order.id, {
    paymentData: body,
  })
  return mapResponse(paymentResult)
})

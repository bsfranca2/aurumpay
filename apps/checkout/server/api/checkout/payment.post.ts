export default defineEventHandler(async (event) => {
  requireCheckoutSession(event)

  // const apiSdk = getApiSdk(event)

  // const body = await readBody(event)
  // const response = await apiSdk.checkout.payment(body)
  // return mapResponse(response)
  console.log('Payment processing is not implemented yet.')
})

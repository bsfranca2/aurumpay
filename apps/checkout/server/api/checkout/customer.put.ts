export default defineEventHandler(async (event) => {
  requireCheckoutSession(event)

  const apiSdk = getApiSdk(event)

  const body = await readBody(event)
  const response = await apiSdk.checkout.identifyCustomer(body)
  return mapResponse(response)
})

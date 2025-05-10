export default defineEventHandler(async (event) => {
  requireCheckoutSession(event)

  const apiSdk = getApiSdk(event)

  const body = await readBody(event)
  const response = await apiSdk.checkout.addAddress(body)
  return mapResponse(response)
})
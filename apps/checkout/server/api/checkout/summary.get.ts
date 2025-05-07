export default defineEventHandler(async (event) => {
  const apiSdk = getApiSdk(event)
  const response = await apiSdk.checkout.summary()
  return mapResponse(response)
})

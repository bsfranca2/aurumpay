export default defineEventHandler(async (event) => {
  const apiSdk = getApiSdk(event)

  const store = await apiSdk.store()

  if (store.isLeft()) {
    throw createError({
      statusCode: 404,
    })
  }

  return {
    name: store.value.name,
  }
})

import { length, object, pipe, safeParse, string, transform } from 'valibot'

const cepSchema = object({
  cep: pipe(
    string(),
    transform(s => s.replace(/\D/g, '')),
    length(8),
  ),
})

export default defineEventHandler(async (event) => {
  requireCheckoutSession(event)

  const result = await readValidatedBody(event, body => safeParse(cepSchema, body))
  if (!result.success) {
    throw createError({
      statusCode: 400,
    })
  }

  const response = await cachedCep(result.output.cep)
  if (!response) {
    throw createError({
      statusCode: 400,
    })
  }
  return response
})

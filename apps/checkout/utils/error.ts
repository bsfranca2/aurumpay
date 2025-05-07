import type { ProblemDetails } from '~/server/types/api'
import { FetchError } from 'ofetch'

export type ErrorWithProblemDetails = FetchError & {
  data: {
    data: ProblemDetails
  }
}

export function isProblemDetails(err: unknown): err is ErrorWithProblemDetails {
  if (err instanceof FetchError) {
    const errorData = err.data.data
    return (
      typeof errorData === 'object'
      && Object.keys(errorData)
      && 'type' in errorData
      && 'title' in errorData
      && 'status' in errorData
    )
  }
  return false
}

export function mapToErrorMessage(err: unknown) {
  if (err instanceof FetchError || err instanceof Error) {
    return err.message
  }
  return `${err}`
}

import type { FormContext } from 'vee-validate'
import type { ErrorWithProblemDetails } from '~/utils/error'

export function mapToFormErrors(setFieldError: FormContext['setFieldError'], response: ErrorWithProblemDetails) {
  const errors = response.data.data.validationErrors ?? {}

  for (const fieldName in errors) {
    setFieldError(fieldName, errors[fieldName][0])
  }
}

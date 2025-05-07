import type { FormContext } from 'vee-validate'
import type { FormHandlerResponse } from '../types'

export function mapToFormErrors(response: FormHandlerResponse, setFieldError: FormContext['setFieldError']) {
  const errors = response?.validationErrors
  if (errors) {
    for (const fieldName in errors) {
      setFieldError(fieldName, errors[fieldName][0])
    }
  }
}

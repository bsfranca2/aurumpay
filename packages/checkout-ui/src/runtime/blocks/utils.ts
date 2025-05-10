import type { FormContext } from 'vee-validate'
import type { Cep, FormHandlerResponse } from '../types'

export function mapToFormErrors(response: FormHandlerResponse, setFieldError: FormContext['setFieldError']) {
  const errors = response?.validationErrors
  if (errors) {
    for (const fieldName in errors) {
      setFieldError(fieldName, errors[fieldName][0])
    }
  }
  return !!errors
}

export function noop(...args: any): Promise<FormHandlerResponse> {
  console.warn('Noop handler called', ...args)
  return new Promise(resolve => setTimeout(resolve, 1000))
}

export function noopCep(cep: string): Promise<Cep> {
  console.warn('Noop cep handler called. Please provide a cep handler to the checkout block.')
  return Promise.resolve({
    cep,
    state: '',
    city: '',
    street: '',
    neighborhood: '',
  })
}

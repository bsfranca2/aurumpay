import type { TranslateFn } from './types'
import { validateCPF, validateFullName } from '@aurumpay/lib/checkout'
import { custom, length, maxLength, minLength, object, optional, pipe, rfcEmail, string, transform } from 'valibot'

export function createCustomerSchema(t: TranslateFn) {
  return object({
    fullName: pipe(
      string(t('requiredError')),
      transform(s => s.trim()),
      custom(validateFullName, t('fullNameError')),
    ),

    email: pipe(
      string(t('requiredError')),
      rfcEmail(t('emailError')),
    ),

    cpf: pipe(
      string(t('requiredError')),
      transform(s => s.replace(/\D/g, '')),
      length(11, t('cpfError')),
      custom(validateCPF, t('cpfError')),
    ),

    phoneNumber: pipe(
      string(t('requiredError')),
      transform(s => s.replace(/\D/g, '')),
      minLength(10, t('phoneNumberError')),
      maxLength(11, t('phoneNumberError')),
    ),
  })
}

export function createAddressSchema(t: TranslateFn) {
  return object({
    zipCode: pipe(
      string(t('zipCodeError')),
      transform(s => s.replace(/\D/g, '')),
      length(8, t('zipCodeError')),
    ),

    addressLine1: pipe(
      string(t('fieldRequired')),
      minLength(5, t('fieldInvalid')),
    ),

    number: pipe(
      string(t('fieldRequired')),
      minLength(1, t('fieldInvalid')),
      maxLength(6, t('fieldInvalid')),
    ),

    neighborhood: pipe(
      string(t('fieldRequired')),
      minLength(3, t('fieldInvalid')),
      maxLength(40, t('fieldInvalid')),
    ),

    addressLine2: optional(
      pipe(string(t('fieldInvalid')), maxLength(30, t('fieldInvalid'))),
    ),

    recipient: pipe(
      string(t('requiredError')),
      transform(s => s.trim()),
      minLength(3, t('fieldInvalid')),
      custom(validateFullName, t('fullNameError')),
    ),
  })
}

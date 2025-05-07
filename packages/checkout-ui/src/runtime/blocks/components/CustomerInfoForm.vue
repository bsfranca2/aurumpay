<script setup lang="ts">
import Button from '#checkout-base/components/Button.vue'
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '#checkout-base/components/form'
import Input from '#checkout-base/components/Input.vue'
import { useI18n } from '#imports'
import { validateCPF, validateFullName } from '@aurumpay/lib/checkout'
import { toTypedSchema } from '@vee-validate/valibot'
import { Loader2 } from 'lucide-vue-next'
import { custom, length, maxLength, minLength, object, pipe, rfcEmail, string, transform } from 'valibot'
import { useForm } from 'vee-validate'
import { inject } from 'vue'
import { CUSTOMER_INFO_HANDLER_KEY } from '../constants'
import { mapToFormErrors } from '../utils'

const { t } = useI18n()
const handler = inject(CUSTOMER_INFO_HANDLER_KEY)!

const customerInfoSchema = object({
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

const { isSubmitting, handleSubmit, setFieldError } = useForm({
  validationSchema: toTypedSchema(customerInfoSchema),
})

const onSubmit = handleSubmit(async (values) => {
  const response = await handler(values)
  mapToFormErrors(response, setFieldError)
})
</script>

<template>
  <form class="space-y-4" @submit="onSubmit">
    <FormField v-slot="{ componentField }" name="fullName">
      <FormItem>
        <FormLabel>{{ $t('fullName') }}</FormLabel>
        <FormControl>
          <Input type="text" placeholder="ex.: Ana Luiza Moreira" v-bind="componentField" />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="email">
      <FormItem>
        <FormLabel>{{ $t('email') }}</FormLabel>
        <FormControl>
          <Input type="email" placeholder="ex.: ana@gmail.com" v-bind="componentField" />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="cpf">
      <FormItem>
        <FormLabel>{{ $t('cpf') }}</FormLabel>
        <FormControl>
          <Input v-mask="['###.###.###-##']" type="text" placeholder="000.000.000-00" v-bind="componentField" />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="phoneNumber">
      <FormItem>
        <FormLabel>{{ $t('phoneNumber') }}</FormLabel>
        <FormControl>
          <Input v-mask="['(##) ####-####', '(##) #####-####']" type="text" placeholder="(00) 00000-0000" v-bind="componentField" />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <div>
      <Button type="submit" :disabled="isSubmitting" block>
        <Loader2 v-if="isSubmitting" class="w-4 h-4 mr-2 animate-spin" />
        {{ $t('continue') }}
      </Button>
    </div>
  </form>
</template>

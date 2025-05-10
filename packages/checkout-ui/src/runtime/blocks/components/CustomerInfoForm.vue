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
import { toTypedSchema } from '@vee-validate/valibot'
import { useForm } from 'vee-validate'
import { inject } from 'vue'
import { createCustomerSchema } from '#checkout-ui/schemas'
import { CUSTOMER_INFO_HANDLER_KEY } from '../constants'
import { mapToFormErrors, noop } from '../utils'

const handler = inject(CUSTOMER_INFO_HANDLER_KEY, noop)

const { t } = useI18n()

const customerInfoSchema = createCustomerSchema(t)

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
      <Button type="submit" :loading="isSubmitting" block>
        {{ $t('continue') }}
      </Button>
    </div>
  </form>
</template>

<script setup lang="ts">
import type { Cep } from '#checkout-ui/types'
import Button from '#checkout-base/components/Button.vue'
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '#checkout-base/components/form'
import Input from '#checkout-base/components/Input.vue'
import { createAddressSchema } from '#checkout-ui/schemas'
import { useI18n } from '#imports'
import { toTypedSchema } from '@vee-validate/valibot'
import { useForm } from 'vee-validate'
import { computed, inject, ref, shallowRef, watchEffect } from 'vue'
import { CEP_HANDLER_KEY, CUSTOMER_ADDRESS_HANDLER_KEY, CUSTOMER_ADDRESS_ID_KEY } from '../constants'
import { mapToFormErrors, noop, noopCep } from '../utils'

const emit = defineEmits<{
  (e: 'submitted'): void
}>()

const formHandler = inject(CUSTOMER_ADDRESS_HANDLER_KEY, noop)
const cepHandler = inject(CEP_HANDLER_KEY, noopCep)
const addressId = inject(CUSTOMER_ADDRESS_ID_KEY, undefined)

const { t } = useI18n()

const lastCepHandled = ref<string>()
const cepStatus = ref<'idle' | 'loading' | 'invalid' | 'valid'>('idle')
const isCepValid = computed(() => cepStatus.value === 'valid')
const isCepLoading = computed(() => cepStatus.value === 'loading')
const cepData = shallowRef({} as Cep)

const customerAddressSchema = createAddressSchema(t)

const { values, isSubmitting, handleSubmit, validateField, setFieldValue, setFieldError } = useForm({
  validationSchema: toTypedSchema(customerAddressSchema),
})

const onSubmit = handleSubmit(async (values) => {
  const response = await formHandler({
    id: addressId,
    cep: values.zipCode,
    addressLine1: values.addressLine1,
    addressLine2: values.addressLine2 ?? '',
    number: values.number,
    neighborhood: values.neighborhood,
    city: cepData.value.city,
    state: cepData.value.state,
    recipient: values.recipient,
  })
  const hasErrors = mapToFormErrors(response, setFieldError)
  if (!hasErrors) {
    emit('submitted')
  }
})

async function onCepChange(cep: string | undefined) {
  if (typeof cep !== 'string') {
    cepStatus.value = 'invalid'
    return
  }

  if (cep === lastCepHandled.value) {
    return
  }
  lastCepHandled.value = cep

  const { valid } = await validateField('zipCode')
  if (!valid) {
    cepStatus.value = 'invalid'
    return
  }

  cepStatus.value = 'loading'
  try {
    const data = await cepHandler(cep)
    setFieldValue('neighborhood', data.neighborhood)
    setFieldValue('addressLine1', data.street)
    cepData.value = data
    cepStatus.value = 'valid'
  }
  catch (error) {
    console.error(error)
    cepStatus.value = 'invalid'
    setFieldError('zipCode', t('zipCodeError'))
  }
}

watchEffect(() => {
  onCepChange(values.zipCode)
})
</script>

<template>
  <form class="form" @submit="onSubmit">
    <FormField v-slot="{ componentField }" name="zipCode">
      <FormItem class="mb-4">
        <FormLabel>{{ $t('zipCode') }}</FormLabel>
        <FormControl>
          <Input
            v-mask="['#####-###']"
            type="text"
            placeholder="00000-000"
            v-bind="componentField"
            :disabled="isCepLoading"
            minlength="9"
            maxlength="9"
            autofocus
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <div v-show="isCepValid" class="address-details">
      <p class="state-city">
        {{ cepData.city }} / {{ cepData.state }}
      </p>

      <FormField v-slot="{ componentField }" name="addressLine1">
        <FormItem>
          <FormLabel>{{ $t('addressLine1') }}</FormLabel>
          <FormControl>
            <Input type="text" v-bind="componentField" minlength="5" />
          </FormControl>
          <FormMessage />
        </FormItem>
      </FormField>

      <div class="number-neighborhood">
        <FormField v-slot="{ componentField }" name="number">
          <FormItem>
            <FormLabel>{{ $t('number') }}</FormLabel>
            <FormControl>
              <Input type="text" v-bind="componentField" maxlength="6" />
            </FormControl>
            <FormMessage />
          </FormItem>
        </FormField>

        <FormField v-slot="{ componentField }" name="neighborhood">
          <FormItem>
            <FormLabel>{{ $t('neighborhood') }}</FormLabel>
            <FormControl>
              <Input type="text" v-bind="componentField" minlength="3" maxlength="40" />
            </FormControl>
            <FormMessage />
          </FormItem>
        </FormField>
      </div>

      <FormField v-slot="{ componentField }" name="addressLine2">
        <FormItem>
          <FormLabel>{{ $t('addressLine2') }}</FormLabel>
          <FormControl>
            <Input type="text" v-bind="componentField" maxlength="30" />
          </FormControl>
          <FormMessage />
        </FormItem>
      </FormField>

      <FormField v-slot="{ componentField }" name="recipient">
        <FormItem>
          <FormLabel>{{ $t('recipient') }}</FormLabel>
          <FormControl>
            <Input type="text" v-bind="componentField" minlength="3" />
          </FormControl>
          <FormMessage />
        </FormItem>
      </FormField>

      <Button type="submit" :loading="isSubmitting" block>
        {{ $t('continue') }}
      </Button>
    </div>
  </form>
</template>

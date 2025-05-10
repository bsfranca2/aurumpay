<script setup lang="ts">
import { toTypedSchema } from '@vee-validate/valibot'
import { number, object } from 'valibot'
import { useForm } from 'vee-validate'
import Button from '../../base/components/Button.vue'
import {
  FormControl,
  FormField,
  FormItem,
} from '../../base/components/form'
import { RadioCardGroup, RadioCardItem } from '../../base/components/radio-card'
import { useCheckout } from '../composables/useCheckout'
import AddressCard from './AddressCard.vue'

const { addresses } = useCheckout()

const formSchema = toTypedSchema(object({
  addressId: number(),
}))
const { isSubmitting, handleSubmit } = useForm({
  validationSchema: formSchema,
  initialValues: {
    addressId: addresses[0].id,
  },
})

const onSubmit = handleSubmit(() => {
  console.log('selected address')
})
</script>

<template>
  <form @submit="onSubmit">
    <FormField v-slot="{ componentField }" type="radio" name="addressId">
      <FormItem>
        <FormControl>
          <RadioCardGroup v-bind="componentField">
            <div v-for="address in addresses" :key="address.id">
              <FormItem>
                <RadioCardItem :value="address.id">
                  <AddressCard v-bind="address" />
                </RadioCardItem>
              </FormItem>
            </div>
          </RadioCardGroup>
        </FormControl>
      </FormItem>
    </FormField>

    <Button type="submit" :loading="isSubmitting" block>
      {{ $t('continue') }}
    </Button>
  </form>
</template>

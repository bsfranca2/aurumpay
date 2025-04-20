<script setup lang="ts">
import { toTypedSchema } from '@vee-validate/zod'
import { useForm } from 'vee-validate'

import { Button } from '~/components/ui/button'
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '~/components/ui/form'
import { Input } from '~/components/ui/input'
import { customerInfoSchema } from '~/lib/checkout'

// const apiSdk = useApiSdk()

const form = useForm({
  validationSchema: toTypedSchema(customerInfoSchema),
})

const onSubmit = form.handleSubmit((values) => {
  console.log('Form submitted!', values)
})
</script>

<template>
  <form class="space-y-4" @submit="onSubmit">
    <FormField v-slot="{ componentField }" name="fullName">
      <FormItem>
        <FormLabel>Nome completo</FormLabel>
        <FormControl>
          <Input type="text" placeholder="ex.: Ana Luiza Moreira" v-bind="componentField" />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="email">
      <FormItem>
        <FormLabel>Email</FormLabel>
        <FormControl>
          <Input type="email" placeholder="ex.: ana@gmail.com" v-bind="componentField" />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="cpf">
      <FormItem>
        <FormLabel>CPF</FormLabel>
        <FormControl>
          <Input type="text" placeholder="000.000.000-00" v-bind="componentField" />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="phoneNumber">
      <FormItem>
        <FormLabel>Celular / WhatsApp</FormLabel>
        <FormControl>
          <Input type="text" placeholder="(00) 00000-0000" v-bind="componentField" />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <Button type="submit">
      Continuar
    </Button>
  </form>
</template>

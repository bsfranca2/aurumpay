<script setup lang="ts">
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

const form = useForm()

const onSubmit = form.handleSubmit((values) => {
  console.log('Form submitted!', values)
})
</script>

<template>
  <form class="space-y-4" @submit="onSubmit">
    <FormField v-slot="{ componentField }" name="zipCode">
      <FormItem>
        <FormLabel>CEP</FormLabel>
        <FormControl>
          <Input type="text" placeholder="00000-000" v-bind="componentField" />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="address">
      <FormItem>
        <FormLabel>Endereço</FormLabel>
        <FormControl>
          <Input type="text" placeholder="Informe seu endereço sem número" v-bind="componentField" />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="number">
      <FormItem>
        <FormLabel>Número</FormLabel>
        <FormControl>
          <Input type="number" placeholder="100" v-bind="componentField" />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="neighborhood">
      <FormItem>
        <FormLabel>Bairro</FormLabel>
        <FormControl>
          <Input type="text" placeholder="Informe seu bairro" v-bind="componentField" />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="state">
      <FormItem>
        <FormLabel>Estado</FormLabel>
        <FormControl>
          <Input type="text" placeholder="Informe seu estado" v-bind="componentField" />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="city">
      <FormItem>
        <FormLabel>Cidade</FormLabel>
        <FormControl>
          <Input type="text" placeholder="Informe sua cidade" v-bind="componentField" />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <div>
      <h3>Forma de envio</h3>
      <p>...</p>
    </div>

    <Button type="submit">
      Continuar
    </Button>
  </form>
</template>

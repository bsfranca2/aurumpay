<script setup lang="ts">
import { Label } from 'reka-ui'
import { useFormField } from '../form/useFormField'

const { formItemId } = useFormField()
</script>

<template>
  <Label :for="formItemId" class="radio-card-item__label">
    <slot />
  </Label>
</template>

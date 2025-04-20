import type { VariantProps } from 'class-variance-authority'
import { cva } from 'class-variance-authority'

export { default as Button } from './Button.vue'

export const buttonVariants = cva(
  'button',
  {
    variants: {
      variant: {
        default: 'button--primary',
        destructive: 'button--destructive',
        outline: 'button--outline',
        secondary: 'button--secondary',
        ghost: 'button--ghost',
        link: 'button--link',
      },
      size: {
        default: 'button--md',
        sm: 'button--sm',
        lg: 'button--lg',
        icon: 'button--icon',
      },
    },
    defaultVariants: {
      variant: 'default',
      size: 'default',
    },
  },
)

export type ButtonVariants = VariantProps<typeof buttonVariants>

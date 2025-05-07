import { addComponentsDir, addImportsDir, addPlugin, addVitePlugin, createResolver, defineNuxtModule, hasNuxtModule, installModule } from '@nuxt/kit'
import { defu } from 'defu'
import { name, version } from '../package.json'
import { defaultOptions, defaultUiConfig } from './defaults'
import { addTemplates } from './templates'

export type * from './runtime/types'

export interface ModuleOptions {
  /**
   * Prefix for components
   * @defaultValue `U`
   */
  prefix?: string
}

export default defineNuxtModule<ModuleOptions>({
  meta: {
    name,
    version,
    configKey: 'checkoutUi',
  },
  defaults: defaultOptions,
  async setup(options, nuxt) {
    const { resolve } = createResolver(import.meta.url)

    nuxt.options.alias['#checkout-ui'] = resolve('./runtime')
    nuxt.options.alias['#checkout-base'] = resolve('./runtime/base')
    nuxt.options.alias['#checkout-blocks'] = resolve('./runtime/blocks')

    nuxt.options.appConfig.checkoutUi = defu(nuxt.options.appConfig.checkoutUi || {}, defaultUiConfig)

    // Isolate root node from portaled components
    nuxt.options.app.rootAttrs = nuxt.options.app.rootAttrs || {}
    nuxt.options.app.rootAttrs.class = [nuxt.options.app.rootAttrs.class, 'isolate'].filter(Boolean).join(' ')

    const plugin = await import('@tailwindcss/vite').then(r => r.default)
    addVitePlugin(plugin())

    async function registerModule(name: string, key: string, options: Record<string, any>) {
      if (!hasNuxtModule(name)) {
        await installModule(name, options)
      }
      else {
        (nuxt.options as any)[key] = defu((nuxt.options as any)[key], options)
      }
    }

    await registerModule('@nuxtjs/i18n', 'i18n', {})

    addPlugin({ src: resolve('./runtime/plugins/theme') })

    addComponentsDir({
      path: resolve('./runtime/base/components'),
      prefix: options.prefix,
      pathPrefix: false,
    })

    addComponentsDir({
      path: resolve('./runtime/blocks/components'),
      prefix: options.prefix,
      pathPrefix: false,
    })

    addImportsDir(resolve('./runtime/base/composables'))

    addImportsDir(resolve('./runtime/blocks/composables'))

    addTemplates(options, nuxt, resolve)
  },
})

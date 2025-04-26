import tailwindcss from '@tailwindcss/vite'

// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2024-11-01',
  devtools: { enabled: true },
  css: ['~/assets/css/tailwind.css'],
  vite: {
    plugins: [
      tailwindcss(),
    ],
  },
  app: {
    head: {
      title: 'AurumPay',
      htmlAttrs: {
        lang: 'pt-BR',
      },
      link: [
        { rel: 'preconnect', href: 'https://fonts.googleapis.com' },
        { rel: 'preconnect', href: 'https://fonts.gstatic.com', crossorigin: '' },
        {
          rel: 'stylesheet',
          href: 'https://fonts.googleapis.com/css2?family=Rubik:ital,wght@0,300..900;1,300..900&display=swap',
        },
      ],
    },
  },
  components: [],
  runtimeConfig: {
    checkoutSessionExpiration: 60 * 60 * 1000, // 1 hour
    cartMaxAge: 60 * 60 * 24 * 7, // 7 days
    apiUrl: 'http://localhost:5019',
  },
})

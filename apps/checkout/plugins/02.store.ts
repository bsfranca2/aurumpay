export default defineNuxtPlugin({
  name: 'white-label-plugin',
  enforce: 'pre',
  dependsOn: ['domain-plugin'],
  async setup() {
    await useStore().fetchAndSet()
    // const { whiteLabel } = useWhiteLabel()
    // updateAppConfig({
    //   ui: {
    //     primary: whiteLabel.value.primaryColor.toLowerCase(),
    //     gray: whiteLabel.value.grayColor.toLowerCase(),
    //   },
    // })
    // useHead({
    //   titleTemplate: `%s - ${whiteLabel.value.organizationName}`,
    // })
  },
})

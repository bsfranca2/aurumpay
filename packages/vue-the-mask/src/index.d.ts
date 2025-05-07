import { DirectiveFunction, PluginObject } from "vue";

declare namespace VueTheMaskPlugin {
  interface VueStatic {
    (): void;
  }
}

interface VueTheMaskPlugin extends PluginObject<undefined> {
  mask: DirectiveFunction;
}

declare const VueTheMask: VueTheMaskPlugin;
export = VueTheMask;
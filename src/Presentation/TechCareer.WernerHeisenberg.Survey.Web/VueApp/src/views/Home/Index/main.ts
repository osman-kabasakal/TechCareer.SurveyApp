
// Components
import Index from './Index.vue'

// Composables
import Vue, {createApp } from 'vue'

// Plugins
import { registerPlugins } from '@/plugins'
import {IViewModel} from "@/interfaces/abstracts";

declare global{
  interface Window {
    aspNetViewModel:IViewModel;
  }
}

let viewModelElementId= "viewModelJson";
let viewModelElement = document.getElementById(viewModelElementId);
window.aspNetViewModel = <IViewModel>{};
if(viewModelElement){
  window.aspNetViewModel = <IViewModel>JSON.parse(viewModelElement.innerHTML);
}

const app = createApp(Index)

registerPlugins(app)

app.mount('#app')

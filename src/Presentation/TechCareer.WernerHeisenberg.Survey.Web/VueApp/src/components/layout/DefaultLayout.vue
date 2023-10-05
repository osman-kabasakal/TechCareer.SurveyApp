<script setup lang="ts">
import {UserDto} from "@/interfaces/abstracts";

const props= defineProps({
  activeMenuItem: {
    type: String,
    required: true
  }
})
const user=window.aspNetViewModel.user;
const isInRole=(role:string)=>{
  return ((user?.Roles??[]).filter(x=>x.Name==role).length>0)??false;
}
const links:{
  [key:string]:string
} = {
  "home":"Ana Sayfa",
  "login": user!=undefined?"":"Giriş Yap",
  "signout": user!=undefined?"Çıkış Yap":"",
  "register": user!=undefined?"":"Kayıt Ol",
  "newSurvey":"Anket Oluştur",
  "mySurveys":user!=undefined?"Anketlerim":"",
  "questions": isInRole('Developer') || isInRole('Admin')?"Sorularım":"",
  "myAnswers":user!=undefined?"Katıldığım Anketler":"",
  "users":isInRole('Developer') || isInRole('Admin')?"Kullanıcılar":"",
}
</script>

<template>
  <v-app id="inspire">
    <v-app-bar
      class="px-3"
      flat
      density="compact"
    >
      <v-avatar
        color="grey-darken-1"
        class="hidden-md-and-up"
        size="32"
      ></v-avatar>

      <v-spacer></v-spacer>

      <v-tabs
        centered
        color="grey-darken-2"
      >
        <template  v-for="link in Object.keys(links)"  :key="link">
          <v-tab
            :text="links[link]"
            v-if="links[link]!==''"

          ></v-tab>
        </template>

      </v-tabs>
      <v-spacer></v-spacer>

      <v-avatar
        class="hidden-sm-and-down"
        color="grey-darken-1"
        size="32"
      ></v-avatar>
    </v-app-bar>

    <v-main class="bg-grey-lighten-3">
      <v-container>
        <v-row>
          <v-col
            cols="12"
          >
            <slot name="body">
            </slot>
          </v-col>
        </v-row>
      </v-container>
    </v-main>
  </v-app>
</template>

<style scoped lang="scss">

</style>

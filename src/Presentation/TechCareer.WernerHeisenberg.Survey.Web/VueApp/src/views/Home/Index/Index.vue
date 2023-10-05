<script setup lang="ts">
import DefaultLayout from "@/components/layout/DefaultLayout.vue";
</script>

<template>
  <DefaultLayout active-menu-item="home">
    <template #body>
      <v-container>
        <v-row>
          <v-col cols="12">
            <h1>Home Page</h1>
          </v-col>
        </v-row>
      </v-container>
    </template>
  </DefaultLayout>
</template>

<style lang="scss">
//@import '@mdi/font/css/materialdesignicons.css';
//@import "node_modules/vuetify/dist/vuetify.min";
</style>

<template class="justify-center">
  <div class="jj q-pt-xs q-ma-xl row justify-center item-center">
    <div class="tamaño-card text-center col-xs-12 col-sm-4 col-md-2 col-xl-2 justify-center q-mx-sm"
      style="width: 200px" v-for="(card, index) in cardsFacturacion" :key="index">
      <p class="text-black"></p>
      <MenuCards :title="card.title" :icon="card.icon" :link="card.link" />
    </div>
  </div>
</template>

<script setup>
import MenuCards from 'src/components/cards/MenuCards.vue';
import { cardsFacturacion } from 'src/assets/js/linksCard/linkCards'
</script>

<template>
  <div class="row items-center">
    <q-rating :model-value="rating" readonly :max="5" size="1.2em" color="amber" />
    <span class="q-ml-sm text-caption">{{ rating.toFixed(1) }}</span>
  </div>
</template>
<script setup>
defineProps({
  rating: {
    type: Number,
    required: true
  }
})
</script>

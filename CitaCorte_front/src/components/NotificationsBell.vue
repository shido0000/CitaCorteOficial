<template>
  <q-btn flat round icon="notifications" @click="$router.push('/notificaciones')">
    <q-badge v-if="store.noLeidas > 0" color="red" floating>{{ store.noLeidas }}</q-badge>
  </q-btn>
</template>

<script setup>
import { useNotifStore } from 'src/stores/notificaciones'
const store = useNotifStore()
store.cargar() // cargar al montar la campana (también con intervalo)
</script>

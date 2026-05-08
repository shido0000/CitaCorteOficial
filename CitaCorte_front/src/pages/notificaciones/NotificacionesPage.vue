<template>
  <q-page padding>
    <div class="text-h6">Notificaciones</div>
    <q-list separator>
      <q-item v-for="n in store.items" :key="n.id" clickable @click="abrir(n)">
        <q-item-section avatar>
          <q-icon name="circle" :color="n.leida ? 'grey-5' : 'red'" />
        </q-item-section>
        <q-item-section>
          <q-item-label :class="{'text-bold': !n.leida}">{{ n.titulo }}</q-item-label>
          <q-item-label caption>{{ n.mensaje }}</q-item-label>
        </q-item-section>
        <q-item-section side top>{{ n.fecha }}</q-item-section>
      </q-item>
    </q-list>
  </q-page>
</template>

<script setup>
import { useNotifStore } from 'src/stores/notificaciones'
import { useRouter } from 'vue-router'
const store = useNotifStore()
const router = useRouter()

const abrir = (notif) => {
  store.marcarLeida(notif.id)
  if (notif.urlAccion) router.push(notif.urlAccion)
}
</script>

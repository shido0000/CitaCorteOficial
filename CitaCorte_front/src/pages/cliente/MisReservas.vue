<template>
    <q-page padding>
        <div class="text-h6">Mis Reservas Activas</div>
        <q-list separator>
            <q-item v-for="r in reservas" :key="r.id">
                <q-item-section>
                    <q-item-label>{{ r.proveedorNombre }} - {{ r.servicio }}</q-item-label>
                    <q-item-label caption>{{ r.fechaHora }} · Estado: {{ r.estado }}</q-item-label>
                </q-item-section>
                <q-item-section side v-if="r.estado === 'Pendiente' || r.estado === 'Confirmada'">
                    <q-btn flat label="Cancelar" color="negative" @click="cancelar(r.id)" />
                </q-item-section>
            </q-item>
        </q-list>
    </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useQuasar } from 'quasar'
import { api } from 'src/boot/axios'
import { decodeToken } from 'src/stores/auth'

const $q = useQuasar()
const reservas = ref([])
const cargar = async () => {
    const decoded = decodeToken(sessionStorage.getItem("token"));

    const { data } = await api.get(`Reserva/ObtenerListadoPaginado?clienteId=${decoded.clienteId}`)
    reservas.value = data.result.elementos || []
}
const cancelar = async (id) => {
    await api.put(`/reservas/${id}/cancelar`)
    $q.notify({ type: 'warning', message: 'Reserva cancelada' })
    cargar()
}
onMounted(cargar)
</script>

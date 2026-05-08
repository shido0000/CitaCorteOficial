<template>
    <q-page padding>
        <q-tabs v-model="tab" dense>
            <q-tab name="pendientes" label="Pendientes" />
            <q-tab name="confirmadas" label="Confirmadas" />
            <q-tab name="historial" label="Historial" />
        </q-tabs>
        <q-tab-panels v-model="tab" animated>
            <q-tab-panel name="pendientes">
                <q-list separator>
                    <q-item v-for="r in pendientes" :key="r.id">
                        <q-item-section>
                            <q-item-label>{{ r.clienteNombre }}</q-item-label>
                            <q-item-label caption>{{ r.servicio }} · {{ r.fechaHora }}</q-item-label>
                        </q-item-section>
                        <q-item-section side>
                            <q-btn flat icon="check" color="positive" @click="accionReserva(r.id, 'confirmar')" />
                            <q-btn flat icon="close" color="negative" @click="accionReserva(r.id, 'rechazar')" />
                        </q-item-section>
                    </q-item>
                    <div v-if="pendientes.length === 0" class="text-center q-pa-md">
                        No hay reservas pendientes
                    </div>
                </q-list>
            </q-tab-panel>
            <q-tab-panel name="confirmadas">
                <q-list separator>
                    <q-item v-for="r in confirmadas" :key="r.id">
                        <q-item-section>
                            <q-item-label>{{ r.clienteNombre }} - {{ r.servicio }}</q-item-label>
                            <q-item-label caption>{{ r.fechaHora }}</q-item-label>
                        </q-item-section>
                    </q-item>
                    <div v-if="confirmadas.length === 0" class="text-center q-pa-md">
                        No hay reservas confirmadas
                    </div>
                </q-list>
            </q-tab-panel>
            <q-tab-panel name="historial">
                <q-list separator>
                    <q-item v-for="r in historial" :key="r.id">
                        <q-item-section>
                            <q-item-label>{{ r.clienteNombre }} - {{ r.servicio }}</q-item-label>
                            <q-item-label caption>{{ r.fechaHora }} · {{ r.estado }}</q-item-label>
                        </q-item-section>
                    </q-item>
                    <div v-if="historial.length === 0" class="text-center q-pa-md">
                        Sin historial
                    </div>
                </q-list>
            </q-tab-panel>
        </q-tab-panels>
    </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useQuasar } from 'quasar'
import { api } from 'src/boot/axios'
import { decodeToken } from 'src/stores/auth'

const $q = useQuasar()
const tab = ref('pendientes')
const pendientes = ref([])
const confirmadas = ref([])
const historial = ref([])

const cargar = async () => {
    const decoded = decodeToken(sessionStorage.getItem("token"));

    const { data } = await api.get(`Reserva/ObtenerListadoPaginado?barberoId=${decoded.barberoId}`)
    pendientes.value = data.result.elementos.filter(e => e.EstadoReserva === "Pendiente") || []
    confirmadas.value = data.result.elementos.filter(e => e.EstadoReserva === "Aprobada") || []
    historial.value = data.result.elementos || []
}

const accionReserva = async (id, accion) => {
    try {
        await api.put(`/reservas/${id}/${accion}`)
        $q.notify({
            type: 'positive',
            message: `Reserva ${accion === 'confirmar' ? 'confirmada' : 'rechazada'}`
        })
        cargar()
    } catch (e) {
        $q.notify({ type: 'negative', message: 'Error al procesar' })
    }
}

onMounted(cargar)
</script>

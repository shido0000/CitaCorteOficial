<template>
    <q-page padding>
        <q-select v-model="filtroBarbero" :options="barberos" option-value="id" option-label="nombre"
            label="Filtrar por barbero" clearable class="q-mb-md" />
        <q-tabs v-model="tab" dense>
            <q-tab name="pendientes" label="Pendientes" />
            <q-tab name="confirmadas" label="Confirmadas" />
            <q-tab name="historial" label="Historial" />
        </q-tabs>
        <q-tab-panels v-model="tab" animated>
            <q-tab-panel name="pendientes">
                <q-list separator>
                    <q-item v-for="r in reservasFiltradas('pendientes')" :key="r.id">
                        <q-item-section>
                            <q-item-label>{{ r.clienteNombre }} - {{ r.servicio }}</q-item-label>
                            <q-item-label caption>{{ r.fechaHora }} · Barbero:
                                {{ r.barberoNombre }}</q-item-label>
                        </q-item-section>
                        <q-item-section side>
                            <q-btn flat icon="check" color="positive" @click="accionReserva(r.id, 'confirmar')" />
                            <q-btn flat icon="close" color="negative" @click="accionReserva(r.id, 'rechazar')" />
                        </q-item-section>
                    </q-item>
                    <div v-if="pendientes.length === 0" class="text-center q-pa-md">
                        Sin pendientes
                    </div>
                </q-list>
            </q-tab-panel>
            <q-tab-panel name="confirmadas">
                <q-list separator>
                    <q-item v-for="r in reservasFiltradas('confirmadas')" :key="r.id">
                        <q-item-section>
                            <q-item-label>{{ r.clienteNombre }} - {{ r.servicio }}</q-item-label>
                            <q-item-label caption>{{ r.fechaHora }} · Barbero:
                                {{ r.barberoNombre }}</q-item-label>
                        </q-item-section>
                    </q-item>
                    <div v-if="confirmadas.length === 0" class="text-center q-pa-md">
                        Sin confirmadas
                    </div>
                </q-list>
            </q-tab-panel>
            <q-tab-panel name="historial">
                <q-list separator>
                    <q-item v-for="r in reservasFiltradas('historial')" :key="r.id">
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
const barberos = ref([])
const filtroBarbero = ref(null)

onMounted(async () => {
    const decoded = decodeToken(sessionStorage.getItem("token"));
    const [reservas, barberosData] = await Promise.all([
        api.get(`Reserva/ObtenerListadoPaginado?barberiaId=${decoded.barberiaId}`),
        api.get(`Barbero/ObtenerListadoPaginado?barberiaId=${decoded.barberiaId}`)
    ])
    pendientes.value = reservas.data.result.elementos.filter(e=>e.EstadoReserva==="Pendiente") || []
    confirmadas.value = reservas.data.result.elementos.filter(e=>e.EstadoReserva==="Aprobada") || []
    historial.value = reservas.data.result.elementos || []
    barberos.value = barberosData.data.result.elementos
})

const reservasFiltradas = (tipo) => {
    const fuente =
        tipo === 'pendientes'
            ? pendientes.value
            : tipo === 'confirmadas'
                ? confirmadas.value
                : historial.value
    if (!filtroBarbero.value) return fuente
    return fuente.filter((r) => r.barberoId === filtroBarbero.value)
}

const accionReserva = async (id, accion) => {
    await api.put(`/reservas/${id}/${accion}`)
    $q.notify({
        type: 'positive',
        message: `Reserva ${accion === 'confirmar' ? 'confirmada' : 'rechazada'}`
    })
    // Recargar
    const { data } = await api.get('/barberia/reservas')
    pendientes.value = data.pendientes || []
    confirmadas.value = data.confirmadas || []
    historial.value = data.historial || []
}
</script>

<template>
    <q-page padding>
        <div class="text-h6">Solicitudes de Cambio de Plan</div>
        <q-table :rows="solicitudes" :columns="cols" row-key="id">
            <template v-slot:body-cell-acciones="props">
                <div v-if="props.row.estado === 'Pendiente'">
                    <q-btn flat icon="check" color="positive" @click="aprobar(props.row)" />
                    <q-btn flat icon="close" color="negative" @click="mostrarDialogoRechazo(props.row)" />
                </div>
                <span v-else>{{ props.row.estado }}</span>
            </template>
        </q-table>

        <q-dialog v-model="dialogoRechazo">
            <q-card style="min-width: 350px">
                <q-card-section>
                    <div class="text-h6">Motivo del rechazo</div>
                </q-card-section>
                <q-card-section>
                    <q-input v-model="motivo" type="textarea" outlined autofocus />
                </q-card-section>
                <q-card-actions align="right">
                    <q-btn label="Cancelar" flat v-close-popup />
                    <q-btn label="Rechazar" color="negative" @click="confirmarRechazo" />
                </q-card-actions>
            </q-card>
        </q-dialog>
    </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useQuasar } from 'quasar'
import { api } from 'src/boot/axios'

const $q = useQuasar()
const solicitudes = ref([])
const motivo = ref('')
const dialogoRechazo = ref(false)
let solicitudSeleccionada = null

// Definición de columnas (ajusta los campos según tu API)
const cols = [
    { name: 'id', field: 'id', label: 'ID', align: 'center' },
    { name: 'solicitante', field: 'solicitante', label: 'Solicitante', align: 'center' },
    { name: 'planActual', field: 'planActual', label: 'Plan Actual', align: 'center' },
    { name: 'planSolicitado', field: 'planSolicitado', label: 'Plan Solicitado', align: 'center' },
    { name: 'fecha', field: 'fecha', label: 'Fecha', align: 'center' },
    { name: 'estado', field: 'estado', label: 'Estado', align: 'center' },
    { name: 'acciones', label: 'Acciones', align: 'center' }
]

const cargar = async () => {
    try {
        const { data } = await api.get('/admin/solicitudes-cambio-plan')
        // Ajusta según la estructura real de tu API (data.result o data directamente)
        solicitudes.value = data.result || data
    } catch (error) {
        $q.notify({ type: 'negative', message: 'Error al cargar solicitudes' })
    }
}

const aprobar = async (row) => {
    try {
        await api.put(`/admin/solicitudes-cambio-plan/${row.id}/aprobar`)
        $q.notify({ type: 'positive', message: 'Solicitud aprobada' })
        cargar()
    } catch {
        $q.notify({ type: 'negative', message: 'Error al aprobar' })
    }
}

const mostrarDialogoRechazo = (row) => {
    solicitudSeleccionada = row
    motivo.value = ''
    dialogoRechazo.value = true
}

const confirmarRechazo = async () => {
    if (!motivo.value.trim()) {
        $q.notify({ type: 'warning', message: 'Debe ingresar un motivo' })
        return
    }
    try {
        await api.put(`/admin/solicitudes-cambio-plan/${solicitudSeleccionada.id}/rechazar`, {
            motivo: motivo.value
        })
        $q.notify({ type: 'warning', message: 'Solicitud rechazada' })
        dialogoRechazo.value = false
        cargar()
    } catch {
        $q.notify({ type: 'negative', message: 'Error al rechazar' })
    }
}

onMounted(() => {
    cargar()
})
</script>

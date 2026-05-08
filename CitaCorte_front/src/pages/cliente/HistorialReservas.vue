<template>
    <q-page padding>
        <div class="text-h6">Historial de Reservas</div>
        <q-list separator>
            <q-item v-for="r in historial" :key="r.id">
                <q-item-section>
                    <q-item-label>{{ r.proveedorNombre }} - {{ r.servicio }}</q-item-label>
                    <q-item-label caption>{{ r.fechaHora }} · {{ r.estado }}</q-item-label>
                    <q-item-label v-if="r.comentarioCliente" caption>Tu comentario: {{ r.comentarioCliente
                    }}</q-item-label>
                </q-item-section>
                <q-item-section side v-if="r.estado === 'Completada' && !r.calificacionCliente">
                    <q-btn flat icon="star" label="Calificar" color="amber" @click="abrirCalificacion(r)" />
                </q-item-section>
            </q-item>
        </q-list>

        <q-dialog v-model="dialogoCalificar">
            <q-card style="min-width: 350px">
                <q-card-section>Calificar servicio</q-card-section>
                <q-card-section>
                    <q-rating v-model="calificacion" :max="5" size="2em" color="amber" />
                    <q-input v-model="comentario" label="Comentario (opcional)" type="textarea" class="q-mt-md" />
                </q-card-section>
                <q-card-actions align="right">
                    <q-btn flat label="Cancelar" @click="dialogoCalificar = false" />
                    <q-btn label="Enviar" color="primary" @click="enviarCalificacion" :loading="enviando" />
                </q-card-actions>
            </q-card>
        </q-dialog>
    </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useQuasar } from 'quasar'
import { api } from 'src/boot/axios'
import { decodeToken } from 'src/stores/auth'

const $q = useQuasar()
const historial = ref([])
const dialogoCalificar = ref(false)
const reservaActual = ref(null)
const calificacion = ref(0)
const comentario = ref('')
const enviando = ref(false)

const cargar = async () => {
    const decoded = decodeToken(sessionStorage.getItem("token"));

    const { data } = await api.get(`Reserva/ObtenerListadoPaginado?clienteId=${decoded.clienteId}`)
    historial.value = data.result.elementos || []
}

const abrirCalificacion = (reserva) => {
    reservaActual.value = reserva
    calificacion.value = 0
    comentario.value = ''
    dialogoCalificar.value = true
}

const enviarCalificacion = async () => {
    if (calificacion.value === 0) {
        $q.notify({ type: 'warning', message: 'Asigna una calificación' })
        return
    }
    enviando.value = true
    try {
        await api.put(`/reservas/${reservaActual.value.id}/calificar`, {
            calificacion: calificacion.value,
            comentario: comentario.value
        })
        $q.notify({ type: 'positive', message: '¡Gracias por calificar!' })
        dialogoCalificar.value = false
        cargar()
    } finally {
        enviando.value = false
    }
}

onMounted(cargar)
</script>

<template>
    <q-page padding>
        <div class="text-h6">Registro de Barberías Pendientes</div>
        <q-table :rows="barberias" :columns="cols" row-key="id">
            <template v-slot:body-cell-acciones="props">
                <div class="text-center">
                    <div v-if="props.row.estadoSolicitud === 'Pendiente'">
                        <q-btn flat icon="check" color="positive" @click="aprobar(props.row.id)" />
                        <q-btn flat icon="close" color="negative" @click="mostrarRechazo(props.row)" />
                    </div>
                </div>
            </template>
        </q-table>

        <q-dialog v-model="dialogoRechazo">
            <q-card>
                <q-card-section>Motivo del rechazo</q-card-section>
                <q-card-section>
                    <q-input v-model="motivo" type="textarea" outlined />
                </q-card-section>
                <q-card-actions align="right">
                    <q-btn label="Cancelar" flat @click="dialogoRechazo = false" />
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
const barberias = ref([])
const dialogoRechazo = ref(false)
const motivo = ref('')
let barberiaSeleccionada = null

const cols = [
    { name: 'nombreUsuario', field: 'nombreUsuario', label: 'Nombre Usuario', align: 'center' },
    { name: 'nombreBarberia', field: 'nombreBarberia', label: 'Nombre Barbería / Barbero', align: 'center' },
    { name: 'email', field: 'email', label: 'Email', align: 'center' },
    { name: 'telefono', field: 'telefono', label: 'Teléfono', align: 'center' },
    { name: 'suscripcion', field: 'suscripcion', label: 'Suscripción', align: 'center' },
    { name: 'fechaSolicitud', field: 'fechaSolicitud', label: 'Fecha Solicitud', align: 'center' },
    { name: 'estadoSolicitud', field: 'estadoSolicitud', label: 'Estado', align: 'center' },
    { name: 'acciones', label: 'Acciones' }
]

onMounted(async () => {
    await recargar()
})


const aprobar = async (id) => {
    await api.put(`/comercial/registro-barberias/${id}/aprobar`)
    $q.notify({ type: 'positive', message: 'Barbería aprobada' })
    recargar()
}
const mostrarRechazo = (row) => {
    barberiaSeleccionada = row
    motivo.value = ''
    dialogoRechazo.value = true
}
const confirmarRechazo = async () => {
    const rechazarSuscripcionDto = {
        solicitudId: barberiaSeleccionada.solicitudSuscripcionId,
        motivoRechazo: motivo.value
    }
    await api.post(
        `SolicitudSuscripcion/RechazarSolicitud`, rechazarSuscripcionDto
    )
    dialogoRechazo.value = false
    $q.notify({ type: 'warning', message: 'Barbería rechazada' })
    await recargar()
}
const recargar = async () => {
    const { data } = await api.get('Comercial/ObtenerBarberiasYBarberos')
    barberias.value = data.result
}
</script>

<style scoped lang="scss">
@import 'src/assets/theme/citacorte-design-system.scss';

.q-page {
    background-color: $color-blanco;
    min-height: 100vh;
    padding: $spacing-xl;

    .dark & {
        background-color: #1A1A1A;
    }
}

.text-h6 {
    font-size: $h2-font-size;
    font-weight: $font-weight-semibold;
    color: $color-negro;
    margin-bottom: $spacing-lg;

    .dark & {
        color: $color-blanco;
    }
}

:deep(.q-table) {
    background: $color-blanco;
    border-radius: $border-radius-lg;
    box-shadow: $shadow-md;

    .dark & {
        background: #2D2D2D;
        box-shadow: none;
    }

    :deep(.q-table__card) {
        border-radius: $border-radius-lg;
    }

    :deep(.q-table__heading) {
        background-color: rgba($color-dorado, 0.08);
        color: $color-negro;
        font-weight: $font-weight-semibold;

        .dark & {
            background-color: #252525;
            color: $color-blanco;
        }
    }

    :deep(.q-table__row:hover) {
        background-color: rgba($color-dorado, 0.05);
    }

    :deep(.q-btn) {
        margin: 0 $spacing-xs;
    }
}

:deep(.q-card) {
    background: $color-blanco;
    border-radius: $border-radius-lg;
    box-shadow: $shadow-md;

    .dark & {
        background: #2D2D2D;
    }
}

:deep(.q-input) {
    :deep(.q-field__control) {
        border-radius: $border-radius-md;
    }

    :deep(.q-field__label) {
        font-family: $font-family-primary;
    }
}
</style>

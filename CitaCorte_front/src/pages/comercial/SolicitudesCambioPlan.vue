<template>
  <q-page padding>
    <div class="text-h6">Solicitudes de Cambio de Plan</div>
    <q-table :rows="solicitudes" :columns="cols" row-key="id">
      <template v-slot:body-cell-acciones="props">
        <template v-if="props.row.estado === 'Pendiente'">
          <q-btn
            flat
            icon="check"
            color="positive"
            @click="aprobar(props.row.id)"
          />
          <q-btn
            flat
            icon="close"
            color="negative"
            @click="mostrarRechazo(props.row)"
          />
        </template>
        <span v-else>{{ props.row.estado }}</span>
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
import api from 'src/boot/axios'
import { useQuasar } from 'quasar'

const $q = useQuasar()
const solicitudes = ref([])
const dialogoRechazo = ref(false)
const motivo = ref('')
let solicitudActual = null

const cols = [
  { name: 'solicitante', field: 'solicitante', label: 'Solicitante' },
  { name: 'planActual', field: 'planActual', label: 'Plan actual' },
  { name: 'planSolicitado', field: 'planSolicitado', label: 'Plan solicitado' },
  { name: 'estado', field: 'estado', label: 'Estado' },
  { name: 'acciones', label: 'Acciones' }
]

onMounted(async () => {
  const { data } = await api.get('/comercial/solicitudes-cambio-plan')
  solicitudes.value = data
})

const aprobar = async (id) => {
  await api.put(`/comercial/solicitudes-cambio-plan/${id}/aprobar`)
  $q.notify({ type: 'positive', message: 'Solicitud aprobada' })
  recargar()
}
const mostrarRechazo = (row) => {
  solicitudActual = row
  motivo.value = ''
  dialogoRechazo.value = true
}
const confirmarRechazo = async () => {
  await api.put(
    `/comercial/solicitudes-cambio-plan/${solicitudActual.id}/rechazar`,
    { motivo: motivo.value }
  )
  dialogoRechazo.value = false
  recargar()
}
const recargar = async () => {
  const { data } = await api.get('/comercial/solicitudes-cambio-plan')
  solicitudes.value = data
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

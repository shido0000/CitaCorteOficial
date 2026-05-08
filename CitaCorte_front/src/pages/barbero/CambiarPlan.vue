<template>
    <q-page padding>
        <div class="text-h6">Cambiar Plan de Suscripción</div>
        <div class="row q-col-gutter-md q-mt-md">
            <div v-for="plan in planes" :key="plan.id" class="col-4">
                <q-card class="plan-card">
                    <q-card-section class="flex-grow-1">
                        <div class="text-subtitle1">{{ plan.nombre }}</div>
                        <div class="text-h5">€{{ plan.precio }}</div>
                        <div>{{ plan.descripcion }}</div>
                        <ul>
                            <li v-for="c in plan.caracteristicaSuscripcion" :key="c">{{ c }}</li>
                        </ul>
                    </q-card-section>
                    <q-card-actions align="right" class="q-mt-auto">
                        <q-btn v-if="plan.id !== planActualId" label="Solicitar cambio" color="primary"
                            @click="solicitar(plan.id)" />
                        <q-badge v-else color="green">Plan actual</q-badge>
                    </q-card-actions>
                </q-card>
            </div>
        </div>
    </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useQuasar } from 'quasar'
import { api } from 'src/boot/axios'
import { decodeToken } from 'src/stores/auth'

const $q = useQuasar()
const planes = ref([])
const planActualId = ref(null)

onMounted(async () => {
    try {
        const { data } = await api.get(`Suscripcion/ObtenerListadoPaginado?TipoSuscripcion=Barbero`)
        planes.value = data.result?.elementos || []
        // Si tienes el plan actual desde otro endpoint, descomenta:
        const decoded = decodeToken(token)

        const perfil = await api.get(`Autenticacion/ObtenerInformacionUsuario?id=${decoded.id}`)
        planActualId.value = perfil.data.result.suscripcionId
    } catch (error) {
        console.error(error)
        $q.notify({ type: 'negative', message: 'Error al cargar planes' })
    }
})
const solicitar = async (planId) => {
    const decoded = decodeToken(sessionStorage.getItem("token"));

    try {
        const solicitarSuscripcionDto = {
            nuevaSuscripcionId: planId,
            barberiaId: null,
            barberoId: decoded.barberoId,
        }
        await api.post('SolicitudSuscripcion/SolicitarNuevaSuscripcion', solicitarSuscripcionDto)
        $q.notify({ type: 'info', message: 'Solicitud enviada, espera aprobación' })
    } catch (error) {
        console.log("error: ", error)
        $q.notify({ type: 'negative', message: error.response.data.errorMessage })
    }
}
</script>
<style scoped lang="scss">
@import 'src/assets/theme/citacorte-design-system.scss';

.plan-card {
    height: 100%;
    display: flex;
    flex-direction: column;
    background: $color-blanco;
    border: 1px solid rgba($color-dorado, 0.2);
    border-radius: $border-radius-lg;
    transition: all $transition-base;

    .dark & {
        background: #2D2D2D;
        border-color: #404040;
    }

    &:hover {
        transform: translateY(-4px);
        box-shadow: $shadow-md;
        border-color: rgba($color-dorado, 0.5);
    }
}

.flex-grow-1 {
    flex-grow: 1;
}
</style>

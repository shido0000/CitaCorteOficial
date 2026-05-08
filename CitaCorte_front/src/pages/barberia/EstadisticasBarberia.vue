<template>
    <q-page padding>
        <div class="text-h6">Estadísticas de la Barbería</div>
        <div v-if="cargando">Cargando...</div>
        <template v-else>
            <!-- Elimina q-ma-sm y usa solo q-col-gutter-md -->
            <div class="row q-col-gutter-md q-mt-md">
                <div class="col-12 col-sm-6 col-md-3">
                    <q-card class="full-height">
                        <q-card-section>
                            <div class="text-subtitle2">Reservas Totales</div>
                            <div class="text-h4">{{ stats.totalReservas }}</div>
                        </q-card-section>
                    </q-card>
                </div>

                <div class="col-12 col-sm-6 col-md-3">
                    <q-card class="full-height">
                        <q-card-section>
                            <div class="text-subtitle2">Clientes Únicos</div>
                            <div class="text-h4">{{ stats.clientesUnicos }}</div>
                        </q-card-section>
                    </q-card>
                </div>

                <div class="col-12 col-sm-6 col-md-3">
                    <q-card class="full-height">
                        <q-card-section>
                            <div class="text-subtitle2">Ingresos</div>
                            <div class="text-h4">€{{ stats.ingresos }}</div>
                        </q-card-section>
                    </q-card>
                </div>

                <div class="col-12 col-sm-6 col-md-3">
                    <q-card class="full-height">
                        <q-card-section>
                            <div class="text-subtitle2">Calificación Promedio</div>
                            <div class="text-h4">{{ stats.calificacionPromedio }}</div>
                            <StarRating :rating="stats.calificacionPromedio" />
                        </q-card-section>
                    </q-card>
                </div>
            </div>
        </template>
    </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import StarRating from 'components/StarRating.vue'
import { api } from 'src/boot/axios'
import { decodeToken } from 'src/stores/auth'

const stats = ref({})
const cargando = ref(true)

onMounted(async () => {
    const decoded = decodeToken(sessionStorage.getItem("token"))
    const { data } = await api.get(`Barberia/ObtenerEstadisticasBarberia?barberiaId=${decoded.barberiaId}`)
    stats.value = data.result
    cargando.value = false
})
</script>

<style scoped>
.full-height {
    height: 100%;
}
</style>

<template>
    <q-page padding>
        <div class="text-h6">Estadísticas</div>
        <div v-if="cargando">Cargando...</div>
        <template v-else>
            <div class="row q-col-gutter-md q-mt-md">
                <div class="col-12 col-sm-6 col-md-3">
                    <q-card class="full-height">
                        <q-card-section>
                            <div class="text-subtitle2">Servicios (mes)</div>
                            <div class="text-h4">{{ stats.totalServiciosEnMes }}</div>
                        </q-card-section>
                    </q-card>
                </div>
                <div class="col-12 col-sm-6 col-md-3">
                    <q-card class="full-height">
                        <q-card-section>
                            <div class="text-subtitle2">Clientes únicos</div>
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
                            <div class="text-subtitle2">Calificación</div>
                            <div class="text-h4">{{ stats.calificacionPromedio }}</div>
                            <StarRating :rating="stats.calificacionPromedio" />
                        </q-card-section>
                    </q-card>
                </div>
            </div>

            <div v-if="esPremium" class="q-mt-lg">
                <div class="text-h6">Estadísticas Premium</div>
                <div class="row q-col-gutter-md q-mt-sm">
                     <div class="col-12 col-sm-6 col-md-3">
                    <q-card class="full-height">
                        <q-card-section>
                            <div class="text-subtitle2">Productos vendidos</div>
                            <div class="text-h4">{{ stats.productosVendidos }}</div>
                        </q-card-section>
                      </q-card>
                </div>
                   <div class="col-12 col-sm-6 col-md-3">
                    <q-card class="full-height">
                        <q-card-section>
                            <div class="text-subtitle2">Ingresos productos</div>
                            <div class="text-h4">€{{ stats.ingresosProductos }}</div>
                        </q-card-section>
                 </q-card>
                </div>
                </div>
                <div class="q-mt-md">
                    <div class="text-subtitle2">Servicios más solicitados</div>
                    <q-list separator>
                        <q-item v-for="s in stats.serviciosMasSolicitadosBarberoDto" :key="s.servicio">
                            <q-item-section>{{ s.nombre }}</q-item-section>
                            <q-item-section side>{{ s.cantidad }}</q-item-section>
                        </q-item>
                    </q-list>
                </div>
            </div>
        </template>
    </q-page>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import StarRating from 'components/StarRating.vue'
import { useAuthStore } from 'stores/auth'
import { api } from 'src/boot/axios'
import { decodeToken } from 'src/stores/auth'

const auth = useAuthStore()
const esPremium = computed(() => auth.user?.plan?.tipo === 'Premium')
const stats = ref({})
const cargando = ref(true)

onMounted(async () => {
    const decoded = decodeToken(sessionStorage.getItem("token"))
    const { data } = await api.get(`Barbero/ObtenerEstadisticasBarbero?barberoId=${decoded.barberoId}`)
    stats.value = data.result
    cargando.value = false
})
</script>
<style scoped lang="scss">
@import 'src/assets/theme/citacorte-design-system.scss';

.full-height {
    height: 100%;
}
</style>

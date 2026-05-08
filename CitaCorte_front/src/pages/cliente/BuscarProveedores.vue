<template>
    <q-page padding>
        <div class="text-h5 q-mb-md">Búsqueda avanzada</div>
        <div class="row q-col-gutter-md col-12">
            <q-input v-model="filtros.q" label="Nombre o servicio" outlined class="col-12 col-sm-6 col-md-3" />
            <q-select v-model="filtros.especialidad" :options="especialidades" label="Especialidad" outlined
                class="col-12 col-sm-6 col-md-3" clearable />
            <q-select v-model="filtros.tipo" :options="['Barbero', 'Barberia']" label="Tipo" outlined
                class="col-12 col-sm-4 col-md-3" clearable />
            <q-input v-model.number="filtros.precioMin" label="Precio mínimo" type="number" outlined
                class="col-12 col-sm-4 col-md-3" />
            <q-input v-model.number="filtros.precioMax" label="Precio máximo" type="number" outlined
                class="col-12 col-sm-4 col-md-3" />
            <!-- Rango de calificación corregido -->
            <div class="col-12">
                <div class="text-subtitle2 q-mb-sm">Calificación ({{ filtros.calificacion.min }} - {{
                    filtros.calificacion.max }})</div>
                <q-range v-model="filtros.calificacion" :min="0" :max="5" :step="0.5" label class="full-width" />
            </div>
        </div>
        <q-btn label="Buscar" color="primary" icon="search" @click="buscar" class="q-mt-md" :loading="loading" />
        <div class="row q-col-gutter-md q-mt-md">
            <q-card v-for="p in resultados" :key="p.id" class="col-12 col-sm-6 col-md-4 cursor-pointer"
                @click="$router.push(`/cliente/detalle/${p.tipo}/${p.id}`)">
                <q-img :src="p.foto" ratio="16/9" />
                <q-card-section>
                    <div class="text-subtitle1">{{ p.nombre }}</div>
                    <StarRating :rating="p.calificacion" />
                    <div>
                        {{
                            p.tipo === "Barbero" ? p.especialidades?.join(", ") : p.direccion
                        }}
                    </div>
                </q-card-section>
            </q-card>
        </div>
        <div v-if="resultados.length === 0 && busquedaRealizada" class="text-center q-mt-md">
            Sin resultados.
        </div>
    </q-page>
</template>

<script setup>
import { ref, reactive } from 'vue'
import StarRating from 'components/StarRating.vue'
import { api } from 'src/boot/axios'

const filtros = reactive({
    q: '',
    especialidad: null,
    tipo: null,
    precioMin: null,
    precioMax: null,
    calificacion: { min: 0, max: 5 }
})
const especialidades = [
    'Corte clásico',
    'Barba',
    'Afeitado',
    'Color',
    'Peinado',
    'Tratamiento capilar'
]
const resultados = ref([])
const loading = ref(false)
const busquedaRealizada = ref(false)

const buscar = async () => {
    loading.value = true
    busquedaRealizada.value = true
    try {
        const { data } = await api.get('/buscar', {
            params: {
                q: filtros.q,
                especialidad: filtros.especialidad,
                tipo: filtros.tipo,
                precioMin: filtros.precioMin,
                precioMax: filtros.precioMax,
                calificacionMin: filtros.calificacion.min,
                calificacionMax: filtros.calificacion.max
            }
        })
        resultados.value = data
    } catch (error) {
        console.error('Error en búsqueda:', error)
    } finally {
        loading.value = false
    }
}
</script>

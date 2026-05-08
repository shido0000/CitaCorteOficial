<template>
    <q-page padding class="bg-black text-white page-container">
        <div class="text-h5 q-mb-md text-gold text-center text-md-left">Encuentra tu estilo</div>

        <q-input v-model="busqueda" label="Buscar barbero/barbería o servicio" outlined debounce="300"
            @update:model-value="buscar" class="search-input q-mb-xl" bg-color="dark" color="gold"
            input-class="text-white" label-color="gold" dense>
            <template v-slot:append>
                <q-icon name="search" color="gold" size="22px" />
            </template>
        </q-input>

        <div class="row q-col-gutter-xl justify-center">
            <div v-for="p in proveedores" :key="p.id" class="col-12 col-sm-6 col-md-4 q-mb-lg">
                <q-card flat bordered class="custom-card">
                    <q-img :src="p.logoUrl && p.logoUrl !== '-' ? p.logoUrl : ''" ratio="16/9"
                        class="rounded-borders card-image" :class="{ 'bg-grey-9': !p.logoUrl || p.logoUrl === '-' }"
                        fit="cover">
                        <template v-slot:error>
                            <div class="absolute-full flex flex-center bg-grey-9 text-grey-5">
                                <q-icon name="store" size="3rem" />
                                <div class="text-caption q-ml-sm">Sin imagen</div>
                            </div>
                        </template>
                        <template v-slot:loading>
                            <div class="absolute-full flex flex-center bg-grey-9">
                                <q-spinner color="gold" size="2rem" />
                            </div>
                        </template>
                    </q-img>

                    <q-card-section>
                        <div class="text-h6 text-gold ellipsis">{{ p.nombreBarberia }}</div>
                        <StarRating :rating="p.calificacion" class="q-mt-sm q-mb-md" />

                        <div class="info-row">
                            <q-icon name="location_on" size="sm" color="gold" />
                            <span class="text-grey-3">{{ p.direccion }}</span>
                        </div>

                        <div class="info-row">
                            <q-icon name="schedule" size="sm" color="gold" />
                            <span class="text-grey-3">{{ p.horarioApertura }} - {{ p.horarioCierre }}</span>
                        </div>

                        <div class="info-row">
                            <q-icon name="phone" size="sm" color="gold" />
                            <span class="text-grey-3">{{ p.telefono !== '-' ? p.telefono : 'No disponible' }}</span>
                        </div>
                    </q-card-section>
                </q-card>
            </div>

            <div v-if="proveedores.length === 0 && busquedaRealizada" class="col-12 text-center q-pt-xl">
                <div class="empty-state">
                    <q-icon name="search_off" size="64px" color="gold" />
                    <div class="text-h6 q-mt-md text-grey-4">No se encontraron resultados</div>
                    <div class="text-subtitle2 text-grey-5">Intenta con otros términos</div>
                </div>
            </div>
        </div>
    </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import StarRating from 'components/StarRating.vue'
import { api } from 'src/boot/axios'

const busqueda = ref('')
const proveedores = ref([])
const busquedaRealizada = ref(false)

const buscar = async () => {
    busquedaRealizada.value = true
    try {
        const { data } = await api.get('Home/ObtenerBarberiasYBarberos', {
            params: { texto: busqueda.value }
        })
        proveedores.value = data.result || []
    } catch (e) {
        console.error(e)
        proveedores.value = []
    }
}

onMounted(async () => {
   await buscar()
})
</script>

<style scoped>
/* Colores personalizados */
.bg-black {
    background-color: #0a0a0a;
}

.text-gold {
    color: #d4af37;
}

.text-grey-3 {
    color: #e0e0e0;
}

.text-grey-4 {
    color: #b0b0b0;
}

.text-grey-5 {
    color: #9e9e9e;
}

.bg-grey-9 {
    background-color: #2c2c2c;
}

/* Contenedor principal con padding lateral */
.page-container {
    padding: 24px 5% !important;
}

/* Input de búsqueda mejorado */
.search-input {
    max-width: 600px;
    margin-left: auto;
    margin-right: auto;
}

/* Estilo para las tarjetas */
.custom-card {
    background: linear-gradient(145deg, #1e1e1e 0%, #0f0f0f 100%);
    border: 1px solid rgba(212, 175, 55, 0.25);
    border-radius: 20px;
    transition: all 0.25s ease-in-out;
    overflow: hidden;
    height: 100%;
    display: flex;
    flex-direction: column;
}

.custom-card:hover {
    transform: translateY(-4px);
    box-shadow: 0 20px 30px -12px rgba(0, 0, 0, 0.6), 0 0 0 1px rgba(212, 175, 55, 0.4);
    border-color: rgba(212, 175, 55, 0.7);
}

/* Imagen de la tarjeta */
.card-image {
    border-bottom-left-radius: 0 !important;
    border-bottom-right-radius: 0 !important;
}

.card-image :deep(.q-img__image) {
    object-fit: cover;
}

/* Filas de información */
.info-row {
    display: flex;
    align-items: center;
    gap: 12px;
    margin-top: 10px;
    font-size: 0.9rem;
    line-height: 1.4;
}

/* Ajuste para el componente StarRating */
:deep(.star-rating) {
    color: #d4af37;
    font-size: 1.2rem;
}

/* Estado vacío */
.empty-state {
    padding: 48px 20px;
    background: rgba(255, 255, 255, 0.02);
    border-radius: 32px;
    backdrop-filter: blur(2px);
}

/* Responsive */
@media (max-width: 600px) {
    .page-container {
        padding: 16px 16px !important;
    }

    .text-h5 {
        font-size: 1.5rem;
        text-align: center;
    }

    .custom-card {
        border-radius: 16px;
    }

    .info-row {
        gap: 8px;
        font-size: 0.85rem;
    }

    :deep(.star-rating) {
        font-size: 1rem;
    }
}

@media (min-width: 601px) and (max-width: 1024px) {
    .page-container {
        padding: 24px 4% !important;
    }
}
</style>

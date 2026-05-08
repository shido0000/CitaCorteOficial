<template>
    <q-page padding v-if="proveedor" class="bg-dark text-white">
        <!-- Perfil -->
        <div class="row q-col-gutter-lg q-mt-lg">
            <div class="col-12 col-md-4">
                <q-card class="q-pa-md bg-dark text-white shadow-10">
                    <q-img :src="proveedor.fotoUrl" class="rounded-borders shadow-5" />
                    <div class="text-h5 text-bold q-mt-md">{{ proveedor.nombreBarberia }}</div>
                    <StarRating :rating="proveedor.clasificacion" />
                    <div class="text-subtitle2 q-mt-sm">{{ proveedor.direccion }}</div>
                    <div class="text-caption">
                        Horario: {{ proveedor.horarioApertura }} - {{ proveedor.horarioCierre }}
                    </div>
                </q-card>
            </div>

            <!-- Servicios -->
            <div class="col-12 col-md-8">
                <div class="text-h5 text-bold q-mb-md">Servicios</div>
                <div class="row q-col-gutter-md">
                    <div v-for="s in proveedor.servicios" :key="s.id" class="col-12 col-md-6">
                        <q-card class="q-hoverable shadow-5 bg-grey-9 text-white">
                            <q-card-section>
                                <div class="text-h6 text-amber-8">{{ s.nombre }}</div>
                                <div class="text-caption">{{ s.descripcion }} · {{ s.duracion }} min</div>
                            </q-card-section>
                            <q-card-actions align="between">
                                <div class="text-bold text-amber-8">{{ s.precio }} €</div>
                                <q-btn label="Reservar" color="amber-8" glossy class="q-ml-sm"
                                    @click="seleccionarServicio(s)" />
                            </q-card-actions>
                        </q-card>
                    </div>
                </div>
            </div>
        </div>

        <!-- Reseñas -->
        <div class="q-mt-xl">
            <div class="text-h5 text-bold">Reseñas</div>
            <div class="row q-col-gutter-md q-mt-md">
                <div v-for="r in proveedor.reseñas" :key="r.id" class="col-12 col-md-6">
                    <q-card class="bg-grey-9 text-white shadow-5">
                        <q-card-section>
                            <div class="text-bold">{{ r.cliente }}</div>
                            <StarRating :rating="r.estrellas" />
                            <div class="text-caption q-mt-sm">“{{ r.comentario }}”</div>
                        </q-card-section>
                    </q-card>
                </div>
            </div>
        </div>

        <!-- Diálogo de reserva -->
        <q-dialog v-model="dialogoReserva" transition-show="slide-up" transition-hide="slide-down">
            <q-card style="min-width: 400px" class="bg-grey-10 text-white">
                <q-card-section class="text-h6 text-bold">
                    Reservar {{ servicioSeleccionado?.nombre }}
                </q-card-section>
                <q-card-section>
                    <q-input v-model="fecha" type="date" label="Fecha" :min="hoy" standout="bg-amber-8 text-black"
                        @update:model-value="cargarHorarios" />

                    <!-- Nuevo select de barbero -->
                    <q-select v-model="barberoSeleccionado" :options="barberosOptions" option-label="nombre"
                        option-value="id" label="Barbero" standout="bg-amber-8 text-black"
                        @update:model-value="cargarHorarios" />

                    <q-select v-model="hora" :options="horasDisponibles" label="Hora"
                        standout="bg-amber-8 text-black" />
                </q-card-section>


                <q-card-actions align="right">
                    <q-btn label="Cancelar" flat color="white" @click="dialogoReserva = false" />
                    <q-btn label="Confirmar Reserva" color="amber-8" glossy @click="crearReserva" :loading="creando" />
                </q-card-actions>
            </q-card>
        </q-dialog>
    </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import StarRating from 'components/StarRating.vue'
import { useQuasar } from 'quasar'
import { api } from 'src/boot/axios'

const $q = useQuasar()
const route = useRoute()
const proveedor = ref(null)
const servicios = ref([])
const dialogoReserva = ref(false)
const servicioSeleccionado = ref(null)
const fecha = ref('')
const hora = ref(null)
const creando = ref(false)
const hoy = new Date().toISOString().substring(0, 10)
const slide = ref(0)

// slots disponibles que vienen del backend
const horasDisponibles = ref([])
const barberoSeleccionado = ref(null)
const barberosOptions = ref([])

const cargar = async () => {
    const tipo = route.params.tipo
    const id = route.params.id
    const { data } = await api.get(`/${tipo.toLowerCase()}/ObtenerDatos?id=${id}`)
    proveedor.value = data.result
    servicios.value = data.result.servicios
    barberosOptions.value = data.result.barberos
    console.log("barberosOptions.value: ",barberosOptions.value)
}
onMounted(cargar)

const seleccionarServicio = (serv) => {
    servicioSeleccionado.value = serv
    fecha.value = ''
    hora.value = null
    horasDisponibles.value = [] // limpiar
    dialogoReserva.value = true
}

// Llamar al backend para horarios
const cargarHorarios = async () => {
    if (!fecha.value || !servicioSeleccionado.value || !barberoSeleccionado.value) return
    try {
        const datosParaServicioDisponibleDto = {
            servicioId: servicioSeleccionado.value.id,
            fecha: fecha.value,
            barberiaId: proveedor.value.id,
            barberoId: barberoSeleccionado.value
        }
        const { data } = await api.post('Barberia/ObtenerHorariosDisponiblesDetallados', datosParaServicioDisponibleDto)

        horasDisponibles.value = data.result.map(slot => ({
            label: `${slot.hora} ${slot.disponible ? '' : '(Ocupado)'}`,
            value: slot.hora,
            disable: !slot.disponible
        }))
    } catch (err) {
        $q.notify({ type: 'negative', message: 'Error cargando horarios' })
    }
}


const crearReserva = async () => {
    if (!fecha.value || !hora.value) {
        return $q.notify({ type: 'warning', message: 'Selecciona fecha y hora' })
    }
    creando.value = true
    try {
        await api.post('/reservas', {
            proveedorId: proveedor.value.id,
            tipoProveedor: proveedor.value.tipo,
            servicioId: servicioSeleccionado.value.id,
            fechaHora: `${fecha.value}T${hora.value}:00`
        })
        $q.notify({ type: 'positive', message: 'Reserva solicitada' })
        dialogoReserva.value = false
    } finally {
        creando.value = false
    }
}
</script>


<style scoped lang="scss">
@import 'src/assets/theme/citacorte-design-system.scss';

.page-container {
    background: linear-gradient(135deg, $color-negro 0%, $color-gris-oscuro 100%);
    min-height: 100vh;
    font-family: $font-family-primary;
}

.bg-dark {
    background-color: $color-negro;
}

.text-white {
    color: $color-blanco;
}

.rounded-borders {
    border-radius: $border-radius-lg;
}

.shadow-10 {
    box-shadow: $shadow-lg;
}
</style>

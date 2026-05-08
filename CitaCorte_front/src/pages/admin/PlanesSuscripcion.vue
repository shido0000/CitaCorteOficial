<template>
    <q-page padding>
        <div class="row items-center q-mb-md">
            <div class="text-h6">Planes de Suscripción</div>
            <q-space />
            <q-btn label="Nuevo Plan" color="primary" @click="abrirFormulario()" />
        </div>

        <!-- Grid de cards -->
        <div v-if="planes.length" class="row q-col-gutter-md">
            <div v-for="plan in planes" :key="plan.id" class="col-xs-12 col-sm-6 col-md-4">
                <q-card flat bordered class="plan-card">
                    <q-card-section>
                        <div class="row items-center">
                            <div class="text-h6 text-primary">{{ plan.nombre }}</div>
                            <q-space />
                            <q-toggle v-model="plan.activo" @update:model-value="toggleActivo(plan)"
                                :label="plan.activo ? 'Activo' : 'Inactivo'" color="primary" dense />
                        </div>
                        <div class="text-subtitle2 text-grey-7">
                            {{ plan.tipoSuscripcion }} · {{ plan.nivelSuscripcion }}
                        </div>
                    </q-card-section>

                    <q-card-section>
                        <div class="text-h5 text-primary text-weight-bold">
                            {{ formatPrice(plan.precio) }}
                            <span class="text-caption text-grey">/ {{ plan.tiempoVigencia }} días</span>
                        </div>
                        <div class="q-mt-sm text-body2">{{ plan.descripcion }}</div>
                    </q-card-section>

                    <q-card-section v-if="plan.caracteristicaSuscripcion && plan.caracteristicaSuscripcion.length">
                        <div class="text-subtitle2 q-mb-sm">Características:</div>
                        <div class="row q-col-gutter-xs">
                            <div v-for="(car, idx) in plan.caracteristicaSuscripcion" :key="idx" class="col-auto">
                                <q-chip outline size="sm" color="primary">{{ car }}</q-chip>
                            </div>
                        </div>
                    </q-card-section>

                    <!-- Espaciador flexible para empujar las acciones abajo -->
                    <div class="flex-grow"></div>

                    <q-card-actions align="right" class="q-pa-md">
                        <q-btn flat round icon="edit" @click="abrirFormulario(plan)">
                            <q-tooltip>Editar plan</q-tooltip>
                        </q-btn>
                        <q-btn flat round icon="delete" @click="eliminar(plan.id)">
                            <q-tooltip>Eliminar plan</q-tooltip>
                        </q-btn>
                    </q-card-actions>
                </q-card>
            </div>
        </div>

        <div v-else class="text-center q-mt-xl text-grey-6">
            No hay planes creados.
            <q-btn flat color="primary" label="Crear el primero" @click="abrirFormulario()" />
        </div>

        <!-- Diálogo de formulario (crear/editar) -->
        <q-dialog v-model="dialogo" persistent>
            <q-card style="min-width: 550px">
                <q-card-section>
                    <div class="text-h6">{{ editando ? 'Editar Plan' : 'Nuevo Plan' }}</div>
                </q-card-section>

                <q-card-section class="q-pt-none">
                    <q-form @submit="guardar" class="q-gutter-md">
                        <q-select v-model="form.tipoSuscripcion" :options="tiposSuscripcion" label="Tipo de Suscripción"
                            outlined :rules="[val => !!val || 'Requerido']" />

                        <q-select v-model="form.nivelSuscripcion" :options="niveles" label="Nivel" outlined
                            :rules="[val => !!val || 'Requerido']" />

                        <q-input v-model="form.nombre" label="Nombre del Plan" outlined
                            :rules="[val => !!val || 'Requerido']" />

                        <q-input v-model="form.descripcion" label="Descripción" outlined type="textarea" rows="2" />

                        <q-input v-model.number="form.precio" label="Precio (USD)" type="number" step="0.01" outlined
                            :rules="[val => val !== null && val >= 0 || 'Debe ser >= 0']" />

                        <q-toggle v-model="form.esFree" label="¿Es plan gratuito?" color="primary" />

                        <q-input v-model.number="form.tiempoVigencia" label="Duración (días)" type="number" outlined
                            :rules="[val => val > 0 || 'Debe ser mayor a 0']" />

                        <q-select v-model="form.monedaId" :options="monedas" label="Moneda (opcional)" option-value="id"
                            option-label="descripcion" emit-value map-options outlined clearable />

                        <div class="text-right q-gutter-sm">
                            <q-btn label="Cancelar" flat v-close-popup />
                            <q-btn type="submit" label="Guardar" color="primary" :loading="guardando" />
                        </div>
                    </q-form>
                </q-card-section>
            </q-card>
        </q-dialog>
    </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useQuasar } from 'quasar'
import { api } from 'src/boot/axios'

const $q = useQuasar()
const planes = ref([])
const dialogo = ref(false)
const editando = ref(false)
const guardando = ref(false)
const currentId = ref(null)

// Opciones para selects
const tiposSuscripcion = [
    { label: 'Barbero', value: 'Barbero' },
    { label: 'Barbería', value: 'Barberia' }
]
const niveles = [
    { label: 'Free', value: 'Free' },
    { label: 'Popular', value: 'Popular' },
    { label: 'Premium', value: 'Premium' }
]
const monedas = ref([])

// Formulario alineado con la entidad Suscripcion
const form = ref({
    tipoSuscripcion: null,
    nivelSuscripcion: null,
    nombre: '',
    descripcion: '',
    precio: 0,
    esFree: false,
    tiempoVigencia: 30,
    monedaId: null
})

const formatPrice = (precio) => {
    return new Intl.NumberFormat('es-ES', { style: 'currency', currency: 'USD' }).format(precio || 0)
}

const cargarPlanes = async () => {
    try {
        const { data } = await api.get('Suscripcion/ObtenerListadoPaginado')
        planes.value = (data.result.elementos || []).map(p => ({
            ...p,
            activo: p.activo !== undefined ? p.activo : true
        }))
    } catch (error) {
        $q.notify({ type: 'negative', message: 'Error al cargar los planes' })
    }
}

const cargarMonedas = async () => {
    try {
        // Ajusta la URL según tu API (ejemplo: 'Moneda/ObtenerListado')
        const { data } = await api.get('Moneda/ObtenerListadoPaginado')
        monedas.value = (data.result.elementos || []).map(m => ({
            id: m.id,
            descripcion: m.descripcion || m.codigo
        }))
    } catch (error) {
        console.error('Error cargando monedas', error)
    }
}

const abrirFormulario = (row = null) => {
    if (row) {
        editando.value = true
        currentId.value = row.id
        form.value = {
            tipoSuscripcion: row.tipoSuscripcion,
            nivelSuscripcion: row.nivelSuscripcion,
            nombre: row.nombre,
            descripcion: row.descripcion,
            precio: row.precio,
            esFree: row.esFree,
            tiempoVigencia: row.tiempoVigencia,
            monedaId: row.monedaId || null
        }
    } else {
        editando.value = false
        currentId.value = null
        form.value = {
            tipoSuscripcion: null,
            nivelSuscripcion: null,
            nombre: '',
            descripcion: '',
            precio: 0,
            esFree: false,
            tiempoVigencia: 30,
            monedaId: null
        }
    }
    dialogo.value = true
}

const guardar = async () => {
    guardando.value = true
    try {
        const payload = {
            tipoSuscripcion: form.value.tipoSuscripcion.value,
            nivelSuscripcion: form.value.nivelSuscripcion.value,
            nombre: form.value.nombre,
            descripcion: form.value.descripcion,
            precio: form.value.precio,
            esFree: form.value.esFree,
            tiempoVigencia: form.value.tiempoVigencia,
            monedaId: form.value.monedaId || null
        }

        if (editando.value) {
            await api.put(`Suscripcion/Actualizar/${currentId.value}`, payload)
            $q.notify({ type: 'positive', message: 'Plan actualizado' })
        } else {
            await api.post('Suscripcion/Crear', payload)
            $q.notify({ type: 'positive', message: 'Plan creado' })
        }
        dialogo.value = false
        cargarPlanes()
    } catch (error) {
        $q.notify({ type: 'negative', message: 'Error al guardar el plan' })
    } finally {
        guardando.value = false
    }
}

const eliminar = async (id) => {
    $q.dialog({
        title: 'Eliminar',
        message: '¿Eliminar plan?',
        cancel: true,
        persistent: true
    }).onOk(async () => {
        try {
            await api.delete(`/admin/planes/${id}`)
            $q.notify({ type: 'positive', message: 'Plan eliminado' })
            cargarPlanes()
        } catch {
            $q.notify({ type: 'negative', message: 'Error al eliminar' })
        }
    })
}

const toggleActivo = async (row) => {
    try {
        await api.put(`/admin/planes/${row.id}`, { activo: row.activo })
        $q.notify({ type: 'positive', message: `Plan ${row.activo ? 'activado' : 'desactivado'}` })
    } catch {
        row.activo = !row.activo
        $q.notify({ type: 'negative', message: 'Error al cambiar estado' })
    }
}

onMounted(() => {
    cargarPlanes()
    cargarMonedas()
})
</script>

<style scoped lang="scss">
@import 'src/assets/theme/citacorte-design-system.scss';

.plan-card {
    display: flex;
    flex-direction: column;
    height: 100%;
    transition: transform $transition-base;
    background: $color-blanco;
    border: 1px solid rgba($color-dorado, 0.2);
    border-radius: $border-radius-lg;

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

.flex-grow {
    flex-grow: 1;
}
</style>

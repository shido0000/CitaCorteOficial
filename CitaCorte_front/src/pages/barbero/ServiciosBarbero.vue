<template>
    <q-page padding>
        <div class="row items-center q-mb-md">
            <div class="text-h6">Mis Servicios</div>
            <q-space />
            <q-btn label="Nuevo Servicio" color="primary" @click="abrirFormulario()" />
        </div>

        <q-table :rows="servicios" :columns="cols" row-key="id" :loading="loading">
            <template v-slot:body-cell-acciones="props">
                <div class="text-center">
                    <q-btn flat icon="edit" @click="abrirFormulario(props.row)" />
                    <q-btn flat icon="delete" @click="eliminar(props.row.id)" />
                </div>
            </template>
        </q-table>

        <!-- Diálogo para crear/editar -->
        <q-dialog v-model="dialogo" persistent>
            <q-card style="width: 600px; max-width: 90vw;">
                <q-card-section>
                    <div class="text-h6">{{ editando ? "Editar Servicio" : "Nuevo Servicio" }}</div>
                </q-card-section>
                <q-card-section>
                    <q-form @submit="guardar">
                        <q-input v-model="form.nombre" label="Nombre" outlined :rules="[(v) => !!v || 'Requerido']" />
                        <q-input v-model="form.descripcion" label="Descripción" outlined class="q-mt-sm"
                            type="textarea" />
                        <q-input v-model.number="form.tiempoDemora" label="Duración (minutos)" type="number" outlined
                            class="q-mt-sm" :rules="[(v) => v > 0 || 'Debe ser mayor a 0']" />
                        <q-input v-model.number="form.precio" label="Precio" type="number" outlined class="q-mt-sm"
                            :rules="[(v) => v >= 0 || 'Precio no válido']" prefix="$" />

                        <!-- Campo para subir imagen -->
                        <div class="q-mt-sm">
                            <div class="text-subtitle2">Foto del servicio</div>
                            <q-file v-model="imagenSeleccionada" label="Seleccionar imagen"
                                accept=".jpg, .jpeg, .png, .webp" outlined stack-label
                                @update:model-value="convertirImagenABase64" />
                            <div v-if="form.fotoUrl" class="q-mt-sm">
                                <q-img :src="form.fotoUrl" style="max-height: 150px; max-width: 100%" />
                            </div>
                        </div>

                        <div class="q-mt-md text-right">
                            <q-btn label="Cancelar" flat @click="dialogo = false" />
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
import { useQuasar,Dialog } from 'quasar'
import { api } from 'src/boot/axios'
import { decodeToken } from 'src/stores/auth'

const $q = useQuasar()
const servicios = ref([])
const categorias = ref([])
const dialogo = ref(false)
const editando = ref(false)
const imagenSeleccionada = ref(null)

const form = ref({
    nombre: '',
    descripcion: '',
    precio: 0,
    fotoUrl: '',
    tiempoDemora: 30,
    barberiaId: null,
    barberoId: null
})
const currentId = ref(null)
const loading = ref(false)
const guardando = ref(false)

const cols = [
    { name: 'nombre', field: 'nombre', label: 'Nombre', align: 'center' },
    { name: 'descripcion', field: 'descripcion', label: 'Descripción', align: 'center' },
    { name: 'tiempoDemora', field: 'tiempoDemora', label: 'Duración (min)', align: 'center' },
    { name: 'precio', field: 'precio', label: 'Precio', align: 'center' },
    { name: 'acciones', field: 'acciones', label: 'Acciones', align: 'center' }
]

const cargar = async () => {
    loading.value = true
    const decoded = decodeToken(sessionStorage.getItem('token'))
    const { data } = await
        api.get(`Servicio/ObtenerListadoPaginado?barberoId=${decoded.barberoId}`)
    servicios.value = data.result?.elementos || []
    loading.value = false
}
onMounted(cargar)


const convertirImagenABase64 = (file) => {
    if (!file) {
        form.value.fotoUrl = ''
        return
    }
    const reader = new FileReader()
    reader.onload = (e) => {
        form.value.fotoUrl = e.target.result
    }
    reader.onerror = () => {
        $q.notify({ type: 'negative', message: 'Error al leer la imagen' })
        form.value.fotoUrl = ''
    }
    reader.readAsDataURL(file)
}
const obtenerBarberoId = () => {
    const token = sessionStorage.getItem('token')
    if (!token) return null
    const decoded = decodeToken(token)
    return decoded?.barberoId || null
}

const abrirFormulario = (row = null) => {
    if (row) {
        editando.value = true
        currentId.value = row.id
        form.value = {
            nombre: row.nombre || '',
            descripcion: row.descripcion || '',
            precio: row.precio || 0,
            fotoUrl: row.fotoUrl || '',
            tiempoDemora: row.tiempoDemora || 30,
            barberiaId: null,
            barberoId: row.barberoId || obtenerBarberoId()
        }
        imagenSeleccionada.value = null
    } else {
        editando.value = false
        currentId.value = null
        form.value = {
            nombre: '',
            descripcion: '',
            precio: 0,
            fotoUrl: '',
            tiempoDemora: 30,
            barberiaId: null,
            barberoId: obtenerBarberoId()
        }
        imagenSeleccionada.value = null
    }
    dialogo.value = true
}

const guardar = async () => {
    if (!form.value.nombre) {
        $q.notify({ type: 'warning', message: 'El nombre es obligatorio' })
        return
    }
    guardando.value = true
    try {
        const payload = { ...form.value }
        if (editando.value) {
            payload.id = currentId.value
            // Enviar el id dentro del cuerpo
            await api.put(`Servicio/Actualizar/${payload.id}`, payload)
            $q.notify({ type: 'positive', message: 'Servicio actualizado' })
        } else {
            await api.post('Servicio/Crear', payload)
            $q.notify({ type: 'positive', message: 'Servicio creado' })
        }
        dialogo.value = false
        cargar()
    } catch (error) {
        console.error('Error al guardar:', error)
        $q.notify({ type: 'negative', message: error.response.data.errorMessage })
    } finally {
        guardando.value = false
    }
}
// ✅ CORREGIDO: usar Dialog en lugar de $q.dialog
const eliminar = async (id) => {
    Dialog.create({   // 👈 Usamos la función importada
        title: 'Eliminar',
        message: '¿Estás seguro de eliminar este servicio?',
        cancel: true,
        persistent: true
    }).onOk(async () => {
        try {
            await api.delete(`Servicio/Eliminar/${id}`)
            $q.notify({ type: 'positive', message: 'Servicio eliminado' })
            cargar()
        } catch (error) {
            console.error('Error al eliminar:', error)
            $q.notify({ type: 'negative', message: 'Error al eliminar' })
        }
    })
}
</script>

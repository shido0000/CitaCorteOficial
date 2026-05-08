<template>
    <q-page padding>
        <div class="row items-center q-mb-md">
            <div class="text-h6">Gestión de Usuarios</div>
            <q-space />
            <q-btn label="Nuevo Usuario" color="primary" icon="person_add" @click="abrirDialogoCrear" />
        </div>

        <div class="row q-mb-md col-12 items-center">
            <q-select v-model="filtroRol" option-value="value" option-label="text" :options="roles"
                label="Filtrar por rol" clearable class="col-xs-12 col-sm-4 col-md-4 q-mt-md" />
            <q-input v-model="busqueda" label="Buscar por nombre o email"
                class="col-xs-12 col-sm-4 col-md-4 q-ml-sm q-mt-md" />
            <q-btn label="Buscar" color="primary" icon="search" @click="cargar"
                class="col-xs-12 col-sm-2 col-md-2 q-ml-sm q-mt-md" />
        </div>

        <q-table :rows="usuarios" :columns="cols" row-key="id" :loading="loading">
            <template v-slot:body-cell-activo="props">
                <div class="text-center">
                    <q-toggle v-model="props.row.activo" @update:model-value="toggleActivo(props.row)" />
                </div>
            </template>
        </q-table>

        <!-- Diálogo para crear nuevo usuario -->
        <q-dialog v-model="dialogoCrear" persistent>
            <q-card style="min-width: 400px">
                <q-card-section>
                    <div class="text-h6">Nuevo Usuario</div>
                </q-card-section>
                <q-card-section class="q-pt-none">
                    <q-form @submit="crearUsuario" class="q-gutter-md">
                        <q-select v-model="nuevoUsuario.rol" :options="rolesAdminComercial" option-value="value"
                            option-label="text" label="Rol (Admin o Comercial)" outlined
                            :rules="[val => !!val || 'Seleccione un rol']" />

                        <q-input v-model="nuevoUsuario.nombre" label="Nombre" outlined
                            :rules="[val => !!val || 'Requerido']" />

                        <q-input v-model="nuevoUsuario.apellidos" label="Apellidos" outlined
                            :rules="[val => !!val || 'Requerido']" />

                        <q-input v-model="nuevoUsuario.username" label="Usuario" outlined
                            :rules="[val => !!val || 'Requerido']" />

                        <q-input v-model="nuevoUsuario.correo" label="Correo electrónico" type="email" outlined :rules="[
                            val => !!val || 'Requerido',
                            val => /.+@.+\..+/.test(val) || 'Correo inválido'
                        ]" />

                        <q-input v-model="nuevoUsuario.contrasenna" label="Contraseña" type="password" outlined
                            :rules="[val => !!val && val.length >= 6 || 'Mínimo 6 caracteres']" />

                        <div class="text-right q-gutter-sm">
                            <q-btn label="Cancelar" flat v-close-popup />
                            <q-btn type="submit" label="Guardar" color="primary" :loading="creando" />
                        </div>
                    </q-form>
                </q-card-section>
            </q-card>
        </q-dialog>
    </q-page>
</template>

<script setup>
import { onMounted, ref, computed } from 'vue'
import { useQuasar } from 'quasar'
import { api } from 'src/boot/axios'

const $q = useQuasar()
const usuarios = ref([])
const roles = ref([])           // todos los roles disponibles
const filtroRol = ref(null)
const busqueda = ref('')
const loading = ref(false)
const dialogoCrear = ref(false)
const creando = ref(false)

// Datos del nuevo usuario
const nuevoUsuario = ref({
    rol: null,
    nombre: '',
    apellidos: '',
    username: '',
    correo: '',
    contrasenna: ''
})

// Columnas de la tabla
const cols = [
    { name: 'nombre', field: 'nombre', label: 'Nombre', align: 'center' },
    { name: 'apellidos', field: 'apellidos', label: 'Apellidos', align: 'center' },
    { name: 'username', field: 'username', label: 'Usuario', align: 'center' },
    { name: 'email', field: 'correo', label: 'Email', align: 'center' },
    { name: 'rol', field: 'rol', label: 'Rol', align: 'center' },
    { name: 'activo', field: 'activo', label: 'Activo', align: 'center' }
]

// Filtrar solo roles Admin y Comercial para el select del formulario
const rolesAdminComercial = computed(() => {
    return roles.value.filter(role =>
        role.text.toLowerCase() === 'admin' || role.text.toLowerCase() === 'comercial'
    )
})

// Cargar lista de usuarios con filtros
const cargar = async () => {
    loading.value = true
    try {
        const { data } = await api.get('usuario/ObtenerListadoPaginado', {
            params: { rolId: filtroRol.value?.value, textoBuscar: busqueda.value }
        })
        usuarios.value = data.result.elementos || []
    } catch (error) {
        $q.notify({ type: 'negative', message: 'Error al cargar usuarios' })
    } finally {
        loading.value = false
    }
}

// Cambiar estado activo/inactivo
const toggleActivo = async (row) => {
    try {
        const resultado = await api.post(`usuario/CambiarEstadoUsuario/${row.id}`)
        $q.notify({
            type: 'info',
            message: `Usuario ${resultado.data.result ? 'activado' : 'desactivado'}`
        })
    } catch {
        // Revertir visualmente si falla
        row.activo = !row.activo
        $q.notify({ type: 'negative', message: 'Error al cambiar estado' })
    }
}

// Abrir diálogo y resetear formulario
const abrirDialogoCrear = () => {
    nuevoUsuario.value = {
        rol: null,
        nombre: '',
        apellidos: '',
        username: '',
        correo: '',
        contrasenna: ''
    }
    dialogoCrear.value = true
}

// Crear nuevo usuario (Admin o Comercial)
const crearUsuario = async () => {
    if (!nuevoUsuario.value.rol) {
        $q.notify({ type: 'warning', message: 'Seleccione un rol' })
        return
    }

    creando.value = true
    try {
        const payload = {
            rolId: nuevoUsuario.value.rol.value,
            nombre: nuevoUsuario.value.nombre,
            apellidos: nuevoUsuario.value.apellidos,
            username: nuevoUsuario.value.username,
            contrasenna: nuevoUsuario.value.contrasenna,
            correo: nuevoUsuario.value.correo
        }
        await api.post('Usuario/CrearUsusarioMejorado', payload)   // Ajusta el endpoint si es diferente
        $q.notify({ type: 'positive', message: 'Usuario creado exitosamente' })
        dialogoCrear.value = false
        cargar() // Recargar tabla
    } catch (error) {
        console.error(error)
        $q.notify({ type: 'negative', message: 'Error al crear usuario' })
    } finally {
        creando.value = false
    }
}

// Cargar roles disponibles y luego usuarios
onMounted(async () => {
    try {
        const { data } = await api.get('Rol/ObtenerSelectList?NombreCampoValor=Id&NombreCampoTexto=Nombre')
        roles.value = data.result
    } catch (error) {
        console.error('Error cargando roles:', error)
    }
    await cargar()
})
</script>

<style scoped lang="scss">
@import 'src/assets/theme/citacorte-design-system.scss';

/* Ajustes adicionales si son necesarios */
.q-table {
    background: $color-blanco;
    border-radius: $border-radius-lg;

    .dark & {
        background: #2D2D2D;
    }
}
</style>

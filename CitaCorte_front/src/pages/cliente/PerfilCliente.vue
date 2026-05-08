<template>
    <q-page padding>
        <div class="row justify-center">
            <div class="col-12 col-md-8">
                <q-card>
                    <q-card-section>
                        <div class="text-h6">Mi Perfil</div>
                    </q-card-section>

                    <q-card-section>
                        <q-form @submit="guardarPerfil" class="q-gutter-md">
                            <!-- Sección de Usuario -->
                            <div class="text-subtitle1 text-primary">Datos de usuario</div>
                            <q-input v-model="form.nombre" label="Nombre" outlined
                                :rules="[(v) => !!v || 'Requerido']" />
                            <q-input v-model="form.apellidos" label="Apellidos" outlined
                                :rules="[(v) => !!v || 'Requerido']" />
                            <q-input v-model="form.username" label="Nombre de usuario" outlined
                                :rules="[(v) => !!v || 'Requerido']" />
                            <q-input v-model="form.correo" label="Correo electrónico" type="email" outlined :rules="[
                                (v) => !!v || 'Requerido',
                                (v) => /.+@.+\..+/.test(v) || 'Correo inválido'
                            ]" />
                            <q-input v-model="form.contrasenna"
                                label="Nueva contraseña (dejar vacío si no se desea cambiar)" type="password"
                                outlined />

                                <div class="text-right q-mt-md">
                                <q-btn label="Guardar cambios" type="submit" color="primary" :loading="guardando" />
                            </div>
                        </q-form>
                    </q-card-section>
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
const guardando = ref(false)
const logoArchivo = ref(null)

// Formulario basado en UsuarioNuevoDto
const form = ref({
    id: '',
    nombre: '',
    apellidos: '',
    username: '',
    contrasenna: '',
    correo: '',
})

// Obtener perfil actual al cargar
const cargarPerfil = async () => {
    try {
        const token = sessionStorage.getItem('token')
        const decoded = decodeToken(token)
        const id = decoded?.id

        // Asumimos un endpoint que devuelva los datos completos del usuario + barbería
        // Ejemplo: GET /usuario/perfil
        const { data } = await api.get(`Autenticacion/ObtenerInformacionUsuario?id=${id}`)
        const perfil = data.result

        form.value = {
            id: perfil.id || '',
            nombre: perfil.nombre || '',
            apellidos: perfil.apellidos || '',
            username: perfil.username || '',
            contrasenna: '', // nunca se envía la contraseña actual al front
            correo: perfil.correo || '',
        }
    } catch (error) {
        console.error('Error al cargar perfil:', error)
        $q.notify({ type: 'negative', message: 'No se pudo cargar el perfil' })
    }
}

// Guardar perfil
const guardarPerfil = async () => {
    guardando.value = true
    try {
        const payload = { ...form.value }
        // Si la contraseña está vacía, la eliminamos para que el backend no la actualice
        if (!payload.contrasenna) {
            delete payload.contrasenna
        }

        await api.put('Usuario/ActualizarUsusarioMejorado', payload)
        $q.notify({ type: 'positive', message: 'Perfil actualizado correctamente' })
    } catch (error) {
        console.error('Error al guardar perfil:', error)
        $q.notify({ type: 'negative', message: 'Error al actualizar el perfil' })
    } finally {
        guardando.value = false
    }
}

onMounted(() => {
    cargarPerfil()
})
</script>

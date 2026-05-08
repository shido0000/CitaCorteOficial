<template>
    <div class="flex flex-center full-height bg-grey-2">
        <q-card style="width: 550px" class="q-pa-lg">
            <q-card-section class="q-pb-none">
                <div class="text-h6">Registrar como Barbero</div>
                <p class="text-caption text-grey">Completa todos los campos para crear tu cuenta</p>
            </q-card-section>
            <q-card-section>
                <q-form @submit="registrar" class="q-gutter-md">
                    <!-- Datos Personales -->
                    <div class="text-subtitle2 text-weight-bold">Datos Personales</div>
                    <q-input v-model="nombre" label="Nombre" outlined dense
                        :rules="[val => !!val || 'Campo requerido']" />
                    <q-input v-model="apellidos" label="Apellidos" outlined dense
                        :rules="[val => !!val || 'Campo requerido']" />
                    <q-input v-model="username" label="Usuario" outlined dense
                        :rules="[val => !!val || 'Campo requerido']" />

                    <!-- Datos de Contacto -->
                    <div class="text-subtitle2 text-weight-bold q-mt-lg">Datos de Contacto</div>
                    <q-input v-model="correo" label="Correo Electrónico" type="email" outlined dense :rules="[
                        val => !!val || 'Campo requerido',
                        val => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(val) || 'Email inválido'
                    ]" />
                    <q-input v-model="contrasenna" label="Contraseña" type="password" outlined dense :rules="[
                        val => !!val || 'Campo requerido',
                        val => val.length >= 6 || 'Mínimo 6 caracteres'
                    ]" />
                    <q-input v-model="direccion" label="Dirección" outlined dense
                        :rules="[val => !!val || 'Campo requerido']" />

                    <q-btn type="submit" label="Registrar como Barbero" color="primary" class="full-width q-mt-lg"
                        :loading="loading" unelevated size="lg" />
                </q-form>
            </q-card-section>
        </q-card>
    </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'
import { useQuasar } from 'quasar'
import { api } from 'src/boot/axios'

const $q = useQuasar()
const router = useRouter()

const nombre = ref('')
const apellidos = ref('')
const username = ref('')
const correo = ref('')
const contrasenna = ref('')
const direccion = ref('')
const loading = ref(false)

const BARBERO_ROLEID = 'C0B7E3B3-A06E-4580-B985-BB2FC4336525'
const BARBERO_SUSCRIPCION_ID = 'C0B7E3B3-A06E-4580-B985-BB2FC4336520'

const registrar = async () => {
    loading.value = true
    try {
        const payload = {
            rolId: BARBERO_ROLEID,
            nombre: nombre.value,
            apellidos: apellidos.value,
            username: username.value,
            contrasenna: contrasenna.value,
            correo: correo.value,
            direccion: direccion.value,
            suscripcionId: BARBERO_SUSCRIPCION_ID
        }

        const response = await api.post(`Usuario/CrearUsusarioMejorado`, payload)

        $q.notify({
            type: 'positive',
            message: 'Registro exitoso. Por favor inicia sesión.'
        })
        router.push('/login')
    } catch (error) {
        console.error('Error en registro:', error)
        const mensaje = error.response?.data?.errorMessage || 'Error al registrar. Intenta de nuevo.'
        $q.notify({
            type: 'negative',
            message: mensaje
        })
    } finally {
        loading.value = false
    }
}
</script>

<style scoped lang="scss">
@import 'src/assets/theme/citacorte-design-system.scss';

.full-height {
    min-height: 100vh;
    height: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    background: linear-gradient(135deg, $color-beige-claro 0%, $color-blanco 100%);

    .dark & {
        background: linear-gradient(135deg, #1A1A1A 0%, #2D2D2D 100%);
    }
}
</style>

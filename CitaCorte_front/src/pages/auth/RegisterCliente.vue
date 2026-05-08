<template>
  <div class="flex flex-center full-height bg-grey-2">
    <q-card style="width: 500px" class="q-pa-lg">
      <q-card-section class="q-pb-none">
        <div class="text-h6">Registrar como Cliente</div>
        <p class="text-caption text-grey">Completa todos los campos para crear tu cuenta</p>
      </q-card-section>
      <q-card-section>
        <q-form @submit="registrar" class="q-gutter-md">
          <!-- Datos Personales -->
          <div class="text-subtitle2 text-weight-bold">Datos Personales</div>
          <q-input v-model="nombre" label="Nombre" outlined dense :rules="[val => !!val || 'Campo requerido']" />
          <q-input v-model="apellidos" label="Apellidos" outlined dense :rules="[val => !!val || 'Campo requerido']" />
          <q-input v-model="username" label="Usuario" outlined dense :rules="[val => !!val || 'Campo requerido']" />

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
          <q-input v-model="direccion" label="Dirección" outlined dense :rules="[val => !!val || 'Campo requerido']" />

          <q-btn type="submit" label="Registrarse" color="primary" class="full-width q-mt-lg" :loading="loading"
            unelevated size="lg" />
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

const CLIENTE_ROLEID = 'C0B7E3B3-A06E-4580-B985-BB2FC4336524'

const registrar = async () => {
  loading.value = true
  try {
    const payload = {
      rolId: CLIENTE_ROLEID,
      nombre: nombre.value,
      apellidos: apellidos.value,
      username: username.value,
      contrasenna: contrasenna.value,
      correo: correo.value,
      direccion: direccion.value
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

<style scoped>
.full-height {
  min-height: 100vh;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>

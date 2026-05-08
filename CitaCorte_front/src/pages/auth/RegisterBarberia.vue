<template>
  <div class="flex flex-center full-height bg-grey-2">
    <q-card style="width: 600px" class="q-pa-lg">
      <q-card-section class="q-pb-none">
        <div class="text-h6">Registrar como Barbería</div>
        <p class="text-caption text-grey">Completa todos los campos para crear tu cuenta</p>
      </q-card-section>
      <q-card-section>
        <q-form @submit="registrar" class="q-gutter-md">
          <!-- Campos obligatorios comunes -->
          <div class="text-subtitle2 text-weight-bold">Datos Personales</div>
          <q-input v-model="nombre" label="Nombre" outlined dense :rules="[val => !!val || 'Campo requerido']" />
          <q-input v-model="apellidos" label="Apellidos" outlined dense :rules="[val => !!val || 'Campo requerido']" />
          <q-input v-model="username" label="Usuario" outlined dense :rules="[val => !!val || 'Campo requerido']" />

          <!-- Contacto -->
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
          <q-input v-model="telefono" label="Teléfono" outlined dense :rules="[val => !!val || 'Campo requerido']" />

          <!-- Datos de la Barbería -->
          <div class="text-subtitle2 text-weight-bold q-mt-lg">Datos de la Barbería</div>
          <q-input v-model="nombreBarberia" label="Nombre de la Barbería" outlined dense
            :rules="[val => !!val || 'Campo requerido']" />
          <q-input v-model="logoUrl" label="URL del Logo" outlined dense hint="Ej: https://ejemplo.com/logo.png" />

          <!-- Plan de Suscripción -->
          <div class="text-subtitle2 text-weight-bold q-mt-lg">Plan de Suscripción</div>
          <div v-if="cargandoSuscripciones" class="text-center q-my-md">
            <q-spinner color="primary" size="40px" />
          </div>
          <div v-else-if="suscripciones.length === 0" class="text-warning q-my-md">
            No hay planes disponibles. Contacta con soporte.
          </div>
          <div v-else class="q-gutter-md">
            <q-option-group v-model="suscripcionId" :options="opcionesSuscripcion" color="primary" inline />
            <q-card v-if="suscripcionSeleccionada" class="bg-blue-1 q-mt-md">
              <q-card-section class="q-py-sm">
                <div class="text-weight-bold">{{ suscripcionSeleccionada.nombre }}</div>
                <div class="text-caption text-grey-7">{{ suscripcionSeleccionada.descripcion }}</div>
                <div class="text-weight-bold q-mt-sm">${{ suscripcionSeleccionada.precio }} USD / {{
                  suscripcionSeleccionada.tiempoVigencia }} días</div>
                <ul class="q-pl-md q-my-sm" style="font-size: 12px;">
                  <li v-for="(carac, idx) in suscripcionSeleccionada.caracteristicaSuscripcion" :key="idx">
                    {{ carac }}
                  </li>
                </ul>
              </q-card-section>
            </q-card>
          </div>

          <q-btn type="submit" label="Registrar Barbería" color="primary" class="full-width q-mt-lg" :loading="loading"
            unelevated size="lg" />
        </q-form>
      </q-card-section>
    </q-card>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'
import { useQuasar } from 'quasar'
import { api } from 'src/boot/axios'

const $q = useQuasar()
const router = useRouter()

// Datos personales
const nombre = ref('')
const apellidos = ref('')
const username = ref('')
const correo = ref('')
const contrasenna = ref('')

// Datos de barbería
const nombreBarberia = ref('')
const direccion = ref('')
const telefono = ref('')
const logoUrl = ref('')

// Suscripción
const suscripciones = ref([])
const suscripcionId = ref('')
const cargandoSuscripciones = ref(true)
const loading = ref(false)

const BARBERIA_ROLEID = 'C0B7E3B3-A06E-4580-B985-BB2FC4336526'

onMounted(async () => {
  await cargarSuscripciones()
})

const cargarSuscripciones = async () => {
  try {
    cargandoSuscripciones.value = true
    const { data } = await api.get(`Suscripcion/ObtenerListadoPaginado?TipoSuscripcion=Barberia`)
    suscripciones.value = data.result.elementos || []
    if (suscripciones.value.length > 0) {
      suscripcionId.value = suscripciones.value[0].id
    }
  } catch (error) {
    console.error('Error cargando suscripciones:', error)
    $q.notify({
      type: 'negative',
      message: 'Error al cargar los planes de suscripción'
    })
  } finally {
    cargandoSuscripciones.value = false
  }
}

const opcionesSuscripcion = computed(() => {
  return suscripciones.value.map(s => ({
    label: `${s.nombre} - $${s.precio} USD`,
    value: s.id
  }))
})

const suscripcionSeleccionada = computed(() => {
  return suscripciones.value.find(s => s.id === suscripcionId.value)
})

const registrar = async () => {
  loading.value = true
  try {
    const payload = {
      rolId: BARBERIA_ROLEID,
      nombre: nombre.value,
      apellidos: apellidos.value,
      username: username.value,
      contrasenna: contrasenna.value,
      correo: correo.value,
      nombreBarberia: nombreBarberia.value,
      direccion: direccion.value,
      telefono: telefono.value,
      logoUrl: logoUrl.value,
      suscripcionId: suscripcionId.value
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

:deep(.q-card) {
    background: $color-blanco;
    border-radius: $border-radius-xl;
    box-shadow: $shadow-lg;
    border: 1px solid rgba($color-dorado, 0.1);

    .dark & {
        background: #2D2D2D;
        border-color: #404040;
    }
}

.text-h6 {
    font-size: $h2-font-size;
    font-weight: $font-weight-bold;
    color: $color-negro;
    margin-bottom: $spacing-xs;

    .dark & {
        color: $color-blanco;
    }
}

.text-caption {
    color: $color-gris-medio;
}

.text-subtitle2 {
    color: $color-negro;
    font-weight: $font-weight-semibold;

    .dark & {
        color: $color-blanco;
    }
}

:deep(.q-input) {
    :deep(.q-field__control) {
        border-radius: $border-radius-md;
    }

    :deep(.q-field__label) {
        font-family: $font-family-primary;
    }

    :deep(.q-field--focused) {
        :deep(.q-field__control) {
            border-color: $color-dorado;
        }
    }
}

:deep(.q-btn) {
    border-radius: $border-radius-md;
    text-transform: none;
    font-weight: $font-weight-semibold;
    letter-spacing: 0.5px;
}

:deep(.q-spinner) {
    color: $color-dorado;
}

.text-warning {
    color: #F59E0B !important;
}

.bg-blue-1 {
    background-color: rgba($color-dorado, 0.08) !important;
    border: 1px solid rgba($color-dorado, 0.2);
}
</style>

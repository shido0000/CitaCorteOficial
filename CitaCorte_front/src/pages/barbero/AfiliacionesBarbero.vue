<template>
    <q-page padding>
        <q-banner v-if="afiliacionActiva" class="bg-green-1 q-mb-md">
            Afiliado a {{ afiliacionActiva.barberia }} desde
            {{ afiliacionActiva.fecha }}
            <q-btn flat label="Romper afiliación" color="negative" @click="romper" />
        </q-banner>
        <div class="text-h6">Buscar Barbería</div>
        <q-input v-model="termino" label="Buscar" @keyup.enter="buscar" />
        <q-list>
            <q-item v-for="b in resultados" :key="b.id">
                <q-item-section>{{ b.nombre }} - {{ b.direccion }}</q-item-section>
                <q-item-section side>
                    <q-btn label="Solicitar afiliación" color="primary" @click="enviarSolicitud(b.id)"
                        :disable="b.cupoLleno" />
                </q-item-section>
            </q-item>
        </q-list>
        <div class="text-h6 q-mt-md">Mis Solicitudes</div>
        <q-list>
            <q-item v-for="s in solicitudes" :key="s.id">
                <q-item-section>{{ s.barberia }} ({{ s.estado }})</q-item-section>
            </q-item>
        </q-list>
    </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useQuasar } from 'quasar'
import { api } from 'src/boot/axios'
import { decodeToken } from 'src/stores/auth'

const $q = useQuasar()
const afiliacionActiva = ref(null)
const resultados = ref([])
const termino = ref('')
const solicitudes = ref([])

onMounted(async () => {
    const decoded = decodeToken(sessionStorage.getItem("token"));

    const { data } = await api.get('SolicitudAfiliacion/ObtenerAfiliacionActivaDeBarbero')
    afiliacionActiva.value = data.result
    console.log("afiliacionActiva.value: ",afiliacionActiva.value)
    const sol = await api.get(`SolicitudAfiliacion/ObtenerSolicitudesDeBarbero?barberoId=${decoded.barberoId}`)
    solicitudes.value = sol.result || []
})

const buscar = async () => {
    if (!termino.value) return
    const { data } = await api.get('Barberia/ObtenerListadoPaginado', {
        params: { textoBuscar: termino.value }
    })
    resultados.value = data
}
const enviarSolicitud = async (barberiaId) => {
    await api.post('/barbero/solicitud-afiliacion', { barberiaId })
    $q.notify('Solicitud enviada')
}
const romper = async () => {
    await api.delete('/barbero/afiliacion-activa')
    afiliacionActiva.value = null
    $q.notify('Afiliación finalizada')
}
</script>

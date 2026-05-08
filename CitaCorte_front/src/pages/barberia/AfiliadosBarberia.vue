<template>
    <q-page padding>
        <div class="text-h6">Barberos Afiliados</div>
        <q-tabs v-model="tab">
            <q-tab name="activos" label="Activos" />
            <q-tab name="solicitudes" label="Solicitudes" />
        </q-tabs>
        <q-tab-panels v-model="tab">
            <q-tab-panel name="activos">
                <q-list>
                    <q-item v-for="b in afiliadosActivos" :key="b.id">
                        <q-item-section>{{ b.barbero }}</q-item-section>
                        <q-item-section side>
                            <q-btn flat icon="link_off" @click="desafiliar(b.id)" />
                        </q-item-section>
                    </q-item>
                </q-list>
            </q-tab-panel>
            <q-tab-panel name="solicitudes">
                <q-list>
                    <q-item v-for="s in solicitudesPendientes" :key="s.id">
                        <q-item-section>{{ s.barbero }} quiere afiliarse</q-item-section>
                        <q-item-section side>
                            <q-btn flat icon="check" color="positive" @click="aprobar(s.id)" :disable="cupo <= 0" />
                            <q-btn flat icon="close" color="negative" @click="rechazar(s.id)" />
                        </q-item-section>
                    </q-item>
                </q-list>
            </q-tab-panel>
        </q-tab-panels>
    </q-page>
</template>

<script setup>
import { api } from 'src/boot/axios'
import { decodeToken } from 'src/stores/auth'
import { ref, onMounted } from 'vue'

const tab = ref('activos')
const afiliadosActivos = ref([])
const solicitudesPendientes = ref([])
const cupo = ref(0)

onMounted(async () => {
    const decoded = decodeToken(sessionStorage.getItem("token"));

    const { data } = await api.get(`Barberia/ObtenerBarberosAfiliadosABarberia?barberiaId=${decoded.barberiaId}`)
    afiliadosActivos.value = data.result.barberos
    solicitudesPendientes.value = data.result.solicitudesPendientes
    cupo.value = data.result.cuposDisponibles
})
const aprobar = async (id) => {
    await api.put(`/barberia/solicitudes-afiliacion/${id}/aprobar`)
    onMounted() // refrescar
}
const rechazar = async (id) => {
    await api.put(`/barberia/solicitudes-afiliacion/${id}/rechazar`)
    onMounted()
}
const desafiliar = async (id) => {
    await api.delete(`/barberia/afiliados/${id}`)
    onMounted()
}
</script>

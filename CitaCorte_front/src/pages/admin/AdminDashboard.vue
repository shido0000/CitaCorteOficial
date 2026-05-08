<template>
    <q-page class="admin-dashboard page-container">
        <!-- Welcome Hero -->
        <HeroSection title="Panel Administrativo" subtitle="Gestiona la plataforma y sus usuarios">
            <template #actions>
                <q-btn label="Gestionar Usuarios" icon="people" color="primary" unelevated to="/admin/usuarios"
                    size="lg" />
                <q-btn label="Solicitudes" icon="inbox" color="secondary" unelevated size="lg" />
            </template>
        </HeroSection>

        <!-- Key Metrics -->
        <div class="page-section">
            <h2 class="section-title">Métricas de la Plataforma</h2>
            <div class="stats-grid">
                <StatCard :value="stats.totalBarberos" label="Barberos" icon="people" color="primary" :change="12"
                    change-type="positive" />
                <StatCard :value="stats.totalBarberias" label="Barberías" icon="home_repair_service" color="info"
                    :change="5" change-type="positive" />
                <StatCard :value="stats.totalIngresosMes" label="Ingresos Mensuales" icon="trending_up" color="success"
                    :change="23" change-type="positive" />
                <StatCard :value="stats.totalSuscripcionesVencidas" label="Suscripciones Vencidas" icon="warning"
                    color="warning" :change="2" change-type="positive" />
            </div>
        </div>

        <!-- Quick Actions -->
        <div class="page-section">
            <h2 class="section-title">Acciones Rápidas</h2>
            <div class="actions-grid">
                <ActionCard title="Gestionar Usuarios" description="Administra todos los usuarios del sistema"
                    icon="people" to="/admin/usuarios" />
                <ActionCard title="Planes de Suscripción" description="Configura y modifica los planes"
                    icon="card_membership" to="/admin/planes" />
                <ActionCard title="Solicitudes de Cambio" description="Revisa y aprueba cambios de plan"
                    icon="assignment" to="/admin/solicitudes" />
                <ActionCard title="Reportes" description="Analiza datos y tendencias" icon="bar_chart"
                    to="/admin/reportes" />
            </div>
        </div>

        <!-- Pending Plan Changes -->
        <!-- <div class="page-section">
            <div class="section-header">
                <h2 class="section-title">Cambios de Plan Pendientes</h2>
                <q-chip label="5 pendientes" color="warning" text-color="white" />
            </div>
            <div v-if="pendingChanges.length === 0" class="empty-state">
                <q-icon name="done_all" size="lg" />
                <p>No hay cambios de plan pendientes</p>
            </div>
            <div v-else class="requests-list">
                <div v-for="change in pendingChanges" :key="change.id" class="request-item">
                    <div class="request-header">
                        <div class="request-info">
                            <h5>{{ change.businessName }}</h5>
                            <p>{{ change.currentPlan }} → {{ change.requestedPlan }}</p>
                        </div>
                        <div class="request-date">
                            {{ formatDate(change.requestDate) }}
                        </div>
                    </div>
                    <div class="request-details">
                        <span>Solicitante: {{ change.requesterName }}</span>
                        <span>Razón: {{ change.reason }}</span>
                    </div>
                    <div class="request-actions">
                        <q-btn label="Aprobar" color="positive" flat size="sm" @click="approveChange(change.id)" />
                        <q-btn label="Rechazar" color="negative" flat size="sm" @click="rejectChange(change.id)" />
                    </div>
                </div>
            </div>
        </div>-->

        <!-- Pending Registrations -->
        <div class="page-section">
            <div class="section-header">
                <h2 class="section-title">Registros Pendientes de Aprobación</h2>
                <q-chip :label="stats.listaSolicitudesSuscripcion.length + ' pendientes'" color="warning"
                    text-color="white" />
            </div>
            <div v-if="stats.listaSolicitudesSuscripcion.length === 0" class="empty-state">
                <q-icon name="check_circle" size="lg" />
                <p>Todos los registros han sido aprobados</p>
            </div>
            <div v-else class="registrations-table">
                <q-table :rows="stats.listaSolicitudesSuscripcion" :columns="registrationColumns" row-key="id" flat
                    bordered :pagination="{ rowsPerPage: 5 }">
                    <template #body-cell-actions="props">
                        <q-td :props="props">
                            <q-btn label="Ver" color="primary" flat size="sm" @click="viewRegistration(props.row)" />
                            <q-btn label="Aprobar" color="positive" flat size="sm"
                                @click="approveRegistration(props.row.id)" />
                            <q-btn label="Rechazar" color="negative" flat size="sm"
                                @click="rejectRegistration(props.row.id)" />
                        </q-td>
                    </template>
                </q-table>
            </div>
        </div>

        <!-- Recent Activity -->
        <!-- <div class="page-section">
            <h2 class="section-title">Actividad Reciente</h2>
            <div class="activity-timeline">
                <div v-for="(activity, index) in recentActivity" :key="activity.id" class="activity-item"
                    :class="{ 'last-item': index === recentActivity.length - 1 }">
                    <div class="activity-marker">
                        <q-icon :name="activity.icon" :color="activity.color" />
                    </div>
                    <div class="activity-content">
                        <h6>{{ activity.title }}</h6>
                        <p>{{ activity.description }}</p>
                        <small>{{ formatTimeAgo(activity.timestamp) }}</small>
                    </div>
                </div>
            </div>
        </div>-->
    </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import HeroSection from 'src/components/HeroSection.vue'
import StatCard from 'src/components/StatCard.vue'
import ActionCard from 'src/components/ActionCard.vue'
import { api } from 'src/boot/axios'

const stats = ref({
    totalBarberos: 0,
    totalBarberias: 0,
    totalIngresosMes: 0,
    totalSuscripcionesVencidas: 0,
    listaSolicitudesSuscripcion: []
})

const pendingChanges = [
    {
        id: 1,
        businessName: 'Barbería Central',
        currentPlan: 'Plan Professional',
        requestedPlan: 'Plan Business',
        requesterName: 'Juan García',
        requestDate: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000),
        reason: 'Necesitamos más barberos afiliados'
    },
    {
        id: 2,
        businessName: 'La Navaja',
        currentPlan: 'Plan Starter',
        requestedPlan: 'Plan Professional',
        requesterName: 'Carlos López',
        requestDate: new Date(Date.now() - 5 * 24 * 60 * 60 * 1000),
        reason: 'Expansión del negocio'
    },
    {
        id: 3,
        businessName: 'Cortes Premium',
        currentPlan: 'Plan Professional',
        requestedPlan: 'Plan Business',
        requesterName: 'Miguel Rodríguez',
        requestDate: new Date(Date.now() - 7 * 24 * 60 * 60 * 1000),
        reason: 'Mayor capacidad requerida'
    }
]

const pendingRegistrations = [
    {
        id: 1,
        nombre: 'Barbería Nueva Era',
        email: 'info@nuevaera.com',
        ciudad: 'Madrid',
        fecha: new Date(Date.now() - 3 * 24 * 60 * 60 * 1000),
        tipo: 'Barbería'
    },
    {
        id: 2,
        nombre: 'Cortes Modernos',
        email: 'contacto@cortesmodernos.com',
        ciudad: 'Barcelona',
        fecha: new Date(Date.now() - 1 * 24 * 60 * 60 * 1000),
        tipo: 'Barbería'
    },
    {
        id: 3,
        nombre: 'Juan Peluquero',
        email: 'juan@ejemplo.com',
        ciudad: 'Valencia',
        fecha: new Date(Date.now() - 5 * 60 * 60 * 1000),
        tipo: 'Barbero'
    }
]

const registrationColumns = [
    { name: 'nombre', align: 'center', label: 'Nombre', field: 'nombre' },
    { name: 'email', align: 'center', label: 'Email', field: 'email' },
    { name: 'tipo', align: 'center', label: 'Tipo', field: 'tipo' },
    { name: 'fecha', align: 'center', label: 'Fecha', field: 'fechaSolicitud' },
    { name: 'actions', align: 'center', label: 'Acciones' }
]

const recentActivity = [
    {
        id: 1,
        icon: 'app_registration',
        color: 'info',
        title: 'Nueva Barbería Registrada',
        description: 'Barbería Central se registró en la plataforma',
        timestamp: new Date(Date.now() - 1 * 60 * 60 * 1000)
    },
    {
        id: 2,
        icon: 'card_membership',
        color: 'positive',
        title: 'Cambio de Plan Aprobado',
        description: 'La Navaja cambió de Professional a Business',
        timestamp: new Date(Date.now() - 2 * 60 * 60 * 1000)
    },
    {
        id: 3,
        icon: 'warning',
        color: 'warning',
        title: 'Suscripción Próxima a Vencer',
        description: 'Cortes Premium vence en 3 días',
        timestamp: new Date(Date.now() - 5 * 60 * 60 * 1000)
    },
    {
        id: 4,
        icon: 'person_add',
        color: 'primary',
        title: 'Nuevo Barbero Afiliado',
        description: 'Pedro García se unió a Barbería Central',
        timestamp: new Date(Date.now() - 8 * 60 * 60 * 1000)
    }
]

const formatDate = (date) => {
    if (!date) return 'N/A'
    const options = { month: 'short', day: 'numeric', year: 'numeric' }
    return new Date(date).toLocaleDateString('es-ES', options)
}

const formatTimeAgo = (date) => {
    if (!date) return 'N/A'
    const now = new Date()
    const diff = now - new Date(date)
    const minutes = Math.floor(diff / 60000)
    const hours = Math.floor(diff / 3600000)
    const days = Math.floor(diff / 86400000)

    if (minutes < 60) return `hace ${minutes} minuto${minutes !== 1 ? 's' : ''}`
    if (hours < 24) return `hace ${hours} hora${hours !== 1 ? 's' : ''}`
    return `hace ${days} día${days !== 1 ? 's' : ''}`
}

const approveChange = (id) => {
    console.log('Aprobar cambio:', id)
}

const rejectChange = (id) => {
    console.log('Rechazar cambio:', id)
}

const approveRegistration = (id) => {
    console.log('Aprobar registro:', id)
}

const rejectRegistration = (id) => {
    console.log('Rechazar registro:', id)
}

const viewRegistration = (registration) => {
    console.log('Ver registro:', registration)
}

onMounted(async () => {
    try {
        const { data } = await api.get('Admin/ObtenerDatosDashBoardAdmin')
        stats.value = data.result
    } catch (error) {
        console.error('Error loading admin dashboard:', error)
    }
})
</script>

<style scoped lang="scss">
@import 'src/assets/theme/citacorte-design-system.scss';

.admin-dashboard {
    background-color: $color-blanco;

    .dark & {
        background-color: #1A1A1A;
    }
}

.page-container {
    max-width: 1400px;
    margin: 0 auto;
    padding: $spacing-xl;
    display: flex;
    flex-direction: column;
    gap: $spacing-xxl;
}

.page-section {
    display: flex;
    flex-direction: column;
    gap: $spacing-lg;
}

.section-title {
    font-size: $h2-font-size;
    font-weight: $font-weight-semibold;
    color: $color-negro;
    margin: 0;

    .dark & {
        color: $color-blanco;
    }
}

.section-header {
    display: flex;
    align-items: center;
    justify-content: space-between;
    flex-wrap: wrap;
    gap: $spacing-md;

    .section-title {
        margin: 0;
    }
}

.stats-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
    gap: $spacing-lg;
}

.actions-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
    gap: $spacing-lg;
}

.empty-state {
    text-align: center;
    padding: $spacing-xl $spacing-lg;
    background: linear-gradient(135deg, rgba($color-dorado, 0.05) 0%, rgba($color-dorado, 0) 100%);
    border: 2px dashed rgba($color-dorado, 0.3);
    border-radius: $border-radius-lg;

    :deep(.q-icon) {
        color: $color-gris-medio;
        margin-bottom: $spacing-md;
    }

    p {
        color: $color-gris-oscuro;
        margin-bottom: $spacing-md;
    }
}

.requests-list {
    display: flex;
    flex-direction: column;
    gap: $spacing-md;

    .request-item {
        background-color: $color-blanco;
        border: 1px solid #E5E5E5;
        border-radius: $border-radius-lg;
        padding: $spacing-lg;
        transition: all $transition-base;

        .dark & {
            background-color: #2D2D2D;
            border-color: #404040;
        }

        &:hover {
            border-color: rgba($color-dorado, 0.3);
            box-shadow: 0 4px 12px rgba($color-dorado, 0.1);
        }

        .request-header {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            margin-bottom: $spacing-md;
            padding-bottom: $spacing-md;
            border-bottom: 1px solid rgba($color-dorado, 0.1);

            .request-info {
                h5 {
                    font-size: $h4-font-size;
                    font-weight: $font-weight-semibold;
                    color: $color-negro;
                    margin: 0 0 $spacing-xs 0;

                    .dark & {
                        color: $color-blanco;
                    }
                }

                p {
                    font-size: 14px;
                    color: $color-gris-medio;
                    margin: 0;
                }
            }

            .request-date {
                font-size: 12px;
                color: $color-gris-medio;
                white-space: nowrap;
            }
        }

        .request-details {
            display: flex;
            gap: $spacing-lg;
            margin-bottom: $spacing-md;
            font-size: 14px;
            color: $color-gris-oscuro;

            .dark & {
                color: $color-gris-medio;
            }
        }

        .request-actions {
            display: flex;
            gap: $spacing-sm;

            :deep(.q-btn) {
                padding: 4px 16px;
            }
        }
    }
}

.registrations-table {
    background-color: $color-blanco;
    border-radius: $border-radius-lg;
    overflow: hidden;

    .dark & {
        background-color: #2D2D2D;

        :deep(.q-table__card) {
            background-color: #2D2D2D;
            box-shadow: none;
        }

        :deep(.q-table) {
            color: $color-blanco;
        }

        :deep(.q-table__heading) {
            background-color: #1A1A1A;
            color: $color-blanco;
        }

        :deep(.q-table__row) {
            border-color: #404040;
        }

        :deep(.q-table__row:hover) {
            background-color: #353535;
        }
    }

    // Estilos para modo claro (sin .dark)
    :deep(.q-table__heading) {
        background-color: #F5F5F5;
        color: $color-negro;
    }

    :deep(.q-table__row:hover) {
        background-color: rgba($color-dorado, 0.05);
    }
}

.activity-timeline {
    display: flex;
    flex-direction: column;

    .activity-item {
        display: flex;
        gap: $spacing-lg;
        padding: $spacing-lg 0;
        border-left: 2px solid rgba($color-dorado, 0.2);
        padding-left: $spacing-lg;
        position: relative;

        &:not(.last-item) {
            border-left-color: rgba($color-dorado, 0.3);
        }

        .activity-marker {
            position: absolute;
            left: -10px;
            top: 20px;
            width: 20px;
            height: 20px;
            background-color: $color-blanco;
            border: 2px solid rgba($color-dorado, 0.3);
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 10px;

            .dark & {
                background-color: #1A1A1A;
                border-color: rgba($color-dorado, 0.5);
            }
        }

        .activity-content {
            h6 {
                font-size: $h4-font-size;
                font-weight: $font-weight-semibold;
                color: $color-negro;
                margin: 0 0 4px 0;

                .dark & {
                    color: $color-blanco;
                }
            }

            p {
                font-size: 14px;
                color: $color-gris-oscuro;
                margin: 0 0 $spacing-xs 0;

                .dark & {
                    color: $color-gris-medio;
                }
            }

            small {
                color: $color-gris-medio;
                font-size: 12px;
            }
        }
    }
}

@media (max-width: 1024px) {
    .page-container {
        padding: $spacing-lg;
        gap: $spacing-lg;
    }

    .stats-grid {
        grid-template-columns: repeat(2, 1fr);
    }

    .actions-grid {
        grid-template-columns: 1fr;
    }

    .request-header {
        flex-direction: column;
        gap: $spacing-md;
    }

    .request-actions {
        flex-wrap: wrap;
    }
}

@media (max-width: 600px) {
    .page-container {
        padding: $spacing-md;
        gap: $spacing-md;
    }

    .stats-grid {
        grid-template-columns: 1fr;
    }

    .section-header {
        flex-direction: column;
        align-items: flex-start;
    }

    .request-details {
        flex-direction: column;
        gap: $spacing-sm;
    }

    .activity-item {
        padding-left: 0;
        border-left: none;
        border-top: 2px solid rgba($color-dorado, 0.2);
        padding-top: $spacing-lg;

        .activity-marker {
            left: 0;
            top: -10px;
        }
    }
}
</style>

<template>
    <q-page class="comercial-dashboard page-container">
        <!-- Welcome Hero -->
        <HeroSection title="Panel Comercial" subtitle="Gestiona ventas y suscripciones">
            <template #actions>
                <q-btn label="Nuevos Registros" icon="app_registration" color="primary" unelevated
                    to="/comercial/registros" size="lg" />
                <q-btn label="Solicitudes Cambio" icon="assignment" color="secondary" unelevated
                    to="/comercial/solicitudes" size="lg" />
            </template>
        </HeroSection>

        <!-- Sales Metrics -->
        <div class="page-section">
            <h2 class="section-title">Métricas de Ventas</h2>
            <div class="stats-grid">
                <StatCard :value="stats.totalBarberos" label="Total Barberos" icon="store" color="success" :change="25"
                    change-type="positive" />
                <StatCard :value="stats.totalBarberias" label="Total Barberías" icon="store" color="success"
                    :change="25" change-type="positive" />

                <StatCard :value="stats.totalSuscripcionesVencidas" label="Suscripciones Vencidas" icon="warning"
                    color="warning" :change="-2" change-type="negative" />
            </div>
        </div>

        <!-- Quick Actions -->
        <div class="page-section">
            <h2 class="section-title">Acciones Rápidas</h2>
            <div class="actions-grid">
                <ActionCard title="Nuevos Registros" description="Revisa barberías registradas recientemente"
                    icon="app_registration" to="/comercial/registros" />
                <ActionCard title="Solicitudes Cambio" description="Gestiona cambios de plan pendientes"
                    icon="assignment" to="/comercial/solicitudes" />
                <ActionCard title="Clientes Potenciales" description="Identifica oportunidades de venta"
                    icon="person_add" />
                <ActionCard title="Reportes de Ventas" description="Analiza tendencias de suscripciones"
                    icon="bar_chart" />
            </div>
        </div>

        <!-- Recent Registrations-->
        <!-- <div class="page-section">
            <div class="section-header">
                <h2 class="section-title">Registros Recientes</h2>
                <q-btn flat label="Ver Todos" icon="arrow_forward" color="primary" size="sm"
                    to="/comercial/registros" />
            </div>
            <div v-if="recentRegistrations.length === 0" class="empty-state">
                <q-icon name="app_registration" size="lg" />
                <p>No hay registros recientes</p>
            </div>
            <div v-else class="registrations-list">
                <div v-for="registration in recentRegistrations" :key="registration.id" class="registration-item">
                    <div class="registration-header">
                        <div class="registration-info">
                            <h5>{{ registration.nombre }}</h5>
                            <p>{{ registration.ciudad }}</p>
                        </div>
                        <div class="registration-date">
                            {{ formatDate(registration.fecha) }}
                        </div>
                    </div>
                    <div class="registration-details">
                        <q-chip :label="registration.tipo" color="primary" text-color="white" size="sm" />
                        <span class="contact">{{ registration.email }}</span>
                    </div>
                    <div class="registration-actions">
                        <q-btn label="Contactar" color="primary" flat size="sm" />
                        <q-btn label="Detalles" flat size="sm" />
                    </div>
                </div>
            </div>
        </div>
-->
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

        <!-- Performance Indicators -->
        <div class="page-section">
            <h2 class="section-title">Indicadores de Desempeño</h2>
            <div class="performance-grid">
                <div class="performance-card">
                    <h4>Tasa de Conversión</h4>
                    <div class="performance-value">
                        <span class="number">45%</span>
                        <q-icon name="trending_up" color="positive" />
                    </div>
                    <p>Registros → Clientes Activos</p>
                </div>
                <div class="performance-card">
                    <h4>Tiempo Promedio Conversión</h4>
                    <div class="performance-value">
                        <span class="number">8 días</span>
                        <q-icon name="schedule" />
                    </div>
                    <p>Desde registro hasta activación</p>
                </div>
                <div class="performance-card">
                    <h4>Churn Rate</h4>
                    <div class="performance-value">
                        <span class="number">3.2%</span>
                        <q-icon name="trending_down" color="positive" />
                    </div>
                    <p>Cancelaciones mensualmente</p>
                </div>
                <div class="performance-card">
                    <h4>Ingresos Promedio</h4>
                    <div class="performance-value">
                        <span class="number">$1,245</span>
                        <q-icon name="attach_money" color="warning" />
                    </div>
                    <p>Por cliente mensualmente</p>
                </div>
            </div>
        </div>
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
    totalSuscripcionesVencidas: 0,
    listaSolicitudesSuscripcion: []
})

const registrationColumns = [
    { name: 'nombre', align: 'center', label: 'Nombre', field: 'nombre' },
    { name: 'email', align: 'center', label: 'Email', field: 'email' },
    { name: 'tipo', align: 'center', label: 'Tipo', field: 'tipo' },
    { name: 'fecha', align: 'center', label: 'Fecha', field: 'fechaSolicitud' },
    { name: 'actions', align: 'center', label: 'Acciones' }
]

const recentRegistrations = [
    {
        id: 1,
        nombre: 'Barbería Moderna',
        ciudad: 'Madrid',
        email: 'info@moderna.com',
        tipo: 'Barbería',
        fecha: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000)
    },
    {
        id: 2,
        nombre: 'Cortes Premium',
        ciudad: 'Barcelona',
        email: 'contact@premium.com',
        tipo: 'Barbería',
        fecha: new Date(Date.now() - 5 * 24 * 60 * 60 * 1000)
    },
    {
        id: 3,
        nombre: 'Juan Barbero',
        ciudad: 'Valencia',
        email: 'juan@email.com',
        tipo: 'Barbero',
        fecha: new Date(Date.now() - 7 * 24 * 60 * 60 * 1000)
    }
]

const pendingPlanChanges = [
    {
        id: 1,
        businessName: 'Barbería Central',
        currentPlan: 'Professional',
        requestedPlan: 'Business',
        contactName: 'García López',
        requestDate: new Date(Date.now() - 3 * 24 * 60 * 60 * 1000),
        reason: 'Expansión del negocio',
        priceDifference: 45
    },
    {
        id: 2,
        businessName: 'La Navaja',
        currentPlan: 'Starter',
        requestedPlan: 'Professional',
        contactName: 'Carlos Ruiz',
        requestDate: new Date(Date.now() - 6 * 24 * 60 * 60 * 1000),
        reason: 'Mayor capacidad requerida',
        priceDifference: 25
    },
    {
        id: 3,
        businessName: 'Cortes Estilo',
        currentPlan: 'Professional',
        requestedPlan: 'Business',
        contactName: 'Miguel Torres',
        requestDate: new Date(Date.now() - 8 * 24 * 60 * 60 * 1000),
        reason: 'Nuevas sucursales',
        priceDifference: 50
    }
]

const formatDate = (date) => {
    if (!date) return 'N/A'
    const options = { month: 'short', day: 'numeric' }
    return new Date(date).toLocaleDateString('es-ES', options)
}

onMounted(async () => {
    try {
        const { data } = await api.get('Comercial/ObtenerDatosDashBoardComercial')
        stats.value = data.result
    } catch (error) {
        console.error('Error loading comercial dashboard:', error)
    }
})


const approveRegistration = (id) => {
    console.log('Aprobar registro:', id)
}

const rejectRegistration = (id) => {
    console.log('Rechazar registro:', id)
}

const viewRegistration = (registration) => {
    console.log('Ver registro:', registration)
}
</script>

<style scoped lang="scss">
@import 'src/assets/theme/citacorte-design-system.scss';

.comercial-dashboard {
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

.registrations-list {
    display: flex;
    flex-direction: column;
    gap: $spacing-md;

    .registration-item {
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

        .registration-header {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            margin-bottom: $spacing-md;
            padding-bottom: $spacing-md;
            border-bottom: 1px solid rgba($color-dorado, 0.1);

            .registration-info {
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

            .registration-date {
                font-size: 12px;
                color: $color-gris-medio;
            }
        }

        .registration-details {
            display: flex;
            align-items: center;
            gap: $spacing-md;
            margin-bottom: $spacing-md;

            .contact {
                font-size: 14px;
                color: $color-gris-oscuro;

                .dark & {
                    color: $color-gris-medio;
                }
            }
        }

        .registration-actions {
            display: flex;
            gap: $spacing-sm;

            :deep(.q-btn) {
                padding: 4px 16px;
            }
        }
    }
}

.plan-changes-list {
    display: flex;
    flex-direction: column;
    gap: $spacing-md;

    .plan-change-item {
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

        .change-header {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            margin-bottom: $spacing-md;
            padding-bottom: $spacing-md;
            border-bottom: 1px solid rgba($color-dorado, 0.1);

            .change-info {
                h5 {
                    font-size: $h4-font-size;
                    font-weight: $font-weight-semibold;
                    color: $color-negro;
                    margin: 0 0 $spacing-sm 0;

                    .dark & {
                        color: $color-blanco;
                    }
                }

                .plan-transition {
                    display: flex;
                    align-items: center;
                    gap: $spacing-sm;

                    :deep(.q-icon) {
                        color: $color-dorado;
                        margin: 0 $spacing-xs;
                    }
                }
            }

            .change-date {
                font-size: 12px;
                color: $color-gris-medio;
                white-space: nowrap;
            }
        }

        .change-details {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: $spacing-md;
            margin-bottom: $spacing-md;

            .detail {
                display: flex;
                flex-direction: column;
                gap: $spacing-xs;

                .label {
                    font-size: 12px;
                    font-weight: $font-weight-semibold;
                    color: $color-gris-medio;
                    text-transform: uppercase;
                }

                span:not(.label) {
                    font-size: 14px;
                    color: $color-negro;

                    .dark & {
                        color: $color-blanco;
                    }

                    &.value {
                        font-weight: $font-weight-semibold;
                        color: $color-dorado;
                    }
                }
            }
        }

        .change-actions {
            display: flex;
            gap: $spacing-sm;

            :deep(.q-btn) {
                padding: 4px 16px;
            }
        }
    }
}

.performance-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(240px, 1fr));
    gap: $spacing-lg;

    .performance-card {
        background-color: $color-blanco;
        border: 2px solid rgba($color-dorado, 0.2);
        border-radius: $border-radius-lg;
        padding: $spacing-lg;
        transition: all $transition-base;

        .dark & {
            background-color: #2D2D2D;
            border-color: rgba($color-dorado, 0.3);
        }

        &:hover {
            border-color: rgba($color-dorado, 0.5);
            box-shadow: 0 8px 16px rgba($color-dorado, 0.15);
            transform: translateY(-4px);
        }

        h4 {
            font-size: $h4-font-size;
            font-weight: $font-weight-semibold;
            color: $color-negro;
            margin: 0 0 $spacing-md 0;

            .dark & {
                color: $color-blanco;
            }
        }

        .performance-value {
            display: flex;
            align-items: baseline;
            gap: $spacing-md;
            margin-bottom: $spacing-md;

            .number {
                font-size: 32px;
                font-weight: $font-weight-bold;
                color: $color-dorado;
            }

            :deep(.q-icon) {
                font-size: 24px;
            }
        }

        p {
            font-size: 12px;
            color: $color-gris-medio;
            margin: 0;
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

    .performance-grid {
        grid-template-columns: repeat(2, 1fr);
    }

    .change-header {
        flex-direction: column;
        gap: $spacing-md;
    }

    .change-details {
        grid-template-columns: 1fr;
    }

    .change-actions {
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

    .performance-grid {
        grid-template-columns: 1fr;
    }

    .registration-details {
        flex-direction: column;
        align-items: flex-start;
    }

    .registration-actions {
        flex-wrap: wrap;
    }
}
</style>

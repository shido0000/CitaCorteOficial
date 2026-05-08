<template>
    <q-page class="barbero-dashboard page-container">
        <!-- Welcome Hero -->
        <HeroSection title="Panel del Barbero" subtitle="Gestiona tus servicios y reservas">
            <template #actions>
                <q-btn label="Nuevos Servicios" icon="add" color="primary" unelevated to="/barbero/servicios"
                    size="lg" />
                <q-btn label="Ver Reservas" icon="calendar_today" color="secondary" unelevated to="/barbero/reservas"
                    size="lg" />
            </template>
        </HeroSection>

        <!-- Key Stats -->
        <div class="page-section">
            <h2 class="section-title">Resumen del Día</h2>
            <div class="stats-grid">
                <StatCard :value="datosDashboard.cantidadReservasHoy" label="Reservas Hoy" icon="calendar_today"
                    color="primary" :change="5" change-type="positive" />
                <StatCard :value="datosDashboard.cantidadReservasCompletadasHoy" label="Completadas" icon="check_circle"
                    color="positive" :change="12" change-type="positive" />
                <StatCard :value="datosDashboard.ingresosHoy" label="Ingresos" icon="trending_up" color="success"
                    :change="8" change-type="positive" />
                <StatCard :value="datosDashboard.calificacion" label="Calificación" icon="star" color="warning" />
            </div>
        </div>

        <!-- Plan Status -->
        <div class="page-section">
            <div class="section-header">
                <h2 class="section-title">Tu Plan Actual</h2>
                <q-btn flat label="Cambiar Plan" icon="upgrade" size="sm" to="/barbero/cambiar-plan" />
            </div>

            <div class="plan-card">
                <div class="plan-header">
                    <div class="plan-name">{{ datosDashboard.suscripcionActualDto.nombre }}</div>
                    <q-badge color="primary" floating>{{ datosDashboard.suscripcionActualDto.estado }}</q-badge>
                </div>
                <div class="plan-details">
                    <div class="detail-row">
                        <span class="label">Precio Mensual:</span>
                        <span class="value">${{ datosDashboard.suscripcionActualDto.precio }}</span>
                    </div>
                    <div class="detail-row">
                        <span class="label">Fecha de Vencimiento:</span>
                        <span class="value">{{ formatDate(datosDashboard.suscripcionActualDto.fechaVencimiento)
                        }}</span>
                    </div>
                    <!--<div class="detail-row">
                        <span class="label">Servicios Permitidos:</span>
                        <span class="value">{{ currentPlan.serviciosPermitidos }} / meses</span>
                    </div>-->
                </div>
                <div class="plan-features">
                    <h4>Incluye:</h4>
                    <ul v-if="datosDashboard?.suscripcionActualDto?.caracteristicas?.length">
                        <li v-for="caract in datosDashboard.suscripcionActualDto.caracteristicas" :key="caract">
                            <q-icon name="check_circle" color="positive" />
                            {{ caract }}
                        </li>
                    </ul>
                    <div v-else class="text-grey-6">
                        No se encontraron características para este plan.
                    </div>
                </div>

            </div>
        </div>

        <!-- Quick Actions -->
        <div class="page-section">
            <h2 class="section-title">Acciones Rápidas</h2>
            <div class="actions-grid">
                <ActionCard title="Gestionar Servicios" description="Agrega, edita o elimina tus servicios"
                    icon="content_cut" to="/barbero/servicios" />
                <ActionCard title="Productos" description="Gestiona productos que ofreces" icon="storefront"
                    to="/barbero/productos" />
                <ActionCard title="Mis Reservas" description="Visualiza y gestiona todas tus reservas"
                    icon="calendar_today" to="/barbero/reservas" />
                <ActionCard title="Estadísticas" description="Analiza tu desempeño y crecimiento" icon="bar_chart"
                    to="/barbero/estadisticas" />
            </div>
        </div>

        <!-- Today's Appointments -->
        <div class="page-section">
            <h2 class="section-title">Citas de Hoy</h2>
            <div v-if="datosDashboard.reservasParaHoy.length === 0" class="empty-state">
                <q-icon name="calendar_today" size="lg" />
                <p>No tienes citas programadas para hoy</p>
            </div>
            <div v-else class="appointments-list">
                <div v-for="appointment in datosDashboard.reservasParaHoy" :key="appointment.id" class="appointment-item">
                    <div class="appointment-time">
                        <div class="time">{{ appointment.fechaReserva }}</div>
                        <div class="duration">{{ appointment.duracionEnMinutos }} min</div>
                    </div>
                    <div class="appointment-info">
                        <h4>{{ appointment.nombreServicio }}</h4>
                        <p>{{ appointment.nombreCliente }}</p>
                        <div class="appointment-meta">
                            <q-chip :label="`$${appointment.precioServicio}`" color="primary" text-color="dark" size="sm" />
                            <q-chip :label="appointment.estadoDeReserva" :color="getStatusColor(appointment.estadoDeReserva)" size="sm" />
                        </div>
                    </div>
                    <q-btn flat round icon="more_vert" size="sm">
                        <q-menu anchor="bottom right" self="top right">
                            <q-list style="min-width: 180px">
                                <q-item clickable v-ripple>
                                    <q-item-section>Ver detalles</q-item-section>
                                </q-item>
                                <q-item clickable v-ripple>
                                    <q-item-section>Marcar como completada</q-item-section>
                                </q-item>
                                <q-item clickable v-ripple class="text-negative">
                                    <q-item-section>Cancelar</q-item-section>
                                </q-item>
                            </q-list>
                        </q-menu>
                    </q-btn>
                </div>
            </div>
        </div>

        <!-- Recent Reviews -->
        <div class="page-section">
            <h2 class="section-title">Reseñas Recientes</h2>
            <div class="reviews-list">
                <div v-for="review in recentReviews" :key="review.id" class="review-item">
                    <div class="review-header">
                        <div class="reviewer-info">
                            <q-avatar :name="review.clientName" color="primary" text-color="white" />
                            <div class="reviewer-text">
                                <h5>{{ review.clientName }}</h5>
                                <small>{{ formatDate(review.date) }}</small>
                            </div>
                        </div>
                        <div class="review-rating">
                            <q-rating :model-value="review.rating" max="5" size="sm" color="warning" readonly />
                        </div>
                    </div>
                    <p class="review-text">{{ review.comment }}</p>
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
import { decodeToken } from 'src/stores/auth'
import { api } from 'src/boot/axios'

const currentPlan = ref({
    nombre: 'Plan Premium',
    estado: 'Activo',
    precioMensual: 29.99,
    fechaVencimiento: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000),
    serviciosPermitidos: 15
})

const datosDashboard = ref({
    cantidadReservasHoy: 0,
    cantidadReservasCompletadasHoy: 0,
    ingresosHoy: 0.00,
    calificacion: 0,
    suscripcionActualDto: {},
    reservasParaHoy:[]
})

const suscripcionActualDto = ref({
    nivelSuscripcion: null,
    nombre: 0,
    precio: 0.00,
    tiempoVigencia: 0,
    fechaVencimiento: null,
    estado: null,
    caracteristicas: {},
})

const todayAppointments = [
    {
        id: 1,
        time: '09:00 AM',
        duration: 30,
        serviceName: 'Corte + Barba',
        clientName: 'Juan Pérez',
        price: 25,
        status: 'Completada'
    },
    {
        id: 2,
        time: '10:00 AM',
        duration: 45,
        serviceName: 'Diseño de Barba',
        clientName: 'Carlos López',
        price: 20,
        status: 'En progreso'
    },
    {
        id: 3,
        time: '11:30 AM',
        duration: 30,
        serviceName: 'Corte Moderno',
        clientName: 'Miguel Rodríguez',
        price: 20,
        status: 'Confirmada'
    }
]

const recentReviews = [
    {
        id: 1,
        clientName: 'Juan Pérez',
        rating: 5,
        date: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000),
        comment: 'Excelente servicio, muy profesional y atento. Lo recomiendo mucho.'
    },
    {
        id: 2,
        clientName: 'Carlos López',
        rating: 4,
        date: new Date(Date.now() - 5 * 24 * 60 * 60 * 1000),
        comment: 'Buen trabajo, solo faltó un poco más de tiempo en los detalles.'
    }
]

const formatDate = (date) => {
    if (!date) return 'N/A'
    const options = { weekday: 'short', month: 'short', day: 'numeric' }
    return new Date(date).toLocaleDateString('es-ES', options)
}

const getStatusColor = (status) => {
    switch (status) {
        case 'Completada':
            return 'positive'
        case 'En progreso':
            return 'info'
        case 'Confirmada':
            return 'primary'
        default:
            return 'grey'
    }
}

onMounted(async () => {
    try {
        // const { data } = await api.get('/barbero/perfil')
        // currentPlan.value = data.plan
        const decoded = decodeToken(sessionStorage.getItem("token"));
        const { data } = await api.get(`Barbero/ObtenerDatosDashBoardBarbero?barberoId=${decoded.barberoId}`)

        datosDashboard.value.cantidadReservasHoy = data.result.cantidadReservasHoy
        datosDashboard.value.cantidadReservasCompletadasHoy = data.result.cantidadReservasCompletadasHoy
        datosDashboard.value.ingresosHoy = data.result.ingresosHoy
        datosDashboard.value.calificacion = data.result.calificacion
        datosDashboard.value.reservasParaHoy = data.result.reservasParaHoy

        suscripcionActualDto.value.nivelSuscripcion = data.result.suscripcionActualDto.nivelSuscripcion
        suscripcionActualDto.value.nombre = data.result.suscripcionActualDto.nombre
        suscripcionActualDto.value.precio = data.result.suscripcionActualDto.precio
        suscripcionActualDto.value.tiempoVigencia = data.result.suscripcionActualDto.tiempoVigencia
        suscripcionActualDto.value.fechaVencimiento = data.result.suscripcionActualDto.fechaVencimiento
        suscripcionActualDto.value.estado = data.result.suscripcionActualDto.estado
        suscripcionActualDto.value.barberosPermitidos = data.result.suscripcionActualDto.barberosPermitidos
        suscripcionActualDto.value.caracteristicas = data.result.suscripcionActualDto.caracteristicas




        datosDashboard.value.suscripcionActualDto = suscripcionActualDto.value


    } catch (error) {
        console.error('Error loading barbero dashboard:', error)
    }
})
</script>

<style scoped lang="scss">
@import 'src/assets/theme/citacorte-design-system.scss';

.barbero-dashboard {
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

.plan-card {
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
        box-shadow: 0 8px 16px rgba($color-dorado, 0.1);
    }

    .plan-header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        margin-bottom: $spacing-lg;
        padding-bottom: $spacing-lg;
        border-bottom: 1px solid #E5E5E5;

        .dark & {
            border-bottom-color: #404040;
        }

        .plan-name {
            font-size: $h3-font-size;
            font-weight: $font-weight-semibold;
            color: $color-dorado;
        }
    }

    .plan-details {
        margin-bottom: $spacing-lg;

        .detail-row {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: $spacing-sm 0;
            border-bottom: 1px solid rgba($color-dorado, 0.1);

            .label {
                font-weight: $font-weight-medium;
                color: $color-gris-oscuro;

                .dark & {
                    color: $color-gris-medio;
                }
            }

            .value {
                font-weight: $font-weight-semibold;
                color: $color-negro;

                .dark & {
                    color: $color-blanco;
                }
            }
        }
    }

    .plan-features {
        h4 {
            font-size: $h4-font-size;
            font-weight: $font-weight-semibold;
            color: $color-negro;
            margin: 0 0 $spacing-md 0;

            .dark & {
                color: $color-blanco;
            }
        }

        ul {
            list-style: none;
            padding: 0;
            margin: 0;

            li {
                display: flex;
                align-items: center;
                gap: $spacing-md;
                padding: $spacing-sm 0;
                color: $color-gris-oscuro;

                .dark & {
                    color: $color-gris-medio;
                }

                :deep(.q-icon) {
                    flex-shrink: 0;
                    font-size: 20px;
                }
            }
        }
    }
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

.appointments-list {
    display: flex;
    flex-direction: column;
    gap: $spacing-md;

    .appointment-item {
        display: flex;
        align-items: center;
        gap: $spacing-lg;
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

        .appointment-time {
            text-align: center;
            padding: $spacing-md;
            border-radius: $border-radius-md;
            background: linear-gradient(135deg, rgba($color-dorado, 0.1) 0%, rgba($color-dorado, 0.05) 100%);
            border: 1px solid rgba($color-dorado, 0.2);
            min-width: 80px;

            .time {
                font-size: 16px;
                font-weight: $font-weight-bold;
                color: $color-dorado;
            }

            .duration {
                font-size: 12px;
                color: $color-gris-medio;
                margin-top: 4px;
            }
        }

        .appointment-info {
            flex: 1;

            h4 {
                font-size: $h4-font-size;
                font-weight: $font-weight-semibold;
                color: $color-negro;
                margin: 0 0 $spacing-xs 0;

                .dark & {
                    color: $color-blanco;
                }
            }

            p {
                font-size: $small-font-size;
                color: $color-gris-medio;
                margin: 0 0 $spacing-sm 0;
            }

            .appointment-meta {
                display: flex;
                gap: $spacing-sm;

                :deep(.q-chip) {
                    padding: 4px 12px !important;
                    font-size: 12px;
                }
            }
        }
    }
}

.reviews-list {
    display: flex;
    flex-direction: column;
    gap: $spacing-md;

    .review-item {
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

        .review-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin-bottom: $spacing-md;
            padding-bottom: $spacing-md;
            border-bottom: 1px solid rgba($color-dorado, 0.1);

            .reviewer-info {
                display: flex;
                align-items: center;
                gap: $spacing-md;

                .reviewer-text {
                    h5 {
                        font-size: $h4-font-size;
                        font-weight: $font-weight-semibold;
                        color: $color-negro;
                        margin: 0;

                        .dark & {
                            color: $color-blanco;
                        }
                    }

                    small {
                        color: $color-gris-medio;
                        font-size: 12px;
                    }
                }
            }

            .review-rating {
                display: flex;
                align-items: center;
            }
        }

        .review-text {
            font-size: $body-font-size;
            color: $color-gris-oscuro;
            margin: 0;
            line-height: 1.6;

            .dark & {
                color: $color-gris-medio;
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

    .appointment-item {
        flex-direction: column;
        text-align: center;

        .appointment-info {
            text-align: left;
            width: 100%;
        }
    }

    .review-header {
        flex-direction: column;
        align-items: flex-start;
        gap: $spacing-md;
    }
}
</style>

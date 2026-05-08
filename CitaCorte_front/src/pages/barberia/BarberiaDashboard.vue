<template>
    <q-page class="barberia-dashboard page-container">
        <!-- Welcome Hero -->
        <HeroSection title="Panel de la Barbería" subtitle="Gestiona tu negocio y equipo">
            <template #actions>
                <q-btn label="Gestionar Barberos" icon="people" color="primary" unelevated to="/barberia/afiliados"
                    size="lg" />
                <q-btn label="Mis Servicios" icon="content_cut" color="secondary" unelevated to="/barberia/servicios"
                    size="lg" />
            </template>
        </HeroSection>

        <!-- Key Stats -->
        <div class="page-section">
            <h2 class="section-title">Resumen del Negocio</h2>
            <div class="stats-grid">
                <StatCard :value="datosDashboard.cantidadReservasHoy" label="Reservas Hoy" icon="calendar_today"
                    color="primary" :change="12" change-type="positive" />
                <StatCard :value="datosDashboard.cantidadBarberosAfiliados" label="Barberos Afiliados" icon="people"
                    color="info" :change="2" change-type="positive" />
                <StatCard :value="datosDashboard.ingresosHoy" label="Ingresos Hoy" icon="trending_up" color="success"
                    :change="18" change-type="positive" />
                <StatCard :value="datosDashboard.calificacion" label="Calificación" icon="star" color="warning" />
            </div>
        </div>

        <!-- Plan Status -->
        <div class="page-section">
            <div class="section-header">
                <h2 class="section-title">Plan {{ datosDashboard.suscripcionActualDto.nivelSuscripcion }}</h2>
                <q-btn flat label="Cambiar Plan" icon="upgrade" size="sm" to="/barberia/cambiar-plan" />
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
                    <div class="detail-row">
                        <span class="label">Barberos Permitidos:</span>
                        <span class="value">{{ datosDashboard.suscripcionActualDto.barberosPermitidos }} barberos</span>
                    </div>
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
                <ActionCard title="Gestionar Equipo" description="Agrega o administra tus barberos" icon="people"
                    to="/barberia/afiliados" />
                <ActionCard title="Servicios" description="Define los servicios que ofreces" icon="content_cut"
                    to="/barberia/servicios" />
                <ActionCard title="Reservas" description="Visualiza y gestiona todas las reservas" icon="calendar_today"
                    to="/barberia/reservas" />
                <ActionCard title="Estadísticas" description="Analiza el rendimiento de tu negocio" icon="bar_chart"
                    to="/barberia/estadisticas" />
            </div>
        </div>

        <!-- Affiliated Barbers -->
        <div class="page-section">
            <div class="section-header">
                <h2 class="section-title">Tu Equipo de Barberos</h2>
                <q-btn flat label="Agregar Barbero" icon="add" color="primary" size="sm" />
            </div>
            <div v-if="affiliatedBarbers.length === 0" class="empty-state">
                <q-icon name="people" size="lg" />
                <p>No tienes barberos afiliados aún</p>
            </div>
            <div v-else class="barbers-grid">
                <div v-for="barber in affiliatedBarbers" :key="barber.id" class="barber-card">
                    <div class="barber-header">
                        <q-avatar :name="barber.nombre" color="primary" text-color="white" size="lg" />
                        <div class="barber-status" :class="barber.estado.toLowerCase()">
                            {{ barber.estado }}
                        </div>
                    </div>
                    <div class="barber-info">
                        <h4>{{ barber.nombre }}</h4>
                        <p class="specialization">{{ barber.especializacion }}</p>
                        <div class="rating">
                            <q-rating :model-value="barber.calificacion" max="5" size="sm" color="warning" readonly />
                            <span>({{ barber.comentarios }} reseñas)</span>
                        </div>
                    </div>
                    <div class="barber-stats">
                        <div class="stat">
                            <span class="value">{{ barber.citasHoy }}</span>
                            <span class="label">Citas Hoy</span>
                        </div>
                        <div class="stat">
                            <span class="value">${{ barber.ingresosMes }}</span>
                            <span class="label">Ingresos</span>
                        </div>
                    </div>
                    <q-btn flat round icon="more_vert" size="sm">
                        <q-menu anchor="bottom right" self="top right">
                            <q-list style="min-width: 180px">
                                <q-item clickable v-ripple>
                                    <q-item-section>Ver perfil</q-item-section>
                                </q-item>
                                <q-item clickable v-ripple>
                                    <q-item-section>Estadísticas</q-item-section>
                                </q-item>
                                <q-item clickable v-ripple class="text-negative">
                                    <q-item-section>Remover</q-item-section>
                                </q-item>
                            </q-list>
                        </q-menu>
                    </q-btn>
                </div>
            </div>
        </div>

        <!-- Recent Reservations -->
        <div class="page-section">
            <h2 class="section-title">Reservas Recientes</h2>
            <div class="reservations-list">
                <div v-for="reservation in recentReservations" :key="reservation.id" class="reservation-item">
                    <div class="reservation-time">
                        <div class="date">{{ formatDateShort(reservation.fecha) }}</div>
                        <div class="time">{{ reservation.hora }}</div>
                    </div>
                    <div class="reservation-info">
                        <h5>{{ reservation.serviceName }}</h5>
                        <p>{{ reservation.clientName }} - {{ reservation.barberName }}</p>
                    </div>
                    <q-chip :label="`$${reservation.price}`" color="primary" text-color="dark" size="sm" />
                    <q-chip :label="reservation.status" :color="getStatusColor(reservation.status)" size="sm" />
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

const datosDashboard = ref({
    cantidadReservasHoy: 0,
    cantidadBarberosAfiliados: 0,
    ingresosHoy: 0.00,
    calificacion: 0,
    suscripcionActualDto: {}
})

const suscripcionActualDto = ref({
    nivelSuscripcion: null,
    nombre: 0,
    precio: 0.00,
    tiempoVigencia: 0,
    fechaVencimiento: null,
    estado: null,
    barberosPermitidos: 1,
    caracteristicas: {},
})

const currentPlan = ref({
    nombre: 'Plan Business',
    estado: 'Activo',
    precioMensual: 99.99,
    fechaVencimiento: new Date(Date.now() + 25 * 24 * 60 * 60 * 1000),
    barberosPermitidos: 10
})

const affiliatedBarbers = [
    {
        id: 1,
        nombre: 'Juan García',
        especializacion: 'Cortes modernos y barba',
        calificacion: 5,
        comentarios: 24,
        estado: 'Activo',
        citasHoy: 6,
        ingresosMes: 850
    },
    {
        id: 2,
        nombre: 'Carlos López',
        especializacion: 'Barbería clásica',
        calificacion: 4.8,
        comentarios: 18,
        estado: 'Activo',
        citasHoy: 5,
        ingresosMes: 720
    },
    {
        id: 3,
        nombre: 'Miguel Rodríguez',
        especializacion: 'Diseños y tatuajes',
        calificacion: 4.9,
        comentarios: 30,
        estado: 'Activo',
        citasHoy: 4,
        ingresosMes: 920
    }
]

const recentReservations = [
    {
        id: 1,
        fecha: new Date(),
        hora: '09:30 AM',
        serviceName: 'Corte + Barba',
        clientName: 'Pedro Sánchez',
        barberName: 'Juan García',
        price: 35,
        status: 'Completada'
    },
    {
        id: 2,
        fecha: new Date(),
        hora: '10:45 AM',
        serviceName: 'Diseño de Barba',
        clientName: 'Roberto Díaz',
        barberName: 'Carlos López',
        price: 25,
        status: 'En progreso'
    },
    {
        id: 3,
        fecha: new Date(),
        hora: '02:00 PM',
        serviceName: 'Corte Moderno',
        clientName: 'Andrés Fernández',
        barberName: 'Miguel Rodríguez',
        price: 30,
        status: 'Confirmada'
    }
]

const formatDate = (date) => {
    if (!date) return 'N/A'
    const options = { weekday: 'short', month: 'short', day: 'numeric' }
    return new Date(date).toLocaleDateString('es-ES', options)
}

const formatDateShort = (date) => {
    if (!date) return 'N/A'
    const options = { month: 'short', day: 'numeric' }
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
        const decoded = decodeToken(sessionStorage.getItem("token"));
        // const { data } = await api.get('/barberia/perfil')
        // currentPlan.value = data.plan
        const { data } = await api.get(`barberia/ObtenerDatosDashBoardBarberia?barberiaId=${decoded.barberiaId}`)
        datosDashboard.value.cantidadReservasHoy = data.result.cantidadReservasHoy
        datosDashboard.value.cantidadBarberosAfiliados = data.result.cantidadBarberosAfiliados
        datosDashboard.value.ingresosHoy = data.result.ingresosHoy
        datosDashboard.value.calificacion = data.result.calificacion

        suscripcionActualDto.value.nivelSuscripcion = data.result.suscripcionActualDto.nivelSuscripcion
        suscripcionActualDto.value.nombre = data.result.suscripcionActualDto.nombre
        suscripcionActualDto.value.precio = data.result.suscripcionActualDto.precio
        suscripcionActualDto.value.tiempoVigencia = data.result.suscripcionActualDto.tiempoVigencia
        suscripcionActualDto.value.fechaVencimiento = data.result.suscripcionActualDto.fechaVencimiento
        suscripcionActualDto.value.estado = data.result.suscripcionActualDto.estado
        suscripcionActualDto.value.barberosPermitidos = data.result.suscripcionActualDto.barberosPermitidos
        suscripcionActualDto.value.caracteristicas = data.result.suscripcionActualDto.caracteristicas




        datosDashboard.value.suscripcionActualDto = suscripcionActualDto.value


        console.log("datosDashboard.value: ", datosDashboard.value)
    } catch (error) {
        console.error('Error loading barberia dashboard:', error)
    }
})
</script>

<style scoped lang="scss">
@import 'src/assets/theme/citacorte-design-system.scss';

.barberia-dashboard {
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

.barbers-grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
    gap: $spacing-lg;

    .barber-card {
        background-color: $color-blanco;
        border: 1px solid #E5E5E5;
        border-radius: $border-radius-lg;
        padding: $spacing-lg;
        transition: all $transition-base;
        display: flex;
        flex-direction: column;
        gap: $spacing-md;

        .dark & {
            background-color: #2D2D2D;
            border-color: #404040;
        }

        &:hover {
            border-color: rgba($color-dorado, 0.3);
            box-shadow: 0 8px 16px rgba($color-dorado, 0.15);
            transform: translateY(-4px);
        }

        .barber-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: $spacing-md;

            .barber-status {
                padding: 4px 12px;
                border-radius: $border-radius-full;
                font-size: 12px;
                font-weight: $font-weight-semibold;
                text-transform: uppercase;

                &.activo {
                    background-color: rgba(76, 175, 80, 0.2);
                    color: #4CAF50;
                }

                &.inactivo {
                    background-color: rgba(244, 67, 54, 0.2);
                    color: #F44336;
                }
            }
        }

        .barber-info {
            h4 {
                font-size: $h4-font-size;
                font-weight: $font-weight-semibold;
                color: $color-negro;
                margin: 0 0 $spacing-xs 0;

                .dark & {
                    color: $color-blanco;
                }
            }

            .specialization {
                font-size: 14px;
                color: $color-gris-medio;
                margin: 0 0 $spacing-sm 0;
            }

            .rating {
                display: flex;
                align-items: center;
                gap: $spacing-sm;
                font-size: 12px;
                color: $color-gris-medio;
            }
        }

        .barber-stats {
            display: flex;
            justify-content: space-around;
            padding: $spacing-md 0;
            border-top: 1px solid rgba($color-dorado, 0.1);
            border-bottom: 1px solid rgba($color-dorado, 0.1);

            .stat {
                text-align: center;

                .value {
                    display: block;
                    font-size: 18px;
                    font-weight: $font-weight-bold;
                    color: $color-dorado;
                }

                .label {
                    display: block;
                    font-size: 11px;
                    color: $color-gris-medio;
                    text-transform: uppercase;
                    margin-top: 4px;
                }
            }
        }
    }
}

.reservations-list {
    display: flex;
    flex-direction: column;
    gap: $spacing-md;

    .reservation-item {
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

        .reservation-time {
            text-align: center;
            padding: $spacing-md;
            border-radius: $border-radius-md;
            background: linear-gradient(135deg, rgba($color-dorado, 0.1) 0%, rgba($color-dorado, 0.05) 100%);
            border: 1px solid rgba($color-dorado, 0.2);
            min-width: 90px;

            .date {
                font-size: 12px;
                color: $color-gris-medio;
                text-transform: uppercase;
            }

            .time {
                font-size: 16px;
                font-weight: $font-weight-bold;
                color: $color-dorado;
                margin-top: 4px;
            }
        }

        .reservation-info {
            flex: 1;

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

    .barbers-grid {
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

    .reservation-item {
        flex-direction: column;
        text-align: center;

        .reservation-info {
            text-align: left;
            width: 100%;
        }
    }

    .barber-card {
        .barber-header {
            flex-direction: column;
            align-items: flex-start;
        }
    }
}
</style>

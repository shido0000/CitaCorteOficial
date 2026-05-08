<template>
    <q-page class="cliente-dashboard page-container">
        <!-- Hero Section -->
        <HeroSection title="¡Bienvenido de vuelta!" :subtitle="`${userGreeting}, ${userName}`">
            <template #actions>
                <q-btn label="Buscar Barbería" icon="search" color="dark" text-color="primary" unelevated
                    to="/cliente/buscar" size="lg" />
                <q-btn label="Nueva Reserva" icon="event" color="primary" unelevated to="/cliente/reservas" size="lg" />
            </template>
        </HeroSection>

        <!-- Statistics Section -->
        <div class="page-section">
            <h2 class="section-title">Tu Actividad</h2>
            <div class="stats-grid">
                <StatCard :value="datosDashboard.totalReservas" label="Reservas" icon="event" color="primary"
                    :change="8" change-type="positive" />
                <StatCard :value="datosDashboard.totalReservasCompletadas" label="Completadas" icon="check_circle"
                    color="positive" :change="15" change-type="positive" />
                <StatCard :value="datosDashboard.totalReservasProximas" label="Próximas" icon="calendar_today"
                    color="info" />
            </div>
        </div>

        <!-- Quick Actions -->
        <div class="page-section">
            <h2 class="section-title">Acciones Rápidas</h2>
            <div class="actions-grid">
                <ActionCard title="Buscar Barberias"
                    description="Encuentra barberias cercanas con los mejores servicios" icon="search"
                    to="/cliente/buscar" />
                <ActionCard title="Mis Reservas" description="Gestiona y visualiza todas tus reservas"
                    icon="calendar_today" to="/cliente/reservas" />
                <ActionCard title="Historial" description="Revisa el historial completo de tus visitas" icon="history"
                    to="/cliente/historial" />
                <ActionCard title="Perfil" description="Edita tu información personal" icon="person"
                    to="/cliente/perfil" />
            </div>
        </div>

        <!-- Recent Reservations -->
        <div class="page-section">
            <div class="section-header">
                <h2 class="section-title">Próximas Reservas</h2>
                <q-btn flat label="Ver todas" icon="arrow_forward" size="sm" to="/cliente/reservas" />
            </div>

            <div class="reservations-list">
                <div v-if="datosDashboard.listadoReservasProximas.length === 0" class="empty-state">
                    <q-icon name="calendar_today" size="lg" />
                    <p>No tienes reservas próximas</p>
                    <q-btn label="Reservar ahora" color="primary" to="/cliente/buscar" />
                </div>
                <div v-else v-for="reservation in datosDashboard.listadoReservasProximas" :key="reservation.id"
                    class="reservation-item">
                    <div class="reservation-time">
                        <div class="date">{{ formatDate(reservation.fecha) }}</div>
                        <div class="time">{{ reservation.hora }}</div>
                    </div>
                    <div class="reservation-info">
                        <h4>{{ reservation.nombreBarberiaBarbero }}</h4>
                        <p>{{ reservation.nombreServicio }}</p>
                        <div class="reservation-meta">
                            <span><q-icon name="place" size="xs" /> {{ reservation.distancia }}</span>
                            <span><q-icon name="star" size="xs" /> {{ reservation.rating }}</span>
                        </div>
                    </div>
                    <q-btn flat round icon="more_vert" size="sm">
                        <q-menu anchor="bottom right" self="top right">
                            <q-list style="min-width: 180px">
                                <q-item clickable v-ripple>
                                    <q-item-section>Ver detalles</q-item-section>
                                </q-item>
                                <q-item clickable v-ripple>
                                    <q-item-section>Editar</q-item-section>
                                </q-item>
                                <q-item clickable v-ripple>
                                    <q-item-section>Cancelar</q-item-section>
                                </q-item>
                            </q-list>
                        </q-menu>
                    </q-btn>
                </div>
            </div>
        </div>

        <!-- Recommended Barber Shops -->
        <div class="page-section">
            <h2 class="section-title">Barberias Recomendadas</h2>
            <div class="barberia-carousel">
                <div v-for="barberia in datosDashboard.listadBarberiasRecomendadas" :key="barberia.id"
                    class="barberia-card">
                    <div class="barberia-image">
                        <q-img :src="barberia.fotoUrl" aspect-ratio="1" />
                    </div>
                    <div class="barberia-content">
                        <h4>{{ barberia.nombreBarberia }}</h4>
                        <div class="rating">
                            <q-icon name="star" color="warning" />
                            <span>{{ barberia.clasificacion }}</span>
                            <span class="reviews">({{ barberia.resenhas }} reseñas)</span>
                        </div>
                        <p class="address">{{ barberia.direccion }}</p>
                        <q-btn label="Reservar" color="primary" unelevated size="sm" class="full-width q-mt-md"
                            @click="navigateToBarberia(barberia.id)" />
                    </div>
                </div>
            </div>
        </div>
    </q-page>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import HeroSection from 'src/components/HeroSection.vue'
import StatCard from 'src/components/StatCard.vue'
import ActionCard from 'src/components/ActionCard.vue'
import { decodeToken } from 'src/stores/auth'
import { api } from 'src/boot/axios'

const router = useRouter()
const userName = ref('Juan Pérez')
const datosDashboard = ref({
    totalReservas: 0,
    totalReservasCompletadas: 0,
    totalReservasProximas: 0,
    listadoReservasProximas: [],
    listadBarberiasRecomendadas: []
})

const userGreeting = computed(() => {
    const hour = new Date().getHours()
    if (hour < 12) return 'Buenos días'
    if (hour < 18) return 'Buenas tardes'
    return 'Buenas noches'
})

const upcomingReservations = [
    {
        id: 1,
        date: new Date(Date.now() + 2 * 24 * 60 * 60 * 1000),
        time: '10:30 AM',
        barberiaName: 'Barbería Premium',
        serviceName: 'Corte + Barba',
        barberoName: 'Carlos',
        distance: '2.5 km',
        rating: '4.8'
    },
    {
        id: 2,
        date: new Date(Date.now() + 5 * 24 * 60 * 60 * 1000),
        time: '3:00 PM',
        barberiaName: 'Barbería Central',
        serviceName: 'Corte Moderno',
        barberoName: 'Miguel',
        distance: '1.8 km',
        rating: '4.6'
    }
]

const recommendedBarber = [
    {
        id: 1,
        name: 'Barbería Premium',
        rating: 4.8,
        reviews: 342,
        address: 'Av. Principal 123',
        image: 'https://images.unsplash.com/photo-1503699876756-6f78fc1d4487?w=400&h=400&fit=crop',
        discount: 20
    },
    {
        id: 2,
        name: 'Barbería Central',
        rating: 4.6,
        reviews: 289,
        address: 'Calle 5 y 6',
        image: 'https://images.unsplash.com/photo-1516975080664-ed2fc6a32937?w=400&h=400&fit=crop',
        discount: 15
    },
    {
        id: 3,
        name: 'Barbería Elite',
        rating: 4.9,
        reviews: 156,
        address: 'Centro Comercial Downtown',
        image: 'https://images.unsplash.com/photo-1503699876756-6f78fc1d4487?w=400&h=400&fit=crop',
        discount: 25
    },
    {
        id: 4,
        name: 'Barbería 24h',
        rating: 4.5,
        reviews: 421,
        address: 'Zona comercial Norte',
        image: 'https://images.unsplash.com/photo-1516975080664-ed2fc6a32937?w=400&h=400&fit=crop',
        discount: 10
    }
]

const formatDate = (date) => {
    const options = { weekday: 'short', month: 'short', day: 'numeric' }
    return date.toLocaleDateString('es-ES', options)
}

const navigateToBarberia = (id) => {
    router.push(`/cliente/detalle/barberia/${id}`)
}


onMounted(async () => {
    const decoded = decodeToken(sessionStorage.getItem("token"));
    const { data } = await api.get(`Cliente/ObtenerDatosDashBoardCliente?clienteId=${decoded.clienteId}`)
    datosDashboard.value.totalReservas = data.result.totalReservas
    datosDashboard.value.totalReservasCompletadas = data.result.totalReservasCompletadas
    datosDashboard.value.totalReservasProximas = data.result.totalReservasProximas
    datosDashboard.value.listadoReservasProximas = data.result.listadoReservasProximas
    datosDashboard.value.listadBarberiasRecomendadas = data.result.listadBarberiasRecomendadas

    userName.value = decoded.nombreUsuario
})
</script>

<style scoped lang="scss">
@import 'src/assets/theme/citacorte-design-system.scss';

.cliente-dashboard {
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

.reservations-list {
    display: flex;
    flex-direction: column;
    gap: $spacing-md;

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
}

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
        min-width: 80px;

        .date {
            font-size: 12px;
            font-weight: $font-weight-semibold;
            color: $color-dorado;
            text-transform: uppercase;
            margin-bottom: 4px;
        }

        .time {
            font-size: 16px;
            font-weight: $font-weight-bold;
            color: $color-negro;

            .dark & {
                color: $color-blanco;
            }
        }
    }

    .reservation-info {
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

        .reservation-meta {
            display: flex;
            gap: $spacing-md;
            font-size: 12px;
            color: $color-gris-medio;

            span {
                display: flex;
                align-items: center;
                gap: 4px;
            }
        }
    }
}

.barberia-carousel {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
    gap: $spacing-lg;
    overflow-x: auto;
    padding-bottom: $spacing-md;

    .barberia-card {
        background-color: $color-blanco;
        border-radius: $border-radius-lg;
        overflow: hidden;
        box-shadow: $shadow-md;
        transition: all $transition-base;

        .dark & {
            background-color: #2D2D2D;
        }

        &:hover {
            transform: translateY(-8px);
            box-shadow: $shadow-lg;

            .barberia-image {
                transform: scale(1.05);
            }
        }

        .barberia-image {
            position: relative;
            overflow: hidden;
            height: 180px;

            :deep(.q-img) {
                transition: transform $transition-base;
            }
        }

        .barberia-content {
            padding: $spacing-md;

            h4 {
                font-size: $h4-font-size;
                font-weight: $font-weight-semibold;
                color: $color-negro;
                margin: 0 0 $spacing-sm 0;

                .dark & {
                    color: $color-blanco;
                }
            }

            .rating {
                display: flex;
                align-items: center;
                gap: 4px;
                margin-bottom: $spacing-sm;
                font-size: 12px;

                span {
                    font-weight: $font-weight-semibold;
                    color: $color-negro;

                    .dark & {
                        color: $color-blanco;
                    }

                    &.reviews {
                        color: $color-gris-medio;
                        font-weight: $font-weight-regular;
                    }
                }
            }

            .address {
                font-size: $small-font-size;
                color: $color-gris-medio;
                margin: 0 0 $spacing-md 0;
                line-height: 1.4;
            }
        }
    }
}

.full-width {
    width: 100%;
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

    .barberia-carousel {
        grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
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
        gap: $spacing-md;
    }

    .reservation-item {
        flex-direction: column;
        text-align: center;

        .reservation-info {
            text-align: center;

            .reservation-meta {
                justify-content: center;
            }
        }
    }

    .barberia-carousel {
        grid-template-columns: 1fr;
    }
}
</style>

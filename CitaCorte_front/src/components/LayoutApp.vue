<template>
  <q-layout view="hHh Lpr fFf" class="citacorte-layout">
    <q-header elevated class="header-citacorte">
      <q-toolbar class="header-toolbar">
        <!-- Menu Button -->
        <q-btn flat dense round icon="menu" @click="leftDrawer = !leftDrawer" class="q-mr-md"
          aria-label="Toggle menu" />

        <!-- Logo and Brand -->
        <div class="header-brand">
          <img src="~assets/logo.png" alt="CitaCorte Logo" class="brand-logo" @error="logoError = true" />
          <div class="brand-text">
            <span class="brand-title">CitaCorte</span>
            <span class="brand-subtitle">Sistema de Reservas</span>
          </div>
        </div>

        <q-space />

        <!-- Notifications -->
        <NotificationsBell v-if="auth.isLoggedIn" class="q-mr-md" />

        <!-- User Menu -->
        <div v-if="auth.isLoggedIn" class="user-menu">
          <q-btn flat round>
            <q-avatar :name="auth.userName || 'U'" color="primary" text-color="dark" size="40px" />
            <q-menu anchor="bottom right" self="top right">
              <q-list style="min-width: 200px">
                <q-item>
                  <q-item-section>
                    <q-item-label class="text-weight-bold">
                      {{ auth.userName || 'Usuario' }}
                    </q-item-label>
                    <q-item-label caption>{{ auth.userRole || 'N/A' }}</q-item-label>
                  </q-item-section>
                </q-item>
                <q-separator />
                <q-item clickable to="/barberia/perfil" v-ripple>
                  <q-item-section avatar>
                    <q-icon name="person" color="primary" />
                  </q-item-section>
                  <q-item-section>Perfil</q-item-section>
                </q-item>
                <q-item clickable to="/configuracion" v-ripple>
                  <q-item-section avatar>
                    <q-icon name="settings" color="primary" />
                  </q-item-section>
                  <q-item-section>Configuración</q-item-section>
                </q-item>
                <q-separator />
                <q-item clickable @click="logout" v-ripple>
                  <q-item-section avatar>
                    <q-icon name="logout" color="negative" />
                  </q-item-section>
                  <q-item-section>Salir</q-item-section>
                </q-item>
              </q-list>
            </q-menu>
          </q-btn>
        </div>

        <!-- Login Button -->
        <q-btn v-else label="Iniciar sesión" to="/login" unelevated class="bg-primary text-dark" />

        <!-- Theme Toggle -->
        <q-btn flat dense round :icon="$q.dark.isActive ? 'light_mode' : 'dark_mode'" @click="$q.dark.toggle()"
          class="q-ml-md" aria-label="Toggle theme" />
      </q-toolbar>
    </q-header>

    <!-- Navigation Drawer -->
    <q-drawer v-model="leftDrawer" side="left" bordered :width="280" :breakpoint="1024" class="drawer-citacorte">
      <q-list padding class="navigation-list">
        <q-item-label header class="text-uppercase">
          <span class="text-weight-bold">Menú</span>
        </q-item-label>

       <template v-for="section in menuSections" :key="section.title">
  <template v-if="section.items?.some(item => item?.roles?.includes(auth.userRole ?? ''))">
    <q-item-label header class="section-label">
      {{ section.title }}
    </q-item-label>

    <!-- Usamos un template con v-for y dentro el q-item sin v-if -->
    <template v-for="item in section.items" :key="item.to ?? item.title">
      <q-item
        v-if="item?.roles?.includes(auth.userRole ?? '')"
        clickable
        :to="item.to"
        :inset-level="0.5"
        class="nav-item"
        active-class="nav-item--active"
      >
        <q-item-section avatar>
          <q-icon :name="item.icon" />
        </q-item-section>
        <q-item-section>
          <q-item-label>{{ item.title }}</q-item-label>
        </q-item-section>
        <q-item-section side v-if="item.badge" top>
          <q-badge color="primary" floating>
            {{ item.badge }}
          </q-badge>
        </q-item-section>
      </q-item>
    </template>

    <q-separator v-if="section !== menuSections[menuSections.length - 1]" class="q-my-md" />
  </template>
</template>
      </q-list>
    </q-drawer>

    <!-- Page Container -->
    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useAuthStore } from 'src/stores/auth'
import { useQuasar } from 'quasar'
import NotificationsBell from './NotificationsBell.vue'

const $q = useQuasar()
const auth = useAuthStore()
const leftDrawer = ref(false)
const logoError = ref(false)

const menuSections = [
  {
    title: 'Cliente',
    items: [
      {
        title: 'Inicio',
        icon: 'home',
        to: '/cliente/dashboard',
        roles: ['Cliente'],
        badge: null
      },
      {
        title: 'Buscar Barberas',
        icon: 'search',
        to: '/cliente/buscar',
        roles: ['Cliente'],
        badge: null
      },
      {
        title: 'Mis Reservas',
        icon: 'calendar_today',
        to: '/cliente/reservas',
        roles: ['Cliente'],
        badge: null
      },
      {
        title: 'Historial',
        icon: 'history',
        to: '/cliente/historial',
        roles: ['Cliente'],
        badge: null
      }
    ]
  },
  {
    title: 'Barbero',
    items: [
      {
        title: 'Panel',
        icon: 'dashboard',
        to: '/barbero/dashboard',
        roles: ['Barbero'],
        badge: null
      },
      {
        title: 'Servicios',
        icon: 'content_cut',
        to: '/barbero/servicios',
        roles: ['Barbero'],
        badge: null
      },
      {
        title: 'Productos',
        icon: 'storefront',
        to: '/barbero/productos',
        roles: ['Barbero'],
        badge: null
      },
      {
        title: 'Reservas',
        icon: 'calendar_today',
        to: '/barbero/reservas',
        roles: ['Barbero'],
        badge: null
      },
      {
        title: 'Estadísticas',
        icon: 'bar_chart',
        to: '/barbero/estadisticas',
        roles: ['Barbero'],
        badge: null
      },
      {
        title: 'Afiliaciones',
        icon: 'link',
        to: '/barbero/afiliaciones',
        roles: ['Barbero'],
        badge: null
      },
      {
        title: 'Cambiar Plan',
        icon: 'upgrade',
        to: '/barbero/cambiar-plan',
        roles: ['Barbero'],
        badge: null
      }
    ]
  },
  {
    title: 'Barbería',
    items: [
      {
        title: 'Panel',
        icon: 'dashboard',
        to: '/barberia/dashboard',
        roles: ['Barberia'],
        badge: null
      },
      {
        title: 'Servicios',
        icon: 'content_cut',
        to: '/barberia/servicios',
        roles: ['Barberia'],
        badge: null
      },
      {
        title: 'Reservas',
        icon: 'calendar_today',
        to: '/barberia/reservas',
        roles: ['Barberia'],
        badge: null
      },
      {
        title: 'Barberos Afiliados',
        icon: 'group',
        to: '/barberia/afiliados',
        roles: ['Barberia'],
        badge: null
      },
      {
        title: 'Estadísticas',
        icon: 'bar_chart',
        to: '/barberia/estadisticas',
        roles: ['Barberia'],
        badge: null
      },
      {
        title: 'Cambiar Plan',
        icon: 'upgrade',
        to: '/barberia/cambiar-plan',
        roles: ['Barberia'],
        badge: null
      }
    ]
  },
  {
    title: 'Administración',
    items: [
      {
        title: 'Panel Admin',
        icon: 'admin_panel_settings',
        to: '/admin/dashboard',
        roles: ['Admin'],
        badge: null
      },
      {
        title: 'Usuarios',
        icon: 'people',
        to: '/admin/usuarios',
        roles: ['Admin'],
        badge: null
      },
      {
        title: 'Planes de Suscripción',
        icon: 'card_membership',
        to: '/admin/planes',
        roles: ['Admin'],
        badge: null
      },
      {
        title: 'Solicitudes de Cambio',
        icon: 'mail',
        to: '/admin/solicitudes-cambio',
        roles: ['Admin'],
        badge: null
      },
      {
        title: 'Barbería Registros',
        icon: 'storefront',
        to: '/admin/registro-barberias',
        roles: ['Admin'],
        badge: null
      }
    ]
  },
  {
    title: 'Comercial',
    items: [
      {
        title: 'Panel',
        icon: 'dashboard',
        to: '/comercial/dashboard',
        roles: ['Comercial'],
        badge: null
      },
      {
        title: 'Registro de Barberias',
        icon: 'storefront',
        to: '/comercial/registro-barberias',
        roles: ['Comercial'],
        badge: null
      },
      {
        title: 'Solicitudes de Cambio',
        icon: 'mail',
        to: '/comercial/solicitudes',
        roles: ['Comercial'],
        badge: null
      }
    ]
  }
]

const logout = () => {
  auth.logout()
}
</script>

<style scoped lang="scss">
@import 'src/assets/theme/citacorte-design-system.scss';

.citacorte-layout {
  background-color: $color-blanco;

  &.dark {
    background-color: #1A1A1A;
  }
}

.header-citacorte {
  background-color: $color-blanco;
  border-bottom: 1px solid #E5E5E5;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);

  &.dark {
    background-color: #2D2D2D;
    border-bottom-color: #404040;
  }
}

.header-toolbar {
  padding: $spacing-md $spacing-lg;
  min-height: 64px;
  display: flex;
  align-items: center;
  gap: $spacing-md;
}

.header-brand {
  display: flex;
  align-items: center;
  gap: $spacing-md;
  min-width: 200px;

  .brand-logo {
    height: 40px;
    width: auto;
    object-fit: contain;
  }

  .brand-text {
    display: flex;
    flex-direction: column;
    line-height: 1;

    .brand-title {
      font-size: $h3-font-size;
      font-weight: $font-weight-bold;
      color: $color-dorado;
      letter-spacing: -0.5px;
    }

    .brand-subtitle {
      font-size: 11px;
      color: $color-gris-medio;
      font-weight: $font-weight-regular;
      text-transform: uppercase;
      letter-spacing: 0.5px;
    }
  }
}

.user-menu {
  display: flex;
  align-items: center;
}

.drawer-citacorte {
  background-color: $color-blanco;
  border-right: 1px solid #E5E5E5;

  &.dark {
    background-color: #2D2D2D;
    border-right-color: #404040;
  }
}

.navigation-list {
  padding: $spacing-md 0;

  .section-label {
    padding: $spacing-md $spacing-lg;
    font-size: 11px;
    font-weight: $font-weight-bold;
    text-transform: uppercase;
    letter-spacing: 1px;
    color: $color-gris-oscuro;
    margin-top: $spacing-md;

    .dark & {
      color: $color-gris-medio;
    }

    &:first-child {
      margin-top: 0;
    }
  }

  .nav-item {
    margin: $spacing-xs $spacing-sm;
    border-radius: $border-radius-md;
    transition: all $transition-base;
    color: $color-gris-oscuro;

    &:hover {
      background-color: rgba($color-dorado, 0.08);
      color: $color-dorado;

      .q-item__section--avatar {
        color: $color-dorado;
      }
    }

    &.nav-item--active {
      background-color: rgba($color-dorado, 0.12);
      color: $color-dorado;
      font-weight: $font-weight-semibold;

      .q-item__section--avatar {
        color: $color-dorado;
      }
    }
  }
}

@media (max-width: 1024px) {
  .header-brand .brand-subtitle {
    display: none;
  }
}

@media (max-width: 600px) {
  .header-toolbar {
    padding: $spacing-sm;
    min-height: 56px;
  }

  .header-brand {
    min-width: auto;
    gap: $spacing-sm;

    .brand-logo {
      height: 32px;
    }

    .brand-text .brand-title {
      font-size: 18px;
    }
  }
}
</style>

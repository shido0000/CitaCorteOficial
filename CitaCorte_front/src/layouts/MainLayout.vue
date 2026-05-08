<template>
    <q-layout view="hHh Lpr lff" id="mainLayout" class="citacorte-layout">
        <q-header class="header-citacorte">
            <q-toolbar class="header-toolbar">
                <div class="header-brand row">
                    <q-btn size="md" class="menu-btn" flat @click="drawer = !drawer" round icon="menu">
                        <q-tooltip>{{ drawer ? "Cerrar Menú" : "Abrir Menú" }}</q-tooltip>
                    </q-btn>
                    <q-toolbar-title class="brand-title text-dorado">
                        CitaCorte
                    </q-toolbar-title>
                </div>
                <q-space />
                <q-btn class="action-btn" flat round dense icon="group_add" color="dorado" />
            </q-toolbar>
        </q-header>

        <q-drawer v-model="drawer" show-if-above :width="275" :breakpoint="500" class="drawer-citacorte">
            <q-scroll-area class="fit">
                <q-list padding class="menu-list">
                    <EssentialLink />
                </q-list>
            </q-scroll-area>
        </q-drawer>
        
        <q-page-container>
            <router-view />
        </q-page-container>
    </q-layout>
</template>

<script setup>
import { ref } from 'vue'
import EssentialLink from 'src/components/EssentialLink.vue'


defineOptions({
  name: 'MainLayout'
})

const drawer = ref(false);

</script>

<style scoped lang="scss">
@import 'src/assets/theme/citacorte-design-system.scss';

.citacorte-layout {
  background-color: $color-beige-claro;
  
  .dark & {
    background-color: #1a1a1a;
  }
}

.header-citacorte {
  background-color: $color-blanco;
  border-bottom: 1px solid rgba($color-dorado, 0.2);
  box-shadow: $shadow-sm;
  
  .dark & {
    background-color: $color-gris-oscuro;
    border-bottom-color: rgba($color-dorado, 0.3);
  }
}

.header-toolbar {
  padding: $spacing-md $spacing-lg;
  min-height: 64px;
  display: flex;
  align-items: center;
}

.header-brand {
  display: flex;
  align-items: center;
  gap: $spacing-md;
  
  .menu-btn {
    color: $color-dorado;
    
    &:hover {
      background-color: rgba($color-dorado, 0.1);
    }
  }
  
  .brand-title {
    font-size: $h2-font-size;
    font-weight: $font-weight-bold;
    letter-spacing: -0.5px;
  }
}

.action-btn {
  color: $color-dorado;
  
  &:hover {
    background-color: rgba($color-dorado, 0.1);
  }
}

.drawer-citacorte {
  background-color: $color-blanco;
  border-right: 1px solid rgba($color-dorado, 0.15);
  
  .dark & {
    background-color: $color-gris-oscuro;
    border-right-color: rgba($color-dorado, 0.2);
  }
}

.menu-list {
  padding: $spacing-md 0;
}
</style>


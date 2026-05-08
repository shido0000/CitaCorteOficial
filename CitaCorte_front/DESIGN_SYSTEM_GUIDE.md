# CitaCorte Design System Guide

## Sistema de Diseño - Manual de Identidad CitaCorte

Este documento describe la implementación del sistema de diseño profesional para la aplicación CitaCorte, basado en el Manual de Identidad Visual corporativo.

---

## 🎨 Paleta de Colores

### Colores Principales (40% + 30% + 20% + 10%)

| Color           | Código    | Uso                                           |
| --------------- | --------- | --------------------------------------------- |
| **Dorado**      | `#D4AF37` | Elemento principal, botones, highlights (40%) |
| **Negro**       | `#0D0D0D` | Textos principales, contrastes fuertes (30%)  |
| **Blanco**      | `#FFFFFF` | Fondos, espacios en blanco (20%)              |
| **Gris Oscuro** | `#333333` | Textos secundarios (10%)                      |

### Colores Secundarios

| Color           | Código    | Uso                          |
| --------------- | --------- | ---------------------------- |
| **Azul Oscuro** | `#1E2A37` | Elementos secundarios        |
| **Gris Medio**  | `#6B7280` | Textos deshabilitados, hints |
| **Beige**       | `#F0E6D2` | Superficies alternativas     |
| **Beige Claro** | `#FAF7F2` | Fondos secundarios           |
| **Café**        | `#5A3E2B` | Acentos, detalles            |

### Colores de Estado

| Estado          | Código    | Propósito             |
| --------------- | --------- | --------------------- |
| **Éxito**       | `#43A047` | Operaciones exitosas  |
| **Error**       | `#E53935` | Errores, validaciones |
| **Advertencia** | `#FB8C00` | Alertas, precaución   |
| **Información** | `#1976D2` | Mensajes informativos |

---

## 📝 Tipografía

### Familia Tipográfica: Poppins

Todas las fuentes son **Poppins** con diferentes pesos:

```
100 - Thin
200 - ExtraLight
300 - Light
400 - Regular
500 - Medium
600 - SemiBold
700 - Bold
800 - ExtraBold
```

### Jerarquía Tipográfica

```scss
H1: Poppins Bold - 32px / 40px line-height
H2: Poppins SemiBold - 24px / 32px line-height
H3: Poppins Medium - 20px / 28px line-height
H4: Poppins Medium - 16px / 24px line-height
Body: Poppins Regular - 14px / 22px line-height
Small: Poppins Regular - 12px / 18px line-height
```

### Uso de Tipografía

-   **Títulos (H1, H2)**: Poppins SemiBold o Bold
-   **Subtítulos (H3, H4)**: Poppins Medium o SemiBold
-   **Cuerpo de Texto**: Poppins Regular o Light
-   **Etiquetas**: Poppins Medium, uppercase, letter-spacing

---

## 🌓 Temas: Modo Claro y Oscuro

### Modo Claro (Predeterminado)

```scss
// Fondo
Background: #FFFFFF
Surface: #FAF7F2
Surface Variant: #F0E6D2

// Texto
Text Primary: #0D0D0D
Text Secondary: #333333
Text Tertiary: #6B7280

// Bordes y Divisores
Border: #E5E5E5
Divider: #D9D9D9
```

### Modo Oscuro

```scss
// Fondo
Background: #1A1A1A
Surface: #2D2D2D
Surface Variant: #3D3D3D

// Texto
Text Primary: #FFFFFF
Text Secondary: #B0B0B0
Text Tertiary: #6B7280

// Bordes y Divisores
Border: #404040
Divider: #2C2C2C
```

---

## 🎯 Espaciado

Sistema de espaciado en múltiplos de 8px:

```scss
$spacing-xs:  4px
$spacing-sm:  8px
$spacing-md:  16px
$spacing-lg:  24px
$spacing-xl:  32px
$spacing-xxl: 48px
```

---

## 🔲 Bordes y Esquinas

```scss
$border-radius-none:  0
$border-radius-sm:    4px
$border-radius-md:    8px
$border-radius-lg:    12px
$border-radius-xl:    16px
$border-radius-full:  9999px
```

---

## 🌟 Sombras

```scss
$shadow-xs:   0 1px 2px rgba(0, 0, 0, 0.05)
$shadow-sm:   0 1px 3px rgba(0, 0, 0, 0.1), 0 1px 2px rgba(0, 0, 0, 0.06)
$shadow-md:   0 4px 6px rgba(0, 0, 0, 0.1), 0 2px 4px rgba(0, 0, 0, 0.06)
$shadow-lg:   0 10px 15px rgba(0, 0, 0, 0.1), 0 4px 6px rgba(0, 0, 0, 0.05)
$shadow-xl:   0 20px 25px rgba(0, 0, 0, 0.1), 0 10px 10px rgba(0, 0, 0, 0.04)
$shadow-2xl:  0 25px 50px rgba(0, 0, 0, 0.15)

// Sombra dorada especial
$shadow-golden: 0 8px 16px rgba(212, 175, 55, 0.15)
```

---

## ⚡ Transiciones

```scss
$transition-fast:  150ms cubic-bezier(0.4, 0, 0.2, 1)
$transition-base:  250ms cubic-bezier(0.4, 0, 0.2, 1)
$transition-slow:  350ms cubic-bezier(0.4, 0, 0.2, 1)
```

---

## 📱 Breakpoints

```scss
$breakpoint-xs: 0
$breakpoint-sm: 600px
$breakpoint-md: 1024px
$breakpoint-lg: 1440px
$breakpoint-xl: 1920px
```

---

## 🧩 Componentes Reutilizables

### StatCard

Muestra estadísticas con iconos, valores y cambios porcentuales.

```vue
<StatCard
    value="1,234"
    label="Reservas"
    icon="event"
    color="primary"
    :change="15"
    change-type="positive"
/>
```

### ActionCard

Tarjeta de acción con ícono, título y descripción.

```vue
<ActionCard
    title="Buscar Barberias"
    description="Encuentra barberias cercanas"
    icon="search"
    to="/cliente/buscar"
/>
```

### HeroSection

Sección hero con título, subtítulo e imagen.

```vue
<HeroSection title="¡Bienvenido!" subtitle="Subtitle text">
  <template #actions>
    <q-btn label="Action" />
  </template>
  <template #image>
    <img src="..." />
  </template>
</HeroSection>
```

---

## 🎨 Clases Utilitarias

### Espaciado

```html
<!-- Margin top -->
<div class="mt-xs mt-sm mt-md mt-lg mt-xl mt-xxl">
    <!-- Margin bottom -->
    <div class="mb-xs mb-sm mb-md mb-lg mb-xl mb-xxl">
        <!-- Padding X (left-right) -->
        <div class="px-xs px-sm px-md px-lg px-xl">
            <!-- Padding Y (top-bottom) -->
            <div class="py-xs py-sm py-md py-lg py-xl"></div>
        </div>
    </div>
</div>
```

### Texto

```html
<!-- Colores -->
<span class="text-primary">Dorado</span>
<span class="text-secondary">Azul oscuro</span>
<span class="text-accent">Café</span>
<span class="text-muted">Gris</span>

<!-- Pesos -->
<span class="font-light">Light</span>
<span class="font-regular">Regular</span>
<span class="font-medium">Medium</span>
<span class="font-semibold">SemiBold</span>
<span class="font-bold">Bold</span>
```

### Fondos

```html
<div class="bg-primary">Dorado</div>
<div class="bg-secondary">Azul</div>
<div class="bg-accent">Café</div>
<div class="bg-light">Beige</div>
```

### Sombras

```html
<div class="shadow-xs"></div>
<div class="shadow-sm"></div>
<div class="shadow-md"></div>
<div class="shadow-lg"></div>
<div class="shadow-xl"></div>
<div class="shadow-2xl"></div>
```

### Bordes

```html
<div class="rounded-none"></div>
<div class="rounded-sm"></div>
<div class="rounded-md"></div>
<div class="rounded-lg"></div>
<div class="rounded-xl"></div>
<div class="rounded-full"></div>
```

### Flex

```html
<div class="flex-center"></div>
<!-- centrado -->
<div class="flex-between"></div>
<!-- espacio entre -->
<div class="flex-col"></div>
<!-- flex column -->
```

---

## 🎪 Guías de Contraste

Asegurar siempre legibilidad y contraste:

-   **Texto sobre Dorado**: Negro (#0D0D0D) - Ratio 10.4:1 ✓
-   **Texto sobre Blanco**: Negro (#0D0D0D) - Ratio 16:1 ✓
-   **Texto sobre Gris Oscuro**: Blanco (#FFFFFF) - Ratio 8.6:1 ✓
-   **Texto sobre Azul Oscuro**: Blanco (#FFFFFF) - Ratio 8.2:1 ✓

---

## 🔄 Modo Oscuro - Alternancia

Para habilitar el cambio entre modo claro y oscuro:

```vue
<q-btn
    flat
    dense
    round
    :icon="$q.dark.isActive ? 'light_mode' : 'dark_mode'"
    @click="$q.dark.toggle()"
/>
```

Las clases CSS se aplican automáticamente con `.dark`

---

## 📐 Ejemplo de Estructura de Página

```vue
<template>
    <q-page class="page-container">
        <!-- Hero Section -->
        <HeroSection title="Título" subtitle="Subtítulo" />

        <!-- Sección con título -->
        <div class="page-section">
            <h2 class="section-title">Sección</h2>
            <div class="stats-grid">
                <StatCard ... />
            </div>
        </div>
    </q-page>
</template>

<style scoped lang="scss">
@import "src/assets/theme/citacorte-design-system.scss";

.page-container {
    max-width: 1400px;
    margin: 0 auto;
    padding: $spacing-xl;
}

.page-section {
    display: flex;
    flex-direction: column;
    gap: $spacing-lg;
    margin-bottom: $spacing-xxl;
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
</style>
```

---

## 🚀 Mejores Prácticas

1. **Siempre importar el design system** en archivos de estilos:

    ```scss
    @import "src/assets/theme/citacorte-design-system.scss";
    ```

2. **Usar variables SCSS** en lugar de valores hardcodeados:

    ```scss
    // ✓ Bien
    color: $color-dorado;
    padding: $spacing-md;
    border-radius: $border-radius-lg;

    // ✗ Mal
    color: #d4af37;
    padding: 16px;
    border-radius: 12px;
    ```

3. **Respetar la jerarquía tipográfica**:

    - Usar h1 para títulos principales
    - h2 para subtítulos de secciones
    - h3/h4 para elementos dentro de secciones
    - body para texto general

4. **Aplicar transiciones smooth**:

    ```scss
    transition: all $transition-base;
    ```

5. **Mantener consistencia en espaciado**:

    - Usar siempre múltiplos del sistema de espaciado
    - Evitar valores arbitrarios

6. **Considerar responsive design**:

    - Usar `grid-template-columns: repeat(auto-fit, minmax(250px, 1fr))`
    - Implementar media queries para breakpoints definidos

7. **Garantizar accesibilidad**:
    - Verificar ratios de contraste WCAG AA
    - Usar `aria-label` en botones de acciones
    - Mantener focus visible en elementos interactivos

---

## 📦 Archivos del Sistema

-   `src/assets/theme/citacorte-design-system.scss` - Variables y configuración
-   `src/css/quasar.variables.scss` - Integración con Quasar
-   `src/css/app.scss` - Estilos globales y componentes
-   `src/components/StatCard.vue` - Componente de estadísticas
-   `src/components/ActionCard.vue` - Componente de acciones
-   `src/components/HeroSection.vue` - Componente hero

---

## 🔗 Recursos

-   [Manual de Identidad CitaCorte](../Manual%20de%20Identidad%20CitaCorte)
-   [Poppins Font Family](https://fonts.google.com/specimen/Poppins)
-   [Quasar Framework Docs](https://quasar.dev)
-   [WCAG Color Contrast](https://webaim.org/articles/contrast/)

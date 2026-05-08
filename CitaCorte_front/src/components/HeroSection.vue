<template>
    <div class="hero-section">
        <div class="hero-content">
            <h1 class="hero-title">{{ title }}</h1>
            <p v-if="subtitle" class="hero-subtitle">{{ subtitle }}</p>
            <div v-if="$slots.actions" class="hero-actions">
                <slot name="actions" />
            </div>
        </div>
        <div v-if="$slots.image" class="hero-image">
            <slot name="image" />
        </div>
    </div>
</template>

<script setup>
defineProps({
    title: {
        type: String,
        required: true
    },
    subtitle: {
        type: String,
        default: null
    }
})
</script>

<style scoped lang="scss">
@import 'src/assets/theme/citacorte-design-system.scss';

.hero-section {
    display: grid;
    grid-template-columns: 1fr 1fr;
    align-items: center;
    gap: $spacing-xxl;
    background: linear-gradient(135deg, $color-dorado 0%, darken($color-dorado, 5%) 100%);
    border-radius: $border-radius-lg;
    padding: $spacing-xxl $spacing-xl;
    color: $color-negro;
    position: relative;
    overflow: hidden;

    &::before {
        content: '';
        position: absolute;
        top: -50%;
        right: -10%;
        width: 300px;
        height: 300px;
        background-color: rgba($color-negro, 0.05);
        border-radius: 50%;
    }

    &::after {
        content: '';
        position: absolute;
        bottom: -30%;
        left: -5%;
        width: 250px;
        height: 250px;
        background-color: rgba($color-negro, 0.03);
        border-radius: 50%;
    }
}

.hero-content {
    position: relative;
    z-index: 1;
}

.hero-title {
    font-size: $h1-font-size;
    font-weight: $font-weight-bold;
    margin-bottom: $spacing-md;
    color: $color-negro;
    line-height: 1.2;
    letter-spacing: -1px;
}

.hero-subtitle {
    font-size: $h3-font-size;
    font-weight: $font-weight-regular;
    margin-bottom: $spacing-lg;
    color: rgba($color-negro, 0.8);
    line-height: 1.6;
}

.hero-actions {
    display: flex;
    gap: $spacing-md;
    flex-wrap: wrap;
}

.hero-image {
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    z-index: 1;
}

@media (max-width: 1024px) {
    .hero-section {
        grid-template-columns: 1fr;
        padding: $spacing-xl $spacing-lg;
        gap: $spacing-lg;
    }

    .hero-title {
        font-size: 28px;
    }

    .hero-subtitle {
        font-size: $h4-font-size;
    }
}

@media (max-width: 600px) {
    .hero-section {
        padding: $spacing-lg;
        gap: $spacing-md;

        &::before,
        &::after {
            display: none;
        }
    }

    .hero-title {
        font-size: 24px;
    }

    .hero-subtitle {
        font-size: $body-font-size;
    }

    .hero-actions {
        flex-direction: column;
        width: 100%;

        :deep(.q-btn) {
            width: 100%;
        }
    }
}
</style>

<template>
    <div class="action-card" @click="navigate">
        <div class="action-icon">
            <q-icon :name="icon" />
        </div>
        <div class="action-body">
            <h4 class="action-title">{{ title }}</h4>
            <p class="action-description">{{ description }}</p>
        </div>
        <div class="action-arrow">
            <q-icon name="arrow_forward" />
        </div>
    </div>
</template>

<script setup>
import { useRouter } from 'vue-router'

const router = useRouter()

const props = defineProps({
    title: {
        type: String,
        required: true
    },
    description: {
        type: String,
        required: true
    },
    icon: {
        type: String,
        required: true
    },
    to: {
        type: String,
        default: null
    },
    onClick: {
        type: Function,
        default: null
    }
})

const navigate = () => {
    if (props.onClick) {
        props.onClick()
    } else if (props.to) {
        router.push(props.to)
    }
}
</script>

<style scoped lang="scss">
@import 'src/assets/theme/citacorte-design-system.scss';

.action-card {
    display: flex;
    align-items: center;
    gap: $spacing-lg;
    background-color: $color-blanco;
    border: 2px solid #E5E5E5;
    border-radius: $border-radius-lg;
    padding: $spacing-lg;
    transition: all $transition-base;
    cursor: pointer;
    position: relative;
    overflow: hidden;

    .dark & {
        background-color: #2D2D2D;
        border-color: #404040;
    }

    &::before {
        content: '';
        position: absolute;
        top: 0;
        left: -100%;
        width: 100%;
        height: 100%;
        background: linear-gradient(90deg, transparent, rgba($color-dorado, 0.1), transparent);
        transition: left $transition-base;
    }

    &:hover {
        border-color: $color-dorado;
        box-shadow: 0 8px 16px rgba($color-dorado, 0.15);
        transform: translateX(4px);

        &::before {
            left: 100%;
        }

        .action-arrow {
            transform: translateX(4px);
            color: $color-dorado;
        }
    }

    .action-icon {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 56px;
        height: 56px;
        border-radius: $border-radius-md;
        background: linear-gradient(135deg, rgba($color-dorado, 0.1) 0%, rgba($color-dorado, 0.05) 100%);
        color: $color-dorado;
        flex-shrink: 0;

        :deep(.q-icon) {
            font-size: 28px;
        }
    }

    .action-body {
        flex: 1;
        position: relative;
        z-index: 1;
    }

    .action-title {
        font-size: $h4-font-size;
        font-weight: $font-weight-semibold;
        color: $color-negro;
        margin-bottom: $spacing-xs;

        .dark & {
            color: $color-blanco;
        }
    }

    .action-description {
        font-size: $small-font-size;
        color: $color-gris-medio;
        line-height: 1.5;
        margin: 0;
    }

    .action-arrow {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 32px;
        height: 32px;
        border-radius: $border-radius-md;
        background-color: rgba($color-dorado, 0.1);
        color: $color-gris-medio;
        flex-shrink: 0;
        transition: all $transition-base;

        :deep(.q-icon) {
            font-size: 20px;
        }
    }
}
</style>

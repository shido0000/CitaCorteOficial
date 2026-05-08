<template>
    <div class="stat-card">
        <div class="stat-content">
            <div class="stat-icon" v-if="icon">
                <q-icon :name="icon" :color="color" />
            </div>
            <div class="stat-body">
                <div class="stat-value">{{ value }}</div>
                <div class="stat-label">{{ label }}</div>
                <div class="stat-change" v-if="change" :class="changeColor">
                    <q-icon :name="changeIcon" size="xs" />
                    <span>{{ change }}%</span>
                </div>
            </div>
        </div>
        <div v-if="$slots.default" class="stat-footer">
            <slot />
        </div>
    </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
    value: {
        type: [String, Number],
        required: true
    },
    label: {
        type: String,
        required: true
    },
    icon: {
        type: String,
        default: null
    },
    color: {
        type: String,
        default: 'primary'
    },
    change: {
        type: [String, Number],
        default: null
    },
    changeType: {
        type: String,
        enum: ['positive', 'negative', 'neutral'],
        default: 'neutral'
    }
})

const changeIcon = computed(() => {
    return props.changeType === 'positive'
        ? 'trending_up'
        : props.changeType === 'negative'
            ? 'trending_down'
            : 'trending_flat'
})

const changeColor = computed(() => {
    return {
        'text-positive': props.changeType === 'positive',
        'text-negative': props.changeType === 'negative',
        'text-info': props.changeType === 'neutral'
    }
})
</script>

<style scoped lang="scss">
@import 'src/assets/theme/citacorte-design-system.scss';

.stat-card {
    background: linear-gradient(135deg, rgba($color-dorado, 0.05) 0%, rgba($color-dorado, 0) 100%);
    border: 1px solid rgba($color-dorado, 0.2);
    border-radius: $border-radius-lg;
    padding: $spacing-lg;
    transition: all $transition-base;
    cursor: pointer;

    &:hover {
        border-color: rgba($color-dorado, 0.5);
        box-shadow: 0 8px 16px rgba($color-dorado, 0.1);
        transform: translateY(-4px);
    }

    .stat-content {
        display: flex;
        align-items: flex-start;
        gap: $spacing-md;
    }

    .stat-icon {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 48px;
        height: 48px;
        border-radius: $border-radius-md;
        background-color: rgba($color-dorado, 0.1);
        flex-shrink: 0;

        :deep(.q-icon) {
            font-size: 24px;
        }
    }

    .stat-body {
        flex: 1;
    }

    .stat-value {
        font-size: 28px;
        font-weight: $font-weight-bold;
        color: $color-negro;
        line-height: 1;
        margin-bottom: $spacing-sm;

        .dark & {
            color: $color-blanco;
        }
    }

    .stat-label {
        font-size: 12px;
        font-weight: $font-weight-medium;
        color: $color-gris-medio;
        text-transform: uppercase;
        letter-spacing: 0.5px;
        margin-bottom: $spacing-sm;
    }

    .stat-change {
        display: inline-flex;
        align-items: center;
        gap: 4px;
        font-size: 12px;
        font-weight: $font-weight-medium;

        &.text-positive {
            color: #43A047;
        }

        &.text-negative {
            color: #E53935;
        }

        &.text-info {
            color: $color-gris-medio;
        }
    }

    .stat-footer {
        margin-top: $spacing-md;
        padding-top: $spacing-md;
        border-top: 1px solid rgba($color-dorado, 0.1);
    }
}
</style>

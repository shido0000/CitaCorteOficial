<template>
    <div class="login-page">
        <div class="login-container">
            <!-- Left Side - Branding -->
            <div class="login-brand">
                <div class="brand-content">
                    <img src="~assets/logo.png" alt="CitaCorte" class="brand-logo" />
                    <h1 class="brand-title">CitaCorte</h1>
                    <p class="brand-tagline">Sistema de Reservas Premium</p>

                    <div class="brand-features q-mt-xl">
                        <div class="feature">
                            <q-icon name="check_circle" color="primary" />
                            <span>Reservas en tiempo real</span>
                        </div>
                        <div class="feature">
                            <q-icon name="check_circle" color="primary" />
                            <span>Gestión profesional</span>
                        </div>
                        <div class="feature">
                            <q-icon name="check_circle" color="primary" />
                            <span>Análisis y estadísticas</span>
                        </div>
                        <div class="feature">
                            <q-icon name="check_circle" color="primary" />
                            <span>Soporte 24/7</span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Right Side - Login Form -->
            <div class="login-form">
                <div class="form-container">
                    <h2 class="form-title">Inicia Sesión</h2>
                    <p class="form-subtitle">Accede a tu cuenta CitaCorte</p>

                    <q-form @submit="login" class="q-mt-lg">
                        <!-- username Input -->
                        <div class="form-group">
                            <label for="username" class="input-label">Usuario</label>
                            <q-input id="username" v-model="username" type="text" outlined dense placeholder="Usuario"
                                :rules="[val => !!val || 'El usuario es requerido']" class="q-mb-sm">
                                <template #prepend>
                                    <q-icon name="person" />
                                </template>
                            </q-input>
                        </div>

                        <!-- Password Input -->
                        <div class="form-group">
                            <label for="password" class="input-label">Contraseña</label>
                            <q-input id="password" v-model="password" :type="showPassword ? 'text' : 'password'"
                                outlined dense placeholder="Ingresa tu contraseña"
                                :rules="[val => !!val || 'La contraseña es requerida']" class="q-mb-sm">
                                <template #prepend>
                                    <q-icon name="lock" />
                                </template>
                                <template #append>
                                    <q-icon :name="showPassword ? 'visibility' : 'visibility_off'"
                                        class="cursor-pointer" @click="showPassword = !showPassword" />
                                </template>
                            </q-input>
                        </div>

                        <!-- Remember Me & Forgot Password -->
                        <div class="forgot-container q-mb-lg">
                            <q-checkbox v-model="rememberMe" label="Recuérdame" color="primary" />
                            <router-link to="/forgot-password" class="forgot-link">
                                ¿Olvidaste tu contraseña?
                            </router-link>
                        </div>

                        <!-- Submit Button -->
                        <q-btn type="submit" label="Inicia Sesión" color="primary" unelevated size="lg"
                            class="full-width q-mb-md" :loading="loading" />
                    </q-form>

                    <!-- Divider -->
                    <div class="divider q-my-lg">
                        <span>¿No tienes cuenta?</span>
                    </div>

                    <!-- Register Options -->
                    <div class="register-options q-mb-lg">
                        <router-link to="/registro/cliente" class="register-btn">
                            <q-icon name="person" />
                            <span>Registro Cliente</span>
                        </router-link>
                        <router-link to="/registro/barbero" class="register-btn">
                            <q-icon name="content_cut" />
                            <span>Registro Barbero</span>
                        </router-link>
                        <router-link to="/registro/barberia" class="register-btn">
                            <q-icon name="storefront" />
                            <span>Registro Barbería</span>
                        </router-link>
                    </div>

                    <!-- Terms and Privacy -->
                    <p class="terms-text">
                        Al iniciar sesión, aceptas nuestros
                        <router-link to="/terms">Términos de Servicio</router-link>
                        y
                        <router-link to="/privacy">Política de Privacidad</router-link>
                    </p>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from 'src/stores/auth'
import { useQuasar } from 'quasar'

const router = useRouter()
const auth = useAuthStore()
const username = ref('')
const password = ref('')
const rememberMe = ref(false)
const showPassword = ref(false)
const loading = ref(false)
const $q = useQuasar()

const getDashboardByRole = (role) => {
    switch (role) {
        case 'Admin': return '/admin/dashboard'
        case 'Barbero': return '/barbero/dashboard'
        case 'Barberia': return '/barberia/dashboard'
        case 'Comercial': return '/comercial/dashboard'
        case 'Cliente': return '/cliente/dashboard'
        default: return '/'
    }
}

const login = async () => {

    loading.value = true
    try {
        await auth.login({
            username: username.value,
            contrasenna: password.value,
            rememberMe: rememberMe.value
        })
        // Redirigir al dashboard correspondiente según el rol
        const dashboardPath = getDashboardByRole(auth.userRole)
        router.push(dashboardPath)
    } catch (error) {
        $q.notify({
            type: 'negative',
            message: error.response.data.errorMessage
        })
        console.error('Login error:', error)
    } finally {
        loading.value = false
    }
}
</script>

<style scoped lang="scss">
@import 'src/assets/theme/citacorte-design-system.scss';

.login-page {
    background: linear-gradient(135deg, $color-beige-claro 0%, $color-blanco 100%);
    min-height: 100vh;
    display: flex;
    align-items: center;
    justify-content: center;

    .dark & {
        background: linear-gradient(135deg, #1A1A1A 0%, #2D2D2D 100%);
    }
}

.login-container {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: $spacing-xxl;
    max-width: 1200px;
    width: 100%;
    padding: $spacing-xl;

    @media (max-width: 1024px) {
        grid-template-columns: 1fr;
        gap: $spacing-lg;
        padding: $spacing-lg;
    }
}

.login-brand {
    display: flex;
    align-items: center;
    justify-content: center;

    .brand-content {
        text-align: center;
    }

    .brand-logo {
        height: 60px;
        width: auto;
        object-fit: contain;
        margin-bottom: $spacing-lg;
    }

    .brand-title {
        font-size: 40px;
        font-weight: $font-weight-bold;
        color: $color-dorado;
        margin: 0 0 $spacing-sm 0;
        letter-spacing: -1px;
    }

    .brand-tagline {
        font-size: $h4-font-size;
        color: $color-gris-medio;
        margin: 0;
        font-weight: $font-weight-regular;
    }

    .brand-features {
        display: flex;
        flex-direction: column;
        gap: $spacing-md;

        .feature {
            display: flex;
            align-items: center;
            gap: $spacing-md;
            font-size: $body-font-size;
            color: $color-negro;

            .dark & {
                color: $color-blanco;
            }

            :deep(.q-icon) {
                font-size: 24px;
            }
        }
    }

    @media (max-width: 1024px) {
        display: none;
    }
}

.login-form {
    display: flex;
    align-items: center;
    justify-content: center;

    .form-container {
        background-color: $color-blanco;
        border-radius: $border-radius-xl;
        padding: $spacing-xl $spacing-lg;
        box-shadow: $shadow-lg;
        width: 100%;
        max-width: 420px;

        .dark & {
            background-color: #2D2D2D;
        }
    }

    .form-title {
        font-size: $h2-font-size;
        font-weight: $font-weight-bold;
        color: $color-negro;
        margin: 0 0 $spacing-sm 0;

        .dark & {
            color: $color-blanco;
        }
    }

    .form-subtitle {
        font-size: $body-font-size;
        color: $color-gris-medio;
        margin: 0;
    }
}

.form-group {
    margin-bottom: $spacing-lg;

    .input-label {
        display: block;
        font-size: $small-font-size;
        font-weight: $font-weight-semibold;
        color: $color-negro;
        margin-bottom: $spacing-sm;
        text-transform: uppercase;
        letter-spacing: 0.5px;

        .dark & {
            color: $color-blanco;
        }
    }

    :deep(.q-field__control) {
        font-family: $font-family-primary;

        input {
            font-family: $font-family-primary;
        }
    }
}

.forgot-container {
    display: flex;
    align-items: center;
    justify-content: space-between;
    font-size: $small-font-size;

    :deep(.q-checkbox) {
        margin: 0;
    }

    .forgot-link {
        color: $color-dorado;
        text-decoration: none;
        font-weight: $font-weight-medium;
        transition: all $transition-base;

        &:hover {
            opacity: 0.8;
        }
    }
}

.full-width {
    width: 100%;
    padding: 12px 16px;
    font-weight: $font-weight-semibold;
    text-transform: none;
    letter-spacing: 0;
    border-radius: $border-radius-md;
}

.divider {
    position: relative;
    text-align: center;
    color: $color-gris-medio;
    font-size: $small-font-size;

    &::before,
    &::after {
        content: '';
        position: absolute;
        top: 50%;
        width: 45%;
        height: 1px;
        background-color: #E5E5E5;

        .dark & {
            background-color: #404040;
        }
    }

    &::before {
        left: 0;
    }

    &::after {
        right: 0;
    }

    span {
        background-color: $color-blanco;
        padding: 0 $spacing-sm;
        position: relative;
        z-index: 1;

        .dark & {
            background-color: #2D2D2D;
        }
    }
}

.register-options {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: $spacing-md;

    .register-btn {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: $spacing-sm;
        padding: $spacing-md;
        border: 2px solid #E5E5E5;
        border-radius: $border-radius-md;
        text-decoration: none;
        color: $color-negro;
        transition: all $transition-base;
        font-size: 12px;
        font-weight: $font-weight-medium;

        .dark & {
            color: $color-blanco;
            border-color: #404040;
        }

        :deep(.q-icon) {
            font-size: 24px;
            color: $color-dorado;
        }

        &:hover {
            border-color: $color-dorado;
            background-color: rgba($color-dorado, 0.05);
            transform: translateY(-2px);
        }
    }

    @media (max-width: 600px) {
        grid-template-columns: 1fr;
    }
}

.terms-text {
    font-size: 11px;
    color: $color-gris-medio;
    text-align: center;
    margin: 0;
    line-height: 1.6;

    a {
        color: $color-dorado;
        text-decoration: none;
        font-weight: $font-weight-medium;

        &:hover {
            text-decoration: underline;
        }
    }
}

@media (max-width: 600px) {
    .login-container {
        padding: $spacing-md;
    }

    .login-form .form-container {
        padding: $spacing-lg;
    }

    .register-options {
        flex-direction: column;
    }
}
</style>

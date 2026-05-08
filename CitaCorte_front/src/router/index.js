import { route } from 'quasar/wrappers'
import { createRouter, createMemoryHistory, createWebHistory, createWebHashHistory } from 'vue-router'
import routes from './routes'
import { useAuthStore } from 'stores/auth'

export default route(function ({ store }) {
  const createHistory = process.env.SERVER
    ? createMemoryHistory
    : (process.env.VUE_ROUTER_MODE === 'history' ? createWebHistory : createWebHashHistory)

  const Router = createRouter({
    scrollBehavior: () => ({ left: 0, top: 0 }),
    routes,
    history: createHistory(process.env.MODE === 'ssr' ? void 0 : process.env.VUE_ROUTER_BASE)
  })

  Router.beforeEach((to, from, next) => {
  const auth = useAuthStore(store) // store es la instancia de Pinia

  // 1. Si la ruta es pública, permitir el paso directamente
  if (to.meta.public) {
    // Opcional: si el usuario está autenticado y va a /login, lo mandamos a su dashboard
    if (to.path === '/login' && auth.isLoggedIn) {
      return next(getDashboardByRole(auth.userRole))
    }
    // Para cualquier otra ruta pública, simplemente dejamos pasar
    return next()
  }

  // 2. Si no está autenticado y la ruta no es pública, redirigir a login
  if (!auth.isLoggedIn) {
    return next('/login')
  }

  // 3. Verificar rol requerido
  if (to.meta.role && auth.userRole !== to.meta.role) {
    return next(getDashboardByRole(auth.userRole) || '/login')
  }

  // 4. Si la ruta solo necesita autenticación genérica (como notificaciones)
  // y el usuario tiene sesión, se permite
  next()
})

  return Router
})

function getDashboardByRole(role) {
  switch (role) {
    case 'Admin': return '/admin/dashboard'
    case 'Barbero': return '/barbero/dashboard'
    case 'Barberia': return '/barberia/dashboard'
    case 'Comercial': return '/comercial/dashboard'
    case 'Cliente': return '/cliente/dashboard'
    default: return '/login'
  }
}

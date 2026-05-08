const routes = [
    // ---- Rutas públicas (fuera del layout) ----
    {
        path: "/login",
        component: () => import("pages/auth/Login.vue"),
        meta: { public: true },
    },
    {
        path: "/registro/cliente",
        component: () => import("pages/auth/RegisterCliente.vue"),
        meta: { public: true },
    },
    {
        path: "/registro/barbero",
        component: () => import("pages/auth/RegisterBarbero.vue"),
        meta: { public: true },
    },
    {
        path: "/registro/barberia",
        component: () => import("pages/auth/RegisterBarberia.vue"),
        meta: { public: true },
    },

    // ---- Layout principal (rutas protegidas y públicas que usan el layout) ----
    {
        path: "/",
        component: () => import("components/LayoutApp.vue"),
        children: [
            // Página de inicio pública que muestra barberos
            {
                path: "",
                component: () => import("pages/Home.vue"),
                meta: { public: true }, // ← permite el acceso sin login
            },
            // Detalle de proveedor (también público)
            {
                path: "cliente/detalle/:tipo/:id",
                component: () => import("pages/cliente/DetalleProveedor.vue"),
                meta: { public: true },
            },

            // ---- Rutas protegidas (requieren inicio de sesión) ----
            {
                path: "cliente/dashboard",
                component: () => import("pages/cliente/ClienteDashboard.vue"),
                meta: { role: "Cliente" },
            },
            {
                path: "cliente/buscar",
                component: () => import("pages/cliente/BuscarProveedores.vue"),
                meta: { role: "Cliente" },
            },
            {
                path: "cliente/reservas",
                component: () => import("pages/cliente/MisReservas.vue"),
                meta: { role: "Cliente" },
            },
            {
                path: "cliente/historial",
                component: () => import("pages/cliente/HistorialReservas.vue"),
                meta: { role: "Cliente" },
            },
            {
                path: "cliente/perfil",
                component: () => import("pages/cliente/PerfilCliente.vue"),
                meta: { role: "Cliente" },
            },
            // Barbero
            {
                path: "barbero/dashboard",
                component: () => import("pages/barbero/BarberoDashboard.vue"),
                meta: { role: "Barbero" },
            },
            {
                path: "barbero/servicios",
                component: () => import("pages/barbero/ServiciosBarbero.vue"),
                meta: { role: "Barbero" },
            },
            {
                path: "barbero/productos",
                component: () => import("pages/barbero/ProductosBarbero.vue"),
                meta: { role: "Barbero" },
            },
            {
                path: "barbero/reservas",
                component: () => import("pages/barbero/ReservasBarbero.vue"),
                meta: { role: "Barbero" },
            },
            {
                path: "barbero/estadisticas",
                component: () =>
                    import("pages/barbero/EstadisticasBarbero.vue"),
                meta: { role: "Barbero" },
            },
            {
                path: "barbero/cambiar-plan",
                component: () => import("pages/barbero/CambiarPlan.vue"),
                meta: { role: "Barbero" },
            },
            {
                path: "barbero/afiliaciones",
                component: () =>
                    import("pages/barbero/AfiliacionesBarbero.vue"),
                meta: { role: "Barbero" },
            },

            // Barbería
            {
                path: "barberia/dashboard",
                component: () => import("pages/barberia/BarberiaDashboard.vue"),
                meta: { role: "Barberia" },
            },
            {
                path: "barberia/servicios",
                component: () => import("pages/barberia/ServiciosBarberia.vue"),
                meta: { role: "Barberia" },
            },
            {
                path: "barberia/reservas",
                component: () => import("pages/barberia/ReservasBarberia.vue"),
                meta: { role: "Barberia" },
            },
            {
                path: "barberia/afiliados",
                component: () => import("pages/barberia/AfiliadosBarberia.vue"),
                meta: { role: "Barberia" },
            },
            {
                path: "barberia/estadisticas",
                component: () =>
                    import("pages/barberia/EstadisticasBarberia.vue"),
                meta: { role: "Barberia" },
            },
            {
                path: "barberia/cambiar-plan",
                component: () => import("pages/barberia/CambiarPlan.vue"),
                meta: { role: "Barberia" },
            },
            {
                path: "barberia/perfil",
                component: () => import("pages/barberia/PerfilBarberia.vue"),
                meta: { role: "Barberia" },
            },

            // Admin
            {
                path: "admin/dashboard",
                component: () => import("pages/admin/AdminDashboard.vue"),
                meta: { role: "Admin" },
            },
            {
                path: "admin/planes",
                component: () => import("pages/admin/PlanesSuscripcion.vue"),
                meta: { role: "Admin" },
            },
            {
                path: "admin/solicitudes-cambio",
                component: () =>
                    import("pages/admin/SolicitudesCambioPlan.vue"),
                meta: { role: "Admin" },
            },
            {
                path: "admin/registro-barberias",
                component: () => import("pages/admin/RegistroBarberias.vue"),
                meta: { role: "Admin" },
            },
            {
                path: "admin/usuarios",
                component: () => import("pages/admin/GestionUsuarios.vue"),
                meta: { role: "Admin" },
            },

            // Comercial
            {
                path: "comercial/dashboard",
                component: () =>
                    import("pages/comercial/ComercialDashboard.vue"),
                meta: { role: "Comercial" },
            },
            {
                path: "comercial/solicitudes",
                component: () =>
                    import("pages/comercial/SolicitudesCambioPlan.vue"),
                meta: { role: "Comercial" },
            },
            {
                path: "comercial/registro-barberias",
                component: () =>
                    import("pages/comercial/RegistroBarberias.vue"),
                meta: { role: "Comercial" },
            },

            // Notificaciones (todos los roles autenticados)
            {
                path: "notificaciones",
                component: () =>
                    import("pages/notificaciones/NotificacionesPage.vue"),
                meta: { requiresAuth: true },
            },
        ],
    },

    // 404
    {
        path: "/:catchAll(.*)*",
        component: () => import("pages/ErrorNotFound.vue"),
    },
];

export default routes;

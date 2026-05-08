import { defineStore } from "pinia";
import { api } from "src/boot/axios";

// Función para decodificar JWT
export function decodeToken(token) {
    try {
        const base64Url = token.split(".")[1];
        const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
        const jsonPayload = decodeURIComponent(
            atob(base64)
                .split("")
                .map(
                    (c) => "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2)
                )
                .join("")
        );
        return JSON.parse(jsonPayload);
    } catch (error) {
        console.error("Error al decodificar token:", error);
        return null;
    }
}

export const useAuthStore = defineStore("auth", {
    state: () => ({
        user: null,
        token: sessionStorage.getItem("token") || "",
        loading: false,
        role: null,
    }),
    getters: {
        isLoggedIn: (state) => !!state.token,
        userRole: (state) => state.role || "",
    },
    actions: {
        async login(credentials) {
            const { data } = await api.post(`Autenticacion/Login`, credentials);
            console.log("data: ", data);
            this.token = data.result.token;
            sessionStorage.setItem("token", data.result.token);

            // Decodificar el token para extraer el rol
            const decoded = decodeToken(data.result.token);
            if (decoded && decoded.rol) {
                this.role = decoded.rol;
                console.log("Usuario logueado con rol:", this.role);
            }

            await this.fetchUser();
        },
        async fetchUser() {
            this.loading = true;
            const decoded = decodeToken(sessionStorage.getItem("token"));
            const id = decoded.id;
            try {
                const { data } = await api.get(
                    `Autenticacion/ObtenerInformacionUsuario?id=${id}`
                );
                this.user = JSON.parse(JSON.stringify(data)); // sanitiza ciclos
            } finally {
                this.loading = false;
            }
        },
        logout() {
            this.token = "";
            this.user = null;
            this.role = null;
            sessionStorage.removeItem("token");
            // Usamos el router importado
            window.location.href = "/"; // o '/login' según necesites
        },
        // Inicializar el role desde el token guardado (para cuando se recarga la página)
        initializeFromToken() {
            if (this.token) {
                const decoded = decodeToken(this.token);
                if (decoded && decoded.rol) {
                    this.role = decoded.rol;
                }
            }
        },
    },
});

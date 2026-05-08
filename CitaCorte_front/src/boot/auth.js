// src/boot/auth.js
import { boot } from "quasar/wrappers";
import { useAuthStore } from "stores/auth";

export default boot(async () => {
    const auth = useAuthStore();

    // Inicializar el role desde el token guardado (para cuando se recarga la página)
    if (auth.isLoggedIn) {
        auth.initializeFromToken();

        if (!auth.user) {
            try {
                await auth.fetchUser();
            } catch (e) {
                auth.logout();
            }
        }
    }
});

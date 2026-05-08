/**
 * Configuración centralizada para el módulo de registro
 * Contiene IDs de roles, endpoints y constantes de suscripción
 */

export const REGISTRO_CONFIG = {
    // API Base URL
    API_BASE: "http://localhost:7208",

    // Role IDs
    ROLES: {
        BARBERIA: "C0B7E3B3-A06E-4580-B985-BB2FC4336526",
        BARBERO: "C0B7E3B3-A06E-4580-B985-BB2FC4336525",
        CLIENTE: "C0B7E3B3-A06E-4580-B985-BB2FC4336524",
    },

    // Endpoints
    ENDPOINTS: {
        CREAR_USUARIO: "/api/Usuario/CrearUsusarioMejorado",
        OBTENER_SUSCRIPCIONES: "/api/Suscripcion/ObtenerListadoPaginado",
    },

    // Suscripción por defecto para Barberos
    SUSCRIPCION_BARBERO_DEFAULT: "C0B7E3B3-A06E-4580-B985-BB2FC4336520",

    // Campos requeridos por tipo de usuario
    CAMPOS_REQUERIDOS: {
        COMUN: [
            "nombre",
            "apellidos",
            "username",
            "contrasenna",
            "correo",
            "direccion",
        ],
        BARBERIA: ["nombreBarberia", "telefono", "suscripcionId"],
        BARBERO: ["suscripcionId"],
        CLIENTE: [],
    },
};

/**
 * Obtiene la URL completa de un endpoint
 * @param {string} endpoint - Nombre de endpoint o ruta completa
 * @returns {string}
 */
export const getApiUrl = (endpoint) => {
    if (endpoint.startsWith("http")) return endpoint;
    return `${REGISTRO_CONFIG.API_BASE}${endpoint}`;
};

/**
 * Valida los campos de registro según el tipo de usuario
 * @param {object} data - Datos del formulario
 * @param {string} tipo - Tipo de usuario: 'BARBERIA', 'BARBERO', 'CLIENTE'
 * @returns {object} - { isValid: boolean, errors: array }
 */
export const validarRegistro = (data, tipo) => {
    const errors = [];
    const camposRequeridos = [...REGISTRO_CONFIG.CAMPOS_REQUERIDOS.COMUN];

    if (tipo === "BARBERIA") {
        camposRequeridos.push(...REGISTRO_CONFIG.CAMPOS_REQUERIDOS.BARBERIA);
    } else if (tipo === "BARBERO") {
        camposRequeridos.push(...REGISTRO_CONFIG.CAMPOS_REQUERIDOS.BARBERO);
    }

    camposRequeridos.forEach((campo) => {
        if (!data[campo]) {
            errors.push(`${campo} es requerido`);
        }
    });

    return {
        isValid: errors.length === 0,
        errors,
    };
};

/**
 * Construye el payload para crear un usuario
 * @param {object} formData - Datos del formulario
 * @param {string} tipo - Tipo de usuario: 'BARBERIA', 'BARBERO', 'CLIENTE'
 * @returns {object} - Payload para enviar al endpoint
 */
export const construirPayload = (formData, tipo) => {
    const rolId = REGISTRO_CONFIG.ROLES[tipo];

    const payload = {
        rolId,
        nombre: formData.nombre,
        apellidos: formData.apellidos,
        username: formData.username,
        contrasenna: formData.contrasenna,
        correo: formData.correo,
        direccion: formData.direccion,
    };

    if (tipo === "BARBERIA") {
        payload.nombreBarberia = formData.nombreBarberia;
        payload.telefono = formData.telefono;
        payload.logoUrl = formData.logoUrl || "";
        payload.suscripcionId = formData.suscripcionId;
    } else if (tipo === "BARBERO") {
        payload.suscripcionId = REGISTRO_CONFIG.SUSCRIPCION_BARBERO_DEFAULT;
    }

    return payload;
};

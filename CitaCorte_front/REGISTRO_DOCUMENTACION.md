# Documentación: Sistema de Registro Actualizado

## 📋 Cambios Realizados

Se han actualizado los formularios de registro para integrar el nuevo endpoint `/api/Usuario/CrearUsusarioMejorado` con una estructura mejorada y validaciones.

### 1. **RegisterBarberia.vue**

Página de registro para usuarios tipo **Barbería**

#### Campos requeridos:

-   **Datos Personales**: nombre, apellidos, username
-   **Datos de Contacto**: correo, contraseña, dirección, teléfono
-   **Datos de la Barbería**: nombreBarberia, logoUrl (opcional)
-   **Plan de Suscripción**: Selector dinámico cargado desde el endpoint de suscripciones

#### Características:

-   Carga dinámica de planes de suscripción desde `/api/Suscripcion/ObtenerListadoPaginado?TipoSuscripcion=Barberia`
-   Muestra detalles del plan seleccionado (nombre, precio, características)
-   Validación de email y contraseña con requisitos mínimos
-   Loading indicator mientras se cargan los planes

#### Payload enviado:

```json
{
    "rolId": "C0B7E3B3-A06E-4580-B985-BB2FC4336526",
    "nombre": "string",
    "apellidos": "string",
    "username": "string",
    "contrasenna": "string",
    "correo": "string",
    "nombreBarberia": "string",
    "direccion": "string",
    "telefono": "string",
    "logoUrl": "string",
    "suscripcionId": "string"
}
```

---

### 2. **RegisterBarbero.vue**

Página de registro para usuarios tipo **Barbero**

#### Campos requeridos:

-   **Datos Personales**: nombre, apellidos, username
-   **Datos de Contacto**: correo, contraseña, dirección

#### Características:

-   Formulario simple y directo
-   Sin selector de suscripción (utiliza el ID por defecto automáticamente)
-   Validación de email y contraseña
-   Menos campos que Barbería para registro más rápido

#### Payload enviado:

```json
{
    "rolId": "C0B7E3B3-A06E-4580-B985-BB2FC4336525",
    "nombre": "string",
    "apellidos": "string",
    "username": "string",
    "contrasenna": "string",
    "correo": "string",
    "direccion": "string",
    "suscripcionId": "C0B7E3B3-A06E-4580-B985-BB2FC4336520"
}
```

**Nota**: El `suscripcionId` se asigna automáticamente al valor por defecto `C0B7E3B3-A06E-4580-B985-BB2FC4336520`

---

### 3. **RegisterCliente.vue**

Página de registro para usuarios tipo **Cliente**

#### Campos requeridos:

-   **Datos Personales**: nombre, apellidos, username
-   **Datos de Contacto**: correo, contraseña, dirección

#### Características:

-   Formulario simplificado (sin suscripción)
-   Validación de email y contraseña
-   Interfaz limpia y rápida

#### Payload enviado:

```json
{
    "rolId": "C0B7E3B3-A06E-4580-B985-BB2FC4336524",
    "nombre": "string",
    "apellidos": "string",
    "username": "string",
    "contrasenna": "string",
    "correo": "string",
    "direccion": "string"
}
```

---

## 🔐 Configuración de Roles

Los IDs de roles se encuentran centralizados en `src/assets/js/config/registroConfig.js`:

```javascript
ROLES: {
  BARBERIA: 'C0B7E3B3-A06E-4580-B985-BB2FC4336526',
  BARBERO: 'C0B7E3B3-A06E-4580-B985-BB2FC4336525',
  CLIENTE: 'C0B7E3B3-A06E-4580-B985-BB2FC4336524'
}
```

## 🔌 Endpoints Utilizados

### Crear Usuario

```
POST /api/Usuario/CrearUsusarioMejorado
```

Endpoint unificado para registro de todos los tipos de usuarios.

### Obtener Suscripciones (Solo Barbería)

```
GET /api/Suscripcion/ObtenerListadoPaginado?TipoSuscripcion=Barberia
```

Devuelve lista de planes disponibles para barbería con sus características.

---

## 📝 Validaciones Implementadas

1. **Campos requeridos**: Todos los campos marcados son obligatorios
2. **Email**: Validación de formato con expresión regular
3. **Contraseña**: Mínimo 6 caracteres
4. **Carga de suscripciones**: Manejo de errores con notificaciones
5. **Envío de formulario**: Validación previa antes de enviar

---

## 🎨 Estilos y UX

-   **Formularios agrupados**: Campos organizados por secciones (Datos Personales, Contacto, etc.)
-   **Indicadores de carga**: Spinners mientras se cargan datos externos
-   **Notificaciones**: Feedback visual de éxito o error
-   **Responsive design**: Soporta desde móviles hasta desktop
-   **Layout centrado**: Tarjeta blanca sobre fondo gris para mejor contraste

---

## 🚀 Cómo Usar

### Acceder a los formularios:

-   **Cliente**: `/registro/cliente`
-   **Barbero**: `/registro/barbero`
-   **Barbería**: `/registro/barberia`

### Flujo esperado:

1. Usuario selecciona el tipo de registro
2. Completa los campos requeridos
3. Sistema valida los datos
4. Se envía la solicitud a `/api/Usuario/CrearUsusarioMejorado`
5. Si es exitoso, redirige a `/login`
6. Si hay error, muestra notificación con el mensaje del servidor

---

## 🔧 Configuración Base API

El `API_BASE` está configurado como:

```
https://localhost:5000
```

Para cambiar esto, edita `src/assets/js/config/registroConfig.js` en la propiedad `API_BASE`.

---

## 📚 Archivos Modificados

-   `src/pages/auth/RegisterBarberia.vue` ✓
-   `src/pages/auth/RegisterBarbero.vue` ✓
-   `src/pages/auth/RegisterCliente.vue` ✓
-   `src/assets/js/config/registroConfig.js` (nuevo)

---

## ⚠️ Notas Importantes

1. **HTTPS**: Los endpoints usan `https://localhost:5000`. Asegúrate de que tu backend esté configurado para HTTPS o cambia a `http` si es necesario.

2. **CORS**: Verifica que tu backend permita las solicitudes desde el origen del frontend.

3. **IDs de Roles**: Los IDs de roles deben coincidirse exactamente con los del backend.

4. **Suscripción de Barbero**: Se asigna automáticamente al ID `C0B7E3B3-A06E-4580-B985-BB2FC4336520`. Si esto cambia, actualiza en `registroConfig.js`.

---

## 🐛 Troubleshooting

### Los planes no cargan en Barbería:

-   Verifica que el endpoint `/api/Suscripcion/ObtenerListadoPaginado?TipoSuscripcion=Barberia` esté activo
-   Revisa la consola del navegador para errores de CORS o conexión

### El registro falla:

-   Verifica que el endpoint `/api/Usuario/CrearUsusarioMejorado` esté disponible
-   Comprueba los IDs de roles en el backend
-   Revisa los logs del servidor para más detalles

### Campos inválidos:

-   Asegúrate de que los campos enviados en el payload coincidan con los esperados por el backend
-   La contraseña debe tener al menos 6 caracteres

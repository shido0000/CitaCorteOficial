/* eslint-disable no-unreachable */
// IMPORT
import { api } from 'src/boot/axios'
import { ref, reactive } from 'vue'
import { Error, Success } from './notify'

const title = ref('')

// Funcion para cargar todos los datos de una tabla
const loadGet = async (endpoint) => {
    return await api
        .get(endpoint)
        .then((r) => {
            return r.data.result.elementos
        })
        .catch((error) => {
            error.response === undefined
                ? Error(error.message)
                : Error(error.response.data.mensajeError)
        })
}
// Funcion que se utiliza para cargar los datos en las opciones de los select
const loadSelectList = async (endpoint) => {
    return await api
        .get(endpoint)
        .then((r) => {
            return r.data.result
        })
        .catch((error) => {
            error.response === undefined
                ? Error(error.message)
                : Error(error.response.data.mensajeError)
        })
}
// Funcion para obtener una lista pasandole el endpoint y el arreglo
const load = async (endpoint, lista) => {
    await api
        .get(endpoint)
        .then((r) => {
            lista.value = r.data.result.elementos
        })
        .catch((error) => {
            error.response === undefined
                ? Error(error.message)
                : Error(error.response.data.mensajeError)
        })
}
// Funcion para guardar y editar
const saveData = async (endpoint, objeto, load, close, dialogLoad) => {
    const respuesta = reactive({
        resultado: null,
        mensajeError: null
    })

    dialogLoad.value = true
    if (objeto.id) {
        return await api.put(`/${endpoint}/${objeto.id}`, objeto).then(async (response) => {
            respuesta.resultado = response
            Success('El elemento ha sido modificado correctamente')
            await load()
            await close()
            dialogLoad.value = false
            return respuesta
        }).catch(async (error) => {
            respuesta.mensajeError = error
            console.log(error)
            await load()
            await close()
            dialogLoad.value = false
            return respuesta
        })
    } else {
        return await api.post(`/${endpoint}`, objeto).then(async (response) => {
            respuesta.resultado = response
            Success.call(this, 'El elemento ha sido creado correctamente')
            await load()
            await close()
            dialogLoad.value = false
            return respuesta
        }).catch(async (error) => {
            respuesta.mensajeError = error
            await load()
            await close()
            dialogLoad.value = false
            return respuesta
        })
    }
}
// Funcion de Eliminar
const eliminarElemento = async (endpoint, id, load, dialogLoad) => {
    const respuesta = reactive({
        resultado: null,
        mensajeError: null
    })
    dialogLoad.value = true
    return await api.delete(`/${endpoint}/${id}`).then(async (response) => {
        respuesta.resultado = response
        Success.call(this, 'El elemento ha sido eliminado correctamente')
        setTimeout(async () => {
            await load()
            dialogLoad.value = false
        })
        return respuesta
    }).catch(async (error) => {
        respuesta.mensajeError = error
        dialogLoad.value = false
        return respuesta
    })
}
// Funcion para Obtener datos por id esto se utiliza generalmente para cargar los datos del formulario
const obtener = async (endpoint, id, objeto, dialogLoad, dialog) => {
    dialogLoad.value = true
    await api.get(`${endpoint}/${id}`)
        .then(r => {
            Object.assign(objeto, r.data.result)
            title.value = `Editar ${endpoint}`
            dialog.value = true
        }).catch((error) => {
            error.response === undefined ? Error(error.message) : Error(error.response.data.mensajeError)
        })
    dialogLoad.value = false
}
// Funcion para verificar que los campos sean unicos
const isValorRepetido = (val, propiedad, objeto, items) => {
    return items.some((e) => e[propiedad] === val && objeto.id !== e.id)
}

// Funcion para resetear los campos del dialogo y cerrarlo
const closeDialog = (objeto, objetoInicial, myForm, dialog) => {
    myForm.value.resetValidation()

    // Eliminar propiedades de 'objeto' que no están en 'objetoInicial'
    // Object.keys(objeto).forEach(key => {
    //     if (!objetoInicial.hasOwnProperty(key)) {

    //  Se utiliza para eliminar propiedades de un objeto. No es una función del objeto en sí, sino un operador del lenguaje.
    //         delete objeto.key;
    //     }
    // });

    // Asignar propiedades de 'objetoInicial' a 'objeto'
    Object.assign(objeto, objetoInicial);

    delete objeto.id;

    // Sintaxis: delete objeto.propiedad o delete objeto['propiedad']
    // Retorno: Devuelve true si la propiedad se elimina con éxito, o false si la propiedad no se puede eliminar(por ejemplo, si es una propiedad no configurable)

    dialog.value = false
    // if (objeto.id !== null || objeto.id !== '' || objeto.id !== undefined) objeto.id = null
}
// Funcion para Filtrado
const filterOptions = (val, update, options, filterField, array) => {
    update(
        () => {
            const needle = val ? val.toLowerCase() : ''
            options = array.filter(
                (v) => v[filterField] && v[filterField].toLowerCase().indexOf(needle) >= 0
            )
        },
        (ref) => {
            if (
                val !== '' &&
                ref.options.length > 0 &&
                ref.getOptionIndex() === -1
            ) {
                ref.moveOptionSelection(1, true) // enfoca la primera opción seleccionable y no actualiza el valor del input
                ref.toggleOption(ref.options[ref.optionIndex], true) // alterna la opción enfocada
            }
        }
    )
    return options
}
// Función para validar carnet
const validarCarnet = (val) => {
    // Verificar que tenga 11 dígitos
    if (val.length !== 11) {
        return false
    }

    // Obtener el número de mes
    const mes = parseInt(val.substr(2, 2), 10)

    // Verificar que el mes esté entre 01 y 12
    if (mes < 1 || mes > 12) {
        return false
    }

    // Obtener el número de días permitidos para el mes
    const diasEnMes = new Date(2024, mes, 0).getDate()

    // Obtener los dígitos 5 y 6
    const diaStr = val.substr(4, 2)
    const dia = parseInt(diaStr, 10)

    // Verificar que el día esté dentro del rango válido
    if (dia < 1 || dia > diasEnMes) {
        return false
    }

    // Si pasó todas las validaciones, es un val válido
    return true
}
// Funcion para validar letras y caracteres especiales(¨Generalmente se utiliza para validar los nombres¨)
const validarLetrasYCaracteresEspeciales = (val) => {
    // Expresión regular que acepta letras y caracteres especiales, pero no números
    const regex = /^[A-Za-záéíóúÁÉÍÓÚñÑ\s#&]+$/

    return regex.test(val)
}
// Funcion para validar correo
const validarCorreo = (val) => {
    // Expresión regular para validar direcciones de correo electrónico
    const regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/

    return regex.test(val)
}
// Funcion para validar solo numeros
const validarSoloNumeros = (val) => {
    // Expresión regular que solo acepta números
    const regex = /^[0-9]+$/

    return regex.test(val)
}

export {
    saveData,
    closeDialog,
    load,
    obtener,
    validarSoloNumeros,
    validarCorreo,
    filterOptions,
    eliminarElemento,
    validarCarnet,
    isValorRepetido,
    validarLetrasYCaracteresEspeciales,
    loadGet,
    loadSelectList,
}

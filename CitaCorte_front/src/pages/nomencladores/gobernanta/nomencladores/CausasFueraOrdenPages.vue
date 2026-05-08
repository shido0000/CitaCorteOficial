<template>
    <div class="q-pa-xl">
        <q-breadcrumbs class="qb cursor-pointer q-pb-md">
            <q-breadcrumbs-el label="Inicio" icon="home" @click="router.push('/inicio')" />
            <q-breadcrumbs-el label="Nomencladores" icon="dashboard" @click="router.push('/gobernanta')" />
            <q-breadcrumbs-el label="Causas Fuera Orden" />
        </q-breadcrumbs>
        <q-table class="q-pa-md" :filter="filter" title="Útiles" :rows="items" :columns="columns" row-key="id"
            no-data-label="No hay elementos disponibles" no-results-label="No hay elementos disponibles"
            loading-label="Cargando..." rows-per-page-label="Filas por página">
            <template v-slot:top>
                <div class="col-4 q-table__title">
                    <span>Causas Fuera Orden</span>
                    <q-input outline color="primary" flat v-model="filter" debounce="500" label="Buscar" />
                </div>
                <q-space />

                <q-btn class="bg-primary" style="width: 20px" color="primary" icon="add" @click="dialog = true">
                    <q-tooltip class="bg-primary" transition-show="flip-right" transition-hide="flip-left"
                        :offset="[10, 10]">Adicionar</q-tooltip>
                </q-btn>
                <q-btn outline class="bg-white q-ml-sm" style="width: 20px" color="primary" icon="print"
                    @click="imprimir()">
                    <q-tooltip class="bg-primary" :offset="[10, 10]" transition-show="flip-right"
                        transition-hide="flip-left">
                        Imprimir
                    </q-tooltip>
                </q-btn>
            </template>
            <template v-slot:body-cell-supervisora="props">
                <q-td :props="props">
                    <q-icon flat :name="(props.value == 0) ? 'highlight_off' : 'check_circle'"
                        :class="(props.value == 0) ? 'text-grey' : 'text-primary'" size="20px" />
                </q-td>
            </template>
            <template v-slot:body-cell-activo="props">
                <q-td :props="props">
                    <q-icon flat :name="(props.value == 0) ? 'highlight_off' : 'check_circle'"
                        :class="(props.value == 0) ? 'text-grey' : 'text-primary'" size="20px" />
                </q-td>
            </template>
            <template v-slot:body-cell-action="props">
                <q-td :props="props">
                    <div class="q-gutter-sm">
                        <q-btn flat dense size="sm" @click="obtenerElementoPorId(props.row.id)" text-color="primary"
                            icon="edit">
                            <q-tooltip class="bg-primary" :offset="[10, 10]" transition-show="flip-right"
                                transition-hide="flip-left">Editar datos</q-tooltip>
                        </q-btn>
                        <q-btn flat dense size="sm" @click="abrirDialogoEliminar(props.row.id)" text-color="negative"
                            icon="delete">
                            <q-tooltip class="bg-red" :offset="[10, 10]" transition-show="flip-right"
                                transition-hide="flip-left">Eliminar</q-tooltip>
                        </q-btn>
                    </div>
                </q-td>
            </template>
        </q-table>
    </div>

    <!-- DIALOGS -->
    <DialogLoad :dialogLoad="dialogLoad" />

    <DialogEliminar v-if="isDialogoEliminarAbierto" :isOpen="isDialogoEliminarAbierto"
        :idElemento="idElementoSeleccionado" @eliminar="eliminar" @closeDialog="handleCloseDialog" />

    <!-- Add & Delete -->
    <q-dialog v-model="dialog" persistent>
        <q-card style="width: 700px; max-width: 80vw; height: auto">
            <header class="q-pa-sm bg-primary">
                <q-toolbar>
                    <q-toolbar-title class="text-subtitle6 text-white">
                        {{
                            objeto?.id ? "Editar Zonas Comunes" : "Adicionar Zonas Comunes"
                        }}</q-toolbar-title>
                </q-toolbar>
            </header>
            <q-form @submit.prevent="Guardar()" @reset="close" ref="myForm">
                <div class="h row q-ma-md">
                    <q-input v-if="!objeto?.id" class="col-xs-12" autofocus label="Código" v-model="objeto.codigo"
                        color="primary" counter maxlength="10" :rules="[onlyLetter_Number, (val) =>
                            (items.length > 0
                                ? !isValorRepetido(val, 'codigo', objeto, items)
                                : true) || 'Ya existe un código con ese valor',]" />
                    <q-input class="col-xs-12 " label="Descripción*" v-model="objeto.descripcion" color="primary"
                        counter maxlength="100" :rules="[onlyLetter_Number, string]" />
                    <!-- TODOO -->
                    <q-checkbox v-if="objeto.id && !objeto?.id" class="q-mr-md" right-label v-model="objeto.activo"
                        label="Activo" color="primary" />
                    <q-card-actions class="col-12 q-mt-md justify-end">
                        <q-btn class="text-white" color="primary" aling="right" type="submit" label="Guardar" />
                        <q-btn outline color="primary" type="reset" label="Cancelar" />
                    </q-card-actions>
                </div>
            </q-form>
        </q-card>
    </q-dialog>
</template>

<script setup>
// IMPORT LIBRERIES
import { dataColumnCausasFueraOrden } from "src/assets/js/column_data/columnDataGobernanta";
import {
    loadGet,
    saveData,
    isValorRepetido,
    eliminarElemento,
    obtener,
    closeDialog,
} from 'src/assets/js/util/funciones'
import { onlyLetter_Number, string } from 'src/assets/js/util/validator_form'
import { Error } from "src/assets/js/util/notify";
import { Error_Notify_DelecteObject } from "src/assets/js/util/dicc_notify";

// IMPORT COMPONENTS
import DialogLoad from 'src/components/dialog_boxes/DialogLoad.vue'
import DialogEliminar from 'src/components/dialog_boxes/DialogEliminar.vue'

// IMPORT HOOKS
import { ref, onMounted, reactive } from "vue";
import { useRouter } from "vue-router";

/***************************************************************************************************
 *                                        VARIABLES                                                *
 **************************************************************************************************/

// HOOKS
const router = useRouter();

// DATOS DE LA TABLA
const columns = dataColumnCausasFueraOrden;
const filter = ref("");
const items = ref([]);

// DIALOGS VAR
const dialog = ref(false)
const dialogLoad = ref(false)
const isDialogoEliminarAbierto = ref(false)

// VAR FORM
const myForm = ref(null)
const idElementoSeleccionado = ref('')

/***************************************************************************************************
 *                                         OBJECT                                                  *
 **************************************************************************************************/
const objetoInicial = {
    descripcion: null,
    codigo: null,
    activo: true
}

// Crear una copia del objeto inicial
const objeto = reactive({ ...objetoInicial })

/***************************************************************************************************
*                                      FUNCIONES                                                   *
 **************************************************************************************************/

// HOOKS

// Craga los datos de la tabla
onMounted(async () => {
    dialogLoad.value = true;
    // Se llena el listado de la pagina
    items.value = (await loadGet('CausasFueraOrden/ObtenerListadoPaginado')) ?? []; // Condicion para en caso de error la tabla no de error ya q la api devulve undefined

    dialogLoad.value = false;
});

// EXTRAS

// Filtrado
// 1- Funcion para pasar parametros en el Adicionar SaveData
const Guardar = () => {
    const url = objeto.id ? 'CausasFueraOrden/Actualizar' : 'CausasFueraOrden/Crear'
    saveData(url, objeto, load, close, dialogLoad)
}

// Funcion para Obtener los datos para editar
const obtenerElementoPorId = async (id) => {
    await obtener('CausasFueraOrden/ObtenerPorId', id, objeto, dialogLoad, dialog)
}

// Funcion para eliminar elemento
const eliminar = async () => {
    await eliminarElemento(
        'CausasFueraOrden/Eliminar',
        idElementoSeleccionado.value,
        load,
        dialogLoad
    ).
        then(async (response) => {
            return response
        }).catch(async () => {
            Error(Error_Notify_DelecteObject)
        })
}

// Funcion para abrir el dialog de eliminar y pasar el id del elemento
const abrirDialogoEliminar = (id) => {
    idElementoSeleccionado.value = id
    isDialogoEliminarAbierto.value = true
}

// 2- Funcion para pasar por parametro el arreglo de los elmentos de la tabla
const load = async () => {
    items.value = await loadGet('CausasFueraOrden/ObtenerListadoPaginado')
}

// Funcion para cerrar el dialog
const handleCloseDialog = () => {
    isDialogoEliminarAbierto.value = false
}

// Funcion para cerrar el dialog principal de Adicionar y Editar y resetear los campos del formulario
const close = async () => {
    closeDialog(objeto, objetoInicial, myForm, dialog)
}
</script>

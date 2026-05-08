<template>
  <q-page padding>
    <div v-if="!esPremium" class="bg-orange-1 q-pa-md text-center">
      <q-icon name="lock" size="md" /> Necesitas el plan Premium para vender
      productos.
      <q-btn
        label="Cambiar plan"
        to="/barbero/cambiar-plan"
        flat
        class="q-ml-md"
      />
    </div>
    <template v-else>
      <div class="row items-center q-mb-md">
        <div class="text-h6">Mis Productos</div>
        <q-space />
        <q-btn
          label="Nuevo Producto"
          color="primary"
          @click="abrirFormulario()"
        />
      </div>
      <q-table
        :rows="productos"
        :columns="cols"
        row-key="id"
        :loading="loading"
      >
        <template v-slot:body-cell-acciones="props">
          <q-btn flat icon="edit" @click="abrirFormulario(props.row)" />
          <q-btn flat icon="delete" @click="eliminar(props.row.id)" />
        </template>
      </q-table>

      <q-dialog v-model="dialogo" persistent>
        <q-card style="width: 500px">
          <q-card-section>{{
            editando ? "Editar Producto" : "Nuevo Producto"
          }}</q-card-section>
          <q-card-section>
            <q-form @submit="guardar">
              <q-input
                v-model="form.nombre"
                label="Nombre"
                outlined
                :rules="[(v) => !!v || 'Requerido']"
              />
              <q-input
                v-model="form.descripcion"
                label="Descripción"
                outlined
                class="q-mt-sm"
              />
              <q-input
                v-model.number="form.precio"
                label="Precio"
                type="number"
                outlined
                class="q-mt-sm"
              />
              <q-input
                v-model.number="form.stock"
                label="Stock"
                type="number"
                outlined
                class="q-mt-sm"
              />
              <q-input
                v-model="form.imagenUrl"
                label="URL Imagen"
                outlined
                class="q-mt-sm"
              />
              <q-select
                v-model="form.categoriaId"
                :options="categorias"
                option-value="id"
                option-label="nombre"
                label="Categoría"
                outlined
                class="q-mt-sm"
              />
              <div class="q-mt-md text-right">
                <q-btn label="Cancelar" flat @click="dialogo = false" />
                <q-btn
                  type="submit"
                  label="Guardar"
                  color="primary"
                  :loading="guardando"
                />
              </div>
            </q-form>
          </q-card-section>
        </q-card>
      </q-dialog>
    </template>
  </q-page>
</template>

<script setup>
import { ref, computed } from 'vue'
import api from 'src/boot/axios'
import { useQuasar } from 'quasar'
import { useAuthStore } from 'stores/auth'

const $q = useQuasar()
const auth = useAuthStore()
const esPremium = computed(() => auth.user?.plan?.tipo === 'Premium')

const productos = ref([])
const categorias = ref([])
const dialogo = ref(false)
const editando = ref(false)
const form = ref({
  nombre: '',
  descripcion: '',
  precio: 0,
  stock: 0,
  imagenUrl: '',
  categoriaId: null
})
const currentId = ref(null)
const loading = ref(false)
const guardando = ref(false)

const cols = [
  { name: 'nombre', field: 'nombre', label: 'Nombre' },
  { name: 'precio', field: 'precio', label: 'Precio' },
  { name: 'stock', field: 'stock', label: 'Stock' },
  { name: 'acciones', label: 'Acciones' }
]

const cargar = async () => {
  if (!esPremium.value) return
  loading.value = true
  const [prod, cat] = await Promise.all([
    api.get('/barbero/productos'),
    api.get('/categorias-producto')
  ])
  productos.value = prod.data
  categorias.value = cat.data
  loading.value = false
}
cargar()

const abrirFormulario = (row = null) => {
  if (row) {
    editando.value = true
    currentId.value = row.id
    form.value = { ...row, categoriaId: row.categoria?.id || row.categoriaId }
  } else {
    editando.value = false
    currentId.value = null
    form.value = {
      nombre: '',
      descripcion: '',
      precio: 0,
      stock: 0,
      imagenUrl: '',
      categoriaId: null
    }
  }
  dialogo.value = true
}

const guardar = async () => {
  guardando.value = true
  try {
    if (editando.value) {
      await api.put(`/barbero/productos/${currentId.value}`, form.value)
      $q.notify({ type: 'positive', message: 'Producto actualizado' })
    } else {
      await api.post('/barbero/productos', form.value)
      $q.notify({ type: 'positive', message: 'Producto creado' })
    }
    dialogo.value = false
    cargar()
  } finally {
    guardando.value = false
  }
}

const eliminar = async (id) => {
  $q.dialog({
    title: 'Eliminar',
    message: '¿Eliminar producto?',
    cancel: true,
    persistent: true
  }).onOk(async () => {
    await api.delete(`/barbero/productos/${id}`)
    cargar()
  })
}
</script>

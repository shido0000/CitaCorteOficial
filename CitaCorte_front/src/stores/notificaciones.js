import { defineStore } from 'pinia'
import { api } from 'src/boot/axios'

export const useNotifStore = defineStore('notificaciones', {
  state: () => ({
    items: [],
    noLeidas: 0
  }),
  actions: {
    async cargar() {
      const { data } = await api.get(`notificaciones`)
      this.items = JSON.parse(JSON.stringify(data)) // sanitiza ciclos
      this.noLeidas = this.items.filter(n => !n.leida).length
    },
    async marcarLeida(id) {
      await axios.put(`${baseURL}/notificaciones/${id}/leer`)
      const notif = this.items.find(n => n.id === id)
      if (notif) notif.leida = true
      this.noLeidas = this.items.filter(n => !n.leida).length
    }
  }
})

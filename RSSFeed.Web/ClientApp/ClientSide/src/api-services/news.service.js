import Axios from 'axios'

const RESOURCE_NAME = '/Home'

export default {
  getAll (page, query, source, sort, categories) {
    return Axios.get(`https://localhost:44301/api/news/?pageNumber=${page}&query=${query}&source=${source}&sort=${sort}&category=${categories}`)
  },

  get (id) {
    return Axios.get(`${RESOURCE_NAME}/${id}`)
  },

  create (data) {
    return Axios.post(RESOURCE_NAME, data)
  },

  update (id, data) {
    return Axios.put(`${RESOURCE_NAME}/${id}`, data)
  },

  delete (id) {
    return Axios.delete(`${RESOURCE_NAME}/${id}`)
  }
}

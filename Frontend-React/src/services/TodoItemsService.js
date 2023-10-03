import axios from 'axios'

const axiosClient = axios.create({
  baseURL: 'https://localhost:44397/api/TodoItems',
  headers: {
    'Content-Type': 'application/json',
  },
})

const getTodoItems = async () => {
  try {
    return await axiosClient.get()
  } catch (error) {
    const message = error?.response?.data?.message ?? error.message
    throw new Error(message)
  }
}

const updateTodoItem = async (id, payload) => {
  try {
    return await axiosClient({
      method: 'put',
      url: `/${id}`,
      data: payload,
    })
  } catch (error) {
    const message = error?.response?.data?.message ?? error.message
    throw new Error(message)
  }
}

const createTodoItem = async (payload) => {
  try {
    return await axiosClient({
      method: 'post',
      data: payload,
    })
  } catch (error) {
    const message = error?.response?.data?.message ?? error.message
    throw new Error(message)
  }
}

export { getTodoItems, createTodoItem, updateTodoItem }

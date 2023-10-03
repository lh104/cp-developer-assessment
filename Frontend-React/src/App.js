import './App.css'
import { Container } from 'react-bootstrap'
import React, { useState, useEffect } from 'react'
import Header from './components/shared/layout/Header'
import Footer from './components/shared/layout/Footer'
import Description from './components/shared/Description'
import AddTodoItem from './components/todo-item/AddTodoItem'
import ListTodoItems from './components/todo-item/ListTodoItems'
import ToastWrapper from './components/shared/ToastWrapper'
import { getTodoItems, updateTodoItem, createTodoItem } from './services/TodoItemsService'

const App = () => {
  const [toastSetting, setToastSetting] = useState(undefined)
  const [items, setItems] = useState([])

  useEffect(() => {
    getItems()
  }, [])

  const getItems = async () => {
    try {
      var result = await getTodoItems()
      setItems(result.data)
    } catch (error) {
      setToastSetting({ type: 'danger', title: 'Error', message: `Failed to load Todo Item list, ${error.message}` })
    }
  }

  const handleOnItemCreated = async (todoItem) => {
    try {
      var result = await createTodoItem(todoItem)
      getItems()
      setToastSetting({
        type: 'success',
        title: 'Success',
        message: `New Todo Item "${result.data.description}" added.`,
      })
    } catch (error) {
      setToastSetting({ type: 'danger', title: 'Error', message: `Failed to create Todo item, ${error.message}` })
    }
  }

  const handleOnItemUpdated = async (todoItem) => {
    try {
      await updateTodoItem(todoItem.id, todoItem)
      getItems()
      setToastSetting({
        type: 'success',
        title: 'Success',
        message: `Todo Item "${todoItem.description}" marked as completed.`,
      })
    } catch (error) {
      setToastSetting({ type: 'danger', title: 'Error', message: `Failed to update Todo item, ${error.message}` })
    }
  }

  return (
    <div className="App">
      <Container>
        <Header />
        <Description />
        <AddTodoItem onItemCreated={handleOnItemCreated}></AddTodoItem>
        <br />
        <ListTodoItems items={items} onItemUpdated={handleOnItemUpdated} onRefresh={getItems}></ListTodoItems>
      </Container>
      <Footer />
      {toastSetting && (
        <ToastWrapper
          title={toastSetting?.title}
          type={toastSetting?.type}
          message={toastSetting?.message}
          onClose={() => setToastSetting(undefined)}
        />
      )}
    </div>
  )
}

export default App

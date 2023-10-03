import './App.css'
import { Container } from 'react-bootstrap'
import React, { useState, useEffect } from 'react'
import Header from './components/shared/layout/Header'
import Footer from './components/shared/layout/Footer'
import Description from './components/shared/Description'
import AddTodoItem from './components/todo-item/AddTodoItem'
import ListTodoItems from './components/todo-item/ListTodoItems'
import ToastWrapper from './components/shared/ToastWrapper'
import { getTodoItems } from './services/TodoItemsService'

const App = () => {
  const [toastSetting, setToastSetting] = useState(undefined)
  const [items, setItems] = useState([])

  useEffect(() => {
    getItems()
  }, [])

  const getItems = async () => {
    getTodoItems()
      .then((response) => {
        setItems(response.data)
      })
      .catch((error) => {
        handleOnError(`Failed to load Todo Item list, ${error.message}`)
      })
  }

  const handleonItemUpSerted = (message) => {
    setToastSetting({ type: 'success', title: 'Success', message })
    getItems()
  }

  const handleOnError = (errorMessage) => {
    setToastSetting({ type: 'danger', title: 'Error', message: errorMessage })
  }

  return (
    <div className="App">
      <Container>
        <Header />
        <Description />
        <AddTodoItem onItemCreated={handleonItemUpSerted} onItemCreatedError={handleOnError}></AddTodoItem>
        <br />
        <ListTodoItems
          items={items}
          onItemUpdated={handleonItemUpSerted}
          onItemUpdatedError={handleOnError}
          onRefresh={getItems}
        ></ListTodoItems>
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

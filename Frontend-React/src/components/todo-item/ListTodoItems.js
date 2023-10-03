import React from 'react'
import { Button, Table } from 'react-bootstrap'
import { updateTodoItem } from '../../services/TodoItemsService'

const ListTodoItems = ({ items, onItemUpdated, onItemUpdatedError, onRefresh }) => {
  const handleMarkAsComplete = async (todoItem) => {
    try {
      await updateTodoItem(todoItem.id, { ...todoItem, isCompleted: true })
      onItemUpdated(`Todo Item "${todoItem.description}" marked as completed.`)
    } catch (error) {
      onItemUpdatedError(`Failed to update Todo item, ${error.message}`)
    }
  }

  return (
    <>
      <h1>
        Showing {items?.length} Item(s){' '}
        <Button variant="primary" className="pull-right" onClick={() => onRefresh()}>
          Refresh
        </Button>
      </h1>

      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Id</th>
            <th>Description</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {items?.map((item) => (
            <tr key={item.id}>
              <td>{item.id}</td>
              <td>{item.description}</td>
              <td>
                <Button variant="warning" size="sm" onClick={() => handleMarkAsComplete(item)}>
                  Mark as completed
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    </>
  )
}

export default ListTodoItems

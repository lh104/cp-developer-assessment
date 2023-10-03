import React, { useState } from 'react'
import { Button, Container, Row, Col, Form, Stack } from 'react-bootstrap'
import RowCol from '../shared/RowCol'
import { createTodoItem } from '../../services/TodoItemsService'

const AddTodoItem = ({ onItemCreated, onItemCreatedError }) => {
  const [description, setDescription] = useState('')

  const handleDescriptionChange = (event) => {
    setDescription(event.target.value)
  }
  const handleClear = () => {
    setDescription('')
  }

  const handleAdd = async () => {
    try {
      var result = await createTodoItem({ description, isCompleted: false })
      onItemCreated(`New Todo Item "${result.data.description}" added.`)
      handleClear()
    } catch (error) {
      onItemCreatedError(`Failed to create Todo item, ${error.message}`)
    }
  }

  return (
    <RowCol>
      <Container>
        <h1>Add Item</h1>
        <Form.Group as={Row} className="mb-3" controlId="formAddTodoItem">
          <Form.Label column sm="2">
            Description
          </Form.Label>
          <Col md="6">
            <Form.Control
              type="text"
              placeholder="Enter description..."
              value={description}
              onChange={handleDescriptionChange}
            />
          </Col>
        </Form.Group>
        <Form.Group as={Row} className="mb-3 offset-md-2" controlId="formAddTodoItem">
          <Stack direction="horizontal" gap={2}>
            <Button variant="primary" onClick={() => handleAdd()}>
              Add Item
            </Button>
            <Button variant="secondary" onClick={() => handleClear()}>
              Clear
            </Button>
          </Stack>
        </Form.Group>
      </Container>
    </RowCol>
  )
}

export default AddTodoItem

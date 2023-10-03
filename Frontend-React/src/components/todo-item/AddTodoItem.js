import React, { useState } from 'react'
import { Button, Container, Row, Col, Form, Stack } from 'react-bootstrap'
import RowCol from '../shared/RowCol'

const AddTodoItem = ({ onItemCreated }) => {
  const [validated, setValidated] = useState(false)
  const [description, setDescription] = useState('')

  const handleDescriptionChange = (event) => {
    setDescription(event.target.value)
  }
  const handleClear = () => {
    setDescription('')
  }

  const handleAdd = async (event) => {
    event.preventDefault()
    event.stopPropagation()
    setValidated(true)
    if (event.currentTarget.checkValidity()) {
      onItemCreated({ description, isCompleted: false })
      handleClear()
      setValidated(false)
    }
  }

  return (
    <RowCol>
      <Container>
        <Form validated={validated} noValidate onSubmit={handleAdd}>
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
                required
              />
            </Col>
          </Form.Group>
          <Form.Group as={Row} className="mb-3 offset-md-2" controlId="formAddTodoItem">
            <Stack direction="horizontal" gap={2}>
              <Button type="submit" variant="primary">
                Add Item
              </Button>
              <Button variant="secondary" onClick={() => handleClear()}>
                Clear
              </Button>
            </Stack>
          </Form.Group>
        </Form>
      </Container>
    </RowCol>
  )
}

export default AddTodoItem

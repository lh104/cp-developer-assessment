import { render, screen, fireEvent } from '@testing-library/react'
import AddTodoItem from '../../components/todo-item/AddTodoItem'

const mockOnItemCreatedFn = jest.fn()

describe('Rendering AddTodoItem', () => {
  test('{onItemCreated} should not be called when submitting the form and the form is invalid.', () => {
    render(<AddTodoItem onItemCreated={mockOnItemCreatedFn} />)

    const addButton = screen.getByRole('button', { name: /Add Item/i })
    fireEvent.click(addButton)
    expect(mockOnItemCreatedFn).not.toHaveBeenCalled()
  })

  test('{onItemCreated} should be called when submitting the form and the form is valid.', () => {
    render(<AddTodoItem onItemCreated={mockOnItemCreatedFn} />)

    const descriptionTextbox = screen.getByRole('textbox')
    const addButton = screen.getByRole('button', { name: /Add Item/ })

    fireEvent.change(descriptionTextbox, { target: { value: 'this is an incompleted TodoItem' } })
    fireEvent.click(addButton)

    expect(mockOnItemCreatedFn).toHaveBeenCalled()
  })

  test('Description field should be empty after clicking on the "Clear" button', () => {
    render(<AddTodoItem />)
    const descriptionTextbox = screen.getByRole('textbox')
    const clearButton = screen.getByRole('button', { name: /Clear/ })

    fireEvent.change(descriptionTextbox, { target: { value: 'this is an incompleted TodoItem' } })
    fireEvent.click(clearButton)

    expect(descriptionTextbox).toHaveValue('')
  })
})

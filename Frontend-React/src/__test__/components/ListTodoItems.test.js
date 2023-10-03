import { render, screen, fireEvent } from '@testing-library/react'
import ListTodoItems from '../../components/todo-item/ListTodoItems'

const sampleTodoItems = [
  { id: '957e8b11-6ffa-4ba7-b0bc-6476041f51b0', description: 'this is an incompleted TodoItem', isCompleted: false },
  {
    id: 'ba3f5d62-b527-4028-854d-32c0aaf30329',
    description: 'this is another incompleted TodoItem',
    isCompleted: false,
  },
  {
    id: '710ca97a-33db-4e18-832c-ece4e019fffd',
    description: 'this is the thrid TodoItem',
    isCompleted: false,
  },
]

describe('Rendering ListTodoItems', () => {
  test('Number of item rendered is correct', async () => {
    render(<ListTodoItems items={sampleTodoItems} />)
    const todoItems = screen.getAllByTestId('todoItems')
    expect(todoItems.length).toEqual(sampleTodoItems.length)
  })

  test('{onRefresh} should be called after clicking on the Refresh button.', () => {
    const mockOnRefreshFn = jest.fn()
    render(<ListTodoItems items={sampleTodoItems} onRefresh={mockOnRefreshFn} />)
    const refreshButton = screen.getByText('Refresh')
    fireEvent.click(refreshButton)
    expect(mockOnRefreshFn).toHaveBeenCalled()
  })

  test('{onItemUpdated} should be called after clicking on any "Mark as completed" button.', () => {
    const mockOnItemUpdatedFn = jest.fn()
    render(<ListTodoItems items={sampleTodoItems} onItemUpdated={mockOnItemUpdatedFn} />)
    const markAsCompletedButton = screen.getAllByText('Mark as completed')[0]
    fireEvent.click(markAsCompletedButton)
    expect(mockOnItemUpdatedFn).toHaveBeenCalled()
  })
})

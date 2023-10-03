import React from 'react'
import { Toast } from 'react-bootstrap'

const ToastWrapper = ({ type, title, message, onClose }) => {
  return (
    <Toast onClose={onClose} className="to-do-item-toast" bg={type} delay={8000} autohide animation>
      <Toast.Header>
        <strong className="me-auto">{title}</strong>
      </Toast.Header>
      <Toast.Body>{message}</Toast.Body>
    </Toast>
  )
}

export default ToastWrapper

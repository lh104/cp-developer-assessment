import React from 'react'
import { Col, Row } from 'react-bootstrap'

const RowCol = ({ children }) => {
  return (
    <Row>
      <Col>{children}</Col>
    </Row>
  )
}

export default RowCol

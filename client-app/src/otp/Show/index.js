import React from "react"
import {
  Heading,
  HeadingLevel
} from 'baseui/heading'
import {
  Card,
  StyledBody,
} from "baseui/card";
import ExpirationTimer from "./ExpirationTimer";
import Centered from "../../components/Centered";

export default function Show({
  password,
  expiresAt,
}) {
  return (
    <Card title="Your one time password is">
      <StyledBody>
          <Centered>
            <HeadingLevel>

              <Heading>{password}</Heading>
            </HeadingLevel>
            <ExpirationTimer expirationDate={expiresAt} />
          </Centered>
      </StyledBody>
    </Card>
  )
}
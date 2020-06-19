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

export default function Show() {
  return (
    <Card title="Your one time password is">
      <StyledBody>
          <Centered>
            <HeadingLevel>

              <Heading>123456</Heading>
            </HeadingLevel>
            <ExpirationTimer expirationDate={new Date(new Date().getTime() + 30000)} />
          </Centered>
      </StyledBody>
    </Card>
  )
}
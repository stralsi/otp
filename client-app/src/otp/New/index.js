import React from "react"
import { FormControl } from "baseui/form-control";
import { Input } from "baseui/input";
import {
  Card,
  StyledBody,
} from "baseui/card";
import Centered from "../../components/Centered";

export default function New() {
  return (
    <Card title="Type your user id">
      <StyledBody>
        <Centered>

          <FormControl
            label="UserId"
          >
            <Input />
          </FormControl>
        </Centered>
      </StyledBody>
    </Card>
  )
}
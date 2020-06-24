import React from "react";
import { FormControl } from "baseui/form-control";
import { PinCode } from "baseui/pin-code";
import {
  Card,
  StyledBody,
} from "baseui/card";
import Centered from "../../components/Centered";

export default function Verify() {
  const [values, setValues] = React.useState(["", "", "", "", "", ""]);

  return (
    <Card title="Type your password">
      <StyledBody>
        <Centered>
          <FormControl
            label="Password"
          >
            <PinCode
              values={values}
              onChange={({ values }) => setValues(values)}
            />
          </FormControl>
        </Centered>
      </StyledBody>
    </Card>
  )
}
import React, { useState } from "react"
import {
  Card,
  StyledBody,
} from "baseui/card";
import {
  Button
} from "baseui/button";
import Centered from "../../components/Centered";
import { Input } from "baseui/input";
import { FormControl } from "baseui/form-control";
import {
  create
} from "./services";

export default function New({
  onPasswordReceived = () => {},
  now = () => new Date(),
}) {
  const [loginId, setLoginId] = useState("");
  const [error, setError] = useState();

  async function handleChange(event) {
    setLoginId(event.target.value);
  }

  async function handleSubmit(event) {
    event.preventDefault();
    const response = await create({ loginId, createdAt: now().toISOString() });
    if (response.ok) {
      var result = await response.json();
      onPasswordReceived(result.oneTimePassword, new Date(result.expiresAt))
    } else {
      if (response.status === 401) {
        setError("Invalid username");
      } else {
        // connection error
        // server error
      }
    }
  }

  return (
    <Card title="Type your login id">
      <StyledBody>
        <Centered>
          <form onSubmit={handleSubmit}>
            <FormControl
              label="Login Id"
              caption={error}
              error={!!error}
            >
              <Input
                id="loginId"
                name="loginId"
                value={loginId}
                onChange={handleChange}
                error={!!error}
                autoComplete="off"
              />
            </FormControl>
            <Button type="submit">Submit</Button>
          </form>
        </Centered>
      </StyledBody>
    </Card>
  )
}

import React from "react";
import New from "./index";
import { render, getByText, waitForDomChange, wait } from "@testing-library/react";
import userEvent from '@testing-library/user-event'

jest.mock("./services", () => ({
  create: jest.fn(async () => ({
    ok:true,
    json: () => ({})
  }))
}))

import services from "./services";

describe("New", () => {
  it("renders the 'Login Id' input", () => {
    const {getByLabelText} = render(<New />)
    const input = getByLabelText(/Login Id/i);
    expect(input).toBeInTheDocument();
  });

  it("submits the value of the input and notifies password received", async () => {
    const testDate = new Date("2020-01-01T00:00:00.000Z");

    const {getByLabelText, getByText} = render(<New now={() => testDate} />)
    const input = getByLabelText(/Login Id/i);
    await userEvent.type(input, "mary")
    userEvent.click(getByText("Submit"));
    
    expect(services.create).toHaveBeenCalledWith({loginId: "mary", createdAt: "2020-01-01T00:00:00.000Z"});
  });

  it("submits the value of the input and notifies password received", async () => {
    const testDate = new Date("2020-01-01T00:00:00.000Z");
    const handlePasswordReceived = jest.fn();
    services.create.mockImplementation(async () => ({
      ok:true,
      json: () => ({
        oneTimePassword: "foo",
        expiresAt: "2020-01-01T00:30:00.000Z",
      })
    }));

    const {getByLabelText, getByText} = render(<New onPasswordReceived={handlePasswordReceived} now={() => testDate} />)
    const input = getByLabelText(/Login Id/i);
    await userEvent.type(input, "mary")
    userEvent.click(getByText("Submit"));
    await wait(() => {
      expect(handlePasswordReceived).toHaveBeenCalledWith("foo", new Date("2020-01-01T00:30:00.000Z"));
    });
  });  

  it("shows error if status 401", async () => {
    services.create.mockImplementation(async () => ({
      ok:false,
      status: 401,
    }));

    const {getByLabelText, getByText} = render(<New />)
    const input = getByLabelText(/Login Id/i);
    await userEvent.type(input, "mary")
    userEvent.click(getByText("Submit"));
    await waitForDomChange();
    expect(getByText(/invalid/i)).toBeInTheDocument();
  });
});
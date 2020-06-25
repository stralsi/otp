import React from "react";
import {
  render,
} from "@testing-library/react";
import Show from "./index";

describe("Show", () => {
  it("displays password", () => {
    const {getByText} = render(<Show password="123456" expiresAt={new Date()}/>)
    const passwordText = getByText("123456");
    expect(passwordText).toBeInTheDocument();
  });

  it("displays seconds until expiration", () => {
    const sut = <Show
      password="123456"
      expiresAt={new Date("2020-01-01T00:00:20.000Z")}
      now={() => new Date("2020-01-01T00:00:00.000Z")}
      />
    const {getByText} = render(sut);
    const expirationText = getByText(/valid for 20s/i);
    expect(expirationText).toBeInTheDocument();
  });

  it("displays expired when expired", () => {
    const sut = <Show
      password="123456"
      expiresAt={new Date("2020-01-01T00:00:00.000Z")}
      now={() => new Date("2020-01-01T00:00:20.000Z")}
      />
    const {getByText} = render(sut);
    const expirationText = getByText(/expired/i);
    expect(expirationText).toBeInTheDocument();
  });
});
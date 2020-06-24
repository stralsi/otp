import React, { useState } from 'react';
import { Router } from "@reach/router";
import {Client as Styletron} from 'styletron-engine-atomic';
import { Provider as StyletronProvider } from 'styletron-react';
import { LightTheme, BaseProvider } from 'baseui';

import New from "./otp/New";
import Show from "./otp/Show";
import Verify from "./otp/Verify";
import NotFound from "./NotFound";

import Centered from "./components/Centered";

const engine = new Styletron();

function App() {
  const [password, setPassword] = useState(); // this is vulnerable to xss, can I use a cookie instead?
  const [expiresAt, setExpiresAt] = useState();

  function handlePasswordReceived(p, exp) {
    console.log("password received", p, exp);
    setPassword(p);
    setExpiresAt(exp);
  }

  return (
    <StyletronProvider value={engine}>
      <BaseProvider theme={LightTheme}>
        <Centered>
          <Router>
            {
              password
              ? <Show path="/" password={password} expiresAt={expiresAt} /> 
              : <New path="/" onPasswordReceived={handlePasswordReceived} /> 
            }
            <Verify path="/verify" />
            <NotFound default />
          </Router>
        </Centered>
      </BaseProvider>
    </StyletronProvider>
  );
}

export default App;

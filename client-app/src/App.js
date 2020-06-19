import React from 'react';
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
  return (
    <StyletronProvider value={engine}>
      <BaseProvider theme={LightTheme}>
        <Centered>
          <Router>
            <New path="/" />
            <Show path="/show" />
            <Verify path="/verify" />
            <NotFound default />
          </Router>
        </Centered>
      </BaseProvider>
    </StyletronProvider>
  );
}

export default App;

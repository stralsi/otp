import React from 'react';
import {Router} from "@reach/router";

import New from "./otp/New";
import Show from "./otp/Show";
import Verify from "./otp/Verify";
import NotFound from "./NotFound";

function App() {
  return (
    <div className="App">
      <Router>
        <New path="/new" />
        <Show path="/show" />
        <Verify path="/verify" />
        <NotFound default />
      </Router>
    </div>
  );
}

export default App;

import React from "react";
import "./App.css";
import "./styles/global.css";
import { Routes } from "./routes/config";
import { GlobalStyles } from "./styles/index";
import { AuthProvider } from "./authentication/use-auth";

//This is almost the base level of your app.  You can also put global things here, but it allows you to escape them.
function App() {
  return (
    <div className="App">
      <GlobalStyles />
      <AuthProvider>
        <Routes />
      </AuthProvider>
    </div>
  );
}

export default App;

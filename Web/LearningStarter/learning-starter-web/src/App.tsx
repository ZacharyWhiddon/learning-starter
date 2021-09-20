import React from "react";
import "./App.css";
import { Routes } from "./routes/config";
import { GlobalStyles } from "./styles/index";
import { AuthProvider } from "./authentication/use-auth";

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

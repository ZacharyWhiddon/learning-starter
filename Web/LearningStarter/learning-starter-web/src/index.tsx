import React from "react";
import ReactDOM from "react-dom";
import "./index.css";
import App from "./App";
import { BrowserRouter as Router } from "react-router-dom";
import "semantic-ui-css/semantic.min.css";

//This is the base level of your app.
//This is where you would put global things (like Router)
ReactDOM.render(
  <Router>
    <App />
  </Router>,
  document.getElementById("root")
);

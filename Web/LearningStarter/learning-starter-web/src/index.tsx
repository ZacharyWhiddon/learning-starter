import ReactDOM from "react-dom/client";
import App from "./App";
import { BrowserRouter as Router } from "react-router-dom";

//This is the base level of your app.
//This is where you would put global things (like Router)

const container = document.getElementById("root");

// Create a root.
const root = ReactDOM.createRoot(container as HTMLElement);

root.render(
  <Router>
    <App />
  </Router>
);

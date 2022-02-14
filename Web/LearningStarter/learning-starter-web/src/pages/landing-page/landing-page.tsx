import React from "react";
import { Header } from "semantic-ui-react";
import "./landing-page.css";

//This is a basic Component, and since it is used inside of
//'../../routes/config.tsx' line 31, that also makes it a page
export const LandingPage = () => {
  return (
    <div className="home-page-container">
      <Header>Home Page</Header>
    </div>
  );
};

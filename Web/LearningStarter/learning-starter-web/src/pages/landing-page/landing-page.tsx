import { Container, Header } from "@mantine/core";
import React from "react";
import "./landing-page.css";

//This is a basic Component, and since it is used inside of
//'../../routes/config.tsx' line 31, that also makes it a page
export const LandingPage = () => {
  return (
    <Container className="home-page-container">
      <Header height={60}>Home Page</Header>
    </Container>
  );
};

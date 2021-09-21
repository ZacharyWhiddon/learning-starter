import React from "react";
import { useUser } from "../../authentication/use-auth";
import { Header, Container, Divider } from "semantic-ui-react";

export const UserPage = () => {
  const user = useUser();
  return (
    <>
      <Header>User Information</Header>
      <Container textAlign="left">
        <Header size="medium">First Name</Header>
        <p>{user.firstName}</p>
        <Divider />
        <Header size="medium">Last Name</Header>
        <p>{user.lastName}</p>
      </Container>
    </>
  );
};

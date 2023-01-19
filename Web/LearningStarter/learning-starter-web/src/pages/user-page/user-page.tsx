import { Container, createStyles, Divider, Text } from "@mantine/core";
import React from "react";
import { useUser } from "../../authentication/use-auth";
import "./user-page.css";

export const UserPage = () => {
  const user = useUser();
  const { classes } = useStyles();
  return (
    <Container className="user-page-container">
      <Container>
        <Text size="lg">User Information</Text>
        <Container className={classes.textAlignLeft}>
          <Text size="md">First Name</Text>
          <p>{user.firstName}</p>
          <Divider />
          <Text size="md">Last Name</Text>
          <p>{user.lastName}</p>
        </Container>
      </Container>
    </Container>
  );
};

const useStyles = createStyles(() => {
  return {
    textAlignLeft: {
      textAlign: "left",
    },
  };
});

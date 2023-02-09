import { Container, createStyles, Text } from "@mantine/core";

//This is a basic Component, and since it is used inside of
//'../../routes/config.tsx' line 31, that also makes it a page
export const LandingPage = () => {
  const { classes } = useStyles();
  return (
    <Container className={classes.homePageContainer}>
      <Text size="lg">Home Page</Text>
    </Container>
  );
};

const useStyles = createStyles(() => {
  return {
    homePageContainer: {
      display: "flex",
      justifyContent: "center",
    },
  };
});

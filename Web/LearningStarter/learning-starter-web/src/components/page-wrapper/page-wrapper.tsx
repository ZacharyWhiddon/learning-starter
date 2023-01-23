import { UserDto } from "../../constants/types";
import { PrimaryNavigation } from "../navigation/navigation";
import { Container } from "@mantine/core";
import { createStyles } from "@mantine/core";

type PageWrapperProps = {
  user?: UserDto;
  children?: React.ReactNode;
};

//This is the wrapper that surrounds every page in the app.  Changes made here will be reflect all over.
export const PageWrapper: React.FC<PageWrapperProps> = ({ user, children }) => {
  const { classes } = useStyles();
  return (
    <div className="content">
      <PrimaryNavigation user={user} />
      <Container px={0} fluid className={classes.mainContent}>
        {children}
      </Container>
    </div>
  );
};

const useStyles = createStyles(() => {
  return {
    mainContent: {
      marginTop: "10px",
    },
  };
});

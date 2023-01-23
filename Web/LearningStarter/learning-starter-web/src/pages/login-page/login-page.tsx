import { ApiResponse } from "../../constants/types";
import { useAsyncFn } from "react-use";
import { PageWrapper } from "../../components/page-wrapper/page-wrapper";
import { FormErrors, useForm } from "@mantine/form";
import {
  Alert,
  Button,
  Container,
  createStyles,
  Input,
  Text,
} from "@mantine/core";
import api from "../../config/axios";
import { showNotification } from "@mantine/notifications";

const baseUrl = process.env.REACT_APP_API_BASE_URL;

type LoginRequest = {
  userName: string;
  password: string;
};

type LoginResponse = ApiResponse<boolean>;

//This is a *fairly* basic form
//The css used in here is a good example of how flexbox works in css
//For more info on flexbox: https://css-tricks.com/snippets/css/a-guide-to-flexbox/
export const LoginPage = ({
  fetchCurrentUser,
}: {
  fetchCurrentUser: () => void;
}) => {
  const { classes } = useStyles();

  const form = useForm<LoginRequest>({
    initialValues: {
      userName: "",
      password: "",
    },
    validate: {
      userName: (value) =>
        value.length <= 0 ? "Username must not be empty" : null,
      password: (value) =>
        value.length <= 0 ? "Password must not be empty" : null,
    },
  });

  const [, submitLogin] = useAsyncFn(async (values: LoginRequest) => {
    if (baseUrl === undefined) {
      return;
    }

    const response = await api.post<LoginResponse>(`/api/authenticate`, values);
    if (response.data.hasErrors) {
      const formErrors: FormErrors = response.data.errors.reduce(
        (prev, curr) => {
          Object.assign(prev, { [curr.property]: curr.message });
          return prev;
        },
        {} as FormErrors
      );
      form.setErrors(formErrors);
    }

    if (response.data.data) {
      showNotification({ message: "Successfully Logged In!", color: "green" });
      fetchCurrentUser();
    }
  }, []);

  return (
    <PageWrapper>
      <Container>
        <Container px={0}>
          {form.errors[""] && (
            <Alert className={classes.generalErrors} color="red">
              <Text>{form.errors[""]}</Text>
            </Alert>
          )}
          <form onSubmit={form.onSubmit(submitLogin)}>
            <Container px={0}>
              <Container className={classes.formField} px={0}>
                <Container px={0}>
                  <label htmlFor="userName">Username</label>
                </Container>
                <Input {...form.getInputProps("userName")} />
                <Text c="red">{form.errors["userName"]}</Text>
              </Container>
              <Container className={classes.formField} px={0}>
                <Container px={0}>
                  <label htmlFor="password">Password</label>
                </Container>
                <Input type="password" {...form.getInputProps("password")} />
                <Text c="red">{form.errors["password"]}</Text>
              </Container>

              <Container px={0}>
                <Button className={classes.loginButton} type="submit">
                  Login
                </Button>
              </Container>
            </Container>
          </form>
        </Container>
      </Container>
    </PageWrapper>
  );
};

const useStyles = createStyles(() => {
  return {
    generalErrors: {
      marginBottom: "8px",
    },

    loginButton: {
      marginTop: "8px",
    },

    formField: {
      marginBottom: "8px",
    },
  };
});

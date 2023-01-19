import "./login-page.css";
import axios from "axios";
import React from "react";
import { ApiResponse } from "../../constants/types";
import { useAsyncFn } from "react-use";
import { PageWrapper } from "../../components/page-wrapper/page-wrapper";
import { loginUser } from "../../authentication/authentication-services";
import { FormErrors, useForm } from "@mantine/form";
import { Button, Input, Text } from "@mantine/core";
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
export const LoginPage = () => {
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
      loginUser();
    }
  }, []);

  return (
    <PageWrapper>
      <div className="flex-box-centered-content-login-page">
        <div className="login-form">
          <form onSubmit={form.onSubmit(submitLogin)}>
            <div>
              <div>
                <div className="field-label">
                  <label htmlFor="userName">UserName</label>
                </div>
                <Input {...form.getInputProps("userName")} />
                <Text c="red">{form.errors["userName"]}</Text>
              </div>
              <div>
                <div className="field-label">
                  <label htmlFor="password">Password</label>
                </div>
                <Input type="password" {...form.getInputProps("password")} />
                <Text c="red">{form.errors["password"]}</Text>
              </div>
              <Text c="red">{form.errors[""]}</Text>
              <div className="button-container-login-page">
                <Button className="login-button" type="submit">
                  Login
                </Button>
              </div>
            </div>
          </form>
        </div>
      </div>
    </PageWrapper>
  );
};

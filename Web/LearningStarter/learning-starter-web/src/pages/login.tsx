import axios from "axios";
import React, { useMemo } from "react";
import { ApiResponse } from "../constants/types";
import { notify } from "../hooks/use-subscription";
import { Formik, Form, Field } from "formik";
import { Button, Input } from "semantic-ui-react";
import { useAsyncFn } from "react-use";

const baseUrl = process.env.REACT_APP_API_BASE_URL;

type LoginRequest = {
  userName: string;
  password: string;
};

type LoginResponse = ApiResponse<boolean>;

type FormValues = LoginRequest;

export const LoginPage = () => {
  const initialValues = useMemo<FormValues>(
    () => ({
      userName: "",
      password: "",
    }),
    []
  );
  const [, submitLogin] = useAsyncFn(async (values: LoginRequest) => {
    if (baseUrl === undefined) {
      return;
    }

    // eslint-disable-next-line no-restricted-syntax
    console.log("debug values:", values);

    axios
      .post<LoginResponse>(`${baseUrl}/api/authenticate`, values)
      .then((response) => {
        if (response.data.data) {
          console.log("Successfully Logged In!");
          notify("user-login", undefined);
        }
      })
      .catch((e) => {
        console.log(e);
      });
  }, []);

  return (
    <Formik initialValues={initialValues} onSubmit={submitLogin}>
      <Form>
        <div>
          <label htmlFor="userName">UserName</label>
          <Field id="userName" name="userName" placeHolder="Username" />
        </div>
        <div>
          <label htmlFor="password">Password</label>
          <Field id="password" name="password" placeHolder="Password" />
        </div>
        <div>
          <Button type="submit">Submit</Button>
        </div>
      </Form>
    </Formik>
  );
};

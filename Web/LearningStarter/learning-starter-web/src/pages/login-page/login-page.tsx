import "./login-page.css";
import axios from "axios";
import React, { useMemo } from "react";
import { ApiResponse } from "../../constants/types";
import { Formik, Form, Field } from "formik";
import { Button } from "semantic-ui-react";
import { useAsyncFn } from "react-use";
import { PageWrapper } from "../../components/page-wrapper/page-wrapper";
import { loginUser } from "../../authentication/authentication-services";

const baseUrl = process.env.REACT_APP_API_BASE_URL;

type LoginRequest = {
  userName: string;
  password: string;
};

type LoginResponse = ApiResponse<boolean>;

type FormValues = LoginRequest;

//This is a *fairly* basic form
//The css used in here is a good example of how flexbox works in css
//For more info on flexbox: https://css-tricks.com/snippets/css/a-guide-to-flexbox/
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

    axios
      .post<LoginResponse>(`${baseUrl}/api/authenticate`, values)
      .then((response) => {
        if (response.data.data) {
          console.log("Successfully Logged In!");
          loginUser();
        }
      })
      .catch((e) => {
        console.log(e);
      });
  }, []);

  return (
    <PageWrapper>
      <div className="flex-box-centered-content-login-page">
        <div className="login-form">
          <Formik initialValues={initialValues} onSubmit={submitLogin}>
            <Form>
              <div>
                <div>
                  <div className="field-label">
                    <label htmlFor="userName">UserName</label>
                  </div>
                  <Field className="field" id="userName" name="userName" />
                </div>
                <div>
                  <div className="field-label">
                    <label htmlFor="password">Password</label>
                  </div>
                  <Field className="field" id="password" name="password" />
                </div>
                <div className="button-container-login-page">
                  <Button className="login-button" type="submit">
                    Login
                  </Button>
                </div>
              </div>
            </Form>
          </Formik>
        </div>
      </div>
    </PageWrapper>
  );
};

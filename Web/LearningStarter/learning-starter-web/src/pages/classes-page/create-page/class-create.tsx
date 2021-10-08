import "./class-create.css";
import axios from "axios";
import React, { useMemo } from "react";
import { Formik, Form, Field } from "formik";
import { Button } from "semantic-ui-react";
import { useHistory } from "react-router-dom";
import { useAsyncFn } from "react-use";
import { ApiResponse, ClassDto } from "../../../constants/types";
import { routes } from "../../../routes/config";

const baseUrl = process.env.REACT_APP_API_BASE_URL;

type CreateClassRequest = Omit<ClassDto, "user" | "id">;

type CreateClassResponse = ApiResponse<ClassDto>;

type FormValues = CreateClassRequest;

export const ClassCreatePage = () => {
  const history = useHistory();
  const initialValues = useMemo<FormValues>(
    () => ({
      subject: "",
      capacity: 0,
      userId: 0,
    }),
    []
  );
  const [, submitCreate] = useAsyncFn(async (values: CreateClassRequest) => {
    if (baseUrl === undefined) {
      return;
    }
    values.capacity = Number(values.capacity);
    values.userId = Number(values.userId);
    console.log("Values: ", values);

    const createdClass = await axios
      .post<CreateClassResponse>(`${baseUrl}/api/classes/create`, values)
      .then((response) => {
        if (response.data.hasErrors) {
          response.data.errors.forEach((err) => {
            console.error(`${err.property}: ${err.message}`);
          });
        }
        console.log("Successfully Created Class");
        alert("Successfully Created");
        history.push(routes.classes);
      })
      .catch((e) => {
        console.log(e);
        return null;
      });

    return createdClass;
  }, []);

  return (
    <div className="flex-box-centered-content">
      <div className="create-class-form">
        <Formik initialValues={initialValues} onSubmit={submitCreate}>
          <Form>
            <div>
              <div>
                <div className="field-label">
                  <label htmlFor="subject">Subject</label>
                </div>
                <Field className="field" id="subject" name="subject" />
              </div>
              <div>
                <div className="field-label">
                  <label htmlFor="capacity">Capacity</label>
                </div>
                <Field className="field" id="capacity" name="capacity" />
              </div>
              <div>
                <div className="field-label">
                  <label htmlFor="userId">User Id</label>
                </div>
                <Field className="field" id="userId" name="userId" />
              </div>
              <div className="button-container">
                <Button className="create-button" type="submit">
                  Create
                </Button>
              </div>
            </div>
          </Form>
        </Formik>
      </div>
    </div>
  );
};

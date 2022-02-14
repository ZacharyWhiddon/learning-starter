import "./class-listing.css";
import axios from "axios";
import React from "react";
import { useAsync } from "react-use";
import { Segment } from "semantic-ui-react";
import { ApiResponse, ClassDto } from "../../../constants/types";

const baseUrl = process.env.REACT_APP_API_BASE_URL;

export const ClassListing = () => {
  const classes = useAsync(async () => {
    const response = await axios.get<ApiResponse<ClassDto[]>>(
      `${baseUrl}/api/classes`
    );

    if (response.data.hasErrors) {
      response.data.errors.forEach((err) => {
        console.error(`${err.property}: ${err.message}`);
      });
      return response.data.data;
    }

    return response.data.data;
  }, []);

  const classesToShow = classes.value;

  return (
    <div className="flex-box-centered-content-class-listing">
      {classesToShow &&
        classesToShow.map((x: ClassDto) => {
          return (
            <div className="flex-row-fill-class-listing">
              <Segment className="class-listing-segments">
                <div>{`Subject: ${x.subject}`}</div>
                <div>{`Capacity: ${x.capacity}`}</div>
                <div>{`User First Name: ${x.user.firstName}`}</div>
                <div>{`User Last Name: ${x.user.lastName}`}</div>
              </Segment>
            </div>
          );
        })}
    </div>
  );
};

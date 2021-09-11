import axios from "axios";
import { env } from "process";
import { ApiResponse } from "../constants/types";
import { notify } from "../hooks/use-subscription";

const baseUrl = env.REACT_APP_API_BASE_URL;

type LoginRequest = {
  userName: string;
  password: string;
};

type LoginResponse = ApiResponse<boolean>;

export const LoginPage = () => {
  const submitLogin = (values: LoginRequest) => {
    if (baseUrl === undefined) {
      return;
    }

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
  };
};

import { showNotification } from "@mantine/notifications";
import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from "axios";
import { ApiResponse } from "../constants/types";
import { Env } from "./env-vars";

const axiosInstance = axios.create({
  baseURL: Env.apiBaseUrl,
});

const errorHandlers = {
  "400": (response) => {
    showNotification({
      title: "Bad Request",
      message: "u messed up",
      color: "red",
    });
    console.log("Bad Request.");
    return Promise.resolve(response);
  },
  "401": (response) => {
    return Promise.resolve({
      data: null,
      hasErrors: true,
      errors: [
        {
          property: "",
          message: "Sign in.",
        },
      ],
    } as ApiResponse<any>);
  },
  "403": (response) => {
    showNotification({
      title: "Unauthorized",
      message: "You are not authorized to perform this action",
      color: "red",
    });
  },
  "500": (response) => {
    showNotification({
      title: "Error",
      message: "We've encountered a problem.",
      color: "red",
    });
    return Promise.resolve(response);
  },
};

export async function handleResponseError(error: AxiosError) {
  if (error.response) {
    const response: AxiosResponse = error.response;
    const handler = errorHandlers[response.status];
    if (handler) {
      const result = await handler(error.response);
      if (result) {
        return result;
      }
    }
  }
}

const baseUrl = Env.apiBaseUrl;

axiosInstance.interceptors.response.use((x: any) => x, handleResponseError);

function post<T>(route: string, data: any) {
  var url = baseUrl + route;
  return axiosInstance.post<T>(url, data);
}

function get<T>(route: string) {
  var url = baseUrl + route;
  return axiosInstance.get<T>(url);
}

function put<T>(route: string, data: any) {
  var url = baseUrl + route;
  return axiosInstance.put<T>(url, data);
}

function remove<T>(route: string) {
  var url = baseUrl + route;
  return axiosInstance.delete<T>(route);
}

type Api = {
  post<T>(route: string, data: any): Promise<AxiosResponse<T>>;
  get<T>(url: string): Promise<AxiosResponse<T>>;
  delete<T>(route: string): Promise<AxiosResponse<T>>;
  put<T>(route: string, data: any): Promise<AxiosResponse<T>>;
};

const api = {} as Api;

api.get = get;
api.put = put;
api.post = post;
api.delete = remove;

export default api;

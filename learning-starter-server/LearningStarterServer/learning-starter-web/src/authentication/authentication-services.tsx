import React from "react";
import { notify } from "../hooks/use-subscription";

export const authenticationService = {
  login,
  logout,
};

const basePath = "";

function login(username: string, password: string) {
  const requestOptions = {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ username, password }),
  };

  return fetch(`${basePath}/api/users/authenticate`, requestOptions)
    .then(handleResponse)
    .then((user) => {
      // store user details and jwt token in local storage to keep user logged in between page refreshes
      notify("user-login", user);
      localStorage.setItem("currentUser", JSON.stringify(user));

      return user;
    });
}

function logout() {
  // remove user from local storage to log user out
  localStorage.removeItem("currentUser");
}

function handleResponse(response: Response) {
  return response.text().then((text) => {
    const data = text && JSON.parse(text);
    if (!response.ok) {
      if ([401, 403].indexOf(response.status) !== -1) {
        // auto logout if 401 Unauthorized or 403 Forbidden response returned from api
        authenticationService.logout();
        window.location.reload();
      }

      const error = (data && data.message) || response.statusText;
      return Promise.reject(error);
    }

    return data;
  });
}

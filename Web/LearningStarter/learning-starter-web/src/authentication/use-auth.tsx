import { createContext, useContext, useState } from "react";
import { useAsyncRetry, useAsyncFn } from "react-use";
import { ApiResponse } from "../constants/types";
import { ApiError } from "../constants/types";
import { LoginPage } from "../pages/login-page/login-page";
import { UserDto } from "../constants/types";
import { StatusCodes } from "../constants/status-codes";
import { Loader } from "@mantine/core";
import api from "../config/axios";

const currentUser = "currentUser";

//functions for setting session storage
const setUserItem = (user: UserDto) => {
  sessionStorage.setItem(currentUser, JSON.stringify(mapUser(user)));
};

const removeUserItem = () => {
  sessionStorage.removeItem(currentUser);
};

type AuthState = {
  user: UserDto | null;
  errors: ApiError[];
  refetchUser: () => void;
  logout: () => void;
};

const INITIAL_STATE: AuthState = {
  user: null,
  errors: [],
  refetchUser: undefined as any,
  logout: undefined as any,
};

export const AuthContext = createContext<AuthState>(INITIAL_STATE);

export const AuthProvider = (props: any) => {
  const [errors, setErrors] = useState<ApiError[]>(INITIAL_STATE.errors);
  const [user, setUser] = useState<UserDto | null>(INITIAL_STATE.user);

  //This is the main function for getting the user information from the database.
  //This function gets called on every "notify("user-login") in order to refetch the user data."
  const fetchCurrentUser = useAsyncRetry(async () => {
    setErrors([]);

    const response = await api.get<GetUserResponse>(`/api/get-current-user`);

    if (response.data.hasErrors) {
      response.data.errors.forEach((err) => {
        console.error(err.message);
      });
      return response.data;
    }

    //Updating the state of the context to have the user data as well as any errors.
    setUser(response.data.data);
    setErrors(response.data.errors);

    //Setting the session storage item of the user.
    setUserItem(response.data.data);
  }, []);

  //Same deal as login.  This function is used to call the logout endpoint
  const [, logoutUser] = useAsyncFn(async () => {
    setErrors([]);

    //Setting up axios call
    const response = await api.post(`/api/logout`);

    if (response.status !== StatusCodes.OK) {
      console.log(`Error on logout: ${response.statusText}`);
      return response;
    }

    console.log("Successfully Logged Out!");

    if (response.status === StatusCodes.OK) {
      removeUserItem();
      setUser(null);
    }

    return response;
  }, []);

  //This returns a Loading screen if the API call takes a long time to get user info
  if (fetchCurrentUser.loading) {
    return <Loader />;
  }

  //Brings unauthenticated users to the login page.
  //This can be made to bring them to a different part of the app eventually
  if (!user && !fetchCurrentUser.loading) {
    return <LoginPage fetchCurrentUser={fetchCurrentUser.retry} />;
  }

  //Once they are logged in and not loading, it brings them to the app.
  return (
    <AuthContext.Provider
      value={{
        user,
        errors,
        refetchUser: fetchCurrentUser.retry,
        logout: logoutUser,
      }}
      {...props}
    />
  );
};

export type GetUserResponse = ApiResponse<UserDto>;

export function useAuth(): AuthState {
  return useContext(AuthContext);
}

//This function is available anywhere wrapped inside of the <AuthProvider>.  See Config.tsx for example.
export function useUser(): UserDto {
  const { user } = useContext(AuthContext);
  if (!user) {
    throw new Error(`useUser must be used within an authenticated app`);
  }
  return user;
}

//This is used to map an object (any type) to a User entity.
export const mapUser = (user: any): UserDto => ({
  id: user.id,
  firstName: user.firstName,
  lastName: user.lastName,
  userName: user.userName,
});

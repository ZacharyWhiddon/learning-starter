import axios from "axios";
import React, { createContext, useContext } from "react";
import { useAsyncRetry, useAsyncFn } from "react-use";
import { Dimmer, Loader } from "semantic-ui-react";
import { ApiResponse } from "../constants/types";
import { useSubscription } from "../hooks/use-subscription";
import { useProduce } from "../hooks/use-produce";
import { Error } from "../constants/types";
import { LoginPage } from "../pages/login-page/login-page";
import { UserDto } from "../constants/types";
import { StatusCodes } from "../constants/status-codes";

const currentUser = "currentUser";
const baseUrl = process.env.REACT_APP_API_BASE_URL;

//functions for setting session storage
const setUserItem = (user: UserDto) => {
  sessionStorage.setItem(currentUser, JSON.stringify(mapUser(user)));
};

const removeUserItem = () => {
  sessionStorage.removeItem(currentUser);
};

type AuthState = {
  user: UserDto | null;
  errors: Error[];
  redirectUrl?: string | null;
};

const INITIAL_STATE: AuthState = {
  user: null,
  errors: [],
  redirectUrl: null,
};

export const AuthContext = createContext<AuthState>(INITIAL_STATE);

export const AuthProvider = (props: any) => {
  const [state, setState] = useProduce<AuthState>(INITIAL_STATE);

  //This is the main function for getting the user information from the database.
  //This function gets called on every "notify("user-login") in order to fetfch the user data."
  const fetchCurrentUser = useAsyncRetry(async () => {
    setState((draft) => {
      draft.errors = [];
    });

    const response = await axios.get<GetUserResponse>(
      `${baseUrl}/api/get-current-user`
    );

    if (response.data.hasErrors) {
      response.data.errors.forEach((err) => {
        console.error(err.message);
      });
      return response.data;
    }

    //Updating the state of the context to have the user data as well as any errors.
    setState((draft) => {
      draft.user = response.data.data;
      draft.errors = response.data.errors;
    });

    //Setting the session storage item of the user.
    setUserItem(response.data.data);
  }, [setState]);

  //Same deal as login.  This function is used to call the logout endpoint
  const [, logoutUser] = useAsyncFn(async () => {
    setState((draft) => {
      draft.errors = [];
    });

    //Setting up axios call
    const response = await axios.post(`${baseUrl}/api/logout`);

    if (response.status !== StatusCodes.OK) {
      console.log(`Error on logout: ${response.statusText}`);
      return response;
    }

    console.log("Successfully Logged Out!");

    if (response.status === StatusCodes.OK) {
      removeUserItem();
      setState((draft) => {
        draft.user = null;
      });
    }

    return response;
  }, []);

  //This listens for any "notify("user-login") and performs the action specified."
  useSubscription("user-login", () => {
    fetchCurrentUser.retry();
  });

  //This listens for any "notify("user-logout") and performs the action specified."
  useSubscription("user-logout", () => {
    logoutUser();
  });

  //This returns a Loading screen if the API call takes a long time to get user info
  if (fetchCurrentUser.loading) {
    return (
      <Dimmer active inverted>
        <Loader indeterminate />
      </Dimmer>
    );
  }

  //Brings unauthenticated users to the login page.
  //This can be made to bring them to a different part of the app eventually
  if (!state.user && !fetchCurrentUser.loading) {
    return <LoginPage />;
  }

  //Once they are logged in and not loading, it brings them to the app.
  return <AuthContext.Provider value={state} {...props} />;
};

type GetUserResponse = ApiResponse<UserDto>;

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

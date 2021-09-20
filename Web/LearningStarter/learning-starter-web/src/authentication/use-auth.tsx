import axios from "axios";
import React, { createContext, useContext, useEffect } from "react";
import { useAsyncRetry } from "react-use";
import { Dimmer, Loader } from "semantic-ui-react";
import { ApiResponse } from "../constants/types";
import { useSubscription } from "../hooks/use-subscription";
import { useProduce } from "../hooks/use-produce";
import { Error } from "../constants/types";
import { LoginPage } from "../pages/login";
import { User } from "../types/index";

const currentUser = "currentUser";
const baseUrl = process.env.REACT_APP_API_BASE_URL;

const getUserItem = () => {
  const userString = sessionStorage.getItem(currentUser);
  if (userString === null) {
    return null;
  }
  const user: User = JSON.parse(userString);
  return user;
};

const setUserItem = (user: UserDto) => {
  sessionStorage.setItem(currentUser, JSON.stringify(mapUser(user)));
};

const removeUserItem = () => {
  sessionStorage.removeItem(currentUser);
};

type AuthState = {
  user: User | null;
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

  const fetchCurrentUser = useAsyncRetry(async () => {
    setState((draft) => {
      draft.errors = [];
    });

    const response = await axios
      .get<GetUserResponse>(`${baseUrl}/api/get-current-user`)
      .then((response) => {
        if (response.data.hasErrors) {
          response.data.errors.forEach((err) => {
            console.error(err.message);
          });
          return response.data;
        }
        return response.data;
      })
      .catch((exc) => {
        console.error(exc);
        return null;
      });

    if (response === null) {
      return null;
    }

    setState((draft) => {
      draft.user = response.data;
      draft.errors = response.errors;
    });

    setUserItem(response.data);
  }, [setState]);

  useSubscription("user-login", () => {
    fetchCurrentUser.retry();
  });

  useSubscription("user-logout", () => {
    removeUserItem();
    setState((draft) => {
      draft.user = null;
    });
  });

  if (fetchCurrentUser.loading) {
    return (
      <Dimmer active inverted>
        <Loader indeterminate />
      </Dimmer>
    );
  }

  if (!state.user && !fetchCurrentUser.loading) {
    return <LoginPage />;
  }

  return <AuthContext.Provider value={state} {...props} />;
};

type UserDto = User & {
  userName: string;
  password: string;
};

type GetUserResponse = ApiResponse<UserDto>;

export function useUser(): User {
  const { user } = useContext(AuthContext);
  if (!user) {
    throw new Error(`useUser must be used within an authenticated app`);
  }
  return user;
}

export const mapUser = (user: any): User => ({
  firstName: user.firstName,
  lastName: user.lastName,
});

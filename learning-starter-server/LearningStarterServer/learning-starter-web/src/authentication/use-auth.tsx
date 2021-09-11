import axios from "axios";
import { resolve } from "path";
import { env } from "process";
import React, {
  createContext,
  Reducer,
  useContext,
  useEffect,
  useReducer,
} from "react";
import { Dimmer, Loader } from "semantic-ui-react";
import { ApiResponse } from "../constants/types";
import { useSubscription } from "../hooks/use-subscription";

const getUserItem = () => {
  return localStorage.getItem("currentUser");
};

const removeUserItem = () => {
  localStorage.removeItem("currentUser");
};

export type User = {
  firstName: string;
  lastName: string;
};

// type Actions = "ON_USER_LOADED" | "ON_USER_UNLOADED";
// type LiteralKeyedObject<K extends string, V> = { [key in K]: V };
// type Handlers = LiteralKeyedObject<Actions, Handler>;

// type Action = {
//   type: Actions;
//   payload?: any;
// };

enum ActionTypes {
  ON_USER_LOADED = "ON_USER_LOADED",
  ON_USER_UNLOADED = "ON_USER_UNLOADED",
  REDIRECTING = "REDIRECTING",
}

type AuthState = {
  pending: boolean;
  user: User | null;
  error: Error | null;
  redirectUrl?: string | null;
};

type AuthAction = {
  type: ActionTypes;
  payload: AuthState;
};

type Handler = (state: AuthState, payload: any) => void;

type HandlerServices = {
  [ActionTypes.ON_USER_LOADED]: Handler;
  [ActionTypes.ON_USER_UNLOADED]: Handler;
};

const handlers: HandlerServices = {
  ON_USER_LOADED: (state, { user }) => {
    state.pending = false;
    state.user = mapUser(user);
    const { url } = user.state || {};
    if (url) {
      state.redirectUrl = url;
    }
  },
  ON_USER_UNLOADED: (state) => {
    state.pending = true;
    state.user = null;
  },
};

const INITIAL_STATE: AuthState = {
  user: null,
  pending: true,
  error: null,
  redirectUrl: null,
};

const reducer: Reducer<AuthState, AuthAction> = (
  state,
  { type, payload }
): AuthState => {
  switch (type) {
    case ActionTypes.ON_USER_LOADED:
      //make await
      const user = getCurrentUser();

      localStorage.setItem("currentUser", JSON.stringify(mapUser(payload)));
      return {
        pending: false,
        user: mapUser(payload),
        error: null,
      };
    case ActionTypes.ON_USER_UNLOADED:
      removeUserItem();
      return {
        pending: false,
        user: null,
        error: null,
      };
    default:
      throw new Error();
  }
};

export const AuthContext = createContext<AuthState>(INITIAL_STATE);

export const AuthProvider = (props: any) => {
  const [state, dispatch] = useReducer(reducer, INITIAL_STATE);

  useEffect(() => {
    (async () => {
      const currentUserString = getUserItem();
      if (currentUserString === null) {
        //break
      }

      if (currentUserString) {
        const user: User = JSON.parse(currentUserString);
        console.log("Initial User: ", user);
        dispatch({
          type: ActionTypes.ON_USER_LOADED,
          payload: { ...state, user },
        });
      }
    })();
  }, [state]);

  useSubscription("user-login", () => {
    dispatch({ type: ActionTypes.ON_USER_LOADED, payload: { ...state } });
  });

  useSubscription("user-logout", () => {
    removeUserItem();
    dispatch({ type: ActionTypes.ON_USER_UNLOADED, payload: { ...state } });
  });

  if (state.pending) {
    return (
      <Dimmer active inverted>
        <Loader indeterminate />
      </Dimmer>
    );
  }

  if (!state.user && !state.pending) {
    return <div>Not Authenticated</div>;
  }

  return <AuthContext.Provider value={state} {...props} />;
};

const baseUrl = env.REACT_APP_API_BASE_URL;

type UserDto = User & {
  userName: string;
  password: string;
};

type GetUserResponse = ApiResponse<UserDto>;

async function getCurrentUser() {
  const user = await axios
    .get<GetUserResponse>(`${baseUrl}/api/get-current-user`)
    .then((response) => {
      if (response.data.hasErrors) {
        response.data.errors.forEach((err) => {
          console.error(err.message);
        });
        return null;
      }
      return response.data.data;
    })
    .catch((exc) => {
      console.error(exc);
      return null;
    });

  return user;
}

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

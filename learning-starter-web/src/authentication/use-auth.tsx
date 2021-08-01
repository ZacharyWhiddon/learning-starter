import React, { createContext, useContext, useEffect, useReducer } from "react";
import produce from "immer";
import { Dimmer, Loader } from "semantic-ui-react";
import { useSubscription } from "../hooks/use-subscription";
import { Env } from "../config/env-vars";

export type User = {
  firstName: string;
  lastName: string;
};

type AuthState = {
  pending: boolean;
  user: User | null;
  error: Error | null;
  redirectUrl?: string;
};

type Actions = "ON_USER_LOADED" | "ON_USER_UNLOADED";
type Handler = (state: AuthState, payload: any) => void;
type LiteralKeyedObject<K extends string, V> = { [key in K]: V };
type Handlers = LiteralKeyedObject<Actions, Handler>;

type Action = {
  type: Actions;
  payload?: any;
};

const handlers: Handlers = {
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
  redirectUrl: undefined,
};

const reducer = (state: AuthState, { type, payload }: Action): AuthState => {
  switch (type) {
    case "ON_USER_LOADED":
      return {
        pending: false,
        user: mapUser(payload),
        error: null,
      };
    case "ON_USER_UNLOADED":
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
      const currentUserString = localStorage.getItem("currentUser");
      if (currentUserString === null) {
        //break
      }

      const user = JSON.parse(currentUserString);
      console.log("Initial user", user);

      useSubscription(
        "user-login",
        dispatch({ type: "ON_USER_LOADED", payload: { user } })
      );

      useSubscription(
        "user-logout",
        dispatch({ type: "ON_USER_UNLOADED", payload: {} })
      );

      if (user) {
        dispatch({ type: "ON_USER_LOADED", payload: { user } });
      } else if (window.location.href.includes("#id_token")) {
        console.log("Handle callback");
        try {
          await userManager.signinRedirectCallback();
        } catch (error) {
          console.log("Callback Error", error);

          window.location = Env.appRoot as any;
        }
      } else {
        dispatch({ type: "REDIRECTING", payload: {} });

        let pathname = window.location.pathname;
        if (Env.subdirectory) {
          pathname = pathname.replace(Env.subdirectory, "");
        }

        let search = window.location.search;

        userManager.signinRedirect({
          state: {
            url: pathname + search,
          },
        });
      }
    })();
  }, []);

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

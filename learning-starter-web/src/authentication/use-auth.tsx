import React, { createContext, Reducer, useContext, useEffect, useReducer } from "react";
import produce from "immer";
import { Dimmer, Loader } from "semantic-ui-react";
import { useSubscription } from "../hooks/use-subscription";
import { Env } from "../config/env-vars";
import { authenticationService } from "./authentication-services";

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
  REDIRECTING = "REDIRECTING"
}

type AuthState = {
  pending: boolean;
  user: User | null;
  error: Error | null;
  redirectUrl?: string | null;
};

type AuthAction = {
  type: ActionTypes
  payload: AuthState
}

type Handler = (state: AuthState, payload: any) => void;

type HandlerServices = {
  [ActionTypes.ON_USER_LOADED]: Handler,
  [ActionTypes.ON_USER_UNLOADED]: Handler
}

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

const reducer: Reducer<AuthState, AuthAction>  = (state, { type, payload }): AuthState => {
  switch (type) {
    case ActionTypes.ON_USER_LOADED:
      return {
        pending: false,
        user: mapUser(payload),
        error: null,
      };
    case ActionTypes.ON_USER_UNLOADED:
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

      if (currentUserString) {
        const user: User = JSON.parse(currentUserString);
        console.log("Initial User: ", user);

        if (user) {
          // useSubscription(
          //   "user-login",
          //   () => dispatch({ type: ActionTypes.ON_USER_LOADED, payload: { ...state, user } })
          // );
    
          // useSubscription(
          //   "user-logout",
          //   () => dispatch({ type: ActionTypes.ON_USER_UNLOADED, payload: { ...state, user } })
          // );

          // dispatch({ type: ActionTypes.ON_USER_LOADED, payload: { ...state, user } });

          // if (window.location.href.includes("#id_token")) {
          //   console.log("Handle callback");
          //   try {
          //     await authenticationService.signinRedirectCallback();
          //   } catch (error) {
          //     console.log("Callback Error", error);

          //     window.location = Env.appRoot as any;
          //   }
          // } else {
          //   dispatch({ type: ActionTypes.REDIRECTING, payload: {...state} });

          //   let pathname = window.location.pathname;
          //   if (Env.subdirectory) {
          //     pathname = pathname.replace(Env.subdirectory, "");
          //   }

          //   let search = window.location.search;

          //   authenticationService.signinRedirect({
          //     state: {
          //       url: pathname + search,
          //     },
          //   });
          // }
    }
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

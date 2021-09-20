import React from "react";
import { Route, Switch } from "react-router-dom";
import { LandingPage } from "../pages/landing-page";
import { NotFoundPage } from "../pages/not-found";
import { useUser } from "../authentication/use-auth";
import { PrimaryNavigation } from "../components/navigation";
import { UserPage } from "../pages/user-page";

export const routes = {
  root: `/`,
  user: `/user`,
};

export const Routes = () => {
  const user = useUser();
  return (
    <>
      <PrimaryNavigation user={user} />
      <Switch>
        <Route path={routes.root} exact>
          <LandingPage />
        </Route>
        <Route path={routes.user} exact>
          <UserPage />
        </Route>
        <Route path="*" exact>
          <NotFoundPage />
        </Route>
      </Switch>
    </>
  );
};

import { notify } from "../hooks/use-subscription";

//this function fires off an event for logout-user, which will be hanled in the use-auth
export function logoutUser() {
  notify("user-logout", undefined);
}

//this function fires off an event for login-user, which will be hanled in the use-auth
export function loginUser() {
  notify("user-login", undefined);
}

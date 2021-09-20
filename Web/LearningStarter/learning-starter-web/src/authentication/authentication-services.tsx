import { notify } from "../hooks/use-subscription";

export function logoutUser() {
  notify("user-logout", undefined);
}

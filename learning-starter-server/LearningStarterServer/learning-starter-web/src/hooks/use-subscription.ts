import mitt from "mitt";
import { useEffect } from "react";
import { User } from "../authentication/use-auth";

let emitter = mitt();

type SubscriptionEventMap = {
  "user-logout": undefined;
  "user-login": undefined;
};

export type SubscriptionEvents = keyof SubscriptionEventMap;

export function useSubscription(
  eventName: SubscriptionEvents,
  cb: (event: SubscriptionEventMap[SubscriptionEvents]) => void
): void {
  useEffect(() => {
    emitter.on(eventName as any, cb as any);
    return () => emitter.off(eventName as any, cb as any);
  });
}

export function notify(
  eventName: SubscriptionEvents,
  event: SubscriptionEventMap[SubscriptionEvents]
) {
  emitter.emit(eventName, event);
}

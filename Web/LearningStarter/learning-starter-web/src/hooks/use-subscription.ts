import mitt from "mitt";
import { useEffect } from "react";

let emitter = mitt();

type SubscriptionEventMap = {
  "user-logout": undefined;
  "user-login": undefined;
};

export type SubscriptionEvents = keyof SubscriptionEventMap;

//This is another custom hook.
//This is less complicated than the useProduce, but still complex nonetheless.
//For the scope of this class you *probably* wont need to make any custom hooks.
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

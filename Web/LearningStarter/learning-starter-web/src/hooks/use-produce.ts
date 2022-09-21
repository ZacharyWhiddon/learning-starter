import produce, { Draft } from "immer";
import { useState, useCallback, useRef } from "react";

export type Producer<T> = (currentState: Draft<T>) => void;

//This is how custom hooks are made.
//This hook uses Generics which are complicated, and *probably* not necessary for your use case.
export function useProduce<T>(initialState: T | (() => T)) {
  const [state, _setState] = useState<T>(initialState);

  const stateRef = useRef<T>();
  stateRef.current = state;

  const producer = useCallback((mutationFn: Producer<T>) => {
    _setState(
      produce(stateRef.current as T, (draft) => {
        mutationFn(draft);
      })
    );
  }, []);

  return [state, producer] as [T, typeof producer];
}

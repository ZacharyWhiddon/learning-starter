//This type uses a generic (<T>).  For more information on generics see: https://www.typescriptlang.org/docs/handbook/2/generics.html
export type ApiResponse<T> = {
  data: T;
  errors: Error[];
  hasErrors: boolean;
};

type Error = {
  property: string;
  message: string;
};

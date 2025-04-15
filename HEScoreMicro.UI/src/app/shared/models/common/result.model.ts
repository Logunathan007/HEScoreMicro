import { Response } from "./response.model";

export interface Result<T> extends Response{
  data?:T
}

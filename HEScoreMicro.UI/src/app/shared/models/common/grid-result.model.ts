import { Response } from "./response.model"

export interface GridResult<T> extends Response{
  data?:T[]
}

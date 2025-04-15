import { Response } from "../common/response.model"

export interface ValidationInputReadModel extends Response {
  errors: string[]
  homeJson: string
}

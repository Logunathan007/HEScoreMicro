import { HttpClient } from "@angular/common/http";
import { CurdService } from "../common/curd.service";
import { SystemsReadModel } from "../../models/heating-cooling-system/systems-model";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class SystemsService extends CurdService<SystemsReadModel> {
  constructor(httpClient: HttpClient) {
    super(httpClient, "HeatingCoolingSystem/Systems")
  }
}

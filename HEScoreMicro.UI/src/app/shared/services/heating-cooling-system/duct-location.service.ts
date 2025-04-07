import { HttpClient } from "@angular/common/http";
import { DuctLocationReadModel } from "../../models/heating-cooling-system/duct-location-model";
import { CurdService } from "../common/curd.service";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class DuctLocationService extends CurdService<DuctLocationReadModel> {
  constructor(httpClient: HttpClient) {
    super(httpClient, "HeatingCoolingSystem/Systems/DuctLocation")
  }
}

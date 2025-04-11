import { Injectable } from '@angular/core';
import { CurdService } from '../common/curd.service';
import { BuildingReadModel } from '../../models/building/building.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class BuildingService extends CurdService<BuildingReadModel> {
  constructor(httpClient: HttpClient) {
    super(httpClient, "Building")
  }
  clearOldBuilding(id:string){
    this.httpClient.get("")
  }
}

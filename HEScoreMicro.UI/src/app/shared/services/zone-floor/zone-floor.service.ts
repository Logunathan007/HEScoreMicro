import { Injectable } from '@angular/core';
import { CurdService } from '../common/curd.service';
import { ZoneFloorReadModel } from '../../models/zone-floor/zone-floor.read.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ZoneFloorService extends CurdService<ZoneFloorReadModel> {
  constructor(httpClient: HttpClient) {
    super(httpClient, "ZoneFloor")
  }
}

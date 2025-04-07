import { Injectable } from '@angular/core';
import { ZoneRoofReadModel } from '../../models/zone-roof/zone-roof.read.model';
import { CurdService } from '../common/curd.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ZoneRoofService extends CurdService<ZoneRoofReadModel> {
  constructor(httpClient: HttpClient) {
    super(httpClient, "ZoneRoof")
  }
}

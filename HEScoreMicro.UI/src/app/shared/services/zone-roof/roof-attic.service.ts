import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CurdService } from '../common/curd.service';
import { RoofAtticReadModel } from '../../models/zone-roof/roof-attic.read.model';

@Injectable({
  providedIn: 'root'
})
export class RoofAtticService extends CurdService<RoofAtticReadModel> {
  constructor(httpClient: HttpClient) {
    super(httpClient, "ZoneRoof/RoofAttic")
  }
}

import { Injectable } from '@angular/core';
import { EnergyStarReadModel } from '../../models/energy-star/energy-star.model';
import { CurdService } from '../common/curd.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class EnergyStarService extends CurdService<EnergyStarReadModel> {
  constructor(httpClient: HttpClient) {
    super(httpClient, "EnergyStar")
  }
}

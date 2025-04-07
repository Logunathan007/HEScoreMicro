import { HeatingCoolingSystemReadModel } from '../../models/heating-cooling-system/heating-cooling-system.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CurdService } from '../common/curd.service';

@Injectable({
  providedIn: 'root'
})
export class HeatingCoolingSystemService extends CurdService<HeatingCoolingSystemReadModel> {
  constructor(httpClient: HttpClient) {
    super(httpClient, "HeatingCoolingSystem")
  }
}

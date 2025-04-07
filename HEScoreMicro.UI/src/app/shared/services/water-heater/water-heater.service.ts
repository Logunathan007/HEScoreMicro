import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CurdService } from '../common/curd.service';
import { WaterHeaterReadModel } from '../../models/water-heater/water-heater.model';

@Injectable({
  providedIn: 'root'
})
export class WaterHeaterService extends CurdService<WaterHeaterReadModel> {
  constructor(httpClient: HttpClient) {
    super(httpClient, "WaterHeater")
  }
}

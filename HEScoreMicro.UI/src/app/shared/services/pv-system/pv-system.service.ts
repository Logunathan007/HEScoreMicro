import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CurdService } from '../common/curd.service';
import { PVSystemReadModel } from '../../models/pv-system/pv-system.model';

@Injectable({
  providedIn: 'root'
})
export class PVSystemService extends CurdService<PVSystemReadModel> {
  constructor(httpClient: HttpClient) {
    super(httpClient, "PVSystem")
  }
}


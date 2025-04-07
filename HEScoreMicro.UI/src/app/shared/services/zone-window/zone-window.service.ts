import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CurdService } from '../common/curd.service';
import { ZoneWindowReadModel } from '../../models/zone-window/zone-window.model';

@Injectable({
  providedIn: 'root'
})
export class ZoneWindowService extends CurdService<ZoneWindowReadModel> {
  constructor(httpClient: HttpClient) {
    super(httpClient, "ZoneWindow")
  }
}

import { Injectable } from '@angular/core';
import { CurdService } from '../common/curd.service';
import { HttpClient } from '@angular/common/http';
import { FoundationReadModel } from '../../models/zone-floor/foundation.read.model';

@Injectable({
  providedIn: 'root'
})
export class FoundationService extends CurdService<FoundationReadModel> {
  constructor(httpClient: HttpClient) {
    super(httpClient, "ZoneFloor/Foundation")
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CurdService } from '../common/curd.service';
import { ZoneWallReadModel } from '../../models/zone-wall/zone-wall.read.model';

@Injectable({
  providedIn: 'root'
})

export class ZoneWallService extends CurdService<ZoneWallReadModel> {
  constructor(httpClient: HttpClient) {
    super(httpClient, "ZoneWall")
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CurdService } from '../common/curd.service';
import { WallReadModel } from '../../models/zone-wall/wall.read.model';

@Injectable({
  providedIn: 'root'
})

export class WallService extends CurdService<WallReadModel> {
  constructor(httpClient: HttpClient) {
    super(httpClient, "ZoneWall")
  }
}

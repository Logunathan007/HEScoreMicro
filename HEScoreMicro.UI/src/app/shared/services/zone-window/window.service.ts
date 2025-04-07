import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CurdService } from '../common/curd.service';
import { WindowReadModel } from '../../models/zone-window/window.model';

@Injectable({
  providedIn: 'root'
})
export class WindowService extends CurdService<WindowReadModel> {
  constructor(httpClient: HttpClient) {
    super(httpClient, "ZoneWindow")
  }
}

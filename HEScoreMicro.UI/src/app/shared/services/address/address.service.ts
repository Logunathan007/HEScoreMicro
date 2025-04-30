import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CurdService } from '../common/curd.service';
import { AddressReadModel } from '../../models/address/address.read.model';
import { Observable } from 'rxjs';
import { Result } from '../../models/common/result.model';

@Injectable({
  providedIn: 'root'
})
export class AddressService extends CurdService<AddressReadModel> {
  constructor(httpClient: HttpClient) {
    super(httpClient, "Address")
  }

  getByBuildingId(id: string): Observable<Result<AddressReadModel>> {
    return this.httpClient.get<Result<AddressReadModel>>(this.url + 'GetByBuildingId/' + id)
  }

}

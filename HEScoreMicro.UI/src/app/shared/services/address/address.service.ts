import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CurdService } from '../common/curd.service';
import { AddressReadModel } from '../../models/address/address.read.model';

@Injectable({
  providedIn: 'root'
})
export class AddressService extends CurdService<AddressReadModel> {
  constructor(httpClient: HttpClient) {
    super(httpClient, "Address")
  }
}

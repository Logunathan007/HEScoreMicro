import { AboutReadModel } from './../../models/about/about.read.model';
import { Injectable } from '@angular/core';
import { CurdService } from '../common/curd.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AboutService extends CurdService<AboutReadModel> {
  constructor(httpClient: HttpClient) {
    super(httpClient, "About")
  }
}


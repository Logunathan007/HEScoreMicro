import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { envVariable } from '../../../environents/environment.development';
import { Result } from '../../models/common/Result';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HpxmlGenerationService {

  private url!: string;
  private serviceName: string = "HPXMLGeneration";
  constructor(public httpClient: HttpClient) {
    this.url = envVariable.API_URL + this.serviceName + '/';
  }

  getHpxmlString(id: string): Observable<Result<string>> {
    return this.httpClient.get<Result<string>>(this.url + "HPXMLString/" + id);
  }

  getHpxmlBase64String(id: string): Observable<Result<string>> {
    return this.httpClient.get<Result<string>>(this.url + "HPXMLBase64String/" + id);
  }
}

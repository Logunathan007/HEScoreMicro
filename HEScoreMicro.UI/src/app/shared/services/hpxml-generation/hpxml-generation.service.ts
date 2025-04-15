import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { envVariable } from '../../../environents/environment.development';
import { Result } from '../../models/common/result.model';
import { Observable } from 'rxjs';
import { ValidationInputReadModel } from '../../models/hpxml-generation/validation-inputs.model';
import { BuildingReadModel } from '../../models/building/building.model';

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

  validateInputs(id: string): Observable<ValidationInputReadModel> {
    return this.httpClient.get<ValidationInputReadModel>(this.url + "ValidateInputs/" + id);
  }

  clearOldBuilding(id: string): Observable<Result<BuildingReadModel>> {
    return this.httpClient.get<Result<BuildingReadModel>>(this.url + "ClearOldPDF/" + id);
  }

  generatePDF(id: string): Observable<Result<string>> {
    return this.httpClient.get<Result<string>>(this.url + "GeneratePdf/" + id);
  }

}

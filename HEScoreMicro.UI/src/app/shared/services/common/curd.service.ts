import { HttpClient } from "@angular/common/http";
import { envVariable } from "../../../environents/environment.development";
import { Observable } from "rxjs";
import { Result } from "../../models/common/result.model";
import { GridResult } from "../../models/common/grid-result.model";

export interface ICurdService<TReadModel> {
  getById(id: string): Observable<Result<TReadModel>>
  getAll(): Observable<GridResult<TReadModel>>
  create(req: TReadModel): Observable<Result<TReadModel>>
  update(req: TReadModel): Observable<Result<TReadModel>>
  delete(id: string): Observable<Result<TReadModel>>
}

export class CurdService<TReadModel> implements ICurdService<TReadModel> {
  protected url!: string;

  constructor(public httpClient: HttpClient, public serviceName: string) {
    this.url = envVariable.API_URL + serviceName + '/';
  }

  getById(id: string): Observable<Result<TReadModel>> {
    return this.httpClient.get<Result<TReadModel>>(this.url + id)
  }

  getByBuildingId(id: string): Observable<Result<TReadModel>> {
    return this.httpClient.get<Result<TReadModel>>(this.url + 'GetByBuildingId/' + id)
  }

  getAll(): Observable<GridResult<TReadModel>> {
    return this.httpClient.get<GridResult<TReadModel>>(this.url)
  }

  create(req: TReadModel): Observable<Result<TReadModel>> {
    return this.httpClient.post<Result<TReadModel>>(this.url, req)
  }

  update(req: TReadModel): Observable<Result<TReadModel>> {
    return this.httpClient.put<Result<TReadModel>>(this.url, req);
  }

  delete(id: string): Observable<Result<TReadModel>> {
    return this.httpClient.delete<Result<TReadModel>>(this.url + id)
  }

  bulkDelete(id: string[]): Observable<Result<TReadModel>> {
    return this.httpClient.delete<Result<TReadModel>>(
      this.url + "DeleteByIds",
      {
        body: id,
        responseType: 'json'
      }
    );
  }
}

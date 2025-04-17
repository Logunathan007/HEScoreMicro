
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  public buildingId: string | undefined | null = "";

  constructor() {
    console.log("Common Service Called");
  }
}

import { RoofAtticReadModel } from "./roof-attic.read.model";

export interface ZoneRoofReadModel {
  id?: string;
  enableSecondRoofAttic:boolean | null;
  buildingId: string;
  roofAttics: RoofAtticReadModel[];
}


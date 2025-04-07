import { FoundationReadModel } from "./foundation.read.model";

export interface ZoneFloorReadModel {
  id?: string;
  buildingId: string;
  enableSecondFoundation: boolean;
  foundations: FoundationReadModel[];
}

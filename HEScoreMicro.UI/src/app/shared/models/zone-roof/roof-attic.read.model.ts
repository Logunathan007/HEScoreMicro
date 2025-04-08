export interface RoofAtticReadModel {
  id?: string;
  atticOrCeilingType: string | null;
  construction: string | null;
  exteriorFinish: string | null;
  cathedralCeilingArea: number | null;
  cathedralCeilingInsulation: number | null;
  roofArea: number | null;
  roofInsulation: number | null;
  roofColor: string | null;
  absorptance: number | null;
  skylightsPresent: boolean | null;
  skylightArea: number | null;
  solarScreen: boolean | null;
  knowSkylightSpecification: boolean | null;
  uFactor: number | null;
  shgc: number | null;
  panes: string | null;
  frameMaterial: string | null;
  glazingType: string | null;
  atticFloorArea: number | null;
  atticFloorInsulation: number | null;
  kneeWallPresent: boolean | null;
  kneeWallArea: number | null;
  kneeWallInsulation: number | null;
  buildingId: string;
}




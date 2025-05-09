export interface AboutReadModel {
  id?: string | null;
  assessmentDate: Date | null;
  comments: string | null;
  yearBuilt: number | null;
  numberOfBedrooms: number | null;
  storiesAboveGroundLevel: number | null;
  interiorFloorToCeilingHeight: number | null;
  totalConditionedFloorArea: number | null;
  directionFacedByFrontOfHome: string | null;
  blowerDoorTestConducted: boolean | null;
  airLeakageRate: number | null;
  manufacturedHomeType: string | null;
  numberofUnitsInBuilding: number | null;
  airSealed: boolean | null;
  buildingId: string | null;
}

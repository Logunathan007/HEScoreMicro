export interface FoundationReadModel {
  id?: string;
  foundationType: string | null;
  foundationArea: number | null;
  slabInsulationLevel: number | null;
  floorInsulation: number | null;
  foundationwallsInsulationLevel: number | null;
  buildingId: string;
}

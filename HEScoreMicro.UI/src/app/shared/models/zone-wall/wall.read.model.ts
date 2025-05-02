
export interface WallReadModel {
  id: string;
  buildingId: string;
  facing: number;
  adjacentTo: string;
  construction: string | null;
  exteriorFinish: string | null;
  wallInsulationLevel: number | null;
}

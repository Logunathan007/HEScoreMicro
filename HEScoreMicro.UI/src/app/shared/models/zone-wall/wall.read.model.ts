
export interface WallReadModel {
    id: string;
    buildingId: string;
    adjacentTo:string;
    construction: string | null;
    exteriorFinish: string | null;
    wallInsulationLevel: number | null;
}

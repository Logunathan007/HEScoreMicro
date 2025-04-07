import { WallReadModel } from "./wall.read.model";

export interface ZoneWallReadModel {
    id: string;
    buildingId: string;
    exteriorWallSame: boolean | null;
    walls: WallReadModel[];
}

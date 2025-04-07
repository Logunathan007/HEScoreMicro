import { WindowReadModel } from "./window.model";

export interface ZoneWindowReadModel {
    id: string;
    buildingId: string;
    windowAreaFront: number | null;
    windowAreaBack: number | null;
    windowAreaLeft: number | null;
    windowAreaRight: number | null;
    windowsSame: boolean | null;
    windows: WindowReadModel[];
}

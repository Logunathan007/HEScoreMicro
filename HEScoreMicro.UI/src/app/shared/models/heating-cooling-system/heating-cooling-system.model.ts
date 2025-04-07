import { SystemsReadModel } from "./systems-model";

export interface HeatingCoolingSystemReadModel {
    id?: string;
    buildingId: string;
    systemCount: number;
    systems: SystemsReadModel[];
}

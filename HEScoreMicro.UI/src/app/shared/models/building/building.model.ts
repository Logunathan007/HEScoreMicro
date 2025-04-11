import { AboutReadModel } from "../about/about.read.model";
import { AddressReadModel } from "../address/address.read.model";
import { HeatingCoolingSystemReadModel } from "../heating-cooling-system/heating-cooling-system.model";
import { PVSystemReadModel } from "../pv-system/pv-system.model";
import { WaterHeaterReadModel } from "../water-heater/water-heater.model";
import { ZoneFloorReadModel } from "../zone-floor/zone-floor.read.model";
import { ZoneRoofReadModel } from "../zone-roof/zone-roof.read.model";
import { ZoneWallReadModel } from "../zone-wall/zone-wall.read.model";
import { ZoneWindowReadModel } from "../zone-window/zone-window.model";

export interface BuildingReadModel {
    id: string;
    number: number | null;
    buildingId: string;
    address: AddressReadModel;
    about: AboutReadModel;
    zoneFloor: ZoneFloorReadModel;
    zoneRoof: ZoneRoofReadModel;
    zoneWall: ZoneWallReadModel;
    zoneWindow: ZoneWindowReadModel;
    heatingCoolingSystem: HeatingCoolingSystemReadModel;
    waterHeater: WaterHeaterReadModel;
    pvSystem: PVSystemReadModel;
}

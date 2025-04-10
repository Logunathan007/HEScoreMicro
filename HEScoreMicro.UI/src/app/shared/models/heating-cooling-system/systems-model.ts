import { DuctLocationReadModel } from "./duct-location-model";

export interface SystemsReadModel {
  id?: string;
  buildingId: string;
  percentAreaServed: number;
  heatingSystemType: string | null;
  knowHeatingEfficiency: boolean | null;
  heatingSystemEfficiencyValue: number | null;
  heatingSystemYearInstalled: number | null;
  coolingSystemType: string | null;
  knowCoolingEfficiency: boolean | null;
  coolingSystemEfficiencyValue: number | null;
  coolingSystemEfficiencyUnit: string | null;
  coolingSystemYearInstalled: number | null;
  ductLeakageTestPerformed: boolean | null;
  ductLeakageTestValue:number | null;
  ductAreProfessionallySealed: boolean | null;
  ductLocationCount: number | null;
  ductLocations: DuctLocationReadModel[];
}

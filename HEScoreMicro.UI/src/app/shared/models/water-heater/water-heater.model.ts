
export interface WaterHeaterReadModel {
  id?: string;
  buildingId: string;
  waterHeaterType: string | null;
  knowWaterHeaterEnergyFactor: boolean | null;
  unit: string | null;
  energyValue: number | null;
  yearOfManufacture: number | null;
}

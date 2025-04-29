export interface EnergyStarReadModel {
  id?: string;
  buildingId: string;
  energyStarPresent: boolean;
  startDate: Date;
  completionDate: Date;
  contractorBusinessName: string | null;
  contractorZipCode: number | null;
}

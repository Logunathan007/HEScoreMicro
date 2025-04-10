
export interface DuctLocationReadModel {
  id?: string;
  buildingId: string;
  location: string | null;
  percentageOfDucts : number | null;
  ductsIsInsulated: boolean | null;
}

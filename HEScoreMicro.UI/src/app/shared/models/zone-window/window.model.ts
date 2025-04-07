
export interface WindowReadModel {
  id?: string;
  buildingId: string;
  solarScreen: boolean | null;
  knowWindowSpecification: boolean | null;
  uFactor: number | null;
  shgc: number | null;
  panes: string | null;
  frameMaterial: string | null;
  glazingType: string | null;
}

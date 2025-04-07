
export interface PVSystemReadModel {
  id?: string;
  buildingId: string;
  hasPhotovoltaic: boolean | null;
  yearInstalled: number | null;
  directionPanelsFace: string | null;
  anglePanelsAreTilted: string | null;
  knowSystemCapacity: boolean | null;
  numberOfPanels: number | null;
  dcCapacity: number | null;
}

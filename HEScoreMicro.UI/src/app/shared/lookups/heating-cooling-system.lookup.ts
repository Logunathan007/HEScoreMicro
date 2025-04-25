export const HeatingSystemTypeOptions = [
  { id: 0, name: "None", value: "None" },
  { id: 1, name: "Central gas furnace", value: "Central gas furnace" },
  { id: 2, name: "Room (through-the-wall) gas furnace", value: "Room (through-the-wall) gas furnace" },
  { id: 3, name: "Gas boiler", value: "Gas boiler" },
  { id: 4, name: "Propane (LPG) central furnace", value: "Propane (LPG) central furnace" },
  { id: 5, name: "Propane (LPG) wall furnace", value: "Propane (LPG) wall furnace" },
  { id: 6, name: "Propane (LPG) boiler", value: "Propane (LPG) boiler" },
  { id: 7, name: "Oil furnace", value: "Oil furnace" },
  { id: 8, name: "Oil boiler", value: "Oil boiler" },
  { id: 9, name: "Electric furnace", value: "Electric furnace" },
  { id: 10, name: "Electric heat pump", value: "Electric heat pump" },
  { id: 11, name: "Electric baseboard heater", value: "Electric baseboard heater" },
  { id: 12, name: "Ground coupled heat pump", value: "Ground coupled heat pump" },
  { id: 13, name: "Minisplit (ductless) heat pump", value: "Minisplit (ductless) heat pump" },
  { id: 14, name: "Electric boiler", value: "Electric boiler" },
  { id: 15, name: "Wood stove", value: "Wood stove" },
  { id: 16, name: "Pellet stove", value: "Pellet stove" },
]

export const HeatingEfficiencyUnitOptions = [
  { name: "Heating Seasonal Performance Factor - Pre 2023 (HSPF)", value: "HSPF" },
  { name: "Heating Seasonal Performance Factor (HSPF2)", value: "HSPF2" },
]

export const CoolingSystemTypeOptions = [
  { id: 0, name: "None", value: "None" },
  { id: 1, name: "Central air conditioner", value: "Central air conditioner" },
  { id: 2, name: "Room air conditioner", value: "Room air conditioner" },
  { id: 3, name: "Electric heat pump", value: "Electric heat pump" },
  { id: 4, name: "Minisplit (ductless) heat pump", value: "Minisplit (ductless) heat pump" },
  { id: 5, name: "Direct evaporative cooling", value: "Direct evaporative cooling" },
  { id: 6, name: "Ground coupled heat pump", value: "Ground coupled heat pump" },
]

export const SEERCoolingEfficiencyUnitOptions = [
  { name: "Seasonal Energy Efficiency Ratio - Pre 2023 (SEER)", value: "SEER" },
  { name: "Seasonal Energy Efficiency Ratio (SEER2)", value: "SEER2" },
]

export const EERCoolingEfficiencyUnitOptions = [
  { name: "Energy Efficiency Ratio (EER)", value: "EER" },
  { name: "Combined Energy Efficiency Ratio (CEER)", value: "CEER" }
]

export const DuctLocationOptions = [
  { id: 0, name: "Conditioned space", value: "Conditioned space" },
  { id: 1, name: "Under slab", value: "Under slab" },
  { id: 2, name: "Exterior wall", value: "Exterior wall" },
  { id: 3, name: "Outside", value: "Outside" },
  //this values are added dynamically
  // { id: 4, name: "Unconditioned Basement", value: "Unconditioned Basement" },
  // { id: 5, name: "Unconditioned Attic", value: "Unconditioned Attic" },
  // { id: 6, name: "Unvented Crawlspace / Unconditioned Garage", value: "Unvented Crawlspace / Unconditioned Garage" },
  // { id: 7, name: "Vented Crawlspace", value: "Vented Crawlspace" },
]

export const SystemCountOptions = [
  { name: "One", value: 1 },
  { name: "Two", value: 2 },
]

export const DuctLocationCountOptions = [
  { name: "One", value: 1 },
  { name: "Two", value: 2 },
  { name: "Three", value: 3 },
]

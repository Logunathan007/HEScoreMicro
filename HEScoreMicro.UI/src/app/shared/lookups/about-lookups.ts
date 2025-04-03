
// common
export const BooleanOptions = [
  { name: "True", value: true },
  { name: "False", value: false, }
]

export const InsulationMaterialOptions = [
  { name: "Batt", value: "Batt" },
  { name: "LooseFill", value: "LooseFill" },
  { name: "Rigid", value: "Rigid" },
  { name: "SprayFoam", value: "SprayFoam" },
  { name: "Other", value: "Other" },
  { name: "None", value: "None" },
  { name: "Unknown", value: "Unknown" },
]

export const BattOptions = [
  { name: "unknown", value: "unknown" },
  { name: "recycled cotton", value: "recycled cotton" },
  { name: "rockwool", value: "rockwool" },
  { name: "fiberglass", value: "fiberglass" },
]

export const LooseFillOptions = [
  { name: "unknown", value: "unknown" },
  { name: "vermiculite", value: "vermiculite" },
  { name: "rockwool", value: "rockwool" },
  { name: "fiberglass", value: "fiberglass" },
  { name: "cellulose", value: "cellulose" },
]

export const RigidOptions = [
  { name: "unknown", value: "unknown" },
  { name: "eps", value: "eps" },
  { name: "xps", value: "xps" },
  { name: "polyisocyanurate", value: "polyisocyanurate" },
]

export const SprayFoamOptions = [
  { name: "unknown", value: "unknown" },
  { name: "closed cell", value: "closed cell" },
  { name: "open cell", value: "open cell" },
]

// for about
export const residentialFacilityTypeOptions = [
  { name: 'single-family detached', value: 'single-family detached' },
  { name: 'single-family attached', value: 'single-family attached' },
  { name: 'manufactured home', value: 'manufactured home' },
  { name: 'multi-family - town homes', value: 'multi-family - town homes' },
  { name: 'apartment unit', value: 'Apartment unit' }
]

export const OrientationOptions = [
  { name: 'north', value: 'north' },
  { name: 'northeast', value: 'northeast' },
  { name: 'east', value: 'east' },
  { name: 'southeast', value: 'southeast' },
  { name: 'south', value: 'south' },
  { name: 'southwest', value: 'southwest' },
  { name: 'west', value: 'west' },
  { name: 'northwest', value: 'northwest' },
]

export const housePressureOptions = [
  { name: 50, value: 50.0 },
]

export const unitofMeasureOptions = [
  { name: 'ACH', value: 'ACH' },
  { name: 'CFM', value: 'CFM' },
]

export const leakinessDescriptionOptions = [
  { name: 'very leaky', value: 'very leaky' },
  { name: 'leaky', value: 'leaky' },
  { name: 'average', value: 'average' },
  { name: 'tight', value: 'tight' },
  { name: 'very tight', value: 'very tight' },
]

export const ManufacturedHomeSectionsOptions = [
  { name: 'CrossMod', value: 'CrossMod' },
  { name: 'triple-wide', value: 'triple-wide' },
  { name: 'double-wide', value: 'double-wide' },
  { name: 'single-wide', value: 'single-wide' },
]

// for zone-floor need to update
export const FoundationTypeOptions = [
  { name: "Basement", value: "Basement" },
  { name: "Crawlspace", value: "Crawlspace" },
  { name: "SlabOnGrade", value: "SlabOnGrade" },
  { name: "Garage", value: "Garage" },
  { name: "Ambient", value: "Ambient" },
  { name: "AboveApartment", value: "AboveApartment" },
]


// for zone-roof
export const AtticTypeOptions = [
  { name: "Attic", value: "Attic" },
  { name: "CathedralCeiling", value: "CathedralCeiling" },
  { name: "FlatRoof", value: "FlatRoof" },
  { name: "BelowApartment", value: "BelowApartment" },
]

export const RoofTypeOptions = [
  { name: "concrete", value: "concrete" },
  { name: "plastic/rubber/synthetic sheeting", value: "plastic/rubber/synthetic sheeting" },
  { name: "metal surfacing", value: "metal surfacing" },
  { name: "asphalt or fiberglass shingles", value: "asphalt or fiberglass shingles" },
  { name: "wood shingles or shakes", value: "wood shingles or shakes" },
  { name: "slate or tile shingles", value: "slate or tile shingles" },
  { name: "shingles", value: "shingles" },
]

export const ExteriorShadingTypeOptions = [
  { name: "none", value: "none" },
  { name: "other", value: "other" },
  { name: "building", value: "building" },
  { name: "evergreen tree", value: "evergreen tree" },
  { name: "deciduous tree", value: "deciduous tree" },
  { name: "solar film", value: "solar film" },
  { name: "solar screens", value: "solar screens" },
  { name: "awnings", value: "awnings" },
  { name: "external overhangs", value: "external overhangs" },
]

export const RoofColorOptions = [
  { name: "reflective", value: "reflective" },
  { name: "dark", value: "dark" },
  { name: "medium dark", value: "medium dark" },
  { name: "medium", value: "medium" },
  { name: "light", value: "light" },
]

export const atticWallTypeOptions = [
  { name: "knee wall", value: 'knee wall' },
]

// for zone-wall
export const ExteriorAdjacentToOptions = [
  { name: "unconditioned space", value: "unconditioned space" },
  { name: "outside", value: "outside" },
  { name: "other housing unit below", value: "other housing unit below" },
  { name: "other housing unit above", value: "other housing unit above" },
  { name: "other housing unit", value: "other housing unit" },
  { name: "other", value: "other" },
  { name: "living space", value: "living space" },
  { name: "ground", value: "ground" },
  { name: "garage - unconditioned", value: "garage - unconditioned" },
  { name: "garage - conditioned", value: "garage - conditioned" },
  { name: "garage", value: "garage" },
  { name: "crawlspace - vented", value: "crawlspace - vented" },
  { name: "crawlspace - unvented", value: "crawlspace - unvented" },
  { name: "crawlspace - unconditioned", value: "crawlspace - unconditioned" },
  { name: "crawlspace - conditioned", value: "crawlspace - conditioned" },
  { name: "crawlspace", value: "crawlspace" },
  { name: "conditioned space", value: "conditioned space" },
  { name: "basement - unconditioned", value: "basement - unconditioned" },
  { name: "basement - conditioned", value: "basement - conditioned" },
  { name: "basement", value: "basement" },
  { name: "attic - vented", value: "attic - vented" },
  { name: "attic - unvented", value: "attic - unvented" },
  { name: "attic - unconditioned", value: "attic - unconditioned" },
  { name: "attic - conditioned", value: "attic - conditioned" },
  { name: "attic", value: "attic" },
]

export const WallExteriorAdjacentToOptions = [
  { name: 'outside', value: 'outside' },
  { name: 'other housing unit', value: 'other housing unit' },
  { name: 'unconditioned space', value: 'unconditioned space' },
]

export const InteriorAdjacentToOptions = [
  { name: "unconditioned space", value: "unconditioned space" },
  { name: "outside", value: "outside" },
  { name: "other housing unit below", value: "other housing unit below" },
  { name: "other housing unit above", value: "other housing unit above" },
  { name: "other housing unit", value: "other housing unit" },
  { name: "other", value: "other" },
  { name: "living space", value: "living space" },
  { name: "ground", value: "ground" },
  { name: "garage - unconditioned", value: "garage - unconditioned" },
  { name: "garage - conditioned", value: "garage - conditioned" },
  { name: "garage", value: "garage" },
  { name: "crawlspace - vented", value: "crawlspace - vented" },
  { name: "crawlspace - unvented", value: "crawlspace - unvented" },
  { name: "crawlspace - unconditioned", value: "crawlspace - unconditioned" },
  { name: "crawlspace - conditioned", value: "crawlspace - conditioned" },
  { name: "crawlspace", value: "crawlspace" },
  { name: "conditioned space", value: "conditioned space" },
  { name: "basement - unconditioned", value: "basement - unconditioned" },
  { name: "basement - conditioned", value: "basement - conditioned" },
  { name: "basement", value: "basement" },
  { name: "attic - vented", value: "attic - vented" },
  { name: "attic - unvented", value: "attic - unvented" },
  { name: "attic - unconditioned", value: "attic - unconditioned" },
  { name: "attic - conditioned", value: "attic - conditioned" },
  { name: "attic", value: "attic" },
]

export const WallInteriorAdjacentToOptions = [
  { name: "living space", value: "living space" },
]

export const WallTypeOptions = [
  { name: "WoodStud", value: "WoodStud" },
  { name: "DoubleWoodStud", value: "DoubleWoodStud" },
  { name: "ConcreteMasonryUnit", value: "ConcreteMasonryUnit" },
  { name: "StructurallyInsulatedPanel", value: "StructurallyInsulatedPanel" },
  { name: "InsulatedConcreteForms", value: "InsulatedConcreteForms" },
  { name: "SteelFrame", value: "SteelFrame" },
  { name: "SolidConcrete", value: "SolidConcrete" },
  { name: "StructuralBrick", value: "StructuralBrick" },
  { name: "StrawBale", value: "StrawBale" },
  { name: "Stone", value: "Stone" },
  { name: "LogWall", value: "LogWall" },
  { name: "Adobe", value: "Adobe" },
  { name: "Other", value: "Other" },
]
export const WallTypeOutsideOptions = [
  { name: "WoodStud", value: "WoodStud" },
  { name: "StructuralBrick", value: "StructuralBrick" },
  { name: "ConcreteMasonryUnit", value: "ConcreteMasonryUnit" },
  { name: "Stone", value: "Stone" },
  { name: "StrawBale", value: "StrawBale" },
]




export const FramingTypeOptions = [
  { name: "other", value: "other" },
  { name: "timber", value: "timber" },
  { name: "balloon", value: "balloon" },
  { name: "platform", value: "platform" },
]

export const SidingOptions = [
  { name: "masonite siding", value: "masonite siding" },
  { name: "composite shingle siding", value: "composite shingle siding" },
  { name: "fiber cement siding", value: "fiber cement siding" },
  { name: "asbestos siding", value: "asbestos siding" },
  { name: "brick veneer", value: "brick veneer" },
  { name: "aluminum siding", value: "aluminum siding" },
  { name: "vinyl siding", value: "vinyl siding" },
  { name: "synthetic stucco", value: "synthetic stucco" },
  { name: "stucco", value: "stucco" },
  { name: "wood siding", value: "wood siding" },
  { name: "other", value: "other" },
  { name: "none", value: "none" },
]
export const SidingWithoutNoneOptions = [
  { name: "masonite siding", value: "masonite siding" },
  { name: "composite shingle siding", value: "composite shingle siding" },
  { name: "fiber cement siding", value: "fiber cement siding" },
  { name: "asbestos siding", value: "asbestos siding" },
  { name: "brick veneer", value: "brick veneer" },
  { name: "aluminum siding", value: "aluminum siding" },
  { name: "vinyl siding", value: "vinyl siding" },
  { name: "synthetic stucco", value: "synthetic stucco" },
  { name: "stucco", value: "stucco" },
  { name: "wood siding", value: "wood siding" },
]
export const InstallationTypeOptions = [
  { name: "continuous - exterior", value: "continuous - exterior" },
  { name: "continuous - interior", value: "continuous - interior" },
  { name: "continuous", value: "continuous" },
  { name: "cavity", value: "cavity" },
]

export const GlassTypeOptions = [
  { name: "other", value: "other" },
  { name: "tinted/reflective", value: "tinted/reflective" },
  { name: "reflective", value: "reflective" },
  { name: "tinted", value: "tinted" },
  { name: "low-e", value: "low-e" },
  { name: "clear", value: "clear" },
]
export const EmptyOptions = [
  {}
]

export const GlassLayersOptions = [
  { name: "triple-pane", value: "triple-pane" },
  { name: "double-pane", value: "double-pane" },
  { name: "single-pane", value: "single-pane" },
]

export const GasFillOptions = [
  { name: "other", value: "other" },
  { name: "nitrogen", value: "nitrogen" },
  { name: "xenon", value: "xenon" },
  { name: "krypton", value: "krypton" },
  { name: "argon", value: "argon" },
  { name: "air", value: "air" },
]

export const FrameTypeOptions = [
  { name: "Aluminum", value: "Aluminum" },
  { name: "Composite", value: "Composite" },
  { name: "Fiberglass", value: "Fiberglass" },
  { name: "Metal", value: "Metal" },
  { name: "Vinyl", value: "Vinyl" },
  { name: "Wood", value: "Wood" },
]


//for systems

export const DuctTypeOptions = [
  { name: "return", value: "return" },
  { name: "supply", value: "supply" },
]

export const LeakinessObservedVisualInspectionOptions = [
  { name: "catastrophic leaks", value: "catastrophic leaks" },
  { name: "significant leaks", value: "significant leaks" },
  { name: "some observable leaks", value: "some observable leaks" },
  { name: "no observable leaks", value: "no observable leaks" },
  { name: "connections sealed w mastic", value: "connections sealed w mastic" },
]

export const UnitsOptions = [
  { name: "Percent", value: "Percent" },
  { name: "CFM per Std 152", value: "CFM per Std 152" },
  { name: "CFM25", value: "CFM25" },
  { name: "CFM50", value: "CFM50" },
]

export const TotalOrToOutsideOptions = [
  { name: "total", value: "total" },
  { name: "to outside", value: "to outside" },
]

export const DuctLocationOptions = [
  { name: 'living space', value: 'living space' },
  { name: 'conditioned space', value: 'conditioned space' },
  { name: 'unconditioned space', value: 'unconditioned space' },
  { name: 'under slab', value: 'under slab' },
  { name: 'basement', value: 'basement' },
  { name: 'basement - unconditioned', value: 'basement - unconditioned' },
  { name: 'basement - conditioned', value: 'basement - conditioned' },
  { name: 'crawlspace - unvented', value: 'crawlspace - unvented' },
  { name: 'crawlspace - vented', value: 'crawlspace - vented' },
  { name: 'crawlspace - unconditioned', value: 'crawlspace - unconditioned' },
  { name: 'crawlspace - conditioned', value: 'crawlspace - conditioned' },
  { name: 'crawlspace', value: 'crawlspace' },
  { name: 'exterior wall', value: 'exterior wall' },
  { name: 'interstitial space', value: 'interstitial space' },
  { name: 'garage - conditioned', value: 'garage - conditioned' },
  { name: 'garage - unconditioned', value: 'garage - unconditioned' },
  { name: 'garage', value: 'garage' },
  { name: 'roof deck', value: 'roof deck' },
  { name: 'outside', value: 'outside' },
  { name: 'attic', value: 'attic' },
  { name: 'attic - unconditioned', value: 'attic - unconditioned' },
  { name: 'attic - conditioned', value: 'attic - conditioned' },
  { name: 'attic - unvented', value: 'attic - unvented' },
  { name: 'attic - vented', value: 'attic - vented' },
]

// for heating and cooling system

export const HeatingSystemFuelOptions = [
  { name: 'electricity', value: 'electricity' },
  { name: 'renewable electricity', value: 'renewable electricity' },
  { name: 'natural gas', value: 'natural gas' },
  { name: 'renewable natural gas', value: 'renewable natural gas' },
  { name: 'fuel oil', value: 'fuel oil' },
  { name: 'fuel oil 1', value: 'fuel oil 1' },
  { name: 'fuel oil 2', value: 'fuel oil 2' },
  { name: 'fuel oil 4', value: 'fuel oil 4' },
  { name: 'fuel oil 5/6', value: 'fuel oil 5/6' },
  { name: 'propane', value: 'propane' },
  { name: 'wood', value: 'wood' },
  { name: 'wood pellets', value: 'wood pellets' },
]

export const HeatingSystemTypeOptions = [
  { name: "Furnace", value: "Furnace" },
  { name: "WallFurnace", value: "WallFurnace" },
  { name: "FloorFurnace", value: "FloorFurnace" },
  { name: "Boiler", value: "Boiler" },
  { name: "ElectricResistance", value: "ElectricResistance" },
  { name: "Stove", value: "Stove" },
]

export const AnnualHeatingEfficiencyUnitsOptions = [
  { name: "Percent", value: "Percent" },
  { name: "AFUE", value: "AFUE" },
  { name: "COP", value: "COP" },
  { name: "HSPF2", value: "HSPF2" },
  { name: "HSPF", value: "HSPF" },
]

export const HeatPumpTypeOptions = [
  { name: "ground-to-water", value: "ground-to-water" },
  { name: "ground-to-air", value: "ground-to-air" },
  { name: "mini-split", value: "mini-split" },
  { name: "air-to-water", value: "air-to-water" },
  { name: "air-to-air", value: "air-to-air" },
  { name: "water-to-water", value: "water-to-water" },
  { name: "water-to-air", value: "water-to-air" },
]

export const CoolingSystemTypeOptions = [
  { name: "evaporative cooler", value: "evaporative cooler" },
  { name: "room air conditioner", value: "room air conditioner" },
  { name: "mini-split", value: "mini-split" },
  { name: "central air conditioner", value: "central air conditioner" },
]

export const AnnualCoolingEfficiencyUnitsOptions = [
  { name: "kW/ton", value: "kW/ton" },
  { name: "COP", value: "COP" },
  { name: "EER2", value: "EER2" },
  { name: "EER", value: "EER" },
  { name: "CEER", value: "CEER" },
  { name: "SEER2", value: "SEER2" },
  { name: "SEER", value: "SEER" },
]

// for waterHeatingSystem
export const FuelTypeOptions = [
  { name: 'electricity', value: 'electricity' },
  { name: 'renewable electricity', value: 'renewable electricity' },
  { name: 'natural gas', value: 'natural gas' },
  { name: 'renewable natural gas', value: 'renewable natural gas' },
  { name: 'fuel oil', value: 'fuel oil' },
  { name: 'fuel oil 1', value: 'fuel oil 1' },
  { name: 'fuel oil 2', value: 'fuel oil 2' },
  { name: 'fuel oil 4', value: 'fuel oil 4' },
  { name: 'fuel oil 5/6', value: 'fuel oil 5/6' },
  { name: 'propane', value: 'propane' },
  { name: 'wood', value: 'wood' },
  { name: 'wood pellets', value: 'wood pellets' },
]

export const WaterHeaterTypeOptions = [
  { name: "space-heating boiler with tankless coil", value: "space-heating boiler with tankless coil" },
  { name: "space-heating boiler with storage tank", value: "space-heating boiler with storage tank" },
  { name: "heat pump water heater", value: "heat pump water heater" },
  { name: "instantaneous water heater", value: "instantaneous water heater" },
  { name: "dedicated boiler with storage tank", value: "dedicated boiler with storage tank" },
  { name: "storage water heater", value: "storage water heater" },
]


import { map } from "rxjs"

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

export const BooleanOptions = [
  { name: "True", value: true },
  { name: "False", value: false, }
]

export const UnitOptions = [
  { name: 'Energy Factor (EF)', value: 'EF' },
  { name: 'Uniform Energy Factor (UEF)', value: 'UEF' },
]

export const Year2000Options = Array.from(
  { length: new Date().getFullYear() - 1999 },
  (_, i) => ({ name: (2000 + i).toString(), value: 2000 + i })
);

export const Year1972Options = Array.from(
  { length: new Date().getFullYear() - 1971 },
  (_, i) => ({ name: (1972 + i).toString(), value: 1972 + i })
);

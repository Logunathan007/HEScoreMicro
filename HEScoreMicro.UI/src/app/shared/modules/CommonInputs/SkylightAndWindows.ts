import { EmptyOptions } from '../../lookups/about.lookups';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

export function skyLightAndWindowInputs(buildingId: string): FormGroup {
  const fb = new FormBuilder();
  const emptyOptions = EmptyOptions
  var skylight = fb.group({
    buildingId: [buildingId],
    id: [null],
    area: [null, [, Validators.required, Validators.min(0)]],
    uFactor: [null, [Validators.min(0), Validators.max(5)]],
    shgc: [null, [Validators.max(1), Validators.min(0)]],
    frameType: [null, []],
    thermalBreak: [null,],
    glassType: [null, []],
    glassLayerOptions: fb.control(emptyOptions),
    glassLayers: [null, []],
    gasFill: [null, []],
    exteriorShadingType: [null, []],
    stormWindow: [null, []],
    stormWindowGlassType: [null, []]
  })
  const frameType = skylight.get('frameType');
  const glassLayerOptions = skylight.get('glassLayerOptions');
  const thermalBreak = skylight.get('thermalBreak');
  frameType?.valueChanges.subscribe((val) => {
    thermalBreak?.reset()
    thermalBreak?.clearValidators()
    if ((val == 'Aluminum' || val == 'Metal')) {
      thermalBreak?.setValidators(Validators.required);
    } else if (val == null) {
      thermalBreak?.clearValidators()
      glassLayerOptions?.setValue(emptyOptions);
    } else {
      glassLayerOptions?.setValue([
        { name: "triple-pane", value: "triple-pane" },
        { name: "double-pane", value: "double-pane" },
        { name: "single-pane", value: "single-pane" },
      ]);
    }
  })
  thermalBreak?.valueChanges.subscribe((val) => {
    if (val) {
      glassLayerOptions?.setValue([
        { name: "single-pane", value: "single-pane" },
      ]);
    } else if (val == null) {
      glassLayerOptions?.setValue(emptyOptions);
    } else {
      glassLayerOptions?.setValue([
        { name: "single-pane", value: "single-pane" },
        { name: "double-pane", value: "double-pane" },
      ]);
    }
  })
  skylight.get('stormWindow')?.valueChanges.subscribe((val) => {
    skylight.get('stormWindowGlassType')?.setValue(null)
  })
  return skylight;
}

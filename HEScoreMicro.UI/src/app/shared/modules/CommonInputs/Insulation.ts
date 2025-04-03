
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from "@angular/forms";

export function insulationMaterialDynamicOptionsObj(layerObj: AbstractControl): FormGroup {
  return layerObj.get('insulationMaterialDynamicOptions') as FormGroup;
}

export function insulationObj(arg1: AbstractControl,slab:boolean=false): FormGroup {
  if(slab) return arg1.get('perimeterInsulation') as FormGroup;
  return arg1.get('insulation') as FormGroup;
}

export function layerObj(insulationObj: AbstractControl): FormArray {
  return insulationObj.get('layers') as FormArray;
}

export function insulationInputs(fb:FormBuilder): FormGroup {
  let insulation = fb.group({
    id:[null],
    assemblyEffectiveRValue: [null, [Validators.min(0)]],
    layers: fb.array([])
  })
  return insulation;
}

export function layerInputs(fb:FormBuilder): FormGroup {
  let layer = fb.group({
    id:[null],
    nominalRValue: [null, [Validators.min(0)]],
    installationType: [null, []],
    insulationMaterial: [null, []],
    insulationMaterialDynamicOptions: fb.group({
      id: [null],
      batt: [null],
      rigid: [null],
      looseFill: [null],
      sprayFoam: [null]
    })
  })
  layer.get('insulationMaterial')?.valueChanges.subscribe((val)=>{
    const options = layer.get('insulationMaterialDynamicOptions') as FormGroup;
    options.get('batt')?.setValue(null)
    options.get('rigid')?.setValue(null)
    options.get('looseFill')?.setValue(null)
    options.get('sprayFoam')?.setValue(null)
  })
  return layer;
}

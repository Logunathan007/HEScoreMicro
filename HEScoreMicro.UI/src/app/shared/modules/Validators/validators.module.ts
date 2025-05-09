import { AbstractControl, ValidationErrors, ValidatorFn, Validators } from "@angular/forms";
import { Observable } from "rxjs";

// Async validator
export function nameValidator(name: string) {
  return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
    return new Promise((resolve) => {
      setTimeout(() => {
        let arr = control.parent?.parent as any;
        if (arr && arr.controls) {
          for (let element of arr.controls) {
            if (control !== element.get(name) && element.get(name)?.value === control.value) {
              resolve({ nameSame: true });
              return;
            }
          }
        }
        resolve(null);
      }, 500);
    });
  };
}

// Static Validation adder
export function setValidations(controls: AbstractControl | AbstractControl[], validations: ValidatorFn[] = [Validators.required]): void {
  const updateControl = (ctrl: AbstractControl) => {
    ctrl.setValidators(validations);
    ctrl.updateValueAndValidity();
  };

  if (Array.isArray(controls)) {
    controls.forEach(updateControl);
  } else {
    updateControl(controls);
  }
}

export function windowAreaAverageValidator(group: AbstractControl): ValidationErrors | null {
  debugger
  const front = group.get('windowAreaFront')?.value || 0;
  const back = group.get('windowAreaBack')?.value || 0;
  const left = group.get('windowAreaLeft')?.value || 0;
  const right = group.get('windowAreaRight')?.value || 0;
  const average = (front + back + left + right) / 4;

  if (average < 0 || average > 999) {
    return { avgOutOfRange: average };
  }

  return null;
}

//Reset Controls
export function resetValuesAndValidations(controls:AbstractControl | AbstractControl[]){
  const updateControl = (ctrl: AbstractControl) => {
    ctrl.reset();
    ctrl.clearValidators();
    ctrl.updateValueAndValidity();
  };

  if (Array.isArray(controls)) {
    controls.forEach(updateControl);
  } else {
    updateControl(controls);
  }
}

export function resetValues(controls:AbstractControl | AbstractControl[]){
  const updateControl = (ctrl: AbstractControl) => {
    ctrl.reset();
  };

  if (Array.isArray(controls)) {
    controls.forEach(updateControl);
  } else {
    updateControl(controls);
  }
}

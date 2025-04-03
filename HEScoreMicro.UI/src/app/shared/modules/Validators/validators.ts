import { AbstractControl, ValidationErrors } from "@angular/forms";
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

import { OrientationOptions, Year2000Options } from '../../shared/lookups/common.lookup';
import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { PVSystemReadModel } from "../../shared/models/pv-system/pv-system.model";
import { BooleanOptions } from "../../shared/lookups/common.lookup";
import { Unsubscriber } from "../../shared/modules/unsubscribe/unsubscribe.component.";
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from "@angular/forms";
import { PVSystemService } from "../../shared/services/pv-system/pv-system.service";
import { takeUntil } from "rxjs";
import { Result } from "../../shared/models/common/result.model";
import { AnglePanelsAreTiltedOptions } from "../../shared/lookups/pv-system.lookup";
import { resetValuesAndValidations, setValidations } from '../../shared/modules/Validators/validators.module';
import { EmitterModel } from '../../shared/models/common/emitter.model';

@Component({
  selector: 'app-pv-system',
  templateUrl: './pv-system.component.html',
  styleUrl: './pv-system.component.scss',
  standalone: false
})
export class PVSystemComponent extends Unsubscriber implements OnInit {
  //variable initializations
  pVSystemForm!: FormGroup | any;
  @Input('buildingId') buildingId: string | null | undefined;
  @Output('update') updateEvent: EventEmitter<EmitterModel<PVSystemReadModel>> = new EventEmitter();
  @Output("move") move: EventEmitter<boolean> = new EventEmitter();
  @Input('input') pVSystemReadModel!: PVSystemReadModel;
  booleanOptions = BooleanOptions
  anglePanelsAreTiltedOptions = AnglePanelsAreTiltedOptions
  orientationOptions = OrientationOptions
  year2000Options = Year2000Options

  get pVSystemControl() {
    return this.pVSystemForm.controls;
  }

  constructor(
    private pVSystemService: PVSystemService,
    public fb: FormBuilder,
  ) {
    super()
  }

  ngOnInit(): void {
    this.variableDeclaration();
    this.getData();
  }

  //variable declarations
  variableDeclaration() {
    this.pVSystemForm = this.pvSystemInput();
  }

  pvSystemInput(): FormGroup {
    const pvSystem = this.fb.group({
      id: [null],
      hasPhotovoltaic: [null, [Validators.required]],
      yearInstalled: [null],
      directionPanelsFace: [null],
      anglePanelsAreTilted: [null],
      knowSystemCapacity: [null],
      numberOfPanels: [null],
      dcCapacity: [null],
      buildingId: [this.buildingId],
    })
    const hasPhotovoltaic = pvSystem.get("hasPhotovoltaic") as AbstractControl
    const yearInstalled = pvSystem.get("yearInstalled") as AbstractControl
    const directionPanelsFace = pvSystem.get("directionPanelsFace") as AbstractControl
    const anglePanelsAreTilted = pvSystem.get("anglePanelsAreTilted") as AbstractControl
    const knowSystemCapacity = pvSystem.get("knowSystemCapacity") as AbstractControl
    const numberOfPanels = pvSystem.get("numberOfPanels") as AbstractControl // 1 - 100
    const dcCapacity = pvSystem.get("dcCapacity") as AbstractControl // 50 - 20000
    hasPhotovoltaic?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      if (val) {
        setValidations([yearInstalled, directionPanelsFace, anglePanelsAreTilted, knowSystemCapacity],)
      } else {
        resetValuesAndValidations([yearInstalled, directionPanelsFace, anglePanelsAreTilted, knowSystemCapacity])
      }
    })
    knowSystemCapacity?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      if (val == true) {
        setValidations(dcCapacity, [Validators.required, Validators.min(50), Validators.max(20000)])
        resetValuesAndValidations(numberOfPanels)
      } else if (val == false) {
        setValidations(numberOfPanels, [Validators.required, Validators.min(1), Validators.max(100)])
        resetValuesAndValidations(dcCapacity)
      } else {
        resetValuesAndValidations([dcCapacity, numberOfPanels])
      }
    })
    return pvSystem;
  }

  getData() {
    if (this.pVSystemReadModel) {
      this.pVSystemForm.patchValue(this.pVSystemReadModel)
    }
  }

  onSave() {
    if (this.pVSystemForm.invalid) {
      this.pVSystemForm.markAllAsTouched();
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }
    if (this.pVSystemForm.value?.id) {
      this.pVSystemReadModel = this.pVSystemForm.value
      this.pVSystemService.update(this.pVSystemReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<PVSystemReadModel>) => {
          if (val?.failed === false) {
            this.pVSystemForm.patchValue(val.data)
            this.updateEvent.emit({
              fieldType: "pv-system",
              field: val.data
            })
          }
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    } else {
      this.pVSystemReadModel = this.pVSystemForm.value
      delete this.pVSystemReadModel.id;
      this.pVSystemService.create(this.pVSystemReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<PVSystemReadModel>) => {
          if (val?.failed === false) {
            this.pVSystemForm.patchValue(val.data)
            this.updateEvent.emit({
              fieldType: "pv-system",
              field: val.data
            })
          }
          console.log(val);
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    }
  }
}


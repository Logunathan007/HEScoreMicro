import { BooleanOptions } from './../../shared/lookups/common.lookup';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Result } from '../../shared/models/common/result.model';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { resetValuesAndValidations, setValidations } from '../../shared/modules/Validators/validators.module';
import { takeUntil } from 'rxjs';
import { EmitterModel } from '../../shared/models/common/emitter.model';
import { Unsubscriber } from '../../shared/modules/unsubscribe/unsubscribe.component.';
import { EnergyStarReadModel } from '../../shared/models/energy-star/energy-star.model';
import { EnergyStarService } from '../../shared/services/energy-star/energy-star.service';

@Component({
  selector: 'app-energy-star',
  standalone: false,
  templateUrl: './energy-star.component.html',
  styleUrl: './energy-star.component.scss'
})
export class EnergyStarComponent extends Unsubscriber implements OnInit {
  //variable initializations
  energyStarForm!: FormGroup | any;
  @Input('buildingId') buildingId: string | null | undefined;
  @Output('update')
  updateEvent: EventEmitter<EmitterModel<EnergyStarReadModel>> = new EventEmitter();
  @Output("move") move: EventEmitter<boolean> = new EventEmitter();
  @Input('input') energyStarReadModel!: EnergyStarReadModel;
  @Input('hasBoiler') hasBoiler!: boolean;
  booleanOptions = BooleanOptions

  get energyStarControl() {
    return this.energyStarForm.controls;
  }

  constructor(
    private energyStarService: EnergyStarService,
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
    this.energyStarForm = this.energyStarInput();
  }

  energyStarInput(): FormGroup {
    var energyStar = this.fb.group({
      id: [null],
      buildingId: [this.buildingId],
      energyStarPresent: [false, Validators.required],
      startDate: [null,],
      completionDate: [null,],
      contractorBusinessName: [null,],
      contractorZipCode: [null,],
    })

    const energyStarPresent = energyStar.get('energyStarPresent') as AbstractControl
    const startDate = energyStar.get("startDate") as AbstractControl
    const completionDate = energyStar.get("completionDate") as AbstractControl
    const contractorBusinessName = energyStar.get("contractorBusinessName") as AbstractControl
    const contractorZipCode = energyStar.get("contractorZipCode") as AbstractControl
    energyStarPresent.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      resetValuesAndValidations([startDate, completionDate, contractorBusinessName, contractorZipCode]);
      if (val) {
        setValidations([startDate, completionDate, contractorBusinessName, contractorZipCode])
      }
    })
    return energyStar;
  }

  getData() {
    if (this.energyStarReadModel) {
      this.patchToForm(this.energyStarReadModel)
    }
  }

  patchToForm(model: EnergyStarReadModel) {
    if (model) {
      if (model.completionDate)
        model.completionDate = new Date(model.completionDate);
      if (model.startDate)
        model.startDate = new Date(model.startDate);
      this.energyStarForm.patchValue(model)
    }
  }

  onSave() {
    if (this.energyStarForm.invalid) {
      this.energyStarForm.markAllAsTouched();
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }
    if (this.energyStarForm.value?.id) {
      this.energyStarReadModel = this.energyStarForm.value
      this.energyStarService.update(this.energyStarReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<EnergyStarReadModel>) => {
          if (val?.failed === false) {
            if (val.data)
              this.patchToForm(val?.data)
            this.updateEvent.emit({
              fieldType: "energy-star",
              field: val.data
            })
          }
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    } else {
      this.energyStarReadModel = this.energyStarForm.value
      delete this.energyStarReadModel.id;
      this.energyStarService.create(this.energyStarReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<EnergyStarReadModel>) => {
          if (val?.failed === false) {
            if (val.data)
              this.patchToForm(val?.data)
            this.updateEvent.emit({
              fieldType: "energy-star",
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

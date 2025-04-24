import { WaterHeaterTypeOptions } from '../../shared/lookups/water-heater.lookup';
import { UnitOptions, Year1998Options } from '../../shared/lookups/common.lookup';
import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { Unsubscriber } from "../../shared/modules/unsubscribe/unsubscribe.component.";
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from "@angular/forms";
import { WaterHeaterReadModel } from "../../shared/models/water-heater/water-heater.model";
import { BooleanOptions } from "../../shared/lookups/common.lookup";
import { WaterHeaterService } from "../../shared/services/water-heater/water-heater.service";
import { takeUntil } from "rxjs";
import { Result } from "../../shared/models/common/result.model";
import { resetValuesAndValidations, setValidations } from '../../shared/modules/Validators/validators.module';
import { EmitterModel } from '../../shared/models/common/emitter.model';

@Component({
  selector: 'app-water-heating-system',
  templateUrl: './water-heater.component.html',
  styleUrl: './water-heater.component.scss',
  standalone: false
})
export class WaterHeaterComponent extends Unsubscriber implements OnInit {
  //variable initializations
  waterHeaterForm!: FormGroup | any;
  @Input('buildingId') buildingId: string | null | undefined;
  @Output('update')
  updateEvent: EventEmitter<EmitterModel<WaterHeaterReadModel>> = new EventEmitter();
  @Input('input') waterHeaterReadModel!: WaterHeaterReadModel;
  booleanOptions = BooleanOptions
  unitOptions = UnitOptions
  waterHeaterTypeOptions = WaterHeaterTypeOptions
  year1998Options = Year1998Options
  setValidations = setValidations
  resetValuesAndValidations = resetValuesAndValidations

  get waterHeaterControl() {
    return this.waterHeaterForm.controls;
  }

  constructor(
    private waterHeaterService: WaterHeaterService,
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
    this.waterHeaterForm = this.waterHeaterInput();
  }

  waterHeaterInput(): FormGroup {
    var waterHeater = this.fb.group({
      id: [null],
      waterHeaterType: [null, Validators.required],
      knowWaterHeaterEnergyFactor: [null],
      unit: [null],
      energyValue: [null],
      yearOfManufacture: [null],
      buildingId: [this.buildingId],
    })
    const waterHeaterType = waterHeater.get('waterHeaterType') as AbstractControl
    const knowWaterHeaterEnergyFactor = waterHeater.get('knowWaterHeaterEnergyFactor') as AbstractControl
    const unit = waterHeater.get('unit') as AbstractControl
    const energyValue = waterHeater.get('energyValue') as AbstractControl
    const yearOfManufacture = waterHeater.get('yearOfManufacture') as AbstractControl
    waterHeaterType.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: string) => {
      if (val.endsWith("Storage")) {
        this.setValidations(knowWaterHeaterEnergyFactor)
        this.resetValuesAndValidations([unit, energyValue])
      } else if (val) {
        this.setValidations(unit)
        this.setValidations(energyValue, this.energyValueValidation(waterHeaterType?.value))
        this.resetValuesAndValidations([knowWaterHeaterEnergyFactor, yearOfManufacture])
      } else {
        this.resetValuesAndValidations([unit, energyValue, knowWaterHeaterEnergyFactor, yearOfManufacture])
      }
    })
    knowWaterHeaterEnergyFactor.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: boolean) => {
      if (val == true) {
        this.setValidations(unit)
        this.setValidations(energyValue, this.energyValueValidation(waterHeaterType?.value))
        this.resetValuesAndValidations(yearOfManufacture)
      }
      else if (val == false) {
        this.setValidations(yearOfManufacture)
        this.resetValuesAndValidations([unit, energyValue])
      } else {
        this.resetValuesAndValidations([yearOfManufacture])
      }
    })
    return waterHeater;
  }
  energyValueValidation(waterHeaterType: string | null): ValidatorFn[] {
    if (waterHeaterType == "Electric Instantaneous" || waterHeaterType == "Electric Storage") {
      return [Validators.required, Validators.min(0.86), Validators.max(0.99)]
    } else if (waterHeaterType == "Propane (LPG) Storage" || waterHeaterType == "Natural Gas Storage") {
      return [Validators.required, Validators.min(0.5), Validators.max(0.95)]
    } else if (waterHeaterType == "Oil Storage") {
      return [Validators.required, Validators.min(0.45), Validators.max(0.95)]
    } else if (waterHeaterType == "Gas Instantaneous" || waterHeaterType == "Propane Instantaneous" || waterHeaterType == "Oil Instantaneous") {
      return [Validators.required, Validators.min(0.7), Validators.max(0.99)]
    } else if (waterHeaterType == "Electric Heat Pump") {
      return [Validators.required, Validators.min(1), Validators.max(5)]
    } else {
      return [Validators.required]
    }
  }

  getData() {
    if (this.waterHeaterReadModel) {
      this.waterHeaterForm.patchValue(this.waterHeaterReadModel)
    }
  }

  onSave() {
    if (this.waterHeaterForm.invalid) {
      this.waterHeaterForm.markAllAsTouched();
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }
    if (this.waterHeaterForm.value?.id) {
      this.waterHeaterReadModel = this.waterHeaterForm.value
      this.waterHeaterService.update(this.waterHeaterReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<WaterHeaterReadModel>) => {
          if (val?.failed == false) {
            this.waterHeaterForm.patchValue(val.data)
            this.updateEvent.emit({
              fieldType: "water-heater",
              field: val.data
            })
          }
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    } else {
      this.waterHeaterReadModel = this.waterHeaterForm.value
      delete this.waterHeaterReadModel.id;
      this.waterHeaterService.create(this.waterHeaterReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<WaterHeaterReadModel>) => {
          if (val?.failed == false) {
            this.waterHeaterForm.patchValue(val.data)
            this.updateEvent.emit({
              fieldType: "water-heater",
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
  @Output("move") move: EventEmitter<boolean> = new EventEmitter();
  goNext() {
    this.move.emit(true);
  }
}

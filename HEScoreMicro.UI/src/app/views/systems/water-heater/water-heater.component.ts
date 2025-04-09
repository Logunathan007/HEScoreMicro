import { WaterHeaterTypeOptions } from './../../../shared/lookups/water-heater.lookup';
import { UnitOptions } from './../../../shared/lookups/common.lookup';
import { Component, OnInit } from "@angular/core";
import { Unsubscriber } from "../../../shared/modules/unsubscribe/unsubscribe.component.";
import { FormBuilder, FormGroup } from "@angular/forms";
import { WaterHeaterReadModel } from "../../../shared/models/water-heater/water-heater.model";
import { BooleanOptions } from "../../../shared/lookups/common.lookup";
import { CommonService } from "../../../shared/services/common/common.service";
import { WaterHeaterService } from "../../../shared/services/water-heater/water-heater.service";
import { ActivatedRoute, Router } from "@angular/router";
import { takeUntil } from "rxjs";
import { Result } from "../../../shared/models/common/Result";

@Component({
  selector: 'app-water-heating-system',
  templateUrl: './water-heater.component.html',
  styleUrl: './water-heater.component.scss',
  standalone: false
})
export class WaterHeaterComponent extends Unsubscriber implements OnInit {
  //variable initializations
  waterHeaterForm!: FormGroup | any;
  buildingId: string | null | undefined;
  waterHeaterReadModel!: WaterHeaterReadModel;
  booleanOptions = BooleanOptions
  unitOptions = UnitOptions
  waterHeaterTypeOptions = WaterHeaterTypeOptions

  get waterHeaterControl() {
    return this.waterHeaterForm.controls;
  }

  constructor(
    protected commonService: CommonService,
    private waterHeaterService: WaterHeaterService,
    public fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) {
    super()
  }

  ngOnInit(): void {
    this.getBuildingId();
    this.variableDeclaration();
    this.getData();
  }

  //variable declarations
  variableDeclaration() {
    this.waterHeaterForm = this.fb.group({
      id: [null],
      waterHeaterType: [null],
      knowWaterHeaterEnergyFactor: [null],
      unit: [null],
      energyValue: [null],
      yearOfManufacture: [null],
      buildingId: [this.buildingId],
    })
  }

  getBuildingId() {
    this.route.queryParamMap.pipe(takeUntil(this.destroy$)).subscribe(params => {
      if (params.get('id')) {
        this.commonService.buildingId = params.get('id');
      }
      this.buildingId = params.get('id') ?? this.commonService.buildingId ?? ""
    })
  }
  getData() {
    if (this.buildingId) {
      this.waterHeaterService.getByBuildingId(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<WaterHeaterReadModel>) => {
          if (val?.failed == false) {
            this.waterHeaterForm.patchValue(val.data)
          }
          console.log(val);
        },
        error: (err: any) => {
          console.log(err);
        }
      })
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
          this.waterHeaterForm.patchValue(val.data)
          console.log(val);
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
            this.buildingId = this.commonService.buildingId = val.data?.buildingId;
          }
          console.log(val);
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    }
  }
  goNext() {
    this.router.navigate(['systems/pv-system'], {
      queryParams: { id: this.buildingId }
    })
  }
}

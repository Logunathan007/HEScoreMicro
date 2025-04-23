import { OrientationOptions, Year2000Options } from '../../shared/lookups/common.lookup';
import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { PVSystemReadModel } from "../../shared/models/pv-system/pv-system.model";
import { BooleanOptions } from "../../shared/lookups/common.lookup";
import { Unsubscriber } from "../../shared/modules/unsubscribe/unsubscribe.component.";
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from "@angular/forms";
import { CommonService } from "../../shared/services/common/common.service";
import { PVSystemService } from "../../shared/services/pv-system/pv-system.service";
import { ActivatedRoute, Router } from "@angular/router";
import { takeUntil } from "rxjs";
import { Result } from "../../shared/models/common/result.model";
import { AnglePanelsAreTiltedOptions } from "../../shared/lookups/pv-system.lookup";
import { resetValuesAndValidations, setValidations } from '../../shared/modules/Validators/validators.module';

@Component({
  selector: 'app-pv-system',
  templateUrl: './pv-system.component.html',
  styleUrl: './pv-system.component.scss',
  standalone: false
})
export class PVSystemComponent extends Unsubscriber implements OnInit {
  //variable initializations
  pVSystemForm!: FormGroup | any;
  buildingId: string | null | undefined;
  pVSystemReadModel!: PVSystemReadModel;
  booleanOptions = BooleanOptions
  anglePanelsAreTiltedOptions = AnglePanelsAreTiltedOptions
  orientationOptions = OrientationOptions
  year2000Options = Year2000Options
  setValidations = setValidations
  resetValuesAndValidations = resetValuesAndValidations
  get pVSystemControl() {
    return this.pVSystemForm.controls;
  }

  constructor(
    protected commonService: CommonService,
    private pVSystemService: PVSystemService,
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
        this.setValidations([yearInstalled, directionPanelsFace, anglePanelsAreTilted, knowSystemCapacity],)
      } else {
        this.resetValuesAndValidations([yearInstalled, directionPanelsFace, anglePanelsAreTilted, knowSystemCapacity])
      }
    })
    knowSystemCapacity?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      if (val == true) {
        this.setValidations(dcCapacity, [Validators.required, Validators.min(50), Validators.max(20000)])
        this.resetValuesAndValidations(numberOfPanels)
      } else if (val == false) {
        this.setValidations(numberOfPanels, [Validators.required, Validators.min(1), Validators.max(100)])
        this.resetValuesAndValidations(dcCapacity)
      } else {
        this.resetValuesAndValidations([dcCapacity, numberOfPanels])
      }
    })
    return pvSystem;
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
      this.pVSystemService.getByBuildingId(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<PVSystemReadModel>) => {
          if (val?.failed == false) {
            this.pVSystemForm.patchValue(val.data)
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
    if (this.pVSystemForm.invalid) {
      this.pVSystemForm.markAllAsTouched();
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }
    if (this.pVSystemForm.value?.id) {
      this.pVSystemReadModel = this.pVSystemForm.value
      this.pVSystemService.update(this.pVSystemReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<PVSystemReadModel>) => {
          this.pVSystemForm.patchValue(val.data)
          console.log(val);
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
          if (val?.failed == false) {
            this.pVSystemForm.patchValue(val.data)
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
  @Output("move") move: EventEmitter<boolean> = new EventEmitter();
  goNext() {
    this.move.emit(true);
  }

}


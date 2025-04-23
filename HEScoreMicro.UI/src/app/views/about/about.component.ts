import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService } from '../../shared/services/common/common.service';
import { AboutService } from '../../shared/services/about/about.service';
import { AboutReadModel } from '../../shared/models/about/about.read.model';
import { Unsubscriber } from '../../shared/modules/unsubscribe/unsubscribe.component.';
import { Result } from '../../shared/models/common/result.model';
import { takeUntil } from 'rxjs';
import { BooleanOptions, OrientationOptions } from '../../shared/lookups/common.lookup';
import { ManufacturedHomeTypeOptions } from '../../shared/lookups/about.lookup';
import { resetValuesAndValidations, setValidations } from '../../shared/modules/Validators/validators.module';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrl: './about.component.scss',
  standalone: false
})
export class AboutComponent extends Unsubscriber implements OnInit {
  //variable initializations
  aboutForm!: FormGroup | any;
  buildingId: string | null | undefined;
  aboutReadModel!: AboutReadModel;
  booleanOptions = BooleanOptions
  orientationOptions = OrientationOptions
  manufacturedHomeTypeOptions = ManufacturedHomeTypeOptions
  setValidations = setValidations
  resetValuesAndValidations = resetValuesAndValidations

  get aboutControl() {
    return this.aboutForm.controls;
  }

  constructor(
    protected commonService: CommonService,
    private aboutService: AboutService,
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
    this.aboutForm = this.aboutInputs();
  }

  aboutInputs(): FormGroup {
    var about = this.fb.group({
      id: [null],
      assessmentDate: [null, [Validators.required]],
      yearBuilt: [null, [Validators.required, Validators.min(1600), Validators.max(new Date().getFullYear())]],
      numberOfBedrooms: [null, [Validators.required, Validators.min(1), Validators.max(10)]],
      // TODO for manufacture home it is missing
      storiesAboveGroundLevel: [null, [Validators.required, Validators.min(1), Validators.max(4)]],
      interiorFloorToCeilingHeight: [null, [Validators.required, Validators.min(6), Validators.max(12)]],
      totalConditionedFloorArea: [null, [Validators.required]],
      directionFacedByFrontOfHome: [null, [Validators.required]],
      blowerDoorTestConducted: [null, [Validators.required]],
      airLeakageRate: [null,],// 0-25000
      airSealed: [null, [Validators.required]],
      numberofUnitsInBuilding: [null], //min:2
      manufacturedHomeType: [null,],
      comments: [null,],
      buildingId: [this.buildingId],
    })
    const blowerDoorTestConducted = about.get('blowerDoorTestConducted') as AbstractControl
    const airLeakageRate = about.get('airLeakageRate') as AbstractControl
    blowerDoorTestConducted?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      if (val) {
        this.setValidations(airLeakageRate, [Validators.required, Validators.min(0), Validators.max(25000)])
      } else {
        this.resetValuesAndValidations(airLeakageRate)
      }
    })
    return about;
  }

  getBuildingId() {
    this.route.queryParamMap.pipe(takeUntil(this.destroy$)).subscribe(params => {
      if (params.get('id')) {
        this.commonService.buildingId = params.get('id');
      }
      this.buildingId = params.get('id') ?? this.commonService.buildingId ?? ""
    })
  }

  patchToForm(val: Result<AboutReadModel>) {
    if (val.data?.assessmentDate)
      val.data.assessmentDate = new Date(val.data?.assessmentDate);
    this.aboutForm.patchValue(val.data)
  }

  getData() {
    if (this.buildingId) {
      this.aboutService.getByBuildingId(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<AboutReadModel>) => {
          if (val?.failed == false) {
            this.patchToForm(val)
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
    if (this.aboutForm.invalid) {
      this.aboutForm.markAllAsTouched();
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }
    if (this.aboutForm.value?.id) {
      this.aboutReadModel = this.aboutForm.value
      this.aboutService.update(this.aboutReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<AboutReadModel>) => {
          this.patchToForm(val)
          console.log(val);
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    } else {
      this.aboutReadModel = this.aboutForm.value
      delete this.aboutReadModel.id;
      this.aboutService.create(this.aboutReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<AboutReadModel>) => {
          if (val?.failed == false) {
            this.patchToForm(val)
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

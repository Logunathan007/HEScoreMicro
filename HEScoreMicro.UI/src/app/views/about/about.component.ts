import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService } from '../../shared/services/common/common.service';
import { AboutService } from '../../shared/services/about/about.service';
import { AboutReadModel } from '../../shared/models/about/about.read.model';
import { Unsubscriber } from '../../shared/modules/unsubscribe/unsubscribe.component.';
import { Result } from '../../shared/models/common/Result';
import { takeUntil } from 'rxjs';
import { BooleanOptions, OrientationOptions } from '../../shared/lookups/common.lookup';
import { ManufacturedHomeTypeOptions } from '../../shared/lookups/about.lookup';

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
    this.aboutForm = this.fb.group({
      id: [null],
      assessmentDate: [null,],
      yearBuilt: [null,],
      numberOfBedrooms: [null,],
      storiesAboveGroundLevel: [null,],
      interiorFloorToCeilingHeight: [null,],
      totalConditionedFloorArea: [null,],
      directionFacedByFrontOfHome: [null,],
      blowerDoorTestConducted: [null,],
      airLeakageRate: [null,],
      airSealed: [null,],
      manufacturedHomeType:[null,],
      comments: [null,],
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
      this.aboutService.getByBuildingId(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<AboutReadModel>) => {
          if(val?.failed == false){
            this.aboutForm.patchValue(val.data)
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
          this.aboutForm.patchValue(val.data)
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
            this.aboutForm.patchValue(val.data)
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
    this.router.navigate(['zones/floor'], {
      queryParams: { id: this.buildingId }
    })
  }
}

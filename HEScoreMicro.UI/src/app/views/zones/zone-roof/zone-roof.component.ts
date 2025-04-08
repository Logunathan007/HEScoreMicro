import { RoofAtticService } from './../../../shared/services/zone-roof/roof-attic.service';
import { Component, OnInit } from '@angular/core';
import { Result } from '../../../shared/models/common/Result';
import { ZoneRoofReadModel } from '../../../shared/models/zone-roof/zone-roof.read.model';
import { takeUntil } from 'rxjs';
import { removeNullIdProperties } from '../../../shared/modules/Transformers/TransormerFunction';
import { Unsubscriber } from '../../../shared/modules/unsubscribe/unsubscribe.component.';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { BooleanOptions } from '../../../shared/lookups/common.lookup';
import { CommonService } from '../../../shared/services/common/common.service';
import { ZoneRoofService } from '../../../shared/services/zone-roof/zone-roof.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AtticOrCeilingTypeOptions, FrameMaterialOptions, GlazingTypeOptions, PaneOptions, RoofColorOptions, RoofConstructionOptions, RoofExteriorFinishOptions } from '../../../shared/lookups/zone-roof.looup';
import { RoofAtticReadModel } from '../../../shared/models/zone-roof/roof-attic.read.model';

@Component({
  selector: 'app-zone-roof',
  templateUrl: './zone-roof.component.html',
  styleUrl: './zone-roof.component.scss',
  standalone: false
})
export class ZoneRoofComponent extends Unsubscriber implements OnInit {
  //variable initializations
  zoneRoofForm!: FormGroup | any;
  buildingId: string | null | undefined;
  zoneRoofReadModel!: ZoneRoofReadModel;
  removeNullIdProperties = removeNullIdProperties
  booleanOptions = BooleanOptions
  atticOrCeilingTypeOptions = AtticOrCeilingTypeOptions
  roofConstructionOptions = RoofConstructionOptions
  roofExteriorFinishOptions = RoofExteriorFinishOptions
  roofColorOptions = RoofColorOptions
  paneOptions = PaneOptions
  frameMaterialOptions = FrameMaterialOptions
  glazingTypeOptions = GlazingTypeOptions



  get zoneRoofControl() {
    return this.zoneRoofForm.controls;
  }
  get roofAtticsObj() {
    return this.zoneRoofControl['roofAttics'] as FormArray
  }


  constructor(
    protected commonService: CommonService,
    private zoneRoofService: ZoneRoofService,
    private roofAtticService: RoofAtticService,
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
    this.zoneRoofForm = this.zoneRoofInputs()
  }

  zoneRoofInputs(): FormGroup {
    var zoneRoof = this.fb.group({
      id: [null],
      buildingId: [this.buildingId],
      enableSecondRoofAttic: [null],
      roofAttics: this.fb.array([this.roofAtticInputs()]),
    })
    zoneRoof.get('enableSecondRoofAttic')?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe({
      next: (val: any) => {
        var floor: FormArray = zoneRoof.get('roofAttics') as FormArray;
        if (val == true) {
          if (floor.length == 1) {
            floor.push(this.roofAtticInputs())
          }
        } else {
          if (floor.length == 2) {
            this.deleteRoofAttic(floor);
          }
        }
      },
      error: (err: any) => {
        console.log(err);
      }
    })
    return zoneRoof;
  }

  roofAtticInputs(): FormGroup {
    var found = this.fb.group({
      id: [null],
      atticOrCeilingType: [null], //ng-select
      construction: [null], //ng-select
      exteriorFinish: [null],//ng-select
      cathedralCeilingArea: [null],
      cathedralCeilingInsulation: [null],
      roofArea:[null,],
      roofInsulation: [null],
      roofColor: [null],//ng-select
      absorptance: [null],
      skylightsPresent: [null], //ng-select
      skylightArea: [null],
      solarScreen: [null],//ng-select
      knowSkylightSpecification: [null],//ng-select
      uFactor: [null],
      shgc: [null],
      panes: [null],//ng-select
      frameMaterial: [null],//ng-select
      glazingType: [null],//ng-select
      atticFloorArea: [null],
      atticFloorInsulation: [null],
      kneeWallPresent: [null],//ng-select
      kneeWallArea: [null],
      kneeWallInsulation: [null],
      buildingId: [this.buildingId],
    })
    return found;
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
      this.zoneRoofService.getByBuildingId(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<ZoneRoofReadModel>) => {
          if (val?.failed == false)
            if (val?.data?.enableSecondRoofAttic) {
              if (this.roofAtticsObj.length == 1) {
                this.roofAtticsObj.push(this.roofAtticInputs())
              }
            }
          this.zoneRoofForm.patchValue(val.data)
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    }
  }
  onSave() {
    if (this.zoneRoofForm.invalid) {
      this.zoneRoofForm.markAllAsTouched();
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }
    this.zoneRoofReadModel = this.zoneRoofForm.value
    this.zoneRoofReadModel = removeNullIdProperties(this.zoneRoofReadModel);
    if (this.zoneRoofForm.value?.id) {
      this.zoneRoofService.update(this.zoneRoofReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<ZoneRoofReadModel>) => {
          this.zoneRoofForm.patchValue(val.data)
          console.log(val);
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    } else {
      this.zoneRoofService.create(this.zoneRoofReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<ZoneRoofReadModel>) => {
          if (val?.failed == false) {
            this.zoneRoofForm.patchValue(val.data)
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
  deleteRoofAttic(arr: FormArray) {
    var id = arr.at(1).get('id')?.value;
    if (id) {
      this.roofAtticService.delete(id).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<RoofAtticReadModel>) => {
          if (val.failed == false) {
            arr.removeAt(1);
          }
        },
        error: (err: any) => console.log(err)
      })
    } else {
      arr.removeAt(1);
    }
  }
  goNext() {
    this.router.navigate(['zones/wall'], {
      queryParams: { id: this.buildingId }
    })
  }
}


import { FoundationService } from './../../../shared/services/zone-floor/foundation.service';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { CommonService } from '../../../shared/services/common/common.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Unsubscriber } from '../../../shared/modules/unsubscribe/unsubscribe.component.';
import { ZoneFloorReadModel } from '../../../shared/models/zone-floor/zone-floor.read.model';
import { BooleanOptions } from '../../../shared/lookups/common.lookup';
import { ZoneFloorService } from '../../../shared/services/zone-floor/zone-floor.service';
import { Result } from '../../../shared/models/common/Result';
import { takeUntil } from 'rxjs';
import { FoundationTypeOptions } from '../../../shared/lookups/zone-floor.lookups';
import { removeNullIdProperties } from '../../../shared/modules/Transformers/TransormerFunction';
import { FoundationReadModel } from '../../../shared/models/zone-floor/foundation.read.model';

@Component({
  selector: 'app-zone-floor',
  templateUrl: './zone-floor.component.html',
  styleUrl: './zone-floor.component.scss',
  standalone: false
})
export class ZoneFloorComponent extends Unsubscriber implements OnInit {
  //variable initializations
  zoneFloorForm!: FormGroup | any;
  buildingId: string | null | undefined;
  zoneFloorReadModel!: ZoneFloorReadModel;
  booleanOptions = BooleanOptions
  foundationTypeOptions = FoundationTypeOptions
  removeNullIdProperties = removeNullIdProperties

  get zoneFloorControl() {
    return this.zoneFloorForm.controls;
  }
  get foundationsObj() {
    return this.zoneFloorControl['foundations'] as FormArray
  }


  constructor(
    protected commonService: CommonService,
    private zoneFloorService: ZoneFloorService,
    private foundationService: FoundationService,
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
    this.zoneFloorForm = this.zoneFloorInputs()
  }

  zoneFloorInputs(): FormGroup {
    var zoneFloor = this.fb.group({
      id: [null],
      buildingId: [this.buildingId],
      enableSecondFoundation: [null],
      foundations: this.fb.array([this.foundationInputs()]),
    })
    zoneFloor.get('enableSecondFoundation')?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe({
      next: (val: any) => {
        var found: FormArray = zoneFloor.get('foundations') as FormArray;
        if (val == true) {
          if (found.length == 1) {
            this.foundationsObj.push(this.foundationInputs())
          }
        } else {
          if (found.length == 2) {
            this.deleteFoundation(found);
          }
        }
      },
      error: (err: any) => {
        console.log(err);
      }
    })
    return zoneFloor;
  }

  foundationInputs(): FormGroup {
    var found = this.fb.group({
      id: [null,],
      foundationType: [null,],
      foundationArea: [null,],
      slabInsulationLevel: [null,],
      floorInsulationLevel: [null,],
      foundationwallsInsulationLevel: [null,],
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
      this.zoneFloorService.getByBuildingId(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<ZoneFloorReadModel>) => {
          if (val?.failed == false)
            if (val?.data?.enableSecondFoundation) {
              if (this.foundationsObj.length == 1) {
                this.foundationsObj.push(this.foundationInputs())
              }
            }
          this.zoneFloorForm.patchValue(val.data)
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    }
  }
  onSave() {
    if (this.zoneFloorForm.invalid) {
      this.zoneFloorForm.markAllAsTouched();
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }
    this.zoneFloorReadModel = this.zoneFloorForm.value
    this.zoneFloorReadModel = removeNullIdProperties(this.zoneFloorReadModel);
    if (this.zoneFloorForm.value?.id) {
      this.zoneFloorService.update(this.zoneFloorReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<ZoneFloorReadModel>) => {
          this.zoneFloorForm.patchValue(val.data)
          console.log(val);
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    } else {
      this.zoneFloorService.create(this.zoneFloorReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<ZoneFloorReadModel>) => {
          if (val?.failed == false) {
            this.zoneFloorForm.patchValue(val.data)
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
  deleteFoundation(arr: FormArray) {
    var id = arr.at(1).get('id')?.value;
    if (id) {
      this.foundationService.delete(id).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<FoundationReadModel>) => {
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
    this.router.navigate(['zones/roof'], {
      queryParams: { id: this.buildingId }
    })
  }
}


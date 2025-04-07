import { WallReadModel } from './../../../shared/models/zone-wall/wall.read.model';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ZoneWallService } from '../../../shared/services/zooe-wall/zooe-wall.service';
import { CommonService } from '../../../shared/services/common/common.service';
import { removeNullIdProperties } from '../../../shared/modules/Transformers/TransormerFunction';
import { BooleanOptions } from '../../../shared/lookups/common.lookup';
import { WallConstructionOptions, WallExteriorFinishOptions } from '../../../shared/lookups/zone-wall.lookup';
import { ZoneWallReadModel } from '../../../shared/models/zone-wall/zone-wall.read.model';
import { Unsubscriber } from '../../../shared/modules/unsubscribe/unsubscribe.component.';
import { takeUntil } from 'rxjs';
import { Result } from '../../../shared/models/common/Result';
import { WallService } from '../../../shared/services/zooe-wall/wall.service';

@Component({
  selector: 'app-zone-wall',
  templateUrl: './zone-wall.component.html',
  styleUrl: './zone-wall.component.scss',
  standalone: false
})
export class ZoneWallComponent extends Unsubscriber implements OnInit {
  //variable initializations
  zoneWallForm!: FormGroup | any;
  buildingId: string | null | undefined;
  zoneWallReadModel!: ZoneWallReadModel;
  removeNullIdProperties = removeNullIdProperties
  booleanOptions = BooleanOptions
  wallConstructionOptions = WallConstructionOptions
  wallExteriorFinishOptions = WallExteriorFinishOptions

  get zoneWallControl() {
    return this.zoneWallForm.controls;
  }
  get wallsObj() {
    return this.zoneWallControl['walls'] as FormArray
  }


  constructor(
    protected commonService: CommonService,
    private zoneWallService: ZoneWallService,
    private wallService: WallService,
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
    this.zoneWallForm = this.zoneWallInputs()
  }

  zoneWallInputs(): FormGroup {
    var zoneWall = this.fb.group({
      id: [null],
      buildingId: [this.buildingId],
      exteriorWallSame: [null],
      walls: this.fb.array([this.wallInputs()]),
    })
    zoneWall.get('exteriorWallSame')?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe({
      next: (val: any) => {
        if (val == false) {
          while (this.wallsObj.length < 4)
            this.wallsObj.push(this.wallInputs())
        } else {
          this.deleteSameWalls();
        }
      },
      error: (err: any) => {
        console.log(err);
      }
    })
    return zoneWall;
  }

  wallInputs(): FormGroup {
    var wall = this.fb.group({
      id: [null],
      buildingId: [this.buildingId],
      construction: [null],
      exteriorFinish: [null],
      wallInsulationLevel: [null],
    })
    return wall;
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
      this.zoneWallService.getByBuildingId(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<ZoneWallReadModel>) => {
          if (val?.failed == false) {
            if (val?.data?.exteriorWallSame == false) {
              while (this.wallsObj.length < 4) {
                this.wallsObj.push(this.wallInputs())
              }
            } else {
              if (this.wallsObj.length != 1) {
                this.wallsObj.clear();
                this.wallsObj.push(this.wallInputs())
              }
            }
            this.zoneWallForm.patchValue(val.data)
          }
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    }
  }
  onSave() {
    if (this.zoneWallForm.invalid) {
      this.zoneWallForm.markAllAsTouched();
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }
    this.zoneWallReadModel = this.zoneWallForm.value
    this.zoneWallReadModel = removeNullIdProperties(this.zoneWallReadModel);
    if (this.zoneWallForm.value?.id) {
      this.zoneWallService.update(this.zoneWallReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<ZoneWallReadModel>) => {
          this.zoneWallForm.patchValue(val.data)
          console.log(val);
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    } else {
      this.zoneWallService.create(this.zoneWallReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<ZoneWallReadModel>) => {
          if (val?.failed == false) {
            this.zoneWallForm.patchValue(val.data)
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
  deleteSameWalls() {
    const wallIds:string[] = this.wallsObj?.value?.reduce((acc: string[], obj: WallReadModel,index: number) => {
      if (obj.id && index!=0) acc.push(obj.id);
      return acc;
    }, []);
    if(wallIds.length){
      this.wallService.bulkDelete(wallIds).subscribe({
        next: (val: Result<WallReadModel>) => {
          if (val.failed == false) {
            while (this.wallsObj.length > 1) {
              this.wallsObj.removeAt(this.wallsObj.length - 1);
            }
          }
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    }else{
      while (this.wallsObj.length > 1) {
        this.wallsObj.removeAt(this.wallsObj.length - 1);
      }
    }
  }
  goNext() {
    this.router.navigate(['systems/hvac-heat-cool'], {
      queryParams: { id: this.buildingId }
    })
  }
}

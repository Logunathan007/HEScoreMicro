
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ZoneWallService } from '../../shared/services/zooe-wall/zooe-wall.service';
import { removeNullIdProperties } from '../../shared/modules/Transformers/TransormerFunction';
import { BooleanOptions } from '../../shared/lookups/common.lookup';
import { WallConstructionOptions, WallExteriorFinishOptions } from '../../shared/lookups/zone-wall.lookup';
import { ZoneWallReadModel } from '../../shared/models/zone-wall/zone-wall.read.model';
import { Unsubscriber } from '../../shared/modules/unsubscribe/unsubscribe.component.';
import { takeUntil } from 'rxjs';
import { Result } from '../../shared/models/common/result.model';
import { WallService } from '../../shared/services/zooe-wall/wall.service';
import { resetValues } from '../../shared/modules/Validators/validators.module';
import { EmitterModel } from '../../shared/models/common/emitter.model';
import { WallReadModel } from '../../shared/models/zone-wall/wall.read.model';

@Component({
  selector: 'app-zone-wall',
  templateUrl: './zone-wall.component.html',
  styleUrl: './zone-wall.component.scss',
  standalone: false
})
export class ZoneWallComponent extends Unsubscriber implements OnInit {
  //variable initializations
  zoneWallForm!: FormGroup | any;
  @Input('buildingId') buildingId: string | null | undefined;
  @Output('update')
  updateEvent: EventEmitter<EmitterModel<ZoneWallReadModel>> = new EventEmitter();
  @Input('input') zoneWallReadModel!: ZoneWallReadModel;
  removeNullIdProperties = removeNullIdProperties
  resetValues = resetValues;
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
    private zoneWallService: ZoneWallService,
    private wallService: WallService,
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
    this.zoneWallForm = this.zoneWallInputs()
  }

  zoneWallInputs(): FormGroup {
    var zoneWall = this.fb.group({
      id: [null],
      buildingId: [this.buildingId],
      exteriorWallSame: [null, [Validators.required]],
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
      construction: [null, [Validators.required]],
      exteriorFinish: [null, [Validators.required]],
      exteriorFinishOptions: [null],
      wallInsulationLevel: [null, [Validators.required]],
    })
    let construction = wall.get('construction') as AbstractControl;
    let exteriorFinish = wall.get('exteriorFinish') as AbstractControl;
    let exteriorFinishOptions = wall.get('exteriorFinishOptions') as AbstractControl;

    construction.valueChanges?.pipe(takeUntil(this.destroy$)).subscribe((val) => {
      switch (val) {
        case "Wood Frame":
          exteriorFinishOptions.setValue(WallExteriorFinishOptions.filter(obj => obj.id != 5))
          break;
        case "Wood Frame with rigid foam sheathing":
          exteriorFinishOptions.setValue(WallExteriorFinishOptions.filter(obj => obj.id != 5))
          break;
        case "Wood Frame with Optimum Value Engineering (OVE)":
          exteriorFinishOptions.setValue(WallExteriorFinishOptions.filter(obj => obj.id != 5))
          break;
        case "Structural Brick":
          exteriorFinishOptions.setValue(WallExteriorFinishOptions.filter(obj => obj.id == 5))
          break;
        case "Concrete Block or Stone":
          exteriorFinishOptions.setValue(WallExteriorFinishOptions.filter(obj => obj.id == 1 || obj.id == 4 || obj.id == 5))
          break;
        case "Straw Bale":
          exteriorFinishOptions.setValue(WallExteriorFinishOptions.filter(obj => obj.id == 1))
          break;
        case "Steel Frame":
          exteriorFinishOptions.setValue(WallExteriorFinishOptions.filter(obj => obj.id != 5))
          break;
        default:
          exteriorFinishOptions.setValue(null)
          break;
      }
      this.resetValues(exteriorFinish)
    })
    return wall;
  }

  getData() {
    if (this.zoneWallReadModel) {
      if (this.zoneWallReadModel?.exteriorWallSame == false) {
        while (this.wallsObj.length < 4) {
          this.wallsObj.push(this.wallInputs())
        }
      } else {
        if (this.wallsObj.length != 1) {
          this.wallsObj.clear();
          this.wallsObj.push(this.wallInputs())
        }
      }
      this.zoneWallForm.patchValue(this.zoneWallReadModel)
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
          if (val?.failed == false) {
            this.zoneWallForm.patchValue(val.data)
            this.updateEvent.emit({
              fieldType: "zone-wall",
              field: val.data
            })
          }
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
            this.updateEvent.emit({
              fieldType: "zone-wall",
              field: val.data
            })
          }
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    }
  }

  deleteSameWalls() {
    const wallIds: string[] = this.wallsObj?.value?.reduce((acc: string[], obj: WallReadModel, index: number) => {
      if (obj.id && index != 0) acc.push(obj.id);
      return acc;
    }, []);
    if (wallIds.length) {
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
    } else {
      while (this.wallsObj.length > 1) {
        this.wallsObj.removeAt(this.wallsObj.length - 1);
      }
    }
  }
  @Output("move") move: EventEmitter<boolean> = new EventEmitter();
  goNext() {
    this.move.emit(true);
  }

}

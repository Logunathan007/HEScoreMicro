import { WallInsulationOptions } from './../../shared/lookups/support';
import { AdjacentToOptions } from './../../shared/lookups/zone-wall.lookup';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ZoneWallService } from '../../shared/services/zooe-wall/zooe-wall.service';
import { removeNullIdProperties, getDirection } from '../../shared/modules/Transformers/TransormerFunction';
import { BooleanOptions } from '../../shared/lookups/common.lookup';
import { WallConstructionOptions, WallExteriorFinishOptions } from '../../shared/lookups/zone-wall.lookup';
import { ZoneWallReadModel } from '../../shared/models/zone-wall/zone-wall.read.model';
import { Unsubscriber } from '../../shared/modules/unsubscribe/unsubscribe.component.';
import { takeUntil } from 'rxjs';
import { Result } from '../../shared/models/common/result.model';
import { WallService } from '../../shared/services/zooe-wall/wall.service';
import { resetValues, resetValuesAndValidations, setValidations } from '../../shared/modules/Validators/validators.module';
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
  @Input('buildingId') buildingId: string | null | undefined;
  @Input('buildingType') buildingType!: number | null;
  @Input('input') zoneWallReadModel!: ZoneWallReadModel;

  @Output('update') updateEvent: EventEmitter<EmitterModel<ZoneWallReadModel>> = new EventEmitter();
  @Output("move") move: EventEmitter<boolean> = new EventEmitter();

  getDirection = getDirection
  booleanOptions = BooleanOptions
  wallConstructionOptions = WallConstructionOptions
  wallExteriorFinishOptions = WallExteriorFinishOptions
  adjacentToOptions = AdjacentToOptions
  zoneWallForm!: FormGroup | any;

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
      exteriorWallSame: [null,],
      walls: this.fb.array([this.wallInputs()]),
    })
    const exteriorWallSame = zoneWall.get('exteriorWallSame') as AbstractControl;
    const walls = zoneWall.get('walls') as FormArray;

    if (this.buildingType === 0) {
      setValidations(exteriorWallSame);
      zoneWall.get('exteriorWallSame')?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: any) => {
          if (val == false) {
            while (walls.length < 4)
              walls.push(this.wallInputs())
          } else {
            this.deleteSameWalls();
          }
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    } else {
      while (walls.length < 4)
        walls.push(this.wallInputs())
    }
    return zoneWall;
  }

  wallInputs(): FormGroup {
    var wall = this.fb.group({
      id: [null],
      facing: [0, [Validators.required]],
      adjacentTo: ["Outside", [Validators.required]],
      construction: [null, [Validators.required]],
      exteriorFinish: [null, [Validators.required]],
      exteriorFinishOptions: [null],
      wallInsulationLevel: [null, [Validators.required]],
      wallInsulationOptions: [null]
    })
    let adjacentTo = wall.get('adjacentTo') as AbstractControl;
    let construction = wall.get('construction') as AbstractControl;
    let exteriorFinish = wall.get('exteriorFinish') as AbstractControl;
    let exteriorFinishOptions = wall.get('exteriorFinishOptions') as AbstractControl;
    let wallInsulationLevel = wall.get('wallInsulationLevel') as AbstractControl;
    let wallInsulationOptions = wall.get('wallInsulationOptions') as AbstractControl;
    adjacentTo.valueChanges?.pipe(takeUntil(this.destroy$)).subscribe((val) => {
      resetValuesAndValidations([construction, exteriorFinish, wallInsulationLevel,wallInsulationOptions])
      if (val == "Outside") {
        setValidations([construction, exteriorFinish, wallInsulationLevel])
      } else if (val && val !== "Other Unit") {
        setValidations([wallInsulationLevel])
        wallInsulationOptions.setValue(WallInsulationOptions.filter(obj => ([0, 3, 7, 11, 13, 15, 19, 21]).includes(obj.value)))
      }
    })
    construction.valueChanges?.pipe(takeUntil(this.destroy$)).subscribe((val) => {
      switch (val) {
        case "Wood Frame":
          wallInsulationOptions.setValue(WallInsulationOptions.filter(obj => ([0, 3, 7, 11, 13, 15, 19, 21]).includes(obj.value)))
          exteriorFinishOptions.setValue(WallExteriorFinishOptions.filter(obj => obj.id != 5))
          break;
        case "Wood Frame with rigid foam sheathing":
          wallInsulationOptions.setValue(WallInsulationOptions.filter(obj => ([0, 3, 7, 11, 13, 15, 19, 21]).includes(obj.value)))
          exteriorFinishOptions.setValue(WallExteriorFinishOptions.filter(obj => obj.id != 5))
          break;
        case "Wood Frame with Optimum Value Engineering (OVE)":
          wallInsulationOptions.setValue(WallInsulationOptions.filter(obj => obj.value >= 19))
          exteriorFinishOptions.setValue(WallExteriorFinishOptions.filter(obj => obj.id != 5))
          break;
        case "Structural Brick":
          wallInsulationOptions.setValue(WallInsulationOptions.filter(obj => ([0, 5, 10]).includes(obj.value)))
          exteriorFinishOptions.setValue(WallExteriorFinishOptions.filter(obj => obj.id == 5))
          break;
        case "Concrete Block or Stone":
          wallInsulationOptions.setValue(WallInsulationOptions.filter(obj => ([0, 3, 6]).includes(obj.value)))
          exteriorFinishOptions.setValue(WallExteriorFinishOptions.filter(obj => obj.id == 1 || obj.id == 4 || obj.id == 5))
          break;
        case "Straw Bale":
          wallInsulationOptions.setValue(WallInsulationOptions.filter(obj => obj.value == 0))
          exteriorFinishOptions.setValue(WallExteriorFinishOptions.filter(obj => obj.id == 1))
          break;
        case "Steel Frame":
          wallInsulationOptions.setValue(WallInsulationOptions.filter(obj => ([0, 3, 7, 11, 13, 15, 19, 21]).includes(obj.value)))
          exteriorFinishOptions.setValue(WallExteriorFinishOptions.filter(obj => obj.id != 5))
          break;
        default:
          exteriorFinishOptions.setValue(null)
          break;
      }
      resetValues([exteriorFinish, wallInsulationLevel, exteriorFinish])
    })
    return wall;
  }

  getData() {
    if (this.zoneWallReadModel) {
      while (this.wallsObj.length < this.zoneWallReadModel.walls.length) {
        this.wallsObj.push(this.wallInputs())
      }
      this.zoneWallForm.patchValue(this.zoneWallReadModel)
    }
  }
  updateFacingValues() {
    this.wallsObj.controls.forEach((control, index) => {
      control.get('facing')?.setValue(index);
    });
  }
  onSave() {
    if (this.zoneWallForm.invalid) {
      this.zoneWallForm.markAllAsTouched();
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }
    this.updateFacingValues();
    this.zoneWallReadModel = this.zoneWallForm.value
    this.zoneWallReadModel = removeNullIdProperties(this.zoneWallReadModel);
    if (this.zoneWallForm.value?.id) {
      this.zoneWallService.update(this.zoneWallReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<ZoneWallReadModel>) => {
          if (val?.failed === false) {
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
          if (val?.failed === false) {
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
          if (val.failed === false) {
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
}

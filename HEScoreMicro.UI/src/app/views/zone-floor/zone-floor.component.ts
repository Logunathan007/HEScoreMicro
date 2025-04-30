import { FoundationService } from '../../shared/services/zone-floor/foundation.service';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChange, SimpleChanges } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Unsubscriber } from '../../shared/modules/unsubscribe/unsubscribe.component.';
import { ZoneFloorReadModel } from '../../shared/models/zone-floor/zone-floor.read.model';
import { BooleanOptions } from '../../shared/lookups/common.lookup';
import { ZoneFloorService } from '../../shared/services/zone-floor/zone-floor.service';
import { Result } from '../../shared/models/common/result.model';
import { takeUntil } from 'rxjs';
import { FoundationFloorInsulationOptions, FoundationSlabInsulationOptions, FoundationTypeOptions, FoundationWallInsulationOptions } from '../../shared/lookups/zone-floor.lookups';
import { removeNullIdProperties } from '../../shared/modules/Transformers/TransormerFunction';
import { FoundationReadModel } from '../../shared/models/zone-floor/foundation.read.model';
import { isValidFoundationArea, resetValuesAndValidations, setValidations } from '../../shared/modules/Validators/validators.module';
import { EmitterModel } from '../../shared/models/common/emitter.model';

@Component({
  selector: 'app-zone-floor',
  templateUrl: './zone-floor.component.html',
  styleUrl: './zone-floor.component.scss',
  standalone: false
})
export class ZoneFloorComponent extends Unsubscriber implements OnInit, OnChanges {
  //variable initializations
  zoneFloorForm!: FormGroup | any;
  @Input('buildingId') buildingId: string | null | undefined;
  @Input('buildingType') buildingType!: number | null;
  @Input('totalRoofArea') totalRoofArea!: number | null | undefined;
  @Input('footPrint') footPrint: number | null | undefined;

  @Input('input') zoneFloorReadModel!: ZoneFloorReadModel;
  @Output('update')
  updateEvent: EventEmitter<EmitterModel<ZoneFloorReadModel>> = new EventEmitter();
  booleanOptions = BooleanOptions
  foundationTypeOptions: any;
  foundationSlabInsulationOptions = FoundationSlabInsulationOptions
  foundationFloorInsulationOptions = FoundationFloorInsulationOptions
  foundationWallInsulationOptions = FoundationWallInsulationOptions

  get zoneFloorControl() {
    return this.zoneFloorForm.controls;
  }
  get foundationsObj() {
    return this.zoneFloorControl['foundations'] as FormArray
  }

  constructor(
    private zoneFloorService: ZoneFloorService,
    private foundationService: FoundationService,
    public fb: FormBuilder,
  ) {
    super()
  }

  ngOnInit(): void {
    this.variableDeclaration();
    this.getData();
  }

  ngOnChanges(changes: SimpleChanges): void {
    const buildingTypeChange: SimpleChange | undefined = changes['buildingType'];
    if (buildingTypeChange) {
      if (buildingTypeChange.currentValue == 0)
        this.foundationTypeOptions = FoundationTypeOptions.filter(obj => obj.id != 5)
      else if (buildingTypeChange.currentValue == 1)
        this.foundationTypeOptions = FoundationTypeOptions
    }
  }

  //variable declarations
  variableDeclaration() {
    this.zoneFloorForm = this.zoneFloorInputs()
  }

  zoneFloorInputs(): FormGroup {
    var zoneFloor = this.fb.group({
      id: [null],
      buildingId: [this.buildingId],
      enableSecondFoundation: [false, [Validators.required]],
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
      foundationType: [null, [Validators.required]],
      foundationArea: [null, [Validators.required,Validators.min(1),Validators.max(25000)]],
      tracker: [{ wall: false, floor: false, slab: false }],
      slabInsulationLevel: [null,],
      floorInsulationLevel: [null,],
      foundationwallsInsulationLevel: [null,],
    })

    const foundationType = found.get('foundationType') as AbstractControl
    const tracker = found.get('tracker') as AbstractControl
    const slabInsulationLevel = found.get('slabInsulationLevel') as AbstractControl
    const floorInsulationLevel = found.get('floorInsulationLevel') as AbstractControl
    const foundationwallsInsulationLevel = found.get('foundationwallsInsulationLevel') as AbstractControl

    foundationType.valueChanges?.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      resetValuesAndValidations([floorInsulationLevel, foundationwallsInsulationLevel, slabInsulationLevel])
      switch (foundationType.value) {
        case "Slab-on-grade foundation":
          tracker.setValue({ wall: false, floor: false, slab: true })
          setValidations(slabInsulationLevel)
          break;
        case "Unconditioned Basement":
          tracker.setValue({ wall: true, floor: true, slab: false })
          setValidations([floorInsulationLevel, foundationwallsInsulationLevel])
          break;
        case "Conditioned Basement":
          tracker.setValue({ wall: true, floor: false, slab: false })
          setValidations(foundationwallsInsulationLevel)
          break;
        case "Unvented Crawlspace / Unconditioned Garage":
          tracker.setValue({ wall: true, floor: true, slab: false })
          setValidations([floorInsulationLevel, foundationwallsInsulationLevel])
          break;
        case "Vented Crawlspace":
          tracker.setValue({ wall: true, floor: true, slab: false })
          setValidations([floorInsulationLevel, foundationwallsInsulationLevel])
          break;
        // for sandbeta version
        // case "Belly and Wing":
        //   tracker.setValue({ wall: false, floor: true, slab: false })
        //   setValidations(floorInsulationLevel)
        //   break;
        default: // "Above Other Unit"
          tracker.setValue({ wall: false, floor: false, slab: false })
      }
    })
    return found;
  }

  getData() {
    if (this.zoneFloorReadModel) {
      if (this.zoneFloorReadModel?.enableSecondFoundation) {
        if (this.foundationsObj.length == 1) {
          this.foundationsObj.push(this.foundationInputs())
        }
      }
      this.zoneFloorForm.patchValue(this.zoneFloorReadModel)
    }
  }
  validateArea(): boolean {
    let values = this.zoneFloorForm.value;
    let totalArea = values.foundations?.reduce((sum: number, item: any) => {
      return sum + (item?.foundationArea ?? 0)
    }, 0)
    let res = isValidFoundationArea(totalArea, this.footPrint as number, this.totalRoofArea as number)
    if (!res[0]) {
      alert(`Error : Sum of Foundation Area must be ${res[1]} - ${res[2]}, Current Area: ${totalArea}`)
      return false;
    }
    return true
  }
  onSave() {
    if (this.zoneFloorForm.invalid) {
      this.zoneFloorForm.markAllAsTouched();
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }
    if (!this.validateArea()) return;
    this.zoneFloorReadModel = this.zoneFloorForm.value
    this.zoneFloorReadModel = removeNullIdProperties(this.zoneFloorReadModel);
    if (this.zoneFloorForm.value?.id) {
      this.zoneFloorService.update(this.zoneFloorReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<ZoneFloorReadModel>) => {
          this.zoneFloorForm.patchValue(val.data)
          this.updateEvent.emit({
            fieldType: "zone-floor",
            field: val.data
          })
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    } else {
      this.zoneFloorService.create(this.zoneFloorReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<ZoneFloorReadModel>) => {
          if (val?.failed === false) {
            this.zoneFloorForm.patchValue(val.data)
            this.updateEvent.emit({
              fieldType: "zone-floor",
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
  deleteFoundation(arr: FormArray) {
    var id = arr.at(1).get('id')?.value;
    if (id) {
      this.foundationService.delete(id).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<FoundationReadModel>) => {
          if (val.failed === false) {
            arr.removeAt(1);
          }
        },
        error: (err: any) => console.log(err)
      })
    } else {
      arr.removeAt(1);
    }
  }
  @Output("move") move: EventEmitter<boolean> = new EventEmitter();
  goNext() {
    this.move.emit(true);
  }
}


import { RoofAtticService } from './../../../shared/services/zone-roof/roof-attic.service';
import { Component, OnInit } from '@angular/core';
import { Result } from '../../../shared/models/common/result.model';
import { ZoneRoofReadModel } from '../../../shared/models/zone-roof/zone-roof.read.model';
import { takeUntil } from 'rxjs';
import { removeNullIdProperties } from '../../../shared/modules/Transformers/TransormerFunction';
import { Unsubscriber } from '../../../shared/modules/unsubscribe/unsubscribe.component.';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BooleanOptions } from '../../../shared/lookups/common.lookup';
import { CommonService } from '../../../shared/services/common/common.service';
import { ZoneRoofService } from '../../../shared/services/zone-roof/zone-roof.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AtticOrCeilingTypeOptions, FrameMaterialOptions, GlazingTypeOptions, PaneOptions, RoofColorOptions, RoofConstructionOptions, RoofExteriorFinishOptions } from '../../../shared/lookups/zone-roof.looup';
import { RoofAtticReadModel } from '../../../shared/models/zone-roof/roof-attic.read.model';
import { resetValues, resetValuesAndValidations, setValidations } from '../../../shared/modules/Validators/validators.module';

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
  setValidations = setValidations
  resetValuesAndValidations = resetValuesAndValidations
  resetValues = resetValues
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
      enableSecondRoofAttic: [false, [Validators.required]],
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
      atticOrCeilingType: [null, [Validators.required]], //ng-select
      construction: [null, [Validators.required]], //ng-select
      exteriorFinish: [null, [Validators.required]],//ng-select
      roofArea: [null, [Validators.required]],
      roofInsulation: [null, [Validators.required]],
      roofColor: [null, [Validators.required]],//ng-select
      absorptance: [null], //0-1
      skylightsPresent: [null, [Validators.required]], //ng-select
      skylightArea: [null],
      solarScreen: [null],//ng-select
      knowSkylightSpecification: [null],//ng-select
      uFactor: [null],
      shgc: [null],
      panes: [null],
      panesOptions: [null],
      frameMaterial: [null],
      frameMaterialOptions: [null],
      glazingType: [null],
      glazingTypeOptions: [null],
      atticFloorArea: [null],
      atticFloorInsulation: [null],
      kneeWallPresent: [null],//ng-select
      kneeWallArea: [null],
      kneeWallInsulation: [null],
      buildingId: [this.buildingId],
    })
    const atticOrCeilingType = found.get('atticOrCeilingType') as AbstractControl
    const roofColor = found.get('roofColor') as AbstractControl
    const absorptance = found.get('absorptance') as AbstractControl
    const skylightsPresent = found.get('skylightsPresent') as AbstractControl
    const skylightArea = found.get('skylightArea') as AbstractControl
    const solarScreen = found.get('solarScreen') as AbstractControl
    const knowSkylightSpecification = found.get('knowSkylightSpecification') as AbstractControl
    const uFactor = found.get('uFactor') as AbstractControl
    const shgc = found.get('shgc') as AbstractControl
    const panes = found.get('panes') as AbstractControl
    const frameMaterial = found.get('frameMaterial') as AbstractControl
    const glazingType = found.get('glazingType') as AbstractControl
    const atticFloorArea = found.get('atticFloorArea') as AbstractControl
    const atticFloorInsulation = found.get('atticFloorInsulation') as AbstractControl
    const kneeWallPresent = found.get('kneeWallPresent') as AbstractControl
    const kneeWallArea = found.get('kneeWallArea') as AbstractControl
    const kneeWallInsulation = found.get('kneeWallInsulation') as AbstractControl
    const frameMaterialOptions = found.get('frameMaterialOptions') as AbstractControl
    const glazingTypeOptions = found.get('glazingTypeOptions') as AbstractControl

    atticOrCeilingType.valueChanges?.pipe(takeUntil(this.destroy$)).subscribe((val: string) => {
      if (val == "Unconditioned Attic") {
        this.setValidations([atticFloorArea, atticFloorInsulation, kneeWallPresent])
      } else {
        this.resetValuesAndValidations([atticFloorArea, atticFloorInsulation, kneeWallPresent])
      }
    })
    roofColor.valueChanges?.pipe(takeUntil(this.destroy$)).subscribe((val: string) => {
      if (val == "Cool Color") {
        this.setValidations(absorptance, [Validators.required, Validators.min(0), Validators.max(1)])
      } else {
        this.resetValuesAndValidations(absorptance)
      }
    })
    skylightsPresent.valueChanges?.pipe(takeUntil(this.destroy$)).subscribe((val: string) => {
      if (val) {
        this.setValidations(solarScreen)
        this.setValidations(skylightArea,[Validators.required,Validators.min(1),Validators.max(300)])
      } else {
        this.resetValuesAndValidations([solarScreen, skylightArea])
      }
    })
    knowSkylightSpecification.valueChanges?.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      if (val) {
        this.setValidations(uFactor, [Validators.required, Validators.min(0.1), Validators.max(5)])
        this.setValidations(shgc, [Validators.required, Validators.min(0), Validators.max(0.99)])
        this.resetValuesAndValidations([panes, frameMaterial, glazingType])
      } else if (val == false) {
        this.resetValuesAndValidations([uFactor, shgc])
        this.setValidations([panes, frameMaterial, glazingType])
      } else {
        this.resetValuesAndValidations([panes, frameMaterial, glazingType, uFactor, shgc])
      }
    })
    panes.valueChanges?.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      switch (val) {
        case "Single-Pane":
          frameMaterialOptions.setValue(FrameMaterialOptions.filter(obj => obj.id == 0 || obj.id == 2))
          break;
        case "Double-Pane":
          frameMaterialOptions.setValue(FrameMaterialOptions)
          break;
        case "Triple-Pane":
          frameMaterialOptions.setValue(FrameMaterialOptions.filter(obj => obj.id == 2))
          break;
        default:
          this.resetValues([frameMaterialOptions])
      }
      this.resetValues([frameMaterial, glazingType, glazingTypeOptions])
    })
    frameMaterial.valueChanges?.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      switch (val) {
        case "Aluminum":
          switch (panes?.value) {
            case "Single-Pane":
              glazingTypeOptions.setValue(GlazingTypeOptions.filter(obj => obj.id == 0 || obj.id == 1))
              break;
            case "Double-Pane":
              glazingTypeOptions.setValue(GlazingTypeOptions.filter(obj => obj.id == 0 || obj.id == 1 || obj.id == 2))
              break;
          }
          break;
        case "Aluminum with Thermal Break":
          glazingTypeOptions.setValue(GlazingTypeOptions.filter(obj => obj.id == 0 || obj.id == 1 || obj.id == 2 || obj.id == 5))
          break;
        case "Wood or Vinyl":
          switch (panes?.value) {
            case "Single-Pane":
              glazingTypeOptions.setValue(GlazingTypeOptions.filter(obj => obj.id == 0 || obj.id == 1))
              break;
            case "Double-Pane":
              glazingTypeOptions.setValue(GlazingTypeOptions)
              break;
            case "Triple-Pane":
              glazingTypeOptions.setValue(GlazingTypeOptions.filter(obj => obj.id == 5))
              break;
          }
          break;
        default:
          this.resetValues([glazingTypeOptions])
      }
      this.resetValues(glazingType)
    })

    kneeWallPresent.valueChanges?.pipe(takeUntil(this.destroy$)).subscribe((val: string) => {
      if (val) {
        this.setValidations([kneeWallArea, kneeWallInsulation])
      } else {
        this.resetValuesAndValidations([kneeWallArea, kneeWallInsulation])
      }
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
          if (val?.failed == false) {
            if (val?.data?.enableSecondRoofAttic) {
              if (this.roofAtticsObj.length == 1) {
                this.roofAtticsObj.push(this.roofAtticInputs())
              }
            }
            this.zoneRoofForm.patchValue(val.data)
          }
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


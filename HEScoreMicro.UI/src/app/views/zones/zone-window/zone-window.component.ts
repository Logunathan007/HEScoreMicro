import { WindowService } from './../../../shared/services/zone-window/window.service';
import { Component, OnInit } from '@angular/core';
import { Unsubscriber } from '../../../shared/modules/unsubscribe/unsubscribe.component.';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ZoneWindowReadModel } from '../../../shared/models/zone-window/zone-window.model';
import { removeNullIdProperties } from '../../../shared/modules/Transformers/TransormerFunction';
import { BooleanOptions } from '../../../shared/lookups/common.lookup';
import { takeUntil } from 'rxjs';
import { CommonService } from '../../../shared/services/common/common.service';
import { ZoneWindowService } from '../../../shared/services/zone-window/zone-window.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Result } from '../../../shared/models/common/result.model';
import { FrameMaterialOptions, GlazingTypeOptions, PaneOptions } from '../../../shared/lookups/zone-roof.looup';
import { WindowReadModel } from '../../../shared/models/zone-window/window.model';
import { resetControls, resetValuesAndValidations, setValidations } from '../../../shared/modules/Validators/validators.module';

@Component({
  selector: 'app-zone-window',
  standalone: false,
  templateUrl: './zone-window.component.html',
  styleUrl: './zone-window.component.scss'
})
export class ZoneWindowComponent extends Unsubscriber implements OnInit {
  //variable initializations
  zoneWindowForm!: FormGroup | any;
  buildingId: string | null | undefined;
  zoneWindowReadModel!: ZoneWindowReadModel;
  removeNullIdProperties = removeNullIdProperties
  booleanOptions = BooleanOptions
  paneOptions = PaneOptions
  frameMaterialOptions = FrameMaterialOptions
  glazingTypeOptions = GlazingTypeOptions
  setValidations = setValidations
  resetControls = resetControls
  resetValuesAndValidations = resetValuesAndValidations

  get zoneWindowControl() {
    return this.zoneWindowForm.controls;
  }
  get windowsObj() {
    return this.zoneWindowControl['windows'] as FormArray
  }


  constructor(
    protected commonService: CommonService,
    private zoneWindowService: ZoneWindowService,
    private windowService: WindowService,
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
    this.zoneWindowForm = this.zoneWindowInputs()
  }

  zoneWindowInputs(): FormGroup {
    var zoneWindow = this.fb.group({
      id: [null],
      buildingId: [this.buildingId],
      windowAreaFront: [null, [Validators.required, Validators.min(0)]],
      windowAreaBack: [null, [Validators.required, Validators.min(0)]],
      windowAreaLeft: [null, [Validators.required, Validators.min(0)]],
      windowAreaRight: [null, [Validators.required, Validators.min(0)]],
      windowsSame: [null, [Validators.required]],
      windows: this.fb.array([this.windowInputs()]),
    })
    const windowsSame = zoneWindow.get("windowsSame") as AbstractControl
    windowsSame?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe({
      next: (val: any) => {
        if (val == false) {
          while (this.windowsObj.length < 4)
            this.windowsObj.push(this.windowInputs())
        } else {
          this.deleteWindows()
        }
      },
      error: (err: any) => {
        console.log(err);
      }
    })
    return zoneWindow;
  }

  windowInputs(): FormGroup {
    var window = this.fb.group({
      id: [null],
      buildingId: [this.buildingId],
      solarScreen: [null, [Validators.required]],
      knowWindowSpecification: [null, [Validators.required]],
      uFactor: [null],
      shgc: [null],
      panes: [null],
      frameMaterial: [null],
      glazingType: [null],
    })
    const knowWindowSpecification = window.get('knowWindowSpecification') as AbstractControl
    const uFactor = window.get('uFactor') as AbstractControl
    const shgc = window.get('shgc') as AbstractControl
    const panes = window.get('panes') as AbstractControl
    const frameMaterial = window.get('frameMaterial') as AbstractControl
    const glazingType = window.get('glazingType') as AbstractControl
    knowWindowSpecification.valueChanges?.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      if (val) {
        this.setValidations(uFactor, [Validators.required, Validators.min(0.1), Validators.max(5)])
        this.setValidations(shgc, [Validators.required, Validators.min(0), Validators.max(1)])
        this.resetValuesAndValidations([panes, frameMaterial, glazingType])
      } else if (val == false) {
        this.resetValuesAndValidations([uFactor, shgc])
        this.setValidations([panes, frameMaterial, glazingType])
      }
    })
    return window;
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
      this.zoneWindowService.getByBuildingId(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<ZoneWindowReadModel>) => {
          if (val?.failed == false)
            if (val?.data?.windowsSame == false) {
              while (this.windowsObj.length < 4) {
                this.windowsObj.push(this.windowInputs())
              }
            }
          this.zoneWindowForm.patchValue(val.data)
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    }
  }

  onSave() {
    if (this.zoneWindowForm.invalid) {
      this.zoneWindowForm.markAllAsTouched();
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }
    this.zoneWindowReadModel = this.zoneWindowForm.value
    this.zoneWindowReadModel = removeNullIdProperties(this.zoneWindowReadModel);
    if (this.zoneWindowForm.value?.id) {
      this.zoneWindowService.update(this.zoneWindowReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<ZoneWindowReadModel>) => {
          this.zoneWindowForm.patchValue(val.data)
          console.log(val);
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    } else {
      this.zoneWindowService.create(this.zoneWindowReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<ZoneWindowReadModel>) => {
          if (val?.failed == false) {
            this.zoneWindowForm.patchValue(val.data)
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
  deleteWindows() {
    const windowIds: string[] = this.windowsObj?.value?.reduce((acc: string[], obj: WindowReadModel, index: number) => {
      if (obj.id && index != 0) acc.push(obj.id);
      return acc;
    }, []);
    if (windowIds.length) {
      this.windowService.bulkDelete(windowIds).subscribe({
        next: (val: Result<WindowReadModel>) => {
          if (val.failed == false) {
            while (this.windowsObj.length > 1) {
              this.windowsObj.removeAt(this.windowsObj.length - 1);
            }
          }
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    } else {
      while (this.windowsObj.length > 1) {
        this.windowsObj.removeAt(this.windowsObj.length - 1);
      }
    }
  }
  goNext() {
    this.router.navigate(['systems/heat-cool-system'], {
      queryParams: { id: this.buildingId }
    })
  }
}

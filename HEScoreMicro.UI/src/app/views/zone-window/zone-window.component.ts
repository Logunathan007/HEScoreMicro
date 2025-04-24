import { FrameMaterialOptions, GlazingTypeOptions } from './../../shared/lookups/zone-roof.looup';
import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { Unsubscriber } from "../../shared/modules/unsubscribe/unsubscribe.component.";
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ZoneWindowReadModel } from "../../shared/models/zone-window/zone-window.model";
import { removeNullIdProperties } from "../../shared/modules/Transformers/TransormerFunction";
import { BooleanOptions, EmptyOptions } from "../../shared/lookups/common.lookup";
import { PaneOptions } from "../../shared/lookups/zone-roof.looup";
import { resetValues, resetValuesAndValidations, setValidations, windowAreaAverageValidator } from "../../shared/modules/Validators/validators.module";
import { ZoneWindowService } from "../../shared/services/zone-window/zone-window.service";
import { WindowService } from "../../shared/services/zone-window/window.service";
import { takeUntil } from "rxjs";
import { Result } from '../../shared/models/common/result.model';
import { WindowReadModel } from '../../shared/models/zone-window/window.model';
import { EmitterModel } from '../../shared/models/common/emitter.model';
import { ZoneRoofReadModel } from '../../shared/models/zone-roof/zone-roof.read.model';

@Component({
  selector: 'app-zone-window',
  standalone: false,
  templateUrl: './zone-window.component.html',
  styleUrl: './zone-window.component.scss'
})
export class ZoneWindowComponent extends Unsubscriber implements OnInit {
  //variable initializations
  zoneWindowForm!: FormGroup | any;
  @Input('buildingId') buildingId: string | null | undefined;
  @Output('update')
  updateEvent: EventEmitter<EmitterModel<ZoneWindowReadModel>> = new EventEmitter();
  @Input('input') zoneWindowReadModel!: ZoneWindowReadModel;
  removeNullIdProperties = removeNullIdProperties
  booleanOptions = BooleanOptions
  paneOptions = PaneOptions
  frameMaterialOptions = EmptyOptions
  glazingTypeOptions = EmptyOptions
  setValidations = setValidations
  resetValuesAndValidations = resetValuesAndValidations
  resetValues = resetValues
  windowAreaAverageValidator = windowAreaAverageValidator

  get zoneWindowControl() {
    return this.zoneWindowForm.controls;
  }
  get windowsObj() {
    return this.zoneWindowControl['windows'] as FormArray
  }

  constructor(
    private zoneWindowService: ZoneWindowService,
    private windowService: WindowService,
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
    }, { validators: [windowAreaAverageValidator] })
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
      panesOptions: [null],
      frameMaterial: [null],
      frameMaterialOptions: [null],
      glazingType: [null],
      glazingTypeOptions: [null],
    })
    const knowWindowSpecification = window.get('knowWindowSpecification') as AbstractControl
    const uFactor = window.get('uFactor') as AbstractControl
    const shgc = window.get('shgc') as AbstractControl
    const panes = window.get('panes') as AbstractControl
    const frameMaterial = window.get('frameMaterial') as AbstractControl
    const glazingType = window.get('glazingType') as AbstractControl
    const frameMaterialOptions = window.get('frameMaterialOptions') as AbstractControl
    const glazingTypeOptions = window.get('glazingTypeOptions') as AbstractControl

    knowWindowSpecification.valueChanges?.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
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
    return window;
  }

  getData() {
    if (this.zoneWindowReadModel) {
      if (this.zoneWindowReadModel?.windowsSame == false) {
        while (this.windowsObj.length < 4) {
          this.windowsObj.push(this.windowInputs())
        }
      }
      this.zoneWindowForm.patchValue(this.zoneWindowReadModel)
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
          if (val.failed == false) {
            this.zoneWindowForm.patchValue(val.data)
            this.updateEvent.emit({
              fieldType: "zone-window",
              field: val.data
            })
          }
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
            this.updateEvent.emit({
              fieldType: "zone-window",
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

  @Output("move") move: EventEmitter<boolean> = new EventEmitter();
  goNext() {
    this.move.emit(true);
  }

}

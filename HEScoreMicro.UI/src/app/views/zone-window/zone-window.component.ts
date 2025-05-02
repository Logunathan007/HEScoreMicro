import { FrameMaterialOptions, GlazingTypeOptions } from './../../shared/lookups/zone-roof.looup';
import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { Unsubscriber } from "../../shared/modules/unsubscribe/unsubscribe.component.";
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ZoneWindowReadModel } from "../../shared/models/zone-window/zone-window.model";
import { removeNullIdProperties, getDirection } from "../../shared/modules/Transformers/TransormerFunction";
import { BooleanOptions, EmptyOptions } from "../../shared/lookups/common.lookup";
import { PaneOptions } from "../../shared/lookups/zone-roof.looup";
import { resetValues, resetValuesAndValidations, setValidations, windowAreaAverageValidator } from "../../shared/modules/Validators/validators.module";
import { ZoneWindowService } from "../../shared/services/zone-window/zone-window.service";
import { WindowService } from "../../shared/services/zone-window/window.service";
import { takeUntil } from "rxjs";
import { Result } from '../../shared/models/common/result.model';
import { WindowReadModel } from '../../shared/models/zone-window/window.model';
import { EmitterModel } from '../../shared/models/common/emitter.model';
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
  @Input('buildingType') buildingType!: number | null;
  @Input('windowsAvailable') windowsAvailable: number[] = [];
  @Output('update')
  updateEvent: EventEmitter<EmitterModel<ZoneWindowReadModel>> = new EventEmitter();
  @Input('input') zoneWindowReadModel!: ZoneWindowReadModel;

  getDirection = getDirection
  booleanOptions = BooleanOptions
  paneOptions = PaneOptions
  frameMaterialOptions = EmptyOptions
  glazingTypeOptions = EmptyOptions

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
      windowAreaFront: [null,],
      windowAreaBack: [null,],
      windowAreaLeft: [null,],
      windowAreaRight: [null,],
      windowsSame: [null,],
      windows: this.fb.array([]),
    }, { validators: [windowAreaAverageValidator] })
    const windowsSame = zoneWindow.get("windowsSame") as AbstractControl
    const windowAreaFront = zoneWindow.get("windowAreaFront") as AbstractControl
    const windowAreaBack = zoneWindow.get("windowAreaBack") as AbstractControl
    const windowAreaLeft = zoneWindow.get("windowAreaLeft") as AbstractControl
    const windowAreaRight = zoneWindow.get("windowAreaRight") as AbstractControl
    const windows = zoneWindow.get("windows") as FormArray
    if (this.buildingType == 0) {
      setValidations(windowsSame);
      setValidations([windowAreaFront, windowAreaBack, windowAreaLeft, windowAreaRight], [Validators.required, Validators.min(0), Validators.max(999)])
      windows.push(this.windowInputs())
      windowsSame?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: any) => {
          if (val == false) {
            while (windows.length < 4)
              windows.push(this.windowInputs())
          } else {
            this.deleteWindows()
          }
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    } else {
      if (this.windowsAvailable?.length) {
        let ind = 0;
        while (windows.length < this.windowsAvailable?.length) {
          windows.push(this.windowInputs());
          switch (this.windowsAvailable[ind]) {
            case 0:
              setValidations(windowAreaFront, [Validators.required, Validators.min(0), Validators.max(999)])
              break;
            case 1:
              setValidations(windowAreaBack, [Validators.required, Validators.min(0), Validators.max(999)])
              break;
            case 2:
              setValidations(windowAreaRight, [Validators.required, Validators.min(0), Validators.max(999)])
              break;
            case 3:
              setValidations(windowAreaLeft, [Validators.required, Validators.min(0), Validators.max(999)])
              break;
          }
          ind++;
        }
      }
    }
    return zoneWindow;
  }

  windowInputs(): FormGroup {
    var window = this.fb.group({
      id: [null],
      facing: [0, [Validators.required]],
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
        setValidations(uFactor, [Validators.required, Validators.min(0.1), Validators.max(5)])
        setValidations(shgc, [Validators.required, Validators.min(0), Validators.max(0.99)])
        resetValuesAndValidations([panes, frameMaterial, glazingType])
      } else if (val == false) {
        resetValuesAndValidations([uFactor, shgc])
        setValidations([panes, frameMaterial, glazingType])
      } else {
        resetValuesAndValidations([panes, frameMaterial, glazingType, uFactor, shgc])
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
          resetValues([frameMaterialOptions])
      }
      resetValues([frameMaterial, glazingType, glazingTypeOptions])
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
          resetValues([glazingTypeOptions])
      }
      resetValues(glazingType)
    })
    return window;
  }

  getData() {
    if (this.zoneWindowReadModel) {
      while (this.windowsObj.length < this.zoneWindowReadModel.windows.length) {
        this.windowsObj.push(this.windowInputs())
      }
      this.zoneWindowForm.patchValue(this.zoneWindowReadModel)
    }
  }
  updateFacingValues() {
    this.windowsObj.controls.forEach((control, index) => {
      control.get('facing')?.setValue(this.windowsAvailable[index]);
    });
  }
  onSave() {
    if (this.zoneWindowForm.invalid) {
      this.zoneWindowForm.markAllAsTouched();
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }
    this.updateFacingValues();
    this.zoneWindowReadModel = this.zoneWindowForm.value
    this.zoneWindowReadModel = removeNullIdProperties(this.zoneWindowReadModel);
    if (this.zoneWindowForm.value?.id) {
      this.zoneWindowService.update(this.zoneWindowReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<ZoneWindowReadModel>) => {
          if (val.failed === false) {
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
          if (val?.failed === false) {
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
          if (val.failed === false) {
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

import { SystemsService } from '../../shared/services/heating-cooling-system/systems.service';
import { DuctLocationService } from '../../shared/services/heating-cooling-system/duct-location.service';
import { Component, EventEmitter, Input, OnInit, Output, OnDestroy } from '@angular/core';
import { Unsubscriber } from '../../shared/modules/unsubscribe/unsubscribe.component.';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { HeatingCoolingSystemReadModel } from '../../shared/models/heating-cooling-system/heating-cooling-system.model';
import { removeNullIdProperties } from '../../shared/modules/Transformers/TransormerFunction';
import { BooleanOptions, Year1998Options } from '../../shared/lookups/common.lookup';
import { CoolingSystemTypeOptions, DuctLocationCountOptions, DuctLocationOptions, EERCoolingEfficiencyUnitOptions, HeatingEfficiencyUnitOptions, HeatingSystemTypeOptions, SEERCoolingEfficiencyUnitOptions, SystemCountOptions } from '../../shared/lookups/heating-cooling-system.lookup';
import { HeatingCoolingSystemService } from '../../shared/services/heating-cooling-system/heating-cooling-system.service';
import { takeUntil } from 'rxjs';
import { Result } from '../../shared/models/common/result.model';
import { DuctLocationReadModel } from '../../shared/models/heating-cooling-system/duct-location-model';
import { resetValuesAndValidations, setValidations } from '../../shared/modules/Validators/validators.module';
import { SystemsReadModel } from '../../shared/models/heating-cooling-system/systems-model';
import { EmitterModel } from '../../shared/models/common/emitter.model';

@Component({
  selector: 'app-heating-cooling-system',
  standalone: false,
  templateUrl: './heating-cooling-system.component.html',
  styleUrl: './heating-cooling-system.component.scss'
})
export class HeatingCoolingSystemComponent extends Unsubscriber implements OnInit, OnDestroy {
  //variable initializations
  heatingCoolingSystemForm!: FormGroup | any;
  @Input('buildingId') buildingId: string | null | undefined;
  @Input('roofType1') roofType1: string | null | undefined;
  @Input('roofType2') roofType2: string | null | undefined;
  @Input('floorType1') floorType1: string | null | undefined;
  @Input('floorType2') floorType2: string | null | undefined;
  @Input('input') heatingCoolingSystemReadModel!: HeatingCoolingSystemReadModel;
  @Output("move") move: EventEmitter<boolean> = new EventEmitter();

  @Output('update')
  updateEvent: EventEmitter<EmitterModel<HeatingCoolingSystemReadModel>> = new EventEmitter();

  booleanOptions = BooleanOptions
  heatingSystemTypeOptions = HeatingSystemTypeOptions
  coolingSystemTypeOptions = CoolingSystemTypeOptions
  seerCoolingEfficiencyUnitOptions = SEERCoolingEfficiencyUnitOptions
  eerCoolingEfficiencyUnitOptions = EERCoolingEfficiencyUnitOptions
  systemCountOptions = SystemCountOptions
  ductLocationCountOptions = DuctLocationCountOptions
  heatingEfficiencyUnitOptions = HeatingEfficiencyUnitOptions
  year1998Options = Year1998Options
  ductLocationOptions:any

  get heatingCoolingSystemControl() {
    return this.heatingCoolingSystemForm.controls;
  }

  get systemsObj() {
    return this.heatingCoolingSystemControl['systems'] as FormArray
  }

  ductLocationsObj(systemIndex: number): FormArray {
    return this.systemsObj.at(systemIndex).get('ductLocations') as FormArray;
  }

  constructor(
    private heatingCoolingSystemService: HeatingCoolingSystemService,
    private ductLocationService: DuctLocationService,
    private systemsService: SystemsService,
    public fb: FormBuilder,
  ) {
    super()
  }

  ngOnInit(): void {
    this.variableDeclaration();
    this.addDuctLocations();
    this.getData();
  }

  //variable declarations
  variableDeclaration() {
    this.heatingCoolingSystemForm = this.heatingCoolingSystemInputs()
  }

  addDuctLocations() {
    this.ductLocationOptions = [...DuctLocationOptions]
    var foundations = ["Unconditioned Basement",
      "Unvented Crawlspace / Unconditioned Garage",
      "Vented Crawlspace"]
    if (this.roofType1 == "Unconditioned Attic" || this.roofType2 == "Unconditioned Attic") {
      this.ductLocationOptions.push({ id: 4, name: "Unconditioned Attic", value: "Unconditioned Attic" })
    }
    let index = 5;
    foundations.forEach(ele => {
      if (this.floorType1 === ele) {
        this.ductLocationOptions.push({ id: index++, name: ele, value: ele })
      }
      else if (this.floorType2 == ele) {
        this.ductLocationOptions.push({ id: index++, name: ele, value: ele })
      }
    })
  }

  heatingCoolingSystemInputs(): FormGroup {
    var heatingCoolingSystem = this.fb.group({
      id: [null],
      buildingId: [this.buildingId],
      systemCount: [1, [Validators.required]],
      systems: this.fb.array([this.systemsInputs()]),
    })
    const systemCount = heatingCoolingSystem.get('systemCount') as AbstractControl;
    const systems = heatingCoolingSystem.get('systems') as FormArray;
    systemCount?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      if (val) {
        if (val == 2) {
          if (this.systemsObj.length == 1) {
            this.systemsObj.push(this.systemsInputs())
          }
        } else if (val == 1) {
          if (this.systemsObj.length == 2) {
            this.deleteSystems();
          }
        }
      }
      systems.controls.forEach((control: AbstractControl) => {
        const percentAreaServed = control.get('percentAreaServed') as AbstractControl
        if (val > 1) {
          setValidations(percentAreaServed, [Validators.required, Validators.min(0), Validators.max(100)]);
        } else {
          resetValuesAndValidations(percentAreaServed);
        }
      })
    })
    return heatingCoolingSystem;
  }

  systemsInputs(): FormGroup {
    var systems = this.fb.group({
      id: [null],
      percentAreaServed: [null],
      heatingSystemType: [null],
      heatingTracker: [{ efficiencyValue: false, efficiencyOptions: false, efficiencyUnit: false, ducts: false }],
      knowHeatingEfficiency: [null],
      heatingSystemEfficiencyUnit: [null],
      heatingSystemEfficiencyValue: [null],
      heatingSystemYearInstalled: [null],
      coolingSystemType: [null],
      coolingTracker: [{ efficiencyValue: false, efficiencyOptions: false, efficiencyUnit: false, ducts: false }],
      knowCoolingEfficiency: [null],
      coolingSystemEfficiencyUnit: [null],
      coolingSystemEfficiencyValue: [null],
      coolingSystemYearInstalled: [null],
      ductLeakageTestPerformed: [null, [Validators.required]],
      ductLeakageTestValue: [null],
      ductAreProfessionallySealed: [null],
      ductLocationCount: [null, [Validators.required]],
      ductLocations: this.fb.array([]),
    })

    const heatingTracker = systems.get('heatingTracker') as AbstractControl
    const heatingSystemType = systems.get('heatingSystemType') as AbstractControl
    const knowHeatingEfficiency = systems.get('knowHeatingEfficiency') as AbstractControl
    const heatingSystemEfficiencyUnit = systems.get('heatingSystemEfficiencyUnit') as AbstractControl
    const heatingSystemEfficiencyValue = systems.get('heatingSystemEfficiencyValue') as AbstractControl
    const heatingSystemYearInstalled = systems.get('heatingSystemYearInstalled') as AbstractControl
    const coolingSystemType = systems.get('coolingSystemType') as AbstractControl
    const coolingTracker = systems.get('coolingTracker') as AbstractControl
    const knowCoolingEfficiency = systems.get('knowCoolingEfficiency') as AbstractControl
    const coolingSystemEfficiencyUnit = systems.get('coolingSystemEfficiencyUnit') as AbstractControl
    const coolingSystemEfficiencyValue = systems.get('coolingSystemEfficiencyValue') as AbstractControl
    const coolingSystemYearInstalled = systems.get('coolingSystemYearInstalled') as AbstractControl
    const ductLeakageTestPerformed = systems.get('ductLeakageTestPerformed') as AbstractControl
    const ductLeakageTestValue = systems.get('ductLeakageTestValue') as AbstractControl
    const ductAreProfessionallySealed = systems.get('ductAreProfessionallySealed') as AbstractControl
    const ductLocationCount = systems.get('ductLocationCount') as AbstractControl
    const ductLocations = systems.get('ductLocations') as FormArray

    const heatingSystemTypeValidation = (control: AbstractControl): ValidationErrors | null => {
      let heatType = control?.value
      let coolType = coolingSystemType?.value
      coolingSystemType.setErrors(null)
      if (heatType == "Electric baseboard heater" || heatType?.endsWith("boiler") || heatType?.endsWith("furnace")) {
        if (coolType == "Ground coupled heat pump" || coolType == "Electric heat pump") {
          return { "typeMisMatch": `${heatType} not matched to ${coolType}` }
        }
      } else if (heatType?.endsWith("heat pump")) {
        if (!(!coolType || coolType == "None" || coolType == heatType || coolType == "Direct evaporative cooling" || coolType == "Direct evaporative cooling" || coolType == "Minisplit (ductless) heat pump")) {
          return { "typeMisMatch": `${heatType} not matched to ${coolType}` }
        }
      }
      return null;
    }
    const coolingSystemTypeValidation = (control: AbstractControl): ValidationErrors | null => {
      let heatType = heatingSystemType?.value
      let coolType = control?.value
      heatingSystemType.setErrors(null)
      if (coolType == "Central air conditioner") {
        if (heatType?.endsWith("heat pump")) {
          return { "typeMisMatch": `${coolType} not matched to ${heatType}` }
        }
      } else if (coolType == "Ground coupled heat pump" || coolType == "Electric heat pump") {
        if (!(!heatType || heatType == "None" || coolType == heatType || heatType?.endsWith("stove"))) {
          return { "typeMisMatch": `${coolType} not matched to ${heatType}` }
        }
      } else if (coolType == "Minisplit (ductless) heat pump") {
        if (heatType == "Ground coupled heat pump" || heatType == "Electric heat pump") {
          return { "typeMisMatch": `${coolType} not matched to ${heatType}` }
        }
      }
      return null;
    }
    setValidations(coolingSystemType, [Validators.required, coolingSystemTypeValidation])
    setValidations(heatingSystemType, [Validators.required, heatingSystemTypeValidation])

    const ductSystemsValidation = () => {
      if (heatingTracker.value?.ducts || coolingTracker.value?.ducts) {
        setValidations([ductLeakageTestPerformed, ductLocationCount])
      } else {
        resetValuesAndValidations([ductLeakageTestPerformed, ductLocationCount])
      }
    }

    heatingSystemType?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      resetValuesAndValidations([knowHeatingEfficiency, heatingSystemEfficiencyUnit, heatingSystemEfficiencyValue, heatingSystemYearInstalled]);
      switch (val) {
        case "Central gas furnace":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: false, ducts: true })
          setValidations([knowHeatingEfficiency])
          break;
        case "Room (through-the-wall) gas furnace":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: false, ducts: false })
          setValidations([knowHeatingEfficiency])
          break;
        case "Gas boiler":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: false, ducts: false })
          setValidations([knowHeatingEfficiency])
          break;
        case "Propane (LPG) central furnace":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: false, ducts: true })
          setValidations([knowHeatingEfficiency])
          break;
        case "Propane (LPG) wall furnace":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: false, efficiencyUnit: false, ducts: false })
          setValidations([heatingSystemEfficiencyValue], [Validators.required, Validators.min(0.6), Validators.max(1)])
          break;
        case "Propane (LPG) boiler":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: false, ducts: false })
          setValidations([knowHeatingEfficiency])
          break;
        case "Oil furnace":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: false, ducts: true })
          setValidations([knowHeatingEfficiency])
          break;
        case "Oil boiler":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: false, ducts: false })
          setValidations([knowHeatingEfficiency])
          break;
        case "Electric furnace":
          heatingTracker.setValue({ efficiencyValue: false, efficiencyOptions: false, efficiencyUnit: false, ducts: true })
          break;
        case "Electric heat pump":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: true, ducts: true })
          setValidations([knowHeatingEfficiency])
          break;
        case "Ground coupled heat pump":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: false, efficiencyUnit: false, ducts: true })
          setValidations([heatingSystemEfficiencyValue], [Validators.required, Validators.min(2), Validators.max(5)])
          break;
        case "Minisplit (ductless) heat pump":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: false, efficiencyUnit: true, ducts: false })
          setValidations(heatingSystemEfficiencyValue, [Validators.required, Validators.min(6), Validators.max(20)])
          setValidations(heatingSystemEfficiencyUnit)
          break;
        default: // "Electric baseboard heater" "Electric boiler" "Wood stove" "Pellet stove" "None"
          heatingTracker.setValue({ efficiencyValue: false, efficiencyOptions: false, efficiencyUnit: false, ducts: false })
          break;
      }
      ductSystemsValidation()
    })

    knowHeatingEfficiency?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      if (val) {
        setValidations(heatingSystemEfficiencyValue, [Validators.required, Validators.min(0.6), Validators.max(1)])
        if (heatingSystemType.value == "Electric heat pump") {
          setValidations(heatingSystemEfficiencyValue, [Validators.required, Validators.min(6), Validators.max(20)])
        }
        if (heatingTracker.value?.efficiencyUnit) {
          setValidations(heatingSystemEfficiencyUnit)
        }
        resetValuesAndValidations(heatingSystemYearInstalled)
      } else if (val == false) {
        resetValuesAndValidations([heatingSystemEfficiencyValue, heatingSystemEfficiencyUnit])
        setValidations(heatingSystemYearInstalled)
      }
      // if give else wrong validation occur
    })

    coolingSystemType?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      resetValuesAndValidations([knowCoolingEfficiency, coolingSystemEfficiencyUnit, coolingSystemEfficiencyValue, coolingSystemYearInstalled])
      switch (val) {
        case "Central air conditioner":
          coolingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: true, ducts: true })
          setValidations(knowCoolingEfficiency)
          break;
        case "Room air conditioner":
          coolingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: true, ducts: false })
          setValidations(knowCoolingEfficiency)
          break;
        case "Electric heat pump":
          coolingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: true, ducts: true })
          setValidations(knowCoolingEfficiency)
          break;
        case "Minisplit (ductless) heat pump":
          coolingTracker.setValue({ efficiencyValue: true, efficiencyOptions: false, efficiencyUnit: true, ducts: false })
          setValidations([coolingSystemEfficiencyValue], [Validators.required, Validators.min(8), Validators.max(40)])
          setValidations([coolingSystemEfficiencyUnit])
          break;
        case "Ground coupled heat pump":
          coolingTracker.setValue({ efficiencyValue: true, efficiencyOptions: false, efficiencyUnit: false, ducts: true })
          setValidations([coolingSystemEfficiencyValue], [Validators.required, Validators.min(8), Validators.max(40)])
          break;
        default: //"None" "Direct evaporative cooling"
          coolingTracker.setValue({ efficiencyValue: false, efficiencyOptions: false, efficiencyUnit: false, ducts: false })
          break;
      }
      ductSystemsValidation()
    })

    knowCoolingEfficiency?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      if (val) {
        setValidations(coolingSystemEfficiencyValue, [Validators.required, Validators.min(8), Validators.max(40)])
        if (coolingTracker.value?.efficiencyUnit) {
          setValidations(coolingSystemEfficiencyUnit)
        }
        resetValuesAndValidations(coolingSystemYearInstalled)
      } else if (val == false) {
        setValidations(coolingSystemYearInstalled)
        resetValuesAndValidations([coolingSystemEfficiencyValue, coolingSystemEfficiencyUnit])
      }
      // if give else wrong validation occur
    })

    ductLocationCount?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe({
      next: (val: any) => {
        if (val) {
          if (val > ductLocations.length) {
            while (val > ductLocations.length) {
              ductLocations.push(this.ductLocationInputs());
            }
          } else {
            this.deleteDuctLocation(val, ductLocations);
          }
        }
        ductLocations.controls.forEach((control: AbstractControl) => {
          var percent = control.get('percentageOfDucts') as AbstractControl;
          if (val > 1) {
            setValidations(percent, [Validators.required, Validators.min(0), Validators.max(100)]);
          }
          else if (val) {
            resetValuesAndValidations(percent);
          }
        })
      }
    })

    ductLeakageTestPerformed?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      if (val) {
        setValidations(ductLeakageTestValue, [Validators.required, Validators.min(0), Validators.max(1000)])
        resetValuesAndValidations(ductAreProfessionallySealed)
      } else if (val == false) {
        setValidations(ductAreProfessionallySealed)
        resetValuesAndValidations(ductLeakageTestValue)
      }
    })
    return systems;
  }

  ductLocationInputs(): FormGroup {
    var ducts = this.fb.group({
      id: [null],
      location: [null, [Validators.required]],
      percentageOfDucts: [null],
      ductsIsInsulated: [null, [Validators.required]],
    })
    return ducts;
  }

  getData() {
    if (this.heatingCoolingSystemReadModel) {
      if (this.heatingCoolingSystemReadModel?.systems.length) {
        this.systemsObj.clear()
        var a = 0;
        while (this.heatingCoolingSystemReadModel?.systemCount > this.systemsObj.length) {
          var system: FormGroup = this.systemsInputs()
          var arr = system.get('ductLocations') as FormArray;
          while (this.heatingCoolingSystemReadModel?.systems[a]?.ductLocations?.length > arr.length) {
            arr.push(this.ductLocationInputs());
          }
          this.systemsObj.push(system);
          a++;
        }
      } else {
        this.systemsObj.push(this.systemsInputs())
      }
      this.heatingCoolingSystemForm.patchValue(this.heatingCoolingSystemReadModel)
    }
  }

  onSave() {
    console.log(this.heatingCoolingSystemForm);
    if (this.heatingCoolingSystemForm.invalid) {
      this.heatingCoolingSystemForm.markAllAsTouched();
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }
    this.heatingCoolingSystemReadModel = this.heatingCoolingSystemForm.value
    this.heatingCoolingSystemReadModel = removeNullIdProperties(this.heatingCoolingSystemReadModel);
    if (this.heatingCoolingSystemForm.value?.id) {
      this.heatingCoolingSystemService.update(this.heatingCoolingSystemReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<HeatingCoolingSystemReadModel>) => {
          if (val?.failed === false) {
            this.heatingCoolingSystemForm.patchValue(val.data)
            this.updateEvent.emit({
              fieldType: "heating-cooling-system",
              field: val.data
            })
          }
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    } else {
      this.heatingCoolingSystemService.create(this.heatingCoolingSystemReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<HeatingCoolingSystemReadModel>) => {
          if (val?.failed === false) {
            this.heatingCoolingSystemForm.patchValue(val.data)
            this.updateEvent.emit({
              fieldType: "heating-cooling-system",
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

  deleteSystems() {
    var id = this.systemsObj.at(1).get('id')?.value;
    if (id) {
      this.systemsService.delete(id).pipe(takeUntil(this.destroy$)).subscribe({
        next: (res: Result<SystemsReadModel>) => {
          if (res.failed === false) {
            this.systemsObj.removeAt(1);
          }
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    } else {
      this.systemsObj.removeAt(1);
    }
  }

  deleteDuctLocation(val: number, arr: FormArray) {
    var ids: string[] = []
    var int = val
    while (int < arr.length) {
      var id = arr.at(int++).get('id')?.value
      if (id) ids.push(id);
    }
    if (ids.length) {
      this.ductLocationService?.bulkDelete(ids).pipe(takeUntil(this.destroy$)).subscribe({
        next: (res: Result<DuctLocationReadModel>) => {
          if (res.failed === false) {
            while (val < arr.length) {
              arr.removeAt(arr.length - 1);
            }
          }
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    } else {
      while (val < arr.length) {
        arr.removeAt(arr.length - 1);
      }
    }
  }

}


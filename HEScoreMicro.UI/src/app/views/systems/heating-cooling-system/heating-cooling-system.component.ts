import { SystemsService } from './../../../shared/services/heating-cooling-system/systems.service';
import { DuctLocationService } from './../../../shared/services/heating-cooling-system/duct-location.service';
import { Component, OnInit } from '@angular/core';
import { Unsubscriber } from '../../../shared/modules/unsubscribe/unsubscribe.component.';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { HeatingCoolingSystemReadModel } from '../../../shared/models/heating-cooling-system/heating-cooling-system.model';
import { removeNullIdProperties } from '../../../shared/modules/Transformers/TransormerFunction';
import { BooleanOptions, Year1970Options, Year1998Options } from '../../../shared/lookups/common.lookup';
import { CoolingSystemTypeOptions, DuctLocationCountOptions, DuctLocationOptions, EERCoolingEfficiencyUnitOptions, HeatingEfficiencyUnitOptions, HeatingSystemTypeOptions, SEERCoolingEfficiencyUnitOptions, SystemCountOptions } from '../../../shared/lookups/heating-cooling-system.lookup';
import { CommonService } from '../../../shared/services/common/common.service';
import { HeatingCoolingSystemService } from '../../../shared/services/heating-cooling-system/heating-cooling-system.service';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntil } from 'rxjs';
import { Result } from '../../../shared/models/common/result.model';
import { DuctLocationReadModel } from '../../../shared/models/heating-cooling-system/duct-location-model';
import { SystemsModule } from '../systems.module';
import { resetValuesAndValidations, setValidations } from '../../../shared/modules/Validators/validators.module';

@Component({
  selector: 'app-heating-cooling-system',
  standalone: false,
  templateUrl: './heating-cooling-system.component.html',
  styleUrl: './heating-cooling-system.component.scss'
})
export class HeatingCoolingSystemComponent extends Unsubscriber implements OnInit {
  //variable initializations
  heatingCoolingSystemForm!: FormGroup | any;
  buildingId: string | null | undefined;
  heatingCoolingSystemReadModel!: HeatingCoolingSystemReadModel;
  removeNullIdProperties = removeNullIdProperties
  setValidations = setValidations
  resetValuesAndValidations = resetValuesAndValidations
  booleanOptions = BooleanOptions
  heatingSystemTypeOptions = HeatingSystemTypeOptions
  coolingSystemTypeOptions = CoolingSystemTypeOptions
  seerCoolingEfficiencyUnitOptions = SEERCoolingEfficiencyUnitOptions
  eerCoolingEfficiencyUnitOptions = EERCoolingEfficiencyUnitOptions
  ductLocationOptions = DuctLocationOptions
  systemCountOptions = SystemCountOptions
  ductLocationCountOptions = DuctLocationCountOptions
  heatingEfficiencyUnitOptions = HeatingEfficiencyUnitOptions
  year1998Options = Year1998Options

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
    protected commonService: CommonService,
    private heatingCoolingSystemService: HeatingCoolingSystemService,
    private ductLocationService: DuctLocationService,
    private systemsService: SystemsService,
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
    this.heatingCoolingSystemForm = this.heatingCoolingSystemInputs()
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
          this.setValidations(percentAreaServed, [Validators.required, Validators.min(0), Validators.max(100)]);
        } else {
          this.resetValuesAndValidations(percentAreaServed);
        }
      })
    })
    return heatingCoolingSystem;
  }

  systemsInputs(): FormGroup {
    var systems = this.fb.group({
      id: [null],
      buildingId: [this.buildingId],
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
    this.setValidations(coolingSystemType, [Validators.required, coolingSystemTypeValidation])
    this.setValidations(heatingSystemType, [Validators.required, heatingSystemTypeValidation])

    const ductSystemsValidation = (flag: boolean) => {
      if (flag) {
        this.setValidations([ductLeakageTestPerformed, ductLocationCount])
      } else {
        this.resetValuesAndValidations([ductLeakageTestPerformed, ductLocationCount])
      }
    }

    heatingSystemType?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      switch (val) {
        case "Central gas furnace":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: false, ducts: true })
          this.setValidations([knowHeatingEfficiency])
          this.resetValuesAndValidations([heatingSystemEfficiencyUnit, heatingSystemEfficiencyValue, heatingSystemYearInstalled]);
          break;
        case "Room (through-the-wall) gas furnace":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: false, ducts: false })
          this.setValidations([knowHeatingEfficiency])
          this.resetValuesAndValidations([heatingSystemEfficiencyUnit, heatingSystemEfficiencyValue, heatingSystemYearInstalled]);
          break;
        case "Gas boiler":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: false, ducts: false })
          this.setValidations([knowHeatingEfficiency])
          this.resetValuesAndValidations([heatingSystemEfficiencyUnit, heatingSystemEfficiencyValue, heatingSystemYearInstalled]);
          break;
        case "Propane (LPG) central furnace":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: false, ducts: true })
          this.setValidations([knowHeatingEfficiency])
          this.resetValuesAndValidations([heatingSystemEfficiencyUnit, heatingSystemEfficiencyValue, heatingSystemYearInstalled]);
          break;
        case "Propane (LPG) wall furnace":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: false, efficiencyUnit: false, ducts: false })
          this.setValidations([heatingSystemEfficiencyValue], [Validators.required, Validators.min(0.6), Validators.max(1)])
          this.resetValuesAndValidations([heatingSystemEfficiencyUnit, knowHeatingEfficiency, heatingSystemYearInstalled]);
          break;
        case "Propane (LPG) boiler":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: false, ducts: false })
          this.setValidations([knowHeatingEfficiency])
          this.resetValuesAndValidations([heatingSystemEfficiencyUnit, heatingSystemEfficiencyValue, heatingSystemYearInstalled]);
          break;
        case "Oil furnace":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: false, ducts: true })
          this.setValidations([knowHeatingEfficiency])
          this.resetValuesAndValidations([heatingSystemEfficiencyUnit, heatingSystemEfficiencyValue, heatingSystemYearInstalled]);
          break;
        case "Oil boiler":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: false, ducts: false })
          this.setValidations([knowHeatingEfficiency])
          this.resetValuesAndValidations([heatingSystemEfficiencyUnit, heatingSystemEfficiencyValue, heatingSystemYearInstalled]);
          break;
        case "Electric furnace":
          heatingTracker.setValue({ efficiencyValue: false, efficiencyOptions: false, efficiencyUnit: false, ducts: true })
          this.resetValuesAndValidations([knowHeatingEfficiency, heatingSystemEfficiencyUnit, heatingSystemEfficiencyValue, heatingSystemYearInstalled]);
          break;
        case "Electric heat pump":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: true, ducts: true })
          this.setValidations([knowHeatingEfficiency], [Validators.required, Validators.min(6), Validators.max(20)])
          this.resetValuesAndValidations([heatingSystemEfficiencyUnit, heatingSystemEfficiencyValue, heatingSystemYearInstalled]);
          break;
        case "Ground coupled heat pump":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: false, efficiencyUnit: false, ducts: true })
          this.setValidations([heatingSystemEfficiencyValue], [Validators.required, Validators.min(2), Validators.max(5)])
          this.resetValuesAndValidations([heatingSystemEfficiencyUnit, knowHeatingEfficiency, heatingSystemYearInstalled]);
          break;
        case "Minisplit (ductless) heat pump":
          heatingTracker.setValue({ efficiencyValue: true, efficiencyOptions: false, efficiencyUnit: true, ducts: false })
          this.setValidations(heatingSystemEfficiencyValue, [Validators.required, Validators.min(6), Validators.max(20)])
          this.setValidations(heatingSystemEfficiencyUnit)
          this.resetValuesAndValidations([knowHeatingEfficiency, heatingSystemYearInstalled]);
          break;
        default: // "Electric baseboard heater" "Electric boiler" "Wood stove" "Pellet stove" "None"
          heatingTracker.setValue({ efficiencyValue: false, efficiencyOptions: false, efficiencyUnit: false, ducts: false })
          this.resetValuesAndValidations([knowHeatingEfficiency, heatingSystemEfficiencyUnit, heatingSystemEfficiencyValue, heatingSystemYearInstalled]);
          break;
      }
      ductSystemsValidation(heatingTracker?.value?.ducts)
    })

    knowHeatingEfficiency?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      if (val) {
        this.setValidations(heatingSystemEfficiencyValue, [Validators.required, Validators.min(0.6), Validators.max(1)])
        if (heatingTracker.value?.efficiencyUnit) {
          this.setValidations(heatingSystemEfficiencyUnit)
        }
        this.resetValuesAndValidations(heatingSystemYearInstalled)
      } else if (val == false) {
        this.resetValuesAndValidations([heatingSystemEfficiencyValue, heatingSystemEfficiencyUnit])
        this.setValidations(heatingSystemYearInstalled)
      }
      // if give else wrong validation occur
    })

    coolingSystemType?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      switch (val) {
        case "Central air conditioner":
          coolingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: true, ducts: true })
          this.setValidations(knowCoolingEfficiency)
          this.resetValuesAndValidations([coolingSystemEfficiencyUnit, coolingSystemEfficiencyValue, coolingSystemYearInstalled])
          break;
        case "Room air conditioner":
          coolingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: true, ducts: false })
          this.setValidations(knowCoolingEfficiency)
          this.resetValuesAndValidations([coolingSystemEfficiencyUnit, coolingSystemEfficiencyValue, coolingSystemYearInstalled])
          break;
        case "Electric heat pump":
          coolingTracker.setValue({ efficiencyValue: true, efficiencyOptions: true, efficiencyUnit: true, ducts: true })
          this.setValidations(knowCoolingEfficiency)
          this.resetValuesAndValidations([coolingSystemEfficiencyUnit, coolingSystemEfficiencyValue, coolingSystemYearInstalled])
          break;
        case "Minisplit (ductless) heat pump":
          coolingTracker.setValue({ efficiencyValue: true, efficiencyOptions: false, efficiencyUnit: true, ducts: false })
          this.setValidations([coolingSystemEfficiencyUnit, coolingSystemEfficiencyValue])
          this.resetValuesAndValidations([knowCoolingEfficiency, coolingSystemYearInstalled])
          break;
        case "Ground coupled heat pump":
          coolingTracker.setValue({ efficiencyValue: true, efficiencyOptions: false, efficiencyUnit: false, ducts: true })
          this.setValidations([coolingSystemEfficiencyValue], [Validators.required, Validators.min(8), Validators.max(40)])
          this.resetValuesAndValidations([knowCoolingEfficiency, coolingSystemEfficiencyUnit, coolingSystemYearInstalled])
          break;
        default: //"None" "Direct evaporative cooling"
          coolingTracker.setValue({ efficiencyValue: false, efficiencyOptions: false, efficiencyUnit: false, ducts: false })
          this.resetValuesAndValidations([knowCoolingEfficiency, coolingSystemEfficiencyUnit, coolingSystemEfficiencyValue, coolingSystemYearInstalled]);
          break;
      }
      ductSystemsValidation(coolingTracker?.value?.ducts)
    })

    knowCoolingEfficiency?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      if (val) {
        this.setValidations(coolingSystemEfficiencyValue, [Validators.required, Validators.min(8), Validators.max(40)])
        if (coolingTracker.value?.efficiencyUnit) {
          this.setValidations(coolingSystemEfficiencyUnit)
        }
        this.resetValuesAndValidations(coolingSystemYearInstalled)
      } else if (val == false) {
        this.setValidations(coolingSystemYearInstalled)
        this.resetValuesAndValidations([coolingSystemEfficiencyValue, coolingSystemEfficiencyUnit])
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
            this.setValidations(percent, [Validators.required, Validators.min(0), Validators.max(100)]);
          }
          else if (val) {
            this.resetValuesAndValidations(percent);
          }
        })
      }
    })

    ductLeakageTestPerformed?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      if (val) {
        this.setValidations(ductLeakageTestValue, [Validators.required, Validators.min(0), Validators.max(1000)])
        this.resetValuesAndValidations(ductAreProfessionallySealed)
      } else if (val == false) {
        this.setValidations(ductAreProfessionallySealed)
        this.resetValuesAndValidations(ductLeakageTestValue)
      } else {
        this.resetValuesAndValidations([ductLeakageTestValue, ductAreProfessionallySealed])
      }
    })
    return systems;
  }

  ductLocationInputs(): FormGroup {
    var ducts = this.fb.group({
      id: [null],
      buildingId: [this.buildingId],
      location: [null, [Validators.required]],
      percentageOfDucts: [null],
      ductsIsInsulated: [null, [Validators.required]],
    })
    return ducts;
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
      this.heatingCoolingSystemService.getByBuildingId(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<HeatingCoolingSystemReadModel>) => {
          if (val?.failed == false) {
            if (val?.data?.systems.length) {
              this.systemsObj.clear()
              var a = 0;
              while (val?.data?.systemCount > this.systemsObj.length) {
                var system: FormGroup = this.systemsInputs()
                var arr = system.get('ductLocations') as FormArray;
                while (val?.data?.systems[a]?.ductLocations?.length > arr.length) {
                  arr.push(this.ductLocationInputs());
                }
                this.systemsObj.push(system);
                a++;
              }
            } else {
              this.systemsObj.push(this.systemsInputs())
            }
            this.heatingCoolingSystemForm.patchValue(val.data)
          }
        },
        error: (err: any) => {
          console.log(err);
        }
      })
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
          this.heatingCoolingSystemForm.patchValue(val.data)
          console.log(val);
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    } else {
      this.heatingCoolingSystemService.create(this.heatingCoolingSystemReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<HeatingCoolingSystemReadModel>) => {
          if (val?.failed == false) {
            this.heatingCoolingSystemForm.patchValue(val.data)
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

  deleteSystems() {
    var id = this.systemsObj.at(1).get('id')?.value;
    if (id) {
      this.systemsService.delete(id).pipe(takeUntil(this.destroy$)).subscribe({
        next: (res: Result<SystemsModule>) => {
          if (res.failed == false) {
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
          if (res.failed == false) {
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

  goNext() {
    this.router.navigate(['systems/water-heater'], {
      queryParams: { id: this.buildingId }
    })
  }
}

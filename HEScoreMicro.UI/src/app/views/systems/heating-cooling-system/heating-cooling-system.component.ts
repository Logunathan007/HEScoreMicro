import { SystemsService } from './../../../shared/services/heating-cooling-system/systems.service';
import { DuctLocationService } from './../../../shared/services/heating-cooling-system/duct-location.service';
import { Component, OnInit } from '@angular/core';
import { Unsubscriber } from '../../../shared/modules/unsubscribe/unsubscribe.component.';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { HeatingCoolingSystemReadModel } from '../../../shared/models/heating-cooling-system/heating-cooling-system.model';
import { removeNullIdProperties } from '../../../shared/modules/Transformers/TransormerFunction';
import { BooleanOptions } from '../../../shared/lookups/common.lookup';
import { CoolingEfficiencyUnitOptions, CoolingSystemTypeOptions, DuctLocationCountOptions, DuctLocationOptions, HeatingSystemTypeOptions, SystemCountOptions } from '../../../shared/lookups/heating-cooling-system.lookup';
import { CommonService } from '../../../shared/services/common/common.service';
import { HeatingCoolingSystemService } from '../../../shared/services/heating-cooling-system/heating-cooling-system.service';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntil } from 'rxjs';
import { Result } from '../../../shared/models/common/Result';
import { DuctLocationReadModel } from '../../../shared/models/heating-cooling-system/duct-location-model';
import { ICurdService } from '../../../shared/services/common/curd.service';
import { SystemsModule } from '../systems.module';

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
  booleanOptions = BooleanOptions
  heatingSystemTypeOptions = HeatingSystemTypeOptions
  coolingSystemTypeOptions = CoolingSystemTypeOptions
  coolingEfficiencyUnitOptions = CoolingEfficiencyUnitOptions
  ductLocationOptions = DuctLocationOptions
  systemCountOptions = SystemCountOptions
  ductLocationCountOptions = DuctLocationCountOptions

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
      systemCount: [null],
      systems: this.fb.array([this.systemsInputs()]),
    })
    heatingCoolingSystem.get('systemCount')?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe({
      next: (val: any) => {
        if (val) {
          if (val == 2) {
            if (this.systemsObj.length == 1) {
              this.systemsObj.push(this.systemsInputs())
            }
          }else if(val == 1){
            if(this.systemsObj.length == 2){
              this.deleteSystems();
            }
          }
        }
      },
      error: (err: any) => {
        console.log(err);
      }
    })
    return heatingCoolingSystem;
  }

  systemsInputs(): FormGroup {
    var systems = this.fb.group({
      id: [null],
      buildingId: [this.buildingId],
      percentAreaServed: [null],
      heatingSystemType: [null],
      knowHeatingEfficiency: [null],
      heatingSystemEfficiencyValue: [null],
      heatingSystemYearInstalled: [null],
      coolingSystemType: [null],
      knowCoolingEfficiency: [null],
      coolingSystemEfficiencyUnit: [null],
      coolingSystemEfficiencyValue: [null],
      coolingSystemYearInstalled: [null],
      ductLeakageTestPerformed: [null],
      ductAreProfessionallySealed: [null],
      ductLocationCount: [null],
      ductLocations: this.fb.array([this.ductLocationInputs()]),
    })
    systems.get('ductLocationCount')?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe({
      next: (val: any) => {
        var ductLocations = systems.get('ductLocations') as FormArray;
        if (val) {
          if (val > ductLocations.length) {
            while (val > ductLocations.length) {
              ductLocations.push(this.ductLocationInputs());
            }
          } else {
            this.deleteDuctLocation(val, ductLocations);
          }
        }
      }
    })
    return systems;
  }

  ductLocationInputs(): FormGroup {
    var ducts = this.fb.group({
      id: [null],
      buildingId: [this.buildingId],
      location: [null],
      ductsIsInsulated: [null],
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
    this.router.navigate(['systems/hvac-heat-cool'], {
      queryParams: { id: this.buildingId }
    })
  }
}

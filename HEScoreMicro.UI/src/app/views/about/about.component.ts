import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { AboutService } from '../../shared/services/about/about.service';
import { AboutReadModel } from '../../shared/models/about/about.read.model';
import { Unsubscriber } from '../../shared/modules/unsubscribe/unsubscribe.component.';
import { Result } from '../../shared/models/common/result.model';
import { takeUntil } from 'rxjs';
import { BooleanOptions, OrientationOptions } from '../../shared/lookups/common.lookup';
import { ManufacturedHomeTypeOptions } from '../../shared/lookups/about.lookup';
import { resetValuesAndValidations, setValidations } from '../../shared/modules/Validators/validators.module';
import { ActivatedRoute } from '@angular/router';
import { EmitterModel } from '../../shared/models/common/emitter.model';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrl: './about.component.scss',
  standalone: false
})
export class AboutComponent extends Unsubscriber implements OnInit {
  //variable initializations
  @Input('buildingId') buildingId: string | null | undefined;
  @Input('input') aboutReadModel!: AboutReadModel | undefined;
  @Input('buildingType') buildingType!: number | null;

  @Output("move") move: EventEmitter<boolean> = new EventEmitter();
  @Output('update')
  updateEvent: EventEmitter<EmitterModel<AboutReadModel>> = new EventEmitter();

  aboutForm!: FormGroup | any;
  booleanOptions = BooleanOptions
  orientationOptions = OrientationOptions
  manufacturedHomeTypeOptions = ManufacturedHomeTypeOptions

  @Output() myEvent = new EventEmitter<any>();

  get aboutControl() {
    return this.aboutForm.controls;
  }

  constructor(
    public fb: FormBuilder,
    private aboutService: AboutService,
  ) {
    super()
  }

  ngOnInit(): void {
    this.variableDeclaration();
    this.getData();
  }

  //variable declarations
  variableDeclaration() {
    this.aboutForm = this.aboutInputs();
  }


  aboutInputs(): FormGroup {
    var about = this.fb.group({
      id: [null],
      assessmentDate: [null, [Validators.required]],
      yearBuilt: [null, [Validators.required, Validators.min(1600), Validators.max(new Date().getFullYear())]],
      numberOfBedrooms: [null, [Validators.required, Validators.min(1), Validators.max(10)]],
      storiesAboveGroundLevel: [null, [Validators.required, Validators.min(1), Validators.max(4)]],
      interiorFloorToCeilingHeight: [null, [Validators.required, Validators.min(6), Validators.max(12)]],
      totalConditionedFloorArea: [null,],
      directionFacedByFrontOfHome: [null, [Validators.required]],
      blowerDoorTestConducted: [null, [Validators.required]],
      airLeakageRate: [null,],// 0-25000
      airSealed: [null, [Validators.required]],
      numberofUnitsInBuilding: [null], //min:2
      manufacturedHomeType: [null,],
      comments: [null,],
      buildingId: [this.buildingId],
    })
    const blowerDoorTestConducted = about.get('blowerDoorTestConducted') as AbstractControl
    const airLeakageRate = about.get('airLeakageRate') as AbstractControl
    const storiesAboveGroundLevel = about.get('storiesAboveGroundLevel') as AbstractControl
    const totalConditionedFloorArea = about.get('totalConditionedFloorArea') as AbstractControl

    const footPrintValidator = (control: AbstractControl): ValidationErrors | null => {
      let val = control.value / (storiesAboveGroundLevel.value ?? 1)
      if (val < 250) {
        return { min: { min: 250 * (storiesAboveGroundLevel.value ?? 1) } }
      } else if (val > 25000) {
        return { max: { max: 25000 * (storiesAboveGroundLevel.value ?? 1) } }
      }
      return null;
    }
    setValidations(totalConditionedFloorArea, [Validators.required, footPrintValidator])

    storiesAboveGroundLevel?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      totalConditionedFloorArea.updateValueAndValidity();
    });

    blowerDoorTestConducted?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      if (val) {
        setValidations(airLeakageRate, [Validators.required, Validators.min(0), Validators.max(25000)])
      } else {
        resetValuesAndValidations(airLeakageRate)
      }
    })
    return about;
  }

  patchToForm(val: AboutReadModel | undefined) {
    if (val) {
      if (val?.assessmentDate)
        val.assessmentDate = new Date(val?.assessmentDate);
      this.aboutForm.patchValue(val)
    }
  }

  getData() {
    if (this.aboutReadModel) {
      this.patchToForm(this.aboutReadModel)
    }
  }

  onSave() {
    if (this.aboutForm.invalid) {
      this.aboutForm.markAllAsTouched();
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }
    this.aboutReadModel = this.aboutForm.value
    if (this.aboutReadModel) {
      if (this.aboutForm.value?.id) {
        this.aboutService.update(this.aboutReadModel).pipe(takeUntil(this.destroy$)).subscribe({
          next: (val: Result<AboutReadModel>) => {
            this.patchToForm(val?.data);
            this.updateEvent.emit({
              fieldType: "about",
              field: val.data
            })
          },
          error: (err: any) => {
            console.log(err);
          }
        })
      } else {
        delete this.aboutReadModel.id;
        this.aboutService.create(this.aboutReadModel).pipe(takeUntil(this.destroy$)).subscribe({
          next: (val: Result<AboutReadModel>) => {
            if (val?.failed === false) {
              this.patchToForm(val.data)
              this.updateEvent.emit({
                fieldType: "about",
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
  }
}


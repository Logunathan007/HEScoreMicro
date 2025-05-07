import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject, takeUntil } from 'rxjs';
import { InsulationQualityOptions, InsulationSubTypeOptions, InsulationTypeOptions } from '../../shared/lookups/insulation.lookup';
import { Unsubscriber } from '../../shared/modules/unsubscribe/unsubscribe.component.';
import { resetValuesAndValidations, setValidations } from '../../shared/modules/Validators/validators.module';

@Component({
  selector: 'app-common-insulation-model',
  standalone: false,
  templateUrl: './common-insulation-model.component.html',
  styleUrl: './common-insulation-model.component.scss'
})
export class CommonInsulationModelComponent extends Unsubscriber implements OnInit {
  title?: string;
  type?: string;
  arrayIndex?: number;
  onClose: Subject<any> = new Subject();
  insulationForm!: FormGroup;
  insulationTypeOptions = InsulationTypeOptions
  insulationSubTypeOptions = InsulationSubTypeOptions
  insulationQualityOptions = InsulationQualityOptions
  math = Math;

  constructor(public bsModalRef: BsModalRef, public fb: FormBuilder) { super(); }

  get insulationControl() {
    return this.insulationForm.controls;
  }

  get avgWeightedRValuesObj() {
    return this.insulationControl['avgWeightedRValues'] as FormArray
  }

  get calculatedRValueObj() {
    return this.insulationControl['calculatedRValue'] as FormGroup
  }

  ngOnInit() {
    this.variableDeclaration();
  }

  variableDeclaration() {
    this.insulationForm = this.insulationInputs();
  }

  insulationInputs(): FormGroup {

    let insulation = this.fb.group({
      calculatedRValue: this.fb.group({
        insulationType: [null, [Validators.required]],
        insulationSubType: [null],
        insulationQuality: [null],
        insulationDepth: [null, [Validators.required]],
      }),
      avgWeightedRValues: this.fb.array([
        this.weightedRValueInputs(),
        this.weightedRValueInputs(),
        this.weightedRValueInputs(),
      ])
    });
    const calculatedRValue = insulation.get('calculatedRValue') as FormGroup;
    const insulationType = calculatedRValue.get('insulationType') as FormGroup;
    const insulationSubType = calculatedRValue.get('insulationSubType') as FormGroup;
    const insulationQuality = calculatedRValue.get('insulationQuality') as FormGroup;
    const insulationDepth = calculatedRValue.get('insulationDepth') as FormGroup;

    insulationType.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      resetValuesAndValidations([insulationSubType, insulationQuality]);
      if (val === 3.4 || val === 3.1) {
        setValidations([insulationSubType]);
      }
    });

    insulationSubType.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((val: any) => {
      resetValuesAndValidations([insulationQuality]);
      if (val === 'Batt') {
        setValidations([insulationQuality]);
      }
    });
    return insulation;
  }

  weightedRValueInputs(): FormGroup {
    return this.fb.group({
      columnA: [null],
      columnB: [null],
    });
  }

  emitValueAndClose() {
    const result = {
      status: 'confirmed',
      type: this.type,
      index: this.arrayIndex,
    };
    this.onClose.next(result);
    this.onClose.complete();
    this.bsModalRef.hide();
  }
}



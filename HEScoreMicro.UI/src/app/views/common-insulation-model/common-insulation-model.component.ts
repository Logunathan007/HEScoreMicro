import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { combineLatest, startWith, Subject, takeUntil } from 'rxjs';
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
  // Input for this model
  title?: string;
  type?: string;
  arrayIndex?: number;
  rValues: number[] = [];
  previousValue: any = null;

  //Output for this model
  onClose: Subject<any> = new Subject();

  insulationForm!: FormGroup;
  insulationTypeOptions = InsulationTypeOptions
  insulationSubTypeOptions = InsulationSubTypeOptions
  insulationQualityOptions = InsulationQualityOptions
  cavityUFactor: number = 0;
  WeightedAvgRValue: number = 0;
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
    if (this.previousValue) {
      this.insulationForm.patchValue(this.previousValue);
    }
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

  findNearestValuePreferHigher(x: number): number {
    debugger
    return this.rValues?.reduce((nearest, current) => {
      const diff = Math.abs(current - x);
      const nearestDiff = Math.abs(nearest - x);
      if (diff < nearestDiff) return current;
      if (diff === nearestDiff && current > nearest) return nearest; // prefer lower
      return nearest;
    });
  }

  weightedRValueInputs(): FormGroup {
    const weightedRValue = this.fb.group({
      columnA: [null],
      columnB: [null],
      columnC: [0],
    });
    const columnA = weightedRValue.get('columnA') as AbstractControl;
    const columnB = weightedRValue.get('columnB') as AbstractControl;
    const columnC = weightedRValue.get('columnC') as AbstractControl;
    columnC?.disable();

    combineLatest([
      columnA!.valueChanges.pipe(takeUntil(this.destroy$)),
      columnB!.valueChanges.pipe(takeUntil(this.destroy$))
    ]).subscribe(([a, b]) => {
      //Calculate the value of column C based on the values of column A and B
      let ua = (a && b) ? b / a : 0;
      columnC!.setValue(ua);
      // Calculate the value of U-Factor based on the values of column C and B
      let sums: number[] = this.avgWeightedRValuesObj.controls.reduce(
        (accu: number[], control: AbstractControl) => {
          const colB = control.get('columnB')?.value || 0;
          const colC = control.get('columnC')?.value || 0;
          accu[0] += colB;
          accu[1] += colC;
          return accu;
        }, [0, 0] // column-B and column-C
      );
      this.cavityUFactor = (sums[0] && sums[1]) ? sums[1] / sums[0] : 0;
      this.cavityUFactor = parseFloat(this.cavityUFactor.toFixed(3))
      if (this.cavityUFactor)
        this.WeightedAvgRValue = this.findNearestValuePreferHigher(1 / this.cavityUFactor);
      else
        this.WeightedAvgRValue = 0;
    });
    return weightedRValue;
  }

  emitValueAndClose(value: number) {
    const result = {
      rValue: this.findNearestValuePreferHigher(value),
      type: this.type,
      index: this.arrayIndex,
      previousValue: this.insulationForm.value
    };
    this.onClose.next(result);
    this.onClose.complete();
    this.bsModalRef.hide();
  }

  close() {
    const result = {
      type: this.type,
      index: this.arrayIndex,
      previousValue: this.insulationForm.value
    };
    this.onClose.next(result);
    this.onClose.complete();
    this.bsModalRef.hide();
  }
}



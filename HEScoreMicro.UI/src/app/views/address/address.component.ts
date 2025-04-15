import { AssessmentType, DwellingUnitTypeOptions } from '../../shared/lookups/address.lookup';
import { AddressReadModel } from './../../shared/models/address/address.read.model';
import { AddressService } from './../../shared/services/address/address.service';
import { CommonService } from '../../shared/services/common/common.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { Unsubscriber } from '../../shared/modules/unsubscribe/unsubscribe.component.';
import { takeUntil } from 'rxjs';
import { Result } from '../../shared/models/common/result.model';
@Component({
  selector: 'app-address',
  templateUrl: './address.component.html',
  styleUrl: './address.component.scss',
  standalone: false
})
export class AddressComponent extends Unsubscriber implements OnInit {
  //variable initializations
  addressForm!: FormGroup | any;
  buildingId: string | null | undefined;
  addressReadModel!: AddressReadModel;
  dwellingUnitTypeOptions = DwellingUnitTypeOptions
  assessmentType = AssessmentType

  get addressControl() {
    return this.addressForm.controls;
  }

  constructor(
    protected commonService: CommonService,
    private addressService: AddressService,
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
    this.addressForm = this.fb.group({
      id: [null,],
      dwellingUnitType: [null, Validators.required],
      streetAddress: [null, Validators.required],
      addressLine: [null,],
      city: [null, Validators.required],
      state: [null, Validators.required],
      zipCode: [null, Validators.required],
      assessmentType: [null, Validators.required],
      buildingId: [null],
    })
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
      this.addressService.getByBuildingId(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<AddressReadModel>) => {
          if (val?.failed == false)
            this.addressForm.patchValue(val.data)
          console.log(val);
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    }
  }

  onSave() {
    if (this.addressForm.invalid) {
      this.addressForm.markAllAsTouched();
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }
    if (this.addressForm.value?.id) {
      this.addressReadModel = this.addressForm.value
      this.addressService.update(this.addressReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<AddressReadModel>) => {
          this.addressForm.patchValue(val.data)
          console.log(val);
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    } else {
      this.addressReadModel = this.addressForm.value
      delete this.addressReadModel.id;
      this.addressService.create(this.addressReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<AddressReadModel>) => {
          if (val?.failed == false) {
            this.addressForm.patchValue(val.data)
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
  goNext() {
    this.router.navigate(['about'], {
      queryParams: { id: this.buildingId }
    });
  }
}

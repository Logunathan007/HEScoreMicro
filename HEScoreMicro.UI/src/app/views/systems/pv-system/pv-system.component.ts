import { OrientationOptions } from './../../../shared/lookups/common.lookup';
import { Component, OnInit } from "@angular/core";
import { PVSystemReadModel } from "../../../shared/models/pv-system/pv-system.model";
import { BooleanOptions } from "../../../shared/lookups/common.lookup";
import { Unsubscriber } from "../../../shared/modules/unsubscribe/unsubscribe.component.";
import { FormBuilder, FormGroup } from "@angular/forms";
import { CommonService } from "../../../shared/services/common/common.service";
import { PVSystemService } from "../../../shared/services/pv-system/pv-system.service";
import { ActivatedRoute, Router } from "@angular/router";
import { takeUntil } from "rxjs";
import { Result } from "../../../shared/models/common/Result";
import { AnglePanelsAreTiltedOptions } from "../../../shared/lookups/pv-system.lookup";

@Component({
  selector: 'app-pv-system',
  templateUrl: './pv-system.component.html',
  styleUrl: './pv-system.component.scss',
  standalone: false
})
export class PVSystemComponent extends Unsubscriber implements OnInit {
  //variable initializations
  pVSystemForm!: FormGroup | any;
  buildingId: string | null | undefined;
  pVSystemReadModel!: PVSystemReadModel;
  booleanOptions = BooleanOptions
  anglePanelsAreTiltedOptions = AnglePanelsAreTiltedOptions
  orientationOptions = OrientationOptions

  get pVSystemControl() {
    return this.pVSystemForm.controls;
  }

  constructor(
    protected commonService: CommonService,
    private pVSystemService: PVSystemService,
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
    this.pVSystemForm = this.fb.group({
      id: [null],
      hasPhotovoltaic: [null],
      yearInstalled: [null],
      directionPanelsFace: [null],
      anglePanelsAreTilted: [null],
      knowSystemCapacity: [null],
      numberOfPanels: [null],
      dcCapacity: [null],
      buildingId: [this.buildingId],
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
      this.pVSystemService.getByBuildingId(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<PVSystemReadModel>) => {
          if (val?.failed == false) {
            this.pVSystemForm.patchValue(val.data)
          }
          console.log(val);
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    }
  }
  onSave() {
    if (this.pVSystemForm.invalid) {
      this.pVSystemForm.markAllAsTouched();
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }
    if (this.pVSystemForm.value?.id) {
      this.pVSystemReadModel = this.pVSystemForm.value
      this.pVSystemService.update(this.pVSystemReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<PVSystemReadModel>) => {
          this.pVSystemForm.patchValue(val.data)
          console.log(val);
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    } else {
      this.pVSystemReadModel = this.pVSystemForm.value
      delete this.pVSystemReadModel.id;
      this.pVSystemService.create(this.pVSystemReadModel).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<PVSystemReadModel>) => {
          if (val?.failed == false) {
            this.pVSystemForm.patchValue(val.data)
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
    this.router.navigate(['zones/floor'], {
      queryParams: { id: this.buildingId }
    })
  }
}


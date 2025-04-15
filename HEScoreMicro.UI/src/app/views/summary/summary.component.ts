import { HpxmlGenerationService } from './../../shared/services/hpxml-generation/hpxml-generation.service';
import { CommonService } from './../../shared/services/common/common.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Unsubscriber } from '../../shared/modules/unsubscribe/unsubscribe.component.';
import { BuildingService } from '../../shared/services/building/building.service';
import { takeUntil } from 'rxjs';
import { BuildingReadModel } from '../../shared/models/building/building.model';
import { Result } from '../../shared/models/common/result.model';
import { removeAllIdProperties } from '../../shared/modules/Transformers/TransormerFunction';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrl: './summary.component.scss',
  standalone: false
})
export class SummaryComponent extends Unsubscriber implements OnInit {

  buildingId: string = "";
  buildingData: BuildingReadModel | null | undefined;
  buildingToggle: boolean = false
  removeAllIdProperties = removeAllIdProperties
  validationData: any;
  validationStatus: any;
  validationToggle: boolean = true;
  PDFResponse: any;
  PDFLoader:boolean = false;
  pdfToggle:boolean = true;

  constructor(private buildingService: BuildingService,
    private route: ActivatedRoute, private commonService: CommonService,
    private hpxmlGenerationService:HpxmlGenerationService
  ) {
    super();
  }

  ngOnInit(): void {
    this.getBuildingId();
    this.getData();
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
      this.buildingService.getById(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
        next: (val: Result<BuildingReadModel>) => {
          if (val?.failed == false) {
            this.buildingData = this.removeAllIdProperties(val?.data)
          }
          console.log(val);
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    }
  }

  validate() {
    if(!this.buildingId) return
    this.hpxmlGenerationService.validateInputs(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
      next: (val: any) => {
        if (val?.failed == false) {
          this.validationData = JSON.parse(val?.homeJson);
          this.validationStatus = true;
        } else {
          this.validationData = val?.errors;
          this.validationStatus = false;
        }
        console.log(val);
      },
      error: (err: any) => {
        this.validationStatus = false;
        console.log(err);
      }
    })
  }

  generatePDF() {
    this.PDFLoader = true;
    this.hpxmlGenerationService.generatePDF(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
      next: (val: any) => {
        if (val?.failed == false) {
          this.PDFResponse = val?.data;
        }
        this.PDFLoader = false;
        console.log(val);
      },
      error: (err: any) => {
        this.PDFLoader = false;
        console.log(err);
      }
    })
  }

  clearOldPDF() {
    this.hpxmlGenerationService.clearOldBuilding(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
      next: (val: any) => {
        console.log(val);
      },
      error: (err: any) => {
        console.log(err);
      }
    })
  }


}

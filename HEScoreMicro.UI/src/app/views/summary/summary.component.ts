import { HpxmlGenerationService } from './../../shared/services/hpxml-generation/hpxml-generation.service';
import { Component, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { Unsubscriber } from '../../shared/modules/unsubscribe/unsubscribe.component.';
import { takeUntil } from 'rxjs';
import { BuildingReadModel } from '../../shared/models/building/building.model';
import { removeAllIdProperties } from '../../shared/modules/Transformers/TransormerFunction';
@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrl: './summary.component.scss',
  standalone: false
})
export class SummaryComponent extends Unsubscriber implements OnInit, OnChanges {
  @Input('buildingId') buildingId: string = "";
  @Input('input') buildingData: BuildingReadModel | null | undefined;
  buildingCopy: BuildingReadModel | null | undefined
  buildingToggle: boolean = false
  removeAllIdProperties = removeAllIdProperties
  validationData: any;
  validationStatus: any;
  validationToggle: boolean = true;
  PDFResponse: any;
  PDFLoader: boolean = false;
  pdfToggle: boolean = true;

  constructor(
    private hpxmlGenerationService: HpxmlGenerationService
  ) {
    super();
  }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.buildingCopy = removeAllIdProperties(JSON.parse(JSON.stringify(this.buildingData)))
  }

  validate() {
    if (!this.buildingId) return
    this.hpxmlGenerationService.validateInputs(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
      next: (val: any) => {
        if (val?.failed === false) {
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
    this.PDFResponse = null;
    this.hpxmlGenerationService.generatePDF(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
      next: (val: any) => {
        if (val?.failed === false) {
          this.PDFResponse = val?.data;
          if(val?.data[0]?.link){
            window.open(val?.data[0]?.link)
          }
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

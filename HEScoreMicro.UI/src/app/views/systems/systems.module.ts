import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SystemsRoutingModule } from './systems-routing.module';
import { SystemsComponent } from './systems.component';
import { NgxJsonViewerModule } from 'ngx-json-viewer';
import { NgSelectModule } from '@ng-select/ng-select';
import { ReactiveFormsModule } from '@angular/forms';
import { WaterHeaterComponent } from './water-heater/water-heater.component';
import { PVSystemComponent } from './pv-system/pv-system.component';
import { HeatingCoolingSystemComponent } from './heating-cooling-system/heating-cooling-system.component';
import { PrintHpxmlComponent } from '../print-hpxml/print-hpxml.component';

@NgModule({
  declarations: [
    SystemsComponent,
    WaterHeaterComponent,
    HeatingCoolingSystemComponent,
    PVSystemComponent,
    HeatingCoolingSystemComponent
  ],
  imports: [
    CommonModule,
    SystemsRoutingModule,
    NgxJsonViewerModule,
    NgSelectModule,
    ReactiveFormsModule,
    PrintHpxmlComponent
  ]
})
export class SystemsModule { }

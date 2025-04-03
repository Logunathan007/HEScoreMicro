import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SystemsRoutingModule } from './systems-routing.module';
import { SystemsComponent } from './systems.component';
import { NgxJsonViewerModule } from 'ngx-json-viewer';
import { NgSelectModule } from '@ng-select/ng-select';
import { ReactiveFormsModule } from '@angular/forms';
import { HvacHeatCoolComponent } from './hvac-heat-cool/hvac-heat-cool.component';
import { WaterHeatingSystemComponent } from './water-heating-system/water-heating-system.component';
import { PvSystemComponent } from './pv-system/pv-system.component';

@NgModule({
  declarations: [
    SystemsComponent,
    HvacHeatCoolComponent,
    WaterHeatingSystemComponent,
    PvSystemComponent
  ],
  imports: [
    CommonModule,
    SystemsRoutingModule,
    NgxJsonViewerModule,
    NgSelectModule,
    ReactiveFormsModule,
  ]
})
export class SystemsModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AboutComponent } from './about/about.component';
import { ViewsRoutingModule } from './views-routing.module';
import { AddressComponent } from './address/address.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxJsonViewerModule } from 'ngx-json-viewer';
import { HttpClientModule } from '@angular/common/http';
import { ViewsComponent } from './views.component';
import { SummaryComponent } from './summary/summary.component';
import { UnsubscribeModule } from '../shared/modules/unsubscribe/unsubscribe.module';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PrintHpxmlComponent } from './print-hpxml/print-hpxml.component';
import { ZoneFloorComponent } from './zone-floor/zone-floor.component';
import { ZoneRoofComponent } from './zone-roof/zone-roof.component';
import { ZoneWallComponent } from './zone-wall/zone-wall.component';
import { ZoneWindowComponent } from './zone-window/zone-window.component';
import { PVSystemComponent } from './pv-system/pv-system.component';
import { WaterHeaterComponent } from './water-heater/water-heater.component';
import { HeatingCoolingSystemComponent } from './heating-cooling-system/heating-cooling-system.component';

@NgModule({
  declarations: [
    AboutComponent,
    AddressComponent,
    ViewsComponent,
    SummaryComponent,
    PrintHpxmlComponent,
    ZoneFloorComponent,
    ZoneRoofComponent,
    ZoneWallComponent,
    ZoneWindowComponent,
    PVSystemComponent,
    WaterHeaterComponent,
    HeatingCoolingSystemComponent
  ],
  imports: [
    CommonModule,
    ViewsRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    HttpClientModule,
    NgxJsonViewerModule,
    BsDatepickerModule,
    UnsubscribeModule,
  ],
})
export class ViewsModule { }

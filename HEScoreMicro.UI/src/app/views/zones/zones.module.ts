import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ZonesRoutingModule } from './zones-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxJsonViewerModule } from 'ngx-json-viewer';
import { ZonesComponent } from './zones.component';
import { ZoneRoofComponent } from './zone-roof/zone-roof.component';
import { ZoneWallComponent } from './zone-wall/zone-wall.component';
import { ZoneFloorComponent } from './zone-floor/zone-floor.component';
import { ZoneWindowComponent } from './zone-window/zone-window.component';
import { PrintHpxmlComponent } from '../print-hpxml/print-hpxml.component';


@NgModule({
  declarations: [
    ZoneFloorComponent,
    ZoneWallComponent,
    ZoneRoofComponent,
    ZonesComponent,
    ZoneWindowComponent
  ],
  imports: [
    CommonModule,
    ZonesRoutingModule,
    ReactiveFormsModule,
    NgSelectModule,
    NgxJsonViewerModule,
    PrintHpxmlComponent
  ]
})
export class ZonesModule { }

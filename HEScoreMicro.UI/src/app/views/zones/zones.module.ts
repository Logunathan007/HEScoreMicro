import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ZonesRoutingModule } from './zones-routing.module';
import { ZoneFloorComponent } from './zone-floor/zone-floor.component';
import { ZoneWallComponent } from './zone-wall/zone-wall.component';
import { ZoneRoofComponent } from './zone-roof/zone-roof.component';
import { ZonesComponent } from './zones.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxJsonViewerModule } from 'ngx-json-viewer';


@NgModule({
  declarations: [
    ZoneFloorComponent,
    ZoneWallComponent,
    ZoneRoofComponent,
    ZonesComponent
  ],
  imports: [
    CommonModule,
    ZonesRoutingModule,
    ReactiveFormsModule,
    NgSelectModule,
    NgxJsonViewerModule
  ]
})
export class ZonesModule { }

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ZoneFloorComponent } from './zone-floor/zone-floor.component';
import { ZoneRoofComponent } from './zone-roof/zone-roof.component';
import { ZoneWallComponent } from './zone-wall/zone-wall.component';
import { ZonesComponent } from './zones.component';

const routes: Routes = [
  {
    path: '',
    component:ZonesComponent,
    children:[
      {
        path: '',
        pathMatch:'full',
        redirectTo:"floor"
      },
      {
        path: 'floor',
        component: ZoneFloorComponent,
      },
      {
        path: 'roof',
        component: ZoneRoofComponent,
      },
      {
        path: 'wall',
        component: ZoneWallComponent,
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ZonesRoutingModule { }

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SystemsComponent } from './systems.component';
import { PvSystemComponent } from './pv-system/pv-system.component';
import { HvacHeatCoolComponent } from './hvac-heat-cool/hvac-heat-cool.component';
import { WaterHeatingSystemComponent } from './water-heating-system/water-heating-system.component';

const routes: Routes = [
  {
    path: '',
    component:SystemsComponent,
    children:[
      {
        path: '',
        pathMatch:'full',
        redirectTo:"distribution-system"
      },
      {
        path:'hvac-heat-cool',
        component:HvacHeatCoolComponent
      },
      {
        path:'pv-system',
        component:PvSystemComponent
      },
      {
        path:'water-heating-system',
        component:WaterHeatingSystemComponent
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SystemsRoutingModule { }

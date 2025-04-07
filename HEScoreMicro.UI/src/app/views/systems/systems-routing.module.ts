import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SystemsComponent } from './systems.component';
import { PVSystemComponent } from './pv-system/pv-system.component';
import { WaterHeaterComponent } from './water-heater/water-heater.component';
import { HeatingCoolingSystemComponent } from './heating-cooling-system/heating-cooling-system.component';

const routes: Routes = [
  {
    path: '',
    component:SystemsComponent,
    children:[
      {
        path: '',
        pathMatch:'full',
        redirectTo:"heat-cool-system"
      },
      {
        path:'heat-cool-system',
        component:HeatingCoolingSystemComponent
      },
      {
        path:'pv-system',
        component:PVSystemComponent
      },
      {
        path:'water-heater',
        component:WaterHeaterComponent
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SystemsRoutingModule { }

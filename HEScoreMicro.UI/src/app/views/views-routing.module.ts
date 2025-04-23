import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddressComponent } from './address/address.component';
import { ViewsComponent } from './views.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: "full",
    redirectTo: "building"
  },
  {
    path: 'create-building',
    component: AddressComponent
  },
  {
    path: 'building',
    component: ViewsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ViewsRoutingModule { }

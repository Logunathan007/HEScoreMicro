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
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PrintHpxmlComponent } from './print-hpxml/print-hpxml.component';

@NgModule({
  declarations: [
    AboutComponent,
    AddressComponent,
    ViewsComponent,
    SummaryComponent,
    // PrintHpxmlComponent,
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
    PrintHpxmlComponent
  ],
})
export class ViewsModule { }

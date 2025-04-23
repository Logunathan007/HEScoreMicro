import { CommonService } from './../shared/services/common/common.service';
import { Component, OnInit } from '@angular/core';
import { Unsubscriber } from '../shared/modules/unsubscribe/unsubscribe.component.';
import { takeUntil } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-views',
  templateUrl: './views.component.html',
  standalone: false
})
export class ViewsComponent extends Unsubscriber implements OnInit {
  //variable initializations
  navs!:string[]
  selectedIndex!:number;
  buildingId!:string;

  constructor(
    public commonService:CommonService,public route:ActivatedRoute
  ) {
    super()
  }

  ngOnInit(): void {
    this.getBuildingId();
    this.variableDeclaration();
  }

  //variable declarations
  variableDeclaration() {
    this.navs = ["About","Roof","Floor","Wall","Window","HVAC","Heater","PVSystem","Summary"]
    this.selectedIndex = 0;
  }

  getBuildingId() {
    this.route.queryParamMap.pipe(takeUntil(this.destroy$)).subscribe(params => {
      if (params.get('id')) {
        this.commonService.buildingId = params.get('id');
      }
      this.buildingId = params.get('id') ?? this.commonService.buildingId ?? ""
    })
  }

  move(event:boolean){
    if(event){
      if(this.selectedIndex == this.navs.length) return;
      this.selectedIndex++;
    }else{
      if(this.selectedIndex == 0) return;
      this.selectedIndex--;
    }
  }
}

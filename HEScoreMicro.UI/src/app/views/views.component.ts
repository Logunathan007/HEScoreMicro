import { Component, OnInit } from '@angular/core';
import { Unsubscriber } from '../shared/modules/unsubscribe/unsubscribe.component.';
import { takeUntil } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { BuildingService } from '../shared/services/building/building.service';
import { BuildingReadModel } from '../shared/models/building/building.model';
import { Result } from '../shared/models/common/result.model';

@Component({
  selector: 'app-views',
  templateUrl: './views.component.html',
  standalone: false
})
export class ViewsComponent extends Unsubscriber implements OnInit {
  //variable initializations
  navs!: string[]
  selectedIndex!: number;
  buildingId!: string;
  building!:BuildingReadModel;

  constructor(
    public route: ActivatedRoute,public buildingService: BuildingService
  ) {
    super()
  }

  ngOnInit(): void {
    this.getBuildingId();
    this.variableDeclaration();
    // this.getBuildingData();
  }

  //variable declarations
  variableDeclaration() {
    this.navs = ["About", "Roof", "Floor", "Wall", "Window", "HVAC", "Heater", "PVSystem", "Summary"]
    this.selectedIndex = 0;
  }

  getBuildingId() {
    this.route.queryParamMap.pipe(takeUntil(this.destroy$)).subscribe(params => {
      this.buildingId = params.get('id') ?? ""
    })
  }

  // getBuildingData(){
  //   this.buildingService.getById(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
  //     next:(val:Result<BuildingReadModel>)=>{
  //       if(val.failed == false){
  //         this.building = val.data as BuildingReadModel
  //         console.log(this.building);
  //       }
  //     },
  //     error:(err:any)=>{
  //       console.log(err);
  //     }
  //   })
  // }

  move(event: boolean) {
    if (event) {
      if (this.selectedIndex == this.navs.length) return;
      this.selectedIndex++;
    } else {
      if (this.selectedIndex == 0) return;
      this.selectedIndex--;
    }
  }
}

import { Component, OnInit } from '@angular/core';
import { Unsubscriber } from '../shared/modules/unsubscribe/unsubscribe.component.';
import { takeUntil } from 'rxjs';
import { ActivatedRoute, NavigationEnd, Event, Router } from '@angular/router';
import { BuildingService } from '../shared/services/building/building.service';
import { BuildingReadModel } from '../shared/models/building/building.model';
import { Result } from '../shared/models/common/result.model';
import { EmitterModel } from '../shared/models/common/emitter.model';

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
  building!: BuildingReadModel;

  constructor(
    public route: ActivatedRoute, public buildingService: BuildingService, public router: Router
  ) {
    super()
  }

  ngOnInit(): void {
    this.getBuildingId();
    this.variableDeclaration();
    this.getBuildingData();
  }

  //variable declarations
  variableDeclaration() {
    this.navs = ["About", "Roof", "Floor", "Wall", "Window", "HVAC", "Heater", "PVSystem", "Summary"]
    this.selectedIndex = 0;
  }

  getBuildingId() {
    this.route.queryParamMap.pipe(takeUntil(this.destroy$)).subscribe(params => {
      this.buildingId = params.get('id') ?? ""
      if (!this.buildingId) {
        let res = alert('Building is not selected');
        console.log("Building",res);
      }
    })
  }

  getBuildingData() {
    this.buildingService.getById(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
      next: (val: Result<BuildingReadModel>) => {
        if (val.failed == false) {
          this.building = val.data as BuildingReadModel
        }
      },
      error: (err: any) => {
        console.log(err);
      }
    })
  }

  updateBuilding(data: EmitterModel<any>) {
    switch (data.fieldType) {
      case "about":
        this.building.about = data.field
        break;
      case "zone-roof":
        this.building.zoneRoof = data.field
        break;
      case "zone-floor":
        this.building.zoneFloor = data.field
        break;
      case "zone-wall":
        this.building.zoneWall = data.field
        break;
      case "zone-window":
        this.building.zoneWindow = data.field
        break;
      case "heating-cooling-system":
        this.building.heatingCoolingSystem = data.field
        break;
      case "pv-system":
        this.building.pvSystem = data.field
        break;
      case "water-heater":
        this.building.waterHeater = data.field
        break;
    }
  }

  move(event: boolean | number) {
    window.scrollTo({ top: 0, behavior: 'instant' });
    if (typeof event === 'boolean') {
      if (event) {
        if (this.selectedIndex == this.navs.length) return;
        this.selectedIndex++;
      } else {
        if (this.selectedIndex == 0) return;
        this.selectedIndex--;
      }
    } else if (typeof event === 'number') {
      this.selectedIndex = event
    }
  }
}

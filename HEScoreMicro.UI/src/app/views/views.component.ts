import { Component, OnInit } from '@angular/core';
import { Unsubscriber } from '../shared/modules/unsubscribe/unsubscribe.component.';
import { takeUntil } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { BuildingService } from '../shared/services/building/building.service';
import { BuildingReadModel } from '../shared/models/building/building.model';
import { Result } from '../shared/models/common/result.model';
import { EmitterModel } from '../shared/models/common/emitter.model';
import { WallReadModel } from '../shared/models/zone-wall/wall.read.model';
import { WindowService } from '../shared/services/zone-window/window.service';
import { WindowReadModel } from '../shared/models/zone-window/window.model';
interface Navs {
  name: string;
  id?: string | null | undefined;
}
@Component({
  selector: 'app-views',
  templateUrl: './views.component.html',
  standalone: false
})
export class ViewsComponent extends Unsubscriber implements OnInit {

  //variable initializations
  navs!: Navs[]
  selectedIndex!: number;
  building!: BuildingReadModel;
  buildingId!: string;
  buildingType!: number | null;
  hasBoiler: boolean = false;
  roofType1: string | null | undefined;
  roofType2: string | null | undefined;
  floorType1: string | null | undefined;
  floorType2: string | null | undefined;
  footPrint: number | null | undefined;
  totalRoofArea: number | null | undefined;
  windowsAvailable: number[] = [] //0->Front,1->Back,2->Right,3->Left

  constructor(
    public route: ActivatedRoute, public buildingService: BuildingService, public router: Router,
    private windowService: WindowService,
  ) {
    super()
  }

  ngOnInit(): void {
    this.getBuildingId();
    this.getBuildingData();
    this.variableDeclaration();
  }

  //variable declarations
  variableDeclaration() {
    this.updateNav()
    this.selectedIndex = 1;
  }

  updateNav() {
    this.navs = [
      { name: "About", id: this.building?.about?.id },
      { name: "Roof", id: this.building?.zoneRoof?.id },
      { name: "Floor", id: this.building?.zoneFloor?.id },
      { name: "Wall", id: this.building?.zoneWall?.id },
      { name: "Window", id: this.building?.zoneWindow?.id },
      { name: "HVAC", id: this.building?.heatingCoolingSystem?.id },
      { name: "DHW", id: this.building?.waterHeater?.id },
      { name: "PVSystem", id: this.building?.pvSystem?.id },
      { name: "Star", id: this.building?.energyStar?.id },
      { name: "Summary", id: "summary" }
    ]
  }

  getBuildingId() {
    this.route.queryParamMap.pipe(takeUntil(this.destroy$)).subscribe(params => {
      this.buildingId = params.get('id') ?? ""
    })
  }

  getBuildingData() {
    this.buildingService.getById(this.buildingId).pipe(takeUntil(this.destroy$)).subscribe({
      next: (val: Result<BuildingReadModel>) => {
        if (val.failed === false) {
          this.building = val.data as BuildingReadModel
          this.setBuildingType(this.building?.address?.dwellingUnitType)
          this.checkForBoiler();
          this.checkFoundationType();
          this.checkAtticCellingChanges();
          this.checkFootPrintArea();
          if (this.buildingType == 1) {
            this.windowsAvailable = []
            this.checkWallsAvailability();
          }
          this.updateNav()
        }
      },
      error: (err: any) => {
        console.log(err);
      }
    })
  }

  //Single-Family Detached 0
  //Townhouse/Rowhouse/Duplex 1
  setBuildingType(type: string | undefined) {
    switch (type) {
      case "Single-Family Detached":
        this.buildingType = 0;
        break;
      case "Townhouse/Rowhouse/Duplex":
        this.buildingType = 1;
        break;
      default:
        this.buildingType = null;
        break;
    }
  }

  checkForBoiler() {
    let flag = false;
    this.building?.heatingCoolingSystem?.systems?.forEach(obj => {
      if (obj.heatingSystemType?.endsWith("boiler")) {
        flag = true;
      }
    })
    this.hasBoiler = flag;
  }

  checkAtticCellingChanges() {
    this.roofType1 = this.building?.zoneRoof?.roofAttics?.at(0)?.atticOrCeilingType
    this.roofType2 = this.building?.zoneRoof?.roofAttics?.at(1)?.atticOrCeilingType
    this.totalRoofArea = this.building?.zoneRoof?.roofAttics
      .reduce((sum: number, item: any) => {
        return sum + (((item?.atticOrCeilingType == "Unconditioned Attic") ? item?.atticFloorArea : item?.roofArea) ?? 0)
      }, 0);
  }

  checkFoundationType() {
    this.floorType1 = this.building?.zoneFloor?.foundations?.at(0)?.foundationType
    this.floorType2 = this.building?.zoneFloor?.foundations?.at(1)?.foundationType
  }

  checkFootPrintArea() {
    let noOfFloor: number = this.building?.about?.storiesAboveGroundLevel as number
    let condArea: number = this.building?.about?.totalConditionedFloorArea as number
    this.footPrint = condArea / noOfFloor;
  }

  checkWallsAvailability() {
    this.building?.zoneWall?.walls?.forEach((val: WallReadModel, index: number) => {
      if (val.adjacentTo == "Outside") this.windowsAvailable?.push(val.facing);
    })
    if (this.windowsAvailable?.length != this.building?.zoneWindow?.windows?.length) {
      const ids = this.building?.zoneWindow?.windows?.map(w => w?.id).filter(id => !!id) as string[];
      if (ids.length) {
        this.windowService.bulkDelete(ids).pipe(takeUntil(this.destroy$)).subscribe({
          next: (val: Result<WindowReadModel>) => {
            if (val?.failed === false) {
              this.building.zoneWindow.windows = [];
            }
          },
          error: (err: any) => console.log(err)
        })
      }
    }
  }

  updateBuilding(data: EmitterModel<any>) {
    switch (data.fieldType) {
      case "about":
        this.building.about = data.field
        this.checkFootPrintArea()
        break;
      case "zone-roof":
        this.building.zoneRoof = data.field
        this.checkAtticCellingChanges();
        break;
      case "zone-floor":
        this.building.zoneFloor = data.field
        this.checkFoundationType();
        break;
      case "zone-wall":
        this.building.zoneWall = data.field
        if (this.buildingType === 1) {
          this.windowsAvailable = []
          this.checkWallsAvailability()
        }
        break;
      case "zone-window":
        this.building.zoneWindow = data.field
        break;
      case "heating-cooling-system":
        this.building.heatingCoolingSystem = data.field
        this.checkForBoiler();
        break;
      case "pv-system":
        this.building.pvSystem = data.field
        break;
      case "water-heater":
        this.building.waterHeater = data.field
        break;
      case "energy-star":
        this.building.energyStar = data.field
        break;
    }
    this.updateNav()
  }

  move(event: boolean | number) {
    window.scrollTo({ top: 0, behavior: 'instant' });
    if (typeof event === 'boolean') {
      // true for forward, false for backward
      if (event) {
        if (this.selectedIndex == this.navs.length) return;
        this.selectedIndex++;
      } else {
        if (this.selectedIndex == 0) return;
        this.selectedIndex--;
      }
    } else if (typeof event === 'number') {
      // move to that index
      if (this.navs[event]?.id || this.navs?.at(event - 1)?.id) {
        this.selectedIndex = event
      }
    }
  }
}

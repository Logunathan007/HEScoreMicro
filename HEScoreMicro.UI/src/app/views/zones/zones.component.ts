import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonService } from '../../shared/services/common/common.service';

@Component({
  selector: 'app-zones',
  templateUrl: './zones.component.html',
  styleUrl: './zones.component.scss'
})
export class ZonesComponent {
  constructor(private commonService: CommonService, private router: Router) {
  }

  navigateTo(routerString: string) {
    this.router.navigate(["zones/" + routerString], {
      queryParams: { id: this.commonService.buildingId }
    });
  }
}

import { Component } from '@angular/core';
import { CommonService } from '../../shared/services/common/common.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-systems',
  templateUrl: './systems.component.html',
  styleUrl: './systems.component.scss'
})
export class SystemsComponent {
  constructor(private commonService: CommonService, private router: Router) {
  }
  navigateTo(routerString: string) {
    this.router.navigate(["systems/" + routerString], {
      queryParams: { id: this.commonService.buildingId }
    });
  }
}

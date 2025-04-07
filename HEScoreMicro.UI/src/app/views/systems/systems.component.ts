import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonService } from '../../shared/services/common/common.service';
@Component({
  selector: 'app-systems',
  templateUrl: './systems.component.html',
  styleUrl: './systems.component.scss',
  standalone:false
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

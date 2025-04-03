import { CommonService } from '../../shared/services/common/common.service';
import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  //variables initialization
  headerNavs: any;
  selectedNav: any;

  constructor(private commonService: CommonService, private router: Router) {
    this.variableDeclaration();
  }

  //variable default declaration
  variableDeclaration() {
    this.headerNavs = [
      {
        name: "Address",
        routerLink: "address"
      },
      {
        name: "About",
        routerLink: "about"
      },
      {
        name: "Zones",
        routerLink: "zones",
      },
      {
        name: "Systems",
        routerLink: "systems",
      },
      {
        name: "Summary",
        routerLink: "summary",
      }
    ]
    this.selectedNav = this.headerNavs[0];
  }

  navigateTo(routerString: string) {
    if (routerString == '/address') {
      if(this.commonService.buildingId){
        this.router.navigate(['/address'], {
          queryParams: { id: this.commonService.buildingId }
        });
      }else{
        this.router.navigate(['address']);
      }
    } else {
      this.router.navigate([routerString], {
        queryParams: { id: this.commonService.buildingId }
      });
    }
  }
}

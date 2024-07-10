import { Component, OnInit } from '@angular/core';
import { HomePage } from './models/homeModel';
import { HomePageService } from '../../admin/homePage/services/homepage.service';
import { Router } from '@angular/router';
import { Tecnology } from '../../admin/tecnology/models/tecnology';
import { TecnologyService } from '../../admin/tecnology/services/tecnology.service';
import { Member } from '../../admin/member/models/Member';
import { MemberService } from '../../admin/member/services/member.service';
import { AboutService } from '../../admin/about/services/about.service';
import { About } from '../../admin/about/models/About';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {

  constructor(private homePageService : HomePageService, private router: Router, private tecnologyService:TecnologyService, private memberService: MemberService, private aboutService:AboutService) { }
  homePageList: HomePage[];
  activeProject: HomePage;
  tecnologyList:Tecnology[];
  teamList: Member[];
  teamListToShow: Member[];
  aboutList:About[];
  activeAboutContent: string;

  ngOnInit(): void {
    this.getHomePageList();
    this.getTecnologyList();
    this.getTeamList();
    this.getAboutList();
  }

  getHomePageList() {
		this.homePageService.getHomePageList().subscribe(data => {
			this.homePageList = data;
      this.activeProject = this.homePageList.find(p =>p.id =1)
		});
	}
  getTeamList() {
		this.memberService.getMemberList().subscribe(data => {
			this.teamList = data;
      this.teamListToShow = this.teamList.slice(0, 4);
		});
	}

  getAboutList() {
    this.aboutService.getAboutList().subscribe(data => {
      this.aboutList = data;
      const aboutItem = this.aboutList.find(p => p.id === 1);
      if (aboutItem) {
        this.activeAboutContent = aboutItem.content.length > 250 
          ? aboutItem.content.substring(0, 550) + '...' 
          : aboutItem.content;
      }
    });
  }

  navigateToAbout() {
    this.router.navigate(['/Pabout']);
  }

  navigateToTech() {
    this.router.navigate(['/Ptec']);
  }

  getTecnologyList() {
		this.tecnologyService.getTecnologyList().subscribe(data => {
			this.tecnologyList = data;
      this.tecnologyList = this.tecnologyList.slice(0,3)
		});
	}
}

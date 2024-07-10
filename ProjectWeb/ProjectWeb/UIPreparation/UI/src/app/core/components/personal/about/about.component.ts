import { Component, OnInit } from '@angular/core';
import { AboutService } from '../../admin/about/services/about.service';
import { About } from '../../admin/about/models/About';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {

  constructor(private aboutService:AboutService) { }
  aboutList:About[];
  activeAboutContent: About;

  ngOnInit(): void {
    this.getAboutList();
  }

  getAboutList() {
		this.aboutService.getAboutList().subscribe(data => {
			this.aboutList = data;
      this.activeAboutContent = this.aboutList.find(p =>p.id =1)
		});
	}

}

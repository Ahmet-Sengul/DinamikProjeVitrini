import { Component, OnInit } from '@angular/core';
import { TecnologyService } from '../../admin/tecnology/services/tecnology.service';
import { Tecnology } from '../../admin/tecnology/models/tecnology';

@Component({
  selector: 'app-technological-infrastructure',
  templateUrl: './technological-infrastructure.component.html',
  styleUrls: ['./technological-infrastructure.component.css']
})
export class TechnologicalInfrastructureComponent implements OnInit {

  constructor(private tecnologyService:TecnologyService) { }
  tecnologyList:Tecnology[];


  ngOnInit(): void {
    this.getTecnologyList();
  }

  getTecnologyList() {
		this.tecnologyService.getTecnologyList().subscribe(data => {
			this.tecnologyList = data;
		});
	}
}

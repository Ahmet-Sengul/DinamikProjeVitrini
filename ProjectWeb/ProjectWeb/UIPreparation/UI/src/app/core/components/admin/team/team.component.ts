import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { Team } from './models/Team';
import { TeamService } from './services/Team.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-team',
	templateUrl: './team.component.html',
	styleUrls: ['./team.component.scss']
})
export class TeamComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','createdDate','updatedDate','deletedDate','userId','roleId','imgUrl','description', 'update','delete'];

	teamList:Team[];
	team:Team=new Team();

	teamAddForm: FormGroup;


	teamId:number;

	constructor(private teamService:TeamService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getTeamList();
    }

	ngOnInit() {

		this.createTeamAddForm();
	}


	getTeamList() {
		this.teamService.getTeamList().subscribe(data => {
			this.teamList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.teamAddForm.valid) {
			this.team = Object.assign({}, this.teamAddForm.value)

			if (this.team.id == 0)
				this.addTeam();
			else
				this.updateTeam();
		}

	}

	addTeam(){

		this.teamService.addTeam(this.team).subscribe(data => {
			this.getTeamList();
			this.team = new Team();
			jQuery('#team').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.teamAddForm);

		})

	}

	updateTeam(){

		this.teamService.updateTeam(this.team).subscribe(data => {

			var index=this.teamList.findIndex(x=>x.id==this.team.id);
			this.teamList[index]=this.team;
			this.dataSource = new MatTableDataSource(this.teamList);
            this.configDataTable();
			this.team = new Team();
			jQuery('#team').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.teamAddForm);

		})

	}

	createTeamAddForm() {
		this.teamAddForm = this.formBuilder.group({		
			id : [0],
userId : [0, Validators.required],
roleId : [0, Validators.required],
imgUrl : ["", Validators.required],
description : ["", Validators.required]
		})
	}

	deleteTeam(teamId:number){
		this.teamService.deleteTeam(teamId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.teamList=this.teamList.filter(x=> x.id!=teamId);
			this.dataSource = new MatTableDataSource(this.teamList);
			this.configDataTable();
		})
	}

	getTeamById(teamId:number){
		this.clearFormGroup(this.teamAddForm);
		this.teamService.getTeamById(teamId).subscribe(data=>{
			this.team=data;
			this.teamAddForm.patchValue(data);
		})
	}


	clearFormGroup(group: FormGroup) {

		group.markAsUntouched();
		group.reset();

		Object.keys(group.controls).forEach(key => {
			group.get(key).setErrors(null);
			if (key == 'id')
				group.get(key).setValue(0);
		});
	}

	checkClaim(claim:string):boolean{
		return this.authService.claimGuard(claim)
	}

	configDataTable(): void {
		this.dataSource.paginator = this.paginator;
		this.dataSource.sort = this.sort;
	}

	applyFilter(event: Event) {
		const filterValue = (event.target as HTMLInputElement).value;
		this.dataSource.filter = filterValue.trim().toLowerCase();

		if (this.dataSource.paginator) {
			this.dataSource.paginator.firstPage();
		}
	}

  }

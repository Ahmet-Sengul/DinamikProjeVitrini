import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { TeamRole } from './models/TeamRole';
import { TeamRoleService } from './services/TeamRole.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-teamRole',
	templateUrl: './teamRole.component.html',
	styleUrls: ['./teamRole.component.scss']
})
export class TeamRoleComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','createdDate','updatedDate','deletedDate','roleName','description', 'update','delete'];

	teamRoleList:TeamRole[];
	teamRole:TeamRole=new TeamRole();

	teamRoleAddForm: FormGroup;


	teamRoleId:number;

	constructor(private teamRoleService:TeamRoleService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getTeamRoleList();
    }

	ngOnInit() {

		this.createTeamRoleAddForm();
	}


	getTeamRoleList() {
		this.teamRoleService.getTeamRoleList().subscribe(data => {
			this.teamRoleList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.teamRoleAddForm.valid) {
			this.teamRole = Object.assign({}, this.teamRoleAddForm.value)

			if (this.teamRole.id == 0)
				this.addTeamRole();
			else
				this.updateTeamRole();
		}

	}

	addTeamRole(){

		this.teamRoleService.addTeamRole(this.teamRole).subscribe(data => {
			this.getTeamRoleList();
			this.teamRole = new TeamRole();
			jQuery('#teamrole').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.teamRoleAddForm);

		})

	}

	updateTeamRole(){

		this.teamRoleService.updateTeamRole(this.teamRole).subscribe(data => {

			var index=this.teamRoleList.findIndex(x=>x.id==this.teamRole.id);
			this.teamRoleList[index]=this.teamRole;
			this.dataSource = new MatTableDataSource(this.teamRoleList);
            this.configDataTable();
			this.teamRole = new TeamRole();
			jQuery('#teamrole').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.teamRoleAddForm);

		})

	}

	createTeamRoleAddForm() {
		this.teamRoleAddForm = this.formBuilder.group({		
			id : [0],
roleName : ["", Validators.required],
description : ["", Validators.required]
		})
	}

	deleteTeamRole(teamRoleId:number){
		this.teamRoleService.deleteTeamRole(teamRoleId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.teamRoleList=this.teamRoleList.filter(x=> x.id!=teamRoleId);
			this.dataSource = new MatTableDataSource(this.teamRoleList);
			this.configDataTable();
		})
	}

	getTeamRoleById(teamRoleId:number){
		this.clearFormGroup(this.teamRoleAddForm);
		this.teamRoleService.getTeamRoleById(teamRoleId).subscribe(data=>{
			this.teamRole=data;
			this.teamRoleAddForm.patchValue(data);
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

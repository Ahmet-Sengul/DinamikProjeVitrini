import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { Tecnology } from './models/tecnology';
import { TecnologyService } from './services/tecnology.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-tecnology',
	templateUrl: './tecnology.component.html',
	styleUrls: ['./tecnology.component.scss']
})
export class TecnologyComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','createdDate','updatedDate','deletedDate','tecHeader','tecDescription','tecImgUrl', 'update','delete'];

	tecnologyList:Tecnology[];
	tecnology:Tecnology=new Tecnology();

	tecnologyAddForm: FormGroup;


	tecnologyId:number;

	constructor(private tecnologyService:TecnologyService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getTecnologyList();
    }

	ngOnInit() {

		this.createTecnologyAddForm();
	}


	getTecnologyList() {
		this.tecnologyService.getTecnologyList().subscribe(data => {
			this.tecnologyList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.tecnologyAddForm.valid) {
			this.tecnology = Object.assign({}, this.tecnologyAddForm.value)

			if (this.tecnology.id == 0)
				this.addTecnology();
			else
				this.updateTecnology();
		}

	}

	addTecnology(){

		this.tecnologyService.addTecnology(this.tecnology).subscribe(data => {
			this.getTecnologyList();
			this.tecnology = new Tecnology();
			jQuery('#tecnology').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.tecnologyAddForm);

		})

	}

	updateTecnology(){

		this.tecnologyService.updateTecnology(this.tecnology).subscribe(data => {

			var index=this.tecnologyList.findIndex(x=>x.id==this.tecnology.id);
			this.tecnologyList[index]=this.tecnology;
			this.dataSource = new MatTableDataSource(this.tecnologyList);
            this.configDataTable();
			this.tecnology = new Tecnology();
			jQuery('#tecnology').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.tecnologyAddForm);

		})

	}

	createTecnologyAddForm() {
		this.tecnologyAddForm = this.formBuilder.group({		
			id : [0],
tecHeader : ["", Validators.required],
tecDescription : ["", Validators.required],
tecImgUrl : ["", Validators.required]
		})
	}

	deleteTecnology(tecnologyId:number){
		this.tecnologyService.deleteTecnology(tecnologyId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.tecnologyList=this.tecnologyList.filter(x=> x.id!=tecnologyId);
			this.dataSource = new MatTableDataSource(this.tecnologyList);
			this.configDataTable();
		})
	}

	getTecnologyById(tecnologyId:number){
		this.clearFormGroup(this.tecnologyAddForm);
		this.tecnologyService.getTecnologyById(tecnologyId).subscribe(data=>{
			this.tecnology=data;
			this.tecnologyAddForm.patchValue(data);
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

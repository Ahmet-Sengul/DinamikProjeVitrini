import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { About } from './models/about';
import { AboutService } from './services/about.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-about',
	templateUrl: './about.component.html',
	styleUrls: ['./about.component.scss']
})
export class AboutComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','createdDate','updatedDate','deletedDate','header','content', 'update','delete'];

	aboutList:About[];
	about:About=new About();

	aboutAddForm: FormGroup;


	aboutId:number;

	constructor(private aboutService:AboutService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getAboutList();
    }

	ngOnInit() {

		this.createAboutAddForm();
	}


	getAboutList() {
		this.aboutService.getAboutList().subscribe(data => {
			this.aboutList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.aboutAddForm.valid) {
			this.about = Object.assign({}, this.aboutAddForm.value)

			if (this.about.id == 0)
				this.addAbout();
			else
				this.updateAbout();
		}

	}

	addAbout(){

		this.aboutService.addAbout(this.about).subscribe(data => {
			this.getAboutList();
			this.about = new About();
			jQuery('#about').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.aboutAddForm);

		})

	}

	updateAbout(){

		this.aboutService.updateAbout(this.about).subscribe(data => {

			var index=this.aboutList.findIndex(x=>x.id==this.about.id);
			this.aboutList[index]=this.about;
			this.dataSource = new MatTableDataSource(this.aboutList);
            this.configDataTable();
			this.about = new About();
			jQuery('#about').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.aboutAddForm);

		})

	}

	createAboutAddForm() {
		this.aboutAddForm = this.formBuilder.group({		
			id : [0],
			header : ["", Validators.required],
			content : ["", Validators.required]
		})
	}

	deleteAbout(aboutId:number){
		this.aboutService.deleteAbout(aboutId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.aboutList=this.aboutList.filter(x=> x.id!=aboutId);
			this.dataSource = new MatTableDataSource(this.aboutList);
			this.configDataTable();
		})
	}

	getAboutById(aboutId:number){
		this.clearFormGroup(this.aboutAddForm);
		this.aboutService.getAboutById(aboutId).subscribe(data=>{
			this.about=data;
			this.aboutAddForm.patchValue(data);
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

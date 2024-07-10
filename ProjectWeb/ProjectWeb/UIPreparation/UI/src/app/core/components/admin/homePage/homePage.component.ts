import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { HomePage } from './models/HomePage';
import { HomePageService } from './services/homepage.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-homePage',
	templateUrl: './homePage.component.html',
	styleUrls: ['./homePage.component.scss']
})
export class HomePageComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','createdDate','updatedDate','deletedDate','header','imgUrl','description', 'update','delete'];

	homePageList:HomePage[];
	homePage:HomePage=new HomePage();

	homePageAddForm: FormGroup;


	homePageId:number;
	selectedElement: any = null;


	constructor(private homePageService:HomePageService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getHomePageList();
    }

	ngOnInit() {

		this.createHomePageAddForm();
	}


	getHomePageList() {
		this.homePageService.getHomePageList().subscribe(data => {
			this.homePageList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.homePageAddForm.valid) {
			this.homePage = Object.assign({}, this.homePageAddForm.value)

			if (this.homePage.id == 0)
				this.addHomePage();
			else
				this.updateHomePage();
		}

	}

	addHomePage(){

		this.homePageService.addHomePage(this.homePage).subscribe(data => {
			this.getHomePageList();
			this.homePage = new HomePage();
			jQuery('#homepage').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.homePageAddForm);

		})

	}

	updateHomePage(){

		this.homePageService.updateHomePage(this.homePage).subscribe(data => {

			var index=this.homePageList.findIndex(x=>x.id==this.homePage.id);
			this.homePageList[index]=this.homePage;
			this.dataSource = new MatTableDataSource(this.homePageList);
            this.configDataTable();
			this.homePage = new HomePage();
			jQuery('#homepage').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.homePageAddForm);

		})

	}

	createHomePageAddForm() {
		this.homePageAddForm = this.formBuilder.group({		
			id : [0],
header : ["", Validators.required],
imgUrl : ["", Validators.required],
description : ["", Validators.required]
		})
	}

	deleteHomePage(homePageId:number){
		this.homePageService.deleteHomePage(homePageId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.homePageList=this.homePageList.filter(x=> x.id!=homePageId);
			this.dataSource = new MatTableDataSource(this.homePageList);
			this.configDataTable();
		})
	}

	getHomePageById(homePageId:number){
		this.clearFormGroup(this.homePageAddForm);
		this.homePageService.getHomePageById(homePageId).subscribe(data=>{
			this.homePage=data;
			this.homePageAddForm.patchValue(data);
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

	onSelect(element: any) {
		this.selectedElement = element;
	  }
  }

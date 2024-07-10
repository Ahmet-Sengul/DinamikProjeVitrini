import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { Contact } from './models/Contact';
import { ContactService } from './services/Contact.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-contact',
	templateUrl: './contact.component.html',
	styleUrls: ['./contact.component.scss']
})
export class ContactComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','createdDate','updatedDate','deletedDate','adress','phoneNumber','email', 'update','delete'];

	contactList:Contact[];
	contact:Contact=new Contact();

	contactAddForm: FormGroup;


	contactId:number;

	constructor(private contactService:ContactService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getContactList();
    }

	ngOnInit() {

		this.createContactAddForm();
	}


	getContactList() {
		this.contactService.getContactList().subscribe(data => {
			this.contactList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.contactAddForm.valid) {
			this.contact = Object.assign({}, this.contactAddForm.value)

			if (this.contact.id == 0)
				this.addContact();
			else
				this.updateContact();
		}

	}

	addContact(){

		this.contactService.addContact(this.contact).subscribe(data => {
			this.getContactList();
			this.contact = new Contact();
			jQuery('#contact').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.contactAddForm);

		})

	}

	updateContact(){

		this.contactService.updateContact(this.contact).subscribe(data => {

			var index=this.contactList.findIndex(x=>x.id==this.contact.id);
			this.contactList[index]=this.contact;
			this.dataSource = new MatTableDataSource(this.contactList);
            this.configDataTable();
			this.contact = new Contact();
			jQuery('#contact').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.contactAddForm);

		})

	}

	createContactAddForm() {
		this.contactAddForm = this.formBuilder.group({		
			id : [0],
			adress : ["", Validators.required],
			phoneNumber : ["", Validators.required],
			email : ["", Validators.required]
		})
	}

	deleteContact(contactId:number){
		this.contactService.deleteContact(contactId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.contactList=this.contactList.filter(x=> x.id!=contactId);
			this.dataSource = new MatTableDataSource(this.contactList);
			this.configDataTable();
		})
	}

	getContactById(contactId:number){
		this.clearFormGroup(this.contactAddForm);
		this.contactService.getContactById(contactId).subscribe(data=>{
			this.contact=data;
			this.contactAddForm.patchValue(data);
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

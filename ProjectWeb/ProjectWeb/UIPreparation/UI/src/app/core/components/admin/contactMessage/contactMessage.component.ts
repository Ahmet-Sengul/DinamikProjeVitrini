import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { ContactMessage } from './models/contactmessage';
import { ContactMessageService } from './services/contactmessage.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-contactMessage',
	templateUrl: './contactMessage.component.html',
	styleUrls: ['./contactMessage.component.scss']
})
export class ContactMessageComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','createdDate','updatedDate','deletedDate','name','email','subject','userMessage', 'update','delete'];

	contactMessageList:ContactMessage[];
	contactMessage:ContactMessage=new ContactMessage();

	contactMessageAddForm: FormGroup;


	contactMessageId:number;

	constructor(private contactMessageService:ContactMessageService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getContactMessageList();
    }

	ngOnInit() {

		this.createContactMessageAddForm();
	}


	getContactMessageList() {
		this.contactMessageService.getContactMessageList().subscribe(data => {
			this.contactMessageList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.contactMessageAddForm.valid) {
			this.contactMessage = Object.assign({}, this.contactMessageAddForm.value)

			if (this.contactMessage.id == 0)
				this.addContactMessage();
			else
				this.updateContactMessage();
		}

	}

	addContactMessage(){

		this.contactMessageService.addContactMessage(this.contactMessage).subscribe(data => {
			this.getContactMessageList();
			this.contactMessage = new ContactMessage();
			jQuery('#contactmessage').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.contactMessageAddForm);

		})

	}

	updateContactMessage(){

		this.contactMessageService.updateContactMessage(this.contactMessage).subscribe(data => {

			var index=this.contactMessageList.findIndex(x=>x.id==this.contactMessage.id);
			this.contactMessageList[index]=this.contactMessage;
			this.dataSource = new MatTableDataSource(this.contactMessageList);
            this.configDataTable();
			this.contactMessage = new ContactMessage();
			jQuery('#contactmessage').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.contactMessageAddForm);

		})

	}

	createContactMessageAddForm() {
		this.contactMessageAddForm = this.formBuilder.group({		
			id : [0],
name : ["", Validators.required],
email : ["", Validators.required],
subject : ["", Validators.required],
userMessage : ["", Validators.required]
		})
	}

	deleteContactMessage(contactMessageId:number){
		this.contactMessageService.deleteContactMessage(contactMessageId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.contactMessageList=this.contactMessageList.filter(x=> x.id!=contactMessageId);
			this.dataSource = new MatTableDataSource(this.contactMessageList);
			this.configDataTable();
		})
	}

	getContactMessageById(contactMessageId:number){
		this.clearFormGroup(this.contactMessageAddForm);
		this.contactMessageService.getContactMessageById(contactMessageId).subscribe(data=>{
			this.contactMessage=data;
			this.contactMessageAddForm.patchValue(data);
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

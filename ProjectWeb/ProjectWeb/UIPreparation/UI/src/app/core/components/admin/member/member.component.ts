import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { Member } from './models/Member';
import { MemberService } from './services/Member.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-member',
	templateUrl: './member.component.html',
	styleUrls: ['./member.component.scss']
})
export class MemberComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','createdDate','updatedDate','deletedDate','name','surname','role','email','phoneNumber','description','imageUrl','githubUrl','linkedinUrl','websiteUrl', 'update','delete'];

	memberList:Member[];
	member:Member=new Member();

	memberAddForm: FormGroup;


	memberId:number;

	constructor(private memberService:MemberService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getMemberList();
    }

	ngOnInit() {

		this.createMemberAddForm();
	}


	getMemberList() {
		this.memberService.getMemberList().subscribe(data => {
			this.memberList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.memberAddForm.valid) {
			this.member = Object.assign({}, this.memberAddForm.value)

			if (this.member.id == 0)
				this.addMember();
			else
				this.updateMember();
		}

	}

	addMember(){

		this.memberService.addMember(this.member).subscribe(data => {
			this.getMemberList();
			this.member = new Member();
			jQuery('#member').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.memberAddForm);

		})

	}

	updateMember(){

		this.memberService.updateMember(this.member).subscribe(data => {

			var index=this.memberList.findIndex(x=>x.id==this.member.id);
			this.memberList[index]=this.member;
			this.dataSource = new MatTableDataSource(this.memberList);
            this.configDataTable();
			this.member = new Member();
			jQuery('#member').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.memberAddForm);

		})

	}

	createMemberAddForm() {
		this.memberAddForm = this.formBuilder.group({		
			id : [0],
name : ["", Validators.required],
surname : ["", Validators.required],
role : ["", Validators.required],
email : ["", Validators.required],
phoneNumber : ["", Validators.required],
description : ["", Validators.required],
imageUrl : ["", Validators.required],
githubUrl : ["", Validators.required],
linkedinUrl : ["", Validators.required],
websiteUrl : ["", Validators.required]
		})
	}

	deleteMember(memberId:number){
		this.memberService.deleteMember(memberId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.memberList=this.memberList.filter(x=> x.id!=memberId);
			this.dataSource = new MatTableDataSource(this.memberList);
			this.configDataTable();
		})
	}

	getMemberById(memberId:number){
		this.clearFormGroup(this.memberAddForm);
		this.memberService.getMemberById(memberId).subscribe(data=>{
			this.member=data;
			this.memberAddForm.patchValue(data);
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

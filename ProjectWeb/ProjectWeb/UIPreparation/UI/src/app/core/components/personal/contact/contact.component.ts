import { Component, OnInit } from '@angular/core';
import { Contact } from '../../admin/contact/models/contact';
import { ContactService } from '../../admin/contact/services/contact.service';
import { FormGroup, Validators , FormBuilder, NgForm} from '@angular/forms';
import { ContactMessageService } from '../../admin/contactMessage/services/contactmessage.service';
import { ContactMessage } from '../../admin/contactMessage/models/contactmessage';
import { AlertifyService } from 'app/core/services/alertify.service';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit {


  constructor(private contactService :ContactService,private alertifyService:AlertifyService, private formBuilder: FormBuilder, private contactMessageService: ContactMessageService) { }
  contactList : Contact[];
  activeContact: Contact;
  contactForm: FormGroup;
  contact:ContactMessage=new ContactMessage();


  ngOnInit(): void {
    this.getContactList();
    this.createContactForm();
  }

  createContactForm() {
		this.contactForm = this.formBuilder.group({		
			id : [0],
			adress : ["", Validators.required],
			phoneNumber : ["", Validators.required],
			email : ["", Validators.required]
		})
	}

  addContact(){

		this.contactMessageService.addContactMessage(this.contact).subscribe(data => {
			this.getContactList();
			this.contact = new Contact();
			this.alertifyService.success(data);
			this.clearFormGroup(this.contactForm);

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

  save(){
    console.log(this.contactForm.value)

		if (this.contactForm.valid) {
			this.contact = Object.assign({}, this.contactForm.value)
			this.addContact();
		}

	}


  getContactList() {
		this.contactService.getContactList().subscribe(data => {
			this.contactList = data;
      this.activeContact = this.contactList.find(c => c.id=1)
		});
	}

}

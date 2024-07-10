import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Contact } from '../models/Contact';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class ContactService {

  constructor(private httpClient: HttpClient) { }


  getContactList(): Observable<Contact[]> {

    return this.httpClient.get<Contact[]>(environment.getApiUrl + '/contacts/getall')
  }

  getContactById(id: number): Observable<Contact> {
    return this.httpClient.get<Contact>(environment.getApiUrl + '/contacts/getbyid?id='+id)
  }

  addContact(contact: Contact): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/contacts/', contact, { responseType: 'text' });
  }

  updateContact(contact: Contact): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/contacts/', contact, { responseType: 'text' });

  }

  deleteContact(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/contacts/', { body: { id: id } });
  }


}
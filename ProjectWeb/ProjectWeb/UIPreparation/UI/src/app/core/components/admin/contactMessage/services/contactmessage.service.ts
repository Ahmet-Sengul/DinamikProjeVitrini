import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ContactMessage } from '../models/ContactMessage';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class ContactMessageService {

  constructor(private httpClient: HttpClient) { }


  getContactMessageList(): Observable<ContactMessage[]> {

    return this.httpClient.get<ContactMessage[]>(environment.getApiUrl + '/contactMessages/getall')
  }

  getContactMessageById(id: number): Observable<ContactMessage> {
    return this.httpClient.get<ContactMessage>(environment.getApiUrl + '/contactMessages/getbyid?id='+id)
  }

  addContactMessage(contactMessage: ContactMessage): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/contactMessages/', contactMessage, { responseType: 'text' });
  }

  updateContactMessage(contactMessage: ContactMessage): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/contactMessages/', contactMessage, { responseType: 'text' });

  }

  deleteContactMessage(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/contactMessages/', { body: { id: id } });
  }


}
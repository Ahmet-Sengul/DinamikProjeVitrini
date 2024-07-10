import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from '../models/Member';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class MemberService {

  constructor(private httpClient: HttpClient) { }


  getMemberList(): Observable<Member[]> {

    return this.httpClient.get<Member[]>(environment.getApiUrl + '/members/getall')
  }

  getMemberById(id: number): Observable<Member> {
    return this.httpClient.get<Member>(environment.getApiUrl + '/members/getbyid?id='+id)
  }

  addMember(member: Member): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/members/', member, { responseType: 'text' });
  }

  updateMember(member: Member): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/members/', member, { responseType: 'text' });

  }

  deleteMember(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/members/', { body: { id: id } });
  }


}
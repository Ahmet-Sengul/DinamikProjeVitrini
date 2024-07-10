import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { About } from '../models/about';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AboutService {

  constructor(private httpClient: HttpClient) { }


  getAboutList(): Observable<About[]> {

    return this.httpClient.get<About[]>(environment.getApiUrl + '/abouts/getall')
  }

  getAboutById(id: number): Observable<About> {
    return this.httpClient.get<About>(environment.getApiUrl + '/abouts/getbyid?id='+id)
  }

  addAbout(about: About): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/abouts/', about, { responseType: 'text' });
  }

  updateAbout(about: About): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/abouts/', about, { responseType: 'text' });

  }

  deleteAbout(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/abouts/', { body: { id: id } });
  }


}
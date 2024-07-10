import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HomePage } from '../models/HomePage';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class HomePageService {

  constructor(private httpClient: HttpClient) { }


  getHomePageList(): Observable<HomePage[]> {

    return this.httpClient.get<HomePage[]>(environment.getApiUrl + '/homePages/getall')
  }

  getHomePageById(id: number): Observable<HomePage> {
    return this.httpClient.get<HomePage>(environment.getApiUrl + '/homePages/getbyid?id='+id)
  }

  addHomePage(homePage: HomePage): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/homePages/', homePage, { responseType: 'text' });
  }

  updateHomePage(homePage: HomePage): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/homePages/', homePage, { responseType: 'text' });

  }

  deleteHomePage(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/homePages/', { body: { id: id } });
  }


}
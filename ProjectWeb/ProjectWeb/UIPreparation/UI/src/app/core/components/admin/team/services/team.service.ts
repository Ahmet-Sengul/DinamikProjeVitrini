import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Team } from '../models/Team';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class TeamService {

  constructor(private httpClient: HttpClient) { }


  getTeamList(): Observable<Team[]> {

    return this.httpClient.get<Team[]>(environment.getApiUrl + '/teams/getall')
  }

  getTeamById(id: number): Observable<Team> {
    return this.httpClient.get<Team>(environment.getApiUrl + '/teams/getbyid?id='+id)
  }

  addTeam(team: Team): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/teams/', team, { responseType: 'text' });
  }

  updateTeam(team: Team): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/teams/', team, { responseType: 'text' });

  }

  deleteTeam(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/teams/', { body: { id: id } });
  }


}
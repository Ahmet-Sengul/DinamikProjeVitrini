import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TeamRole } from '../models/TeamRole';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class TeamRoleService {

  constructor(private httpClient: HttpClient) { }


  getTeamRoleList(): Observable<TeamRole[]> {

    return this.httpClient.get<TeamRole[]>(environment.getApiUrl + '/teamRoles/getall')
  }

  getTeamRoleById(id: number): Observable<TeamRole> {
    return this.httpClient.get<TeamRole>(environment.getApiUrl + '/teamRoles/getbyid?id='+id)
  }

  addTeamRole(teamRole: TeamRole): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/teamRoles/', teamRole, { responseType: 'text' });
  }

  updateTeamRole(teamRole: TeamRole): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/teamRoles/', teamRole, { responseType: 'text' });

  }

  deleteTeamRole(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/teamRoles/', { body: { id: id } });
  }


}
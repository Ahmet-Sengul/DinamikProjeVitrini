import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Tecnology } from '../models/Tecnology';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class TecnologyService {

  constructor(private httpClient: HttpClient) { }


  getTecnologyList(): Observable<Tecnology[]> {

    return this.httpClient.get<Tecnology[]>(environment.getApiUrl + '/tecnologies/getall')
  }

  getTecnologyById(id: number): Observable<Tecnology> {
    return this.httpClient.get<Tecnology>(environment.getApiUrl + '/tecnologies/getbyid?id='+id)
  }

  addTecnology(tecnology: Tecnology): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/tecnologies/', tecnology, { responseType: 'text' });
  }

  updateTecnology(tecnology: Tecnology): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/tecnologies/', tecnology, { responseType: 'text' });

  }

  deleteTecnology(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/tecnologies/', { body: { id: id } });
  }


}
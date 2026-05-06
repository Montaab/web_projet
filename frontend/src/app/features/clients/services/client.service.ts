import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Client } from '../../../core/models/client.model';

@Injectable({ providedIn: 'root' })
export class ClientService {
  private url = `${environment.erpApi}/Client`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Client[]> { 
    return this.http.get<Client[]>(this.url); 
  }

  add(c: Client): Observable<Client> { 
    const payload = {
      nom: c.nom || "",
      prenom: c.prenom || "",
      email: c.email || "",
      adresse: c.adresse || "",
      telephone: c.telephone || ""
    };
    return this.http.post<Client>(this.url, payload); 
  }

  update(c: Client): Observable<void> { 
    const payload = {
      idClt: c.idClt,
      nom: c.nom || "",
      prenom: c.prenom || "",
      email: c.email || "",
      adresse: c.adresse || "",
      telephone: c.telephone || ""
    };
    return this.http.put<void>(`${this.url}/${c.idClt}`, payload); 
  }

  delete(id: number): Observable<void> { 
    return this.http.delete<void>(`${this.url}/${id}`); 
  }
}

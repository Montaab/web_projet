import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Souscategorie } from '../models/categorie.model';

@Injectable({ providedIn: 'root' })
export class SouscategorieService {
  private url = `${environment.erpApi}/Souscategorie`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Souscategorie[]> { 
    return this.http.get<Souscategorie[]>(this.url); 
  }

  getById(id: number): Observable<Souscategorie> {
    return this.http.get<Souscategorie>(`${this.url}/${id}`);
  }
}

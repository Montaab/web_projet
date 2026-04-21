import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Categorie } from '../models/categorie.model';

@Injectable({ providedIn: 'root' })
export class CategorieService {
  private url = `${environment.erpApi}/Categorie`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Categorie[]> { 
    return this.http.get<Categorie[]>(this.url); 
  }

  add(c: Categorie): Observable<Categorie> { 
    const payload = {
      CodeCat: c.codeCat,
      Libelle: c.libelle,
      Description: c.description
    };
    return this.http.post<Categorie>(this.url, payload); 
  }

  update(c: Categorie): Observable<void> { 
    const payload = {
      IdCat: c.idCat,
      CodeCat: c.codeCat,
      Libelle: c.libelle,
      Description: c.description
    };
    return this.http.put<void>(`${this.url}/${c.idCat}`, payload); 
  }

  delete(id: number): Observable<void> { 
    return this.http.delete<void>(`${this.url}/${id}`); 
  }
}

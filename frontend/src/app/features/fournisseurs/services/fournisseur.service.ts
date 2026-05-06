import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Fournisseur } from '../../../core/models/fournisseur.model';

@Injectable({ providedIn: 'root' })
export class FournisseurService {
  private url = `${environment.gateway}/ERP/Fournisseur`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Fournisseur[]> { 
    return this.http.get<Fournisseur[]>(this.url); 
  }

  getById(id: number): Observable<Fournisseur> {
    return this.http.get<Fournisseur>(`${this.url}/${id}`);
  }

  add(f: Fournisseur): Observable<Fournisseur> { 
    const payload = {
      nomSociete: f.nomSociete || "",
      tel: f.tel || "",
      email: f.email || "",
      adresse: f.adresse || "",
      ville: f.ville || ""
    };
    return this.http.post<Fournisseur>(this.url, payload); 
  }

  update(f: Fournisseur): Observable<void> { 
    const payload = {
      idFour: f.idFour,
      nomSociete: f.nomSociete || "",
      tel: f.tel || "",
      email: f.email || "",
      adresse: f.adresse || "",
      ville: f.ville || ""
    };
    return this.http.put<void>(`${this.url}/${f.idFour}`, payload); 
  }

  delete(id: number): Observable<void> { 
    return this.http.delete<void>(`${this.url}/${id}`); 
  }
}

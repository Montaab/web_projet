import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LFournisseur } from '../models/lfournisseur.model';

@Injectable({
  providedIn: 'root'
})
export class LFournisseurService {
  private apiUrl = `${environment.erpApi}/LFournisseur`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<LFournisseur[]> {
    return this.http.get<LFournisseur[]>(this.apiUrl);
  }

  get(fournisseurId: number, articleId: number): Observable<LFournisseur> {
    return this.http.get<LFournisseur>(`${this.apiUrl}/${fournisseurId}/${articleId}`);
  }

  create(lfournisseur: LFournisseur): Observable<LFournisseur> {
    return this.http.post<LFournisseur>(this.apiUrl, lfournisseur);
  }

  update(fournisseurId: number, articleId: number, lfournisseur: LFournisseur): Observable<LFournisseur> {
    return this.http.put<LFournisseur>(`${this.apiUrl}/${fournisseurId}/${articleId}`, lfournisseur);
  }

  delete(fournisseurId: number, articleId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${fournisseurId}/${articleId}`);
  }
}

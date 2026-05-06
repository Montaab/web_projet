import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { LCommande } from '../../../core/models/lcommande.model';

@Injectable({
  providedIn: 'root'
})
export class LCommandeService {
  private apiUrl = `${environment.erpApi}/LCommande`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<LCommande[]> {
    return this.http.get<LCommande[]>(this.apiUrl);
  }

  get(commandeId: number, articleId: number): Observable<LCommande> {
    return this.http.get<LCommande>(`${this.apiUrl}/${commandeId}/${articleId}`);
  }

  create(lcommande: LCommande): Observable<LCommande> {
    return this.http.post<LCommande>(this.apiUrl, lcommande);
  }

  update(commandeId: number, articleId: number, lcommande: LCommande): Observable<LCommande> {
    return this.http.put<LCommande>(`${this.apiUrl}/${commandeId}/${articleId}`, lcommande);
  }

  delete(commandeId: number, articleId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${commandeId}/${articleId}`);
  }
}

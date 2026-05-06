import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Facture } from '../../../core/models/facture.model';

@Injectable({ providedIn: 'root' })
export class FactureService {
  private url = `${environment.erpApi}/Facture`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Facture[]> { 
    return this.http.get<Facture[]>(this.url); 
  }

  add(f: Facture): Observable<Facture> { 
    const payload = {
      dateFact: f.dateFact,
      montantTotal: f.montantTotal,
      modePaiement: f.modePaiement,
      idCom: f.idCom
    };
    console.log('📤 JSON envoyé à POST /Facture:', JSON.stringify(payload, null, 2));
    return this.http.post<Facture>(this.url, payload); 
  }

  update(f: Facture): Observable<void> { 
    const payload = {
      idFact: f.idFact,
      dateFact: f.dateFact,
      montantTotal: f.montantTotal,
      modePaiement: f.modePaiement,
      idCom: f.idCom
    };
    console.log(`📤 JSON envoyé à PUT /Facture/${f.idFact}:`, JSON.stringify(payload, null, 2));
    return this.http.put<void>(`${this.url}/${f.idFact}`, payload); 
  }

  delete(id: number): Observable<void> { 
    return this.http.delete<void>(`${this.url}/${id}`); 
  }
}

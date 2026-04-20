import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Commande } from '../models/commande.model';

@Injectable({ providedIn: 'root' })
export class CommandeService {
  private url = `${environment.erpApi}/Commande`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Commande[]> { 
    return this.http.get<Commande[]>(this.url); 
  }

  getById(id: number): Observable<Commande> {
    return this.http.get<Commande>(`${this.url}/${id}`);
  }

  add(c: Commande): Observable<Commande> { 
    const payload = {
      dateCom: c.dateCom,
      statut: c.statut,
      modePaiement: c.modePaiement,
      idClt: c.idClt,
      total: c.total,
      lCommandes: c.lCommandes?.map(l => ({
        idArt: l.idArt,
        quantite: l.quantite,
        prixAchat: l.prixAchat,
        remise: l.remise || 0
      })) || []
    };
    return this.http.post<Commande>(this.url, payload); 
  }

  update(c: Commande): Observable<void> { 
    const payload = {
      idCom: c.idCom,
      dateCom: c.dateCom,
      statut: c.statut,
      modePaiement: c.modePaiement,
      idClt: c.idClt,
      total: c.total,
      lCommandes: c.lCommandes?.map(l => ({
        idCom: c.idCom,
        idArt: l.idArt,
        quantite: l.quantite,
        prixAchat: l.prixAchat,
        remise: l.remise || 0
      })) || []
    };
    return this.http.put<void>(`${this.url}/${c.idCom}`, payload); 
  }

  delete(id: number): Observable<void> { 
    return this.http.delete<void>(`${this.url}/${id}`); 
  }
}

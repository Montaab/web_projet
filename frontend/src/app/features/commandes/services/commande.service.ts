import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Commande } from '../../../core/models/commande.model';

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
      idClt: Number(c.idClt),
      total: Number(c.total),
      lCommandes: c.lCommandes?.map(l => ({
        idArt: Number(l.idArt),
        quantite: Number(l.quantite),
        prixAchat: Number(l.prixAchat),
        remise: Number(l.remise || 0)
      })) || []
    };
    return this.http.post<Commande>(this.url, payload); 
  }

  update(c: Commande): Observable<void> { 
    const payload = {
      idCom: Number(c.idCom),
      dateCom: c.dateCom,
      statut: c.statut,
      modePaiement: c.modePaiement,
      idClt: Number(c.idClt),
      total: Number(c.total),
      lCommandes: c.lCommandes?.map(l => ({
        idCom: Number(c.idCom),
        idArt: Number(l.idArt),
        quantite: Number(l.quantite),
        prixAchat: Number(l.prixAchat),
        remise: Number(l.remise || 0)
      })) || []
    };
    return this.http.put<void>(`${this.url}/${c.idCom}`, payload); 
  }

  delete(id: number): Observable<void> { 
    return this.http.delete<void>(`${this.url}/${id}`); 
  }
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Article } from '../models/article.model';

@Injectable({ providedIn: 'root' })
export class ArticleService {
  private url = `${environment.erpApi}/Article`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Article[]> { 
    return this.http.get<Article[]>(this.url); 
  }

  add(a: Article): Observable<Article> { 
    const payload = {
      designation: a.designation,
      description: a.description || "",
      prixUnitaire: a.prixUnitaire,
      stockDispo: a.stockDispo,
      imageUrl: a.imageUrl || "", // Correction : champ obligatoire
      idScat: a.idScat
    };
    return this.http.post<Article>(this.url, payload); 
  }

  update(a: Article): Observable<void> { 
    const payload = {
      idArt: a.idArt,
      designation: a.designation,
      description: a.description || "",
      prixUnitaire: a.prixUnitaire,
      stockDispo: a.stockDispo,
      imageUrl: a.imageUrl || "", // Correction : champ obligatoire
      idScat: a.idScat
    };
    return this.http.put<void>(`${this.url}/${a.idArt}`, payload); 
  }

  delete(id: number): Observable<void> { 
    return this.http.delete<void>(`${this.url}/${id}`); 
  }
}

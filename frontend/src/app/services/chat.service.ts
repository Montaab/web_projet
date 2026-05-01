import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable, throwError } from 'rxjs'; // Ajout de throwError
import { timeout, catchError } from 'rxjs/operators';

// On définit une interface pour la réponse du backend .NET
interface ChatResponse {
  answer: string;
  data?: any[]; // On ajoute le champ data optionnel
}

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private apiUrl = 'http://localhost:5271/api/chat/ask';

  constructor(private http: HttpClient) { }

  askGemini(userMessage: string): Observable<ChatResponse> {
    const body = { message: userMessage };

    // On précise à HttpClient que la réponse ressemblera à ChatResponse
    return this.http.post<ChatResponse>(this.apiUrl, body).pipe(
      timeout(15000), // Si pas de réponse après 15 sec, on annule
      catchError(err => {
        console.error('Erreur technique service:', err);
        return throwError(() => err);
      })
    );
  }

}
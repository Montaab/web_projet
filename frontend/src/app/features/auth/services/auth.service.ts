import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ResponseLogin } from '../../../core/models/auth.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly TOKEN_KEY = 'access_token';
  private readonly USER_KEY  = 'current_user';

  constructor(private http: HttpClient, private router: Router) {}

  login(credentials: any): Observable<ResponseLogin> {
    return this.http.post<ResponseLogin>(`${environment.userApi}/User/IsLogin`, credentials).pipe(
      tap(res => {
        if (res && res.accessToken) {
          localStorage.setItem(this.TOKEN_KEY, res.accessToken);
          localStorage.setItem(this.USER_KEY, JSON.stringify(res));
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  getCurrentUser(): ResponseLogin | null {
    const u = localStorage.getItem(this.USER_KEY);
    return u ? JSON.parse(u) : null;
  }

  isLoggedIn(): boolean {
  const token = this.getToken();
  if (!token) return false;

  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    const exp = payload.exp;

    const now = Math.floor(Date.now() / 1000);

    if (exp < now) {
      this.logout(); // 🔥 supprime + redirect
      return false;
    }

    return true;
  } catch (e) {
    this.logout(); // token invalide
    return false;
  }
}
}

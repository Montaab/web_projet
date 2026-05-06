import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { User } from '../../../core/models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = `${environment.gateway}/User`;

  constructor(private http: HttpClient) {}

  private toDto(user: User): any {
    const dto: any = {
      Iduser: user.idUser,
      Nom: user.username,
      Username: user.username,
      Email: user.email,
      Telephone: user.telephone ?? '',
      Idrole: user.idrole
    };

    if (user.password) {
      dto.Motpass = user.password;
    }

    return dto;
  }

  getAll(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}`);
  }

  getById(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  create(user: User): Observable<User> {
    return this.http.post<User>(`${this.apiUrl}`, this.toDto(user));
  }

  update(id: number, user: User): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/${id}`, this.toDto(user));
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}

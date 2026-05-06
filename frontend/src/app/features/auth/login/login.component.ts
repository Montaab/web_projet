import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username = '';
  password = '';
  loading  = false;
  error    = '';
  showPass = false;

  constructor(private auth: AuthService, private router: Router) {
    if (this.auth.isLoggedIn()) this.router.navigate(['/dashboard']);
  }

  login() {
    if (!this.username || !this.password) { this.error = 'Veuillez remplir tous les champs.'; return; }
    this.loading = true; this.error = '';
    this.auth.login({ Username: this.username, Password: this.password }).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: () => { this.error = 'Identifiants incorrects. Vérifiez votre username/mot de passe.'; this.loading = false; }
    });
  }
}

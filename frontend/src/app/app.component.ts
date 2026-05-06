import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { ChatbotComponent } from './components/chatbot/chatbot.component';
import { AuthService } from './features/auth/services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, ChatbotComponent],
  templateUrl: './app.component.html' // On pointe vers le fichier HTML
})
export class AppComponent {
  title = 'ERPs'; // Assure-toi d'avoir cette variable si ton HTML l'utilise
  authService = inject(AuthService);
}
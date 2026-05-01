import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ChatbotComponent } from './components/chatbot/chatbot.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ChatbotComponent],
  templateUrl: './app.component.html' // On pointe vers le fichier HTML
})
export class AppComponent {
  title = 'ERPs'; // Assure-toi d'avoir cette variable si ton HTML l'utilise
}
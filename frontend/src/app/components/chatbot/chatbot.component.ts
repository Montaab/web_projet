import { Component } from '@angular/core';
import { ChatService } from '../../services/chat.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

// 1. On définit une interface propre pour le message
interface Message {
  text: string;
  isUser: boolean;
  data?: any[]; // On ajoute le champ data optionnel pour stocker les résultats SQL
}

@Component({
  selector: 'app-chatbot',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './chatbot.component.html',
  styleUrls: ['./chatbot.component.css']
})
export class ChatbotComponent {
  userInput: string = '';
  // 2. On utilise l'interface Message ici
  messages: Message[] = [];
  isLoading: boolean = false;
  isChatVisible: boolean = false;

  constructor(private chatService: ChatService) { }

  toggleChat() {
    this.isChatVisible = !this.isChatVisible;
  }

  // Fonction pour extraire les colonnes du tableau
  getKeys(obj: any): string[] {
    return obj ? Object.keys(obj) : [];
  }

  sendMessage() {
    if (!this.userInput.trim() || this.isLoading) return;

    const userText = this.userInput;

    // Ajout du message utilisateur
    this.messages.push({ text: userText, isUser: true });

    this.userInput = '';
    this.isLoading = true;

    this.chatService.askGemini(userText).subscribe({
      next: (res) => {
        // 3. ON AJOUTE RES.DATA ICI pour que le HTML puisse l'afficher
        this.messages.push({
          text: res.answer,
          isUser: false,
          data: res.data // C'est ici que la magie opère
        });
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Erreur:', err);
        this.messages.push({
          text: "Désolé, je rencontre des difficultés techniques.",
          isUser: false
        });
        this.isLoading = false;
      }
    });
  }
}
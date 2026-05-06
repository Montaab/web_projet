import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ArticleService } from '../articles/services/article.service';
import { ClientService } from '../clients/services/client.service';
import { CommandeService } from '../commandes/services/commande.service';
import { FournisseurService } from '../fournisseurs/services/fournisseur.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  stats = {
    articles: 0,
    clients: 0,
    commandes: 0,
    fournisseurs: 0
  };
  loading = true;
  recentCommandes: any[] = [];
  String = String; // Make String available in template

  constructor(
    private articleService: ArticleService,
    private clientService: ClientService,
    private commandeService: CommandeService,
    private fournisseurService: FournisseurService
  ) {}

  ngOnInit(): void {
    this.loadStats();
  }

  loadStats() {
    forkJoin({
      articles: this.articleService.getAll(),
      clients: this.clientService.getAll(),
      commandes: this.commandeService.getAll(),
      fournisseurs: this.fournisseurService.getAll()
    }).subscribe({
      next: (res) => {
        this.stats.articles = res.articles.length;
        this.stats.clients = res.clients.length;
        this.stats.commandes = res.commandes.length;
        this.stats.fournisseurs = res.fournisseurs.length;
        this.recentCommandes = res.commandes.slice(0, 5).sort((a: any, b: any) => (b.idCom || 0) - (a.idCom || 0));
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }
}

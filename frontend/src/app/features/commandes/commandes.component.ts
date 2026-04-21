import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { CommandeService } from '../../core/services/commande.service';
import { ClientService } from '../../core/services/client.service';
import { ArticleService } from '../../core/services/article.service';
import { Commande } from '../../core/models/commande.model';
import { Client } from '../../core/models/client.model';
import { Article } from '../../core/models/article.model';
import { LCommande } from '../../core/models/lcommande.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-commandes',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './commandes.component.html',
  styleUrls: ['./commandes.component.css']
})
export class CommandesComponent implements OnInit {
  commandes: Commande[] = [];
  clients: Client[] = [];
  articles: Article[] = [];
  loading = true;
  showModal = false;
  editMode = false;

  // Gestion de l'expansion des lignes
  expandedRows = new Set<number>();
  loadingDetails = new Set<number>();
  detailsCache = new Map<number, Commande>();

  currentCom: Commande = this.initNewCommande();

  constructor(
    private service: CommandeService,
    private clientService: ClientService,
    private articleService: ArticleService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.load();
    this.clientService.getAll().subscribe(data => this.clients = data);
    this.articleService.getAll().subscribe(data => {
      this.articles = data;
      this.refreshCurrentLinePrices();
    });
    
    this.route.queryParams.subscribe(params => {
      if (params['action'] === 'add') this.openAdd();
    });
  }

  initNewCommande(): Commande {
    return {
      dateCom: new Date().toISOString().split('T')[0],
      statut: 'En attente',
      modePaiement: 'Especes',
      idClt: 0,
      total: 0,
      lCommandes: []
    };
  }

  // --- Gestion de l'expansion ---
  toggleExpand(com: Commande): void {
    if (!com.idCom) return;
    const id = com.idCom;

    if (this.expandedRows.has(id)) {
      this.expandedRows.delete(id);
      return;
    }

    this.expandedRows.add(id);

    // Si déjà en cache ou si les lignes sont déjà chargées, on ne refait pas l'appel
    if (this.detailsCache.has(id) || (com.lCommandes && com.lCommandes.length > 0)) {
      return;
    }

    this.loadingDetails.add(id);
    this.service.getById(id).subscribe({
      next: (detail) => {
        this.detailsCache.set(id, detail);
        // Mettre à jour la commande dans la liste principale
        const idx = this.commandes.findIndex(c => c.idCom === id);
        if (idx !== -1) {
          this.commandes[idx] = { ...this.commandes[idx], lCommandes: detail.lCommandes };
        }
        this.loadingDetails.delete(id);
      },
      error: () => this.loadingDetails.delete(id)
    });
  }

  isExpanded(id: number | undefined): boolean {
    return !!id && this.expandedRows.has(id);
  }

  isLoadingDetail(id: number | undefined): boolean {
    return !!id && this.loadingDetails.has(id);
  }

  getLineSousTotal(line: LCommande): number {
    return (line.prixAchat * line.quantite) * (1 - (line.remise || 0) / 100);
  }

  load(): void {
    this.loading = true;
    this.service.getAll().subscribe({
      next: (data) => {
        this.commandes = data;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  openAdd(): void {
    this.editMode = false;
    this.currentCom = this.initNewCommande();
    if (this.clients.length > 0) this.currentCom.idClt = this.clients[0].idClt ?? 0;
    this.showModal = true;
  }

  openEdit(c: Commande): void {
    this.editMode = true;
    this.currentCom = {
      ...c,
      lCommandes: (c.lCommandes || []).map(line => ({ ...line }))
    };
    this.showModal = true;
  }

  // --- Gestion des lignes de commande ---
  addLine(): void {
    if (this.articles.length === 0) return;
    const firstArt = this.articles[0];
    const newLine: LCommande = {
      idArt: firstArt.idArt || 0,
      quantite: 1,
      prixAchat: firstArt.prixUnitaire || 0,
      remise: 0
    };
    this.currentCom.lCommandes = [...(this.currentCom.lCommandes || []), newLine];
    this.calculateTotal();
  }

  removeLine(index: number): void {
    this.currentCom.lCommandes?.splice(index, 1);
    this.calculateTotal();
  }

  onArticleChange(line: LCommande): void {
    line.idArt = Number(line.idArt);
    line.prixAchat = this.getArticlePrice(line.idArt);
    this.calculateTotal();
  }

  getArticlePrice(articleId: number): number {
    const art = this.articles.find(a => a.idArt == articleId);
    return art?.prixUnitaire || 0;
  }

  refreshCurrentLinePrices(force = false): void {
    if (this.editMode && !force) {
      return;
    }

    this.currentCom.lCommandes?.forEach(line => {
      line.prixAchat = this.getArticlePrice(line.idArt) || line.prixAchat || 0;
    });
    this.calculateTotal();
  }

  calculateTotal(): void {
    let total = 0;
    this.currentCom.lCommandes?.forEach(l => {
      total += (l.prixAchat * l.quantite) * (1 - (l.remise || 0) / 100);
    });
    this.currentCom.total = total;
  }

  save(): void {
    if (!this.currentCom.idClt) return alert('Veuillez sélectionner un client');
    if (!this.currentCom.lCommandes?.length) return alert('Veuillez ajouter au moins un article');

    this.refreshCurrentLinePrices(true);

    const obs: Observable<any> = this.editMode 
      ? this.service.update(this.currentCom)
      : this.service.add(this.currentCom);

    obs.subscribe({
      next: () => {
        this.showModal = false;
        this.load();
      },
      error: () => alert('Erreur lors de l\'enregistrement')
    });
  }

  showDeleteModal = false;
  itemToDelete: number | undefined = undefined;

  deleteCom(id: number | undefined): void {
    if (id) {
      this.itemToDelete = id;
      this.showDeleteModal = true;
    }
  }

  confirmDelete(): void {
    if (this.itemToDelete) {
      this.service.delete(this.itemToDelete).subscribe({
        next: () => {
          this.showDeleteModal = false;
          this.itemToDelete = undefined;
          this.load();
        },
        error: () => {
          this.showDeleteModal = false;
          alert('Impossible de supprimer cette commande.');
        }
      });
    }
  }

  getClientName(id: number): string {
    const c = this.clients.find(x => x.idClt === id);
    return c ? `${c.nom} ${c.prenom}` : 'Client #' + id;
  }

  getArticleName(id: number): string {
    const a = this.articles.find(x => x.idArt === id);
    return a ? a.designation : 'Article #' + id;
  }

  getStatusClass(status: string | undefined): string {
    if (!status) return 'badge-info';
    const s = status.toLowerCase();
    if (s.includes('valid') || s.includes('payé') || s.includes('livr')) return 'badge-success';
    if (s.includes('attente') || s.includes('cours')) return 'badge-warning';
    if (s.includes('annul')) return 'badge-danger';
    return 'badge-info';
  }
}

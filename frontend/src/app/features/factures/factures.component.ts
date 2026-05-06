import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FactureService } from './services/facture.service';
import { CommandeService } from '../commandes/services/commande.service';
import { ClientService } from '../clients/services/client.service';
import { ArticleService } from '../articles/services/article.service';
import { Facture } from '../../core/models/facture.model';
import { Commande } from '../../core/models/commande.model';
import { Client } from '../../core/models/client.model';
import { Article } from '../../core/models/article.model';
import { LCommande } from '../../core/models/lcommande.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-factures',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './factures.component.html',
  styleUrls: ['./factures.component.css']
})
export class FacturesComponent implements OnInit {
  factures: Facture[] = [];
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

  currentFact: Facture = {
    dateFact: new Date().toISOString().split('T')[0],
    montantHt: 0,
    montantTotal: null as any,
    modePaiement: 'Carte bancaire',
    idCom: 0
  };

  constructor(
    private service: FactureService,
    private comService: CommandeService,
    private clientService: ClientService,
    private articleService: ArticleService
  ) {}

  ngOnInit(): void {
    this.load();
    this.comService.getAll().subscribe(data => this.commandes = data);
    this.clientService.getAll().subscribe(data => this.clients = data);
    this.articleService.getAll().subscribe(data => this.articles = data);
  }

  load(): void {
    this.loading = true;
    this.service.getAll().subscribe({
      next: (data) => {
        this.factures = data;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  // ── Expansion des lignes ────────────────────────────────────────
  toggleExpand(fact: Facture): void {
    if (!fact.idFact) return;
    const id = fact.idFact;

    if (this.expandedRows.has(id)) {
      this.expandedRows.delete(id);
      return;
    }

    this.expandedRows.add(id);

    // Chercher la commande liée dans le cache ou la liste
    const cachedCom = this.detailsCache.get(fact.idCom);
    if (cachedCom) return;

    const existingCom = this.commandes.find(c => c.idCom === fact.idCom);
    if (existingCom?.lCommandes && existingCom.lCommandes.length > 0) {
      this.detailsCache.set(fact.idCom, existingCom);
      return;
    }

    // Charger depuis l'API
    this.loadingDetails.add(id);
    this.comService.getById(fact.idCom).subscribe({
      next: (com) => {
        this.detailsCache.set(fact.idCom, com);
        // Mettre à jour la commande dans la liste locale
        const idx = this.commandes.findIndex(c => c.idCom === com.idCom);
        if (idx !== -1) {
          this.commandes[idx] = com;
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

  getCommandeDetail(idCom: number): Commande | undefined {
    return this.detailsCache.get(idCom) ?? this.commandes.find(c => c.idCom === idCom);
  }

  getLineSousTotal(line: LCommande): number {
    return (line.prixAchat * line.quantite) - (line.remise || 0);
  }

  getClientName(idClt: number): string {
    const c = this.clients.find(x => x.idClt === idClt);
    return c ? `${c.nom} ${c.prenom}` : 'Client #' + idClt;
  }

  getArticleName(idArt: number): string {
    const a = this.articles.find(x => x.idArt === idArt);
    return a ? a.designation : 'Article #' + idArt;
  }

  getPaymentClass(mode: string): string {
    if (!mode) return 'badge-info';
    const m = mode.toLowerCase();
    if (m.includes('carte')) return 'badge-success';
    if (m.includes('virement')) return 'badge-warning';
    return 'badge-info';
  }

  // ── Modal ───────────────────────────────────────────────────────
  openAdd(): void {
    this.editMode = false;
    const firstCom = this.commandes[0];
    this.currentFact = {
      dateFact: new Date().toISOString().split('T')[0],
      montantHt: 0,
      montantTotal: firstCom?.total || 0,
      modePaiement: 'Carte bancaire',
      idCom: firstCom?.idCom || 0
    };
    this.showModal = true;
  }

  openEdit(f: Facture): void {
    this.editMode = true;
    this.currentFact = { ...f };
    this.showModal = true;
  }

  onCommandeChange(): void {
    const com = this.commandes.find(c => c.idCom == this.currentFact.idCom);
    if (com) {
      this.currentFact.montantTotal = com.total;
    }
  }

  save(): void {
    if (!this.currentFact.idCom || this.currentFact.idCom === 0) {
      alert('Veuillez sélectionner une commande');
      return;
    }
    if (!this.currentFact.montantTotal || this.currentFact.montantTotal <= 0) {
      alert('Veuillez saisir un montant total valide');
      return;
    }
    if (!this.currentFact.dateFact) {
      alert('Veuillez saisir une date valide');
      return;
    }

    const obs: Observable<any> = this.editMode
      ? this.service.update(this.currentFact)
      : this.service.add(this.currentFact);

    obs.subscribe({
      next: () => {
        this.showModal = false;
        this.load();
      },
      error: (err) => {
        alert('Erreur : ' + (err.error?.message || 'Vérifiez les champs'));
      }
    });
  }

  deleteFact(id: number | undefined): void {
    if (id && confirm('Supprimer cette facture ?')) {
      this.service.delete(id).subscribe(() => this.load());
    }
  }
}

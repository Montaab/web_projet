import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
import { FournisseurService } from '../../core/services/fournisseur.service';
import { Fournisseur } from '../../core/models/fournisseur.model';
import { LFournisseur } from '../../core/models/lfournisseur.model';

@Component({
  selector: 'app-fournisseurs',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './fournisseurs.component.html',
  styleUrls: ['./fournisseurs.component.css']
})
export class FournisseursComponent implements OnInit {
  fournisseurs: Fournisseur[] = [];
  loading = true;
  showModal = false;
  editMode = false;
  showDeleteModal = false;
  itemToDelete: number | undefined = undefined;

  // Gestion de l'expansion des lignes
  expandedRows = new Set<number>();
  loadingDetails = new Set<number>();

  currentFourn: Fournisseur = {
    nomSociete: '',
    tel: '',
    email: '',
    adresse: '',
    ville: ''
  };

  constructor(private service: FournisseurService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;

    this.service.getAll().subscribe({
      next: (data) => {
        // 🔥 sécurisation importante
        this.fournisseurs = (data || []).map(f => ({
          ...f,
          lFournisseurs: f.lFournisseurs ?? []
        }));

        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  // ── Expansion des lignes ────────────────────────────────────────
  toggleExpand(f: Fournisseur): void {
    if (!f.idFour) return;
    const id = f.idFour;

    if (this.expandedRows.has(id)) {
      this.expandedRows.delete(id);
      return;
    }

    this.expandedRows.add(id);

    // Si les articles sont déjà chargés AVEC navigation (description, stock, prix), pas de requête
    const alreadyLoaded = f.lFournisseurs &&
      f.lFournisseurs.length > 0 &&
      f.lFournisseurs[0].idArtNavigation != null;
    if (alreadyLoaded) return;

    // Charger depuis l'API
    this.loadingDetails.add(id);
    this.service.getById(id).subscribe({
      next: (detail) => {
        const idx = this.fournisseurs.findIndex(x => x.idFour === id);
        if (idx !== -1) {
          this.fournisseurs[idx] = { ...this.fournisseurs[idx], lFournisseurs: detail.lFournisseurs };
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

  getArticleName(line: LFournisseur): string {
    return line.idArtNavigation?.designation ?? `Article #${line.idArt}`;
  }

  getArticlePrice(line: LFournisseur): number {
    return line.idArtNavigation?.prixUnitaire ?? 0;
  }

  getStock(line: LFournisseur): number {
    return line.idArtNavigation?.stockDispo ?? 0;
  }

  getPrixFournisseur(line: LFournisseur): number {
    return line.prixFournisseur ?? line.prixFour ?? 0;
  }

  getDelai(line: LFournisseur): number {
    return line.delaiLivraison ?? line.delaiLivraisonJours ?? 0;
  }

  getMargePercent(line: LFournisseur): string {
    const prixVente = line.idArtNavigation?.prixUnitaire ?? 0;
    const prixAchat = this.getPrixFournisseur(line);
    if (prixAchat === 0) return '—';
    const marge = ((prixVente - prixAchat) / prixVente) * 100;
    return marge.toFixed(1) + '%';
  }

  getMargeClass(line: LFournisseur): string {
    const prixVente = line.idArtNavigation?.prixUnitaire ?? 0;
    const prixAchat = this.getPrixFournisseur(line);
    if (prixAchat === 0) return '';
    const marge = ((prixVente - prixAchat) / prixVente) * 100;
    if (marge >= 30) return 'marge-high';
    if (marge >= 15) return 'marge-mid';
    return 'marge-low';
  }

  // ── Modal ───────────────────────────────────────────────────────
  openAdd(): void {
    this.editMode = false;
    this.currentFourn = { nomSociete: '', tel: '', email: '', adresse: '', ville: '' };
    this.showModal = true;
  }

  openEdit(f: Fournisseur): void {
    this.editMode = true;
    this.currentFourn = { ...f };
    this.showModal = true;
  }

  save(): void {
    const obs: Observable<any> = this.editMode
      ? this.service.update(this.currentFourn)
      : this.service.add(this.currentFourn);

    obs.subscribe({
      next: () => {
        this.showModal = false;
        this.load();
      },
      error: (err) => console.error('Erreur lors de la sauvegarde', err)
    });
  }

  delete(id: number | undefined): void {
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
        error: () => this.showDeleteModal = false
      });
    }
  }
}

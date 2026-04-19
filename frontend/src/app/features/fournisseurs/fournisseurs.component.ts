import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Observable } from 'rxjs'; // Ajout de l'import manquant
import { FournisseurService } from '../../core/services/fournisseur.service';
import { Fournisseur } from '../../core/models/fournisseur.model';

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
        this.fournisseurs = data;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

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
    // On utilise any pour éviter le conflit entre Observable<void> et Observable<Fournisseur>
    const obs: Observable<any> = this.editMode 
      ? this.service.update(this.currentFourn) 
      : this.service.add(this.currentFourn);

    obs.subscribe({
      next: () => {
        this.showModal = false;
        this.load();
      },
      error: (err) => {
        console.error('Erreur lors de la sauvegarde', err);
      }
    });
  }

  showDeleteModal = false;
  itemToDelete: number | undefined = undefined;

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

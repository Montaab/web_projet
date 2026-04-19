import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FactureService } from '../../core/services/facture.service';
import { CommandeService } from '../../core/services/commande.service';
import { Facture } from '../../core/models/facture.model';
import { Commande } from '../../core/models/commande.model';
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
  loading = true;
  showModal = false;
  editMode = false;

  currentFact: Facture = {
    dateFact: new Date().toISOString().split('T')[0],
    montantHt: 0,
    montantTotal: 0,
    modePaiement: 'Carte bancaire',
    idCom: 0
  };

  constructor(
    private service: FactureService,
    private comService: CommandeService
  ) {}

  ngOnInit(): void {
    this.load();
    this.comService.getAll().subscribe(data => this.commandes = data);
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

  openAdd(): void {
    this.editMode = false;
    this.currentFact = {
      dateFact: new Date().toISOString().split('T')[0],
      montantHt: 0,
      montantTotal: 0,
      modePaiement: 'Carte bancaire',
      idCom: this.commandes[0]?.idCom || 0
    };
    this.showModal = true;
  }

  openEdit(f: Facture): void {
    this.editMode = true;
    this.currentFact = { ...f };
    this.showModal = true;
    console.log('Editing facture:', this.currentFact);
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

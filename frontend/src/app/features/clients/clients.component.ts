import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { ClientService } from '../../core/services/client.service';
import { Client } from '../../core/models/client.model';

@Component({
  selector: 'app-clients',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.css']
})
export class ClientsComponent implements OnInit {
  clients: Client[] = [];
  loading = true;
  showModal = false;
  editMode = false;

  currentClient: Client = {
    nom: '',
    prenom: '',
    adresse: '',
    email: '',
    telephone: ''
  };

  constructor(
    private clientService: ClientService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadClients();
    this.route.queryParams.subscribe(params => {
      if (params['action'] === 'add') this.openAdd();
    });
  }

  loadClients(): void {
    this.loading = true;
    this.clientService.getAll().subscribe({
      next: (data) => {
        this.clients = data;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  openAdd(): void {
    this.editMode = false;
    this.currentClient = { nom: '', prenom: '', adresse: '', email: '', telephone: '' };
    this.showModal = true;
  }

  openEdit(client: Client): void {
    this.editMode = true;
    this.currentClient = { ...client };
    this.showModal = true;
  }

  save(): void {
    const obs: Observable<any> = this.editMode 
      ? this.clientService.update(this.currentClient)
      : this.clientService.add(this.currentClient);

    obs.subscribe({
      next: () => {
        this.showModal = false;
        this.loadClients();
      }
    });
  }

  showDeleteModal = false;
  itemToDelete: number | undefined = undefined;

  deleteClient(id: number | undefined): void {
    if (id) {
      this.itemToDelete = id;
      this.showDeleteModal = true;
    }
  }

  confirmDelete(): void {
    if (this.itemToDelete) {
      this.clientService.delete(this.itemToDelete).subscribe({
        next: () => {
          this.showDeleteModal = false;
          this.itemToDelete = undefined;
          this.loadClients();
        },
        error: () => this.showDeleteModal = false
      });
    }
  }
}

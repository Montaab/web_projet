import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RoleService } from './services/role.service';
import { Role } from '../../core/models/role.model';

@Component({
  selector: 'app-roles',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.css']
})
export class RolesComponent implements OnInit {
  roles: Role[] = [];
  loading = true;
  showModal = false;
  editMode = false;

  currentRole: Role = {
    nom: '',
    description: ''
  };

  showDeleteModal = false;
  itemToDelete: number | undefined = undefined;

  constructor(private roleService: RoleService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.roleService.getAll().subscribe({
      next: (data) => {
        this.roles = data;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  openAdd(): void {
    this.editMode = false;
    this.currentRole = { nom: '', description: '' };
    this.showModal = true;
  }

  openEdit(role: Role): void {
    this.editMode = true;
    this.currentRole = { ...role };
    this.showModal = true;
  }

  save(): void {
    if (!this.currentRole.nom) {
      alert('Le nom du rôle est obligatoire.');
      return;
    }

    const request: import('rxjs').Observable<Role | void> = this.editMode && this.currentRole.idrole
      ? this.roleService.update(this.currentRole.idrole, this.currentRole)
      : this.roleService.create(this.currentRole);

    request.subscribe({
      next: () => {
        this.showModal = false;
        this.loadData();
      },
      error: (err: any) => {
        console.error('Erreur Role API:', err);
        alert('Erreur lors de l\'enregistrement du rôle.');
      }
    });
  }

  deleteRole(id: number | undefined): void {
    if (id) {
      this.itemToDelete = id;
      this.showDeleteModal = true;
    }
  }

  confirmDelete(): void {
    if (this.itemToDelete) {
      this.roleService.delete(this.itemToDelete).subscribe({
        next: () => {
          this.showDeleteModal = false;
          this.itemToDelete = undefined;
          this.loadData();
        },
        error: () => {
          alert('Erreur lors de la suppression du rôle.');
          this.showDeleteModal = false;
        }
      });
    }
  }
}

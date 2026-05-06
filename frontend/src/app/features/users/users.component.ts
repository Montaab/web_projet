import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from './services/user.service';
import { User } from '../../core/models/user.model';
import { RoleService } from '../roles/services/role.service';
import { Role } from '../../core/models/role.model';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: User[] = [];
  roles: Role[] = [];
  loading = true;
  showModal = false;
  editMode = false;

  currentUser: User = {
    username: '',
    email: '',
    password: '',
    telephone: '',
    idrole: undefined,
    role: '',
    isActive: true
  };

  constructor(
    private userService: UserService,
    private roleService: RoleService
  ) {}

  ngOnInit(): void {
    this.loadData();
    this.loadRoles();
  }

  loadData(): void {
    this.loading = true;
    this.userService.getAll().subscribe({
      next: (data) => {
        this.users = data.map(user => ({
          ...user,
          role: user.role ?? user.idroleNavigation?.nom
        }));
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  loadRoles(): void {
    this.roleService.getAll().subscribe({
      next: (data) => {
        this.roles = data;
      },
      error: (err) => {
        console.error('Erreur récupération des rôles :', err);
      }
    });
  }

  openAdd(): void {
    this.editMode = false;
    this.currentUser = {
      username: '',
      email: '',
      password: '',
      telephone: '',
      idrole: this.roles.length ? this.roles[0].idrole : undefined,
      role: this.roles.length ? this.roles[0].nom : '',
      isActive: true
    };
    this.showModal = true;
  }

  openEdit(user: User): void {
    this.editMode = true;
    this.currentUser = {
      ...user,
      role: user.role ?? user.idroleNavigation?.nom
    };
    this.showModal = true;
  }

  save(): void {
    if (!this.currentUser.username || !this.currentUser.email) {
      alert('Le nom d\'utilisateur et l\'email sont obligatoires');
      return;
    }

    const request = this.editMode && this.currentUser.idUser
      ? this.userService.update(this.currentUser.idUser, this.currentUser)
      : this.userService.create(this.currentUser);

    request.subscribe({
      next: () => {
        this.showModal = false;
        this.loadData();
      },
      error: (err) => {
        console.error('Erreur API:', err);
        alert('Erreur lors de l\'enregistrement : ' + (err.error?.message || err.message));
      }
    });
  }

  showDeleteModal = false;
  itemToDelete: number | undefined = undefined;

  deleteUser(id: number | undefined): void {
    if (id) {
      this.itemToDelete = id;
      this.showDeleteModal = true;
    }
  }

  confirmDelete(): void {
    if (this.itemToDelete) {
      this.userService.delete(this.itemToDelete).subscribe({
        next: () => {
          this.showDeleteModal = false;
          this.itemToDelete = undefined;
          this.loadData();
        },
        error: (err) => {
          alert('Erreur lors de la suppression');
          this.showDeleteModal = false;
        }
      });
    }
  }
}

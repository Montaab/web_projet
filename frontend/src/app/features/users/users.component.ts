import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from './services/user.service';
import { User } from '../../core/models/user.model';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: User[] = [];
  loading = true;
  showModal = false;
  editMode = false;

  currentUser: User = {
    username: '',
    email: '',
    password: '',
    role: 'User',
    isActive: true
  };

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.userService.getAll().subscribe({
      next: (data) => {
        this.users = data;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  openAdd(): void {
    this.editMode = false;
    this.currentUser = {
      username: '',
      email: '',
      password: '',
      role: 'User',
      isActive: true
    };
    this.showModal = true;
  }

  openEdit(user: User): void {
    this.editMode = true;
    this.currentUser = { ...user };
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

import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
import { CategorieService } from './services/categorie.service';
import { Categorie } from '../../core/models/categorie.model';

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {
  categories: Categorie[] = [];
  loading = true;
  showModal = false;
  editMode = false;

  currentCat: Categorie = {
    codeCat: '',
    libelle: '',
    description: ''
  };

  constructor(private service: CategorieService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.service.getAll().subscribe({
      next: (data) => {
        this.categories = data;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  openAdd(): void {
    this.editMode = false;
    this.currentCat = { codeCat: '', libelle: '', description: '' };
    this.showModal = true;
  }

  openEdit(c: Categorie): void {
    this.editMode = true;
    this.currentCat = { ...c };
    this.showModal = true;
  }

  save(): void {
    const obs: Observable<any> = this.editMode ? this.service.update(this.currentCat) : this.service.add(this.currentCat);
    obs.subscribe(() => {
      this.showModal = false;
      this.load();
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

import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { ArticleService } from './services/article.service';
import { SouscategorieService } from '../categories/services/souscategorie.service';
import { Article } from '../../core/models/article.model';
import { Souscategorie } from '../../core/models/categorie.model';

@Component({
  selector: 'app-articles',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './articles.component.html',
  styleUrls: ['./articles.component.css']
})
export class ArticlesComponent implements OnInit {
  articles: Article[] = [];
  subcategories: Souscategorie[] = [];
  loading = true;
  showModal = false;
  editMode = false;

  currentArticle: Article = {
    designation: '',
    description: '',
    prixUnitaire: 0,
    stockDispo: 0,
    idScat: 0
  };

  constructor(
    private articleService: ArticleService,
    private scatService: SouscategorieService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadData();
    this.route.queryParams.subscribe(params => {
      if (params['action'] === 'add') this.openAdd();
    });
  }

  loadData(): void {
    this.loading = true;
    this.articleService.getAll().subscribe({
      next: (data) => {
        this.articles = data;
        this.loading = false;
      },
      error: () => this.loading = false
    });

    this.scatService.getAll().subscribe(data => this.subcategories = data);
  }

  openAdd(): void {
    this.editMode = false;
    this.currentArticle = { 
      designation: '', 
      description: '', 
      prixUnitaire: 0, 
      stockDispo: 0, 
      idScat: this.subcategories[0]?.idScat || 0 
    };
    this.showModal = true;
  }

  openEdit(art: Article): void {
    this.editMode = true;
    this.currentArticle = { ...art };
    this.showModal = true;
  }

  save(): void {
    if (!this.currentArticle.designation) {
      alert('La désignation est obligatoire');
      return;
    }

    const obs: Observable<any> = this.editMode 
      ? this.articleService.update(this.currentArticle)
      : this.articleService.add(this.currentArticle);

    obs.subscribe({
      next: () => {
        this.showModal = false;
        this.loadData();
      },
      error: (err) => {
        console.error('Erreur API:', err);
        alert('Erreur lors de l\'enregistrement : ' + (err.error?.message || err.message || 'Vérifiez la catégorie'));
      }
    });
  }

  showDeleteModal = false;
  itemToDelete: number | undefined = undefined;

  deleteArticle(id: number | undefined): void {
    if (id) {
      this.itemToDelete = id;
      this.showDeleteModal = true;
    }
  }

  confirmDelete(): void {
    if (this.itemToDelete) {
      this.articleService.delete(this.itemToDelete).subscribe({
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

  getScatName(id: number | undefined): string {
    const s = this.subcategories.find(x => x.idScat === id);
    return s ? s.libelle : 'Catégorie ' + id;
  }
}

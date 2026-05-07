import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RoleService } from './services/role.service';
import { Role } from '../../core/models/role.model';
import { Menu } from '../../core/models/menu.model';
import { MenuService } from '../../core/services/menu.service';

@Component({
  selector: 'app-roles',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.css']
})
export class RolesComponent implements OnInit {
  roles: Role[] = [];
  allMenus: Menu[] = [];
  loading = true;
  showModal = false;
  editMode = false;

  currentRole: Role = {
    nom: '',
    description: '',
    idmenus: []
  };

  // IDs des menus sélectionnés pour le rôle en cours d'édition
  selectedMenuIds: Set<number> = new Set();

  showDeleteModal = false;
  itemToDelete: number | undefined = undefined;

  constructor(
    private roleService: RoleService,
    private menuService: MenuService
  ) {}

  ngOnInit(): void {
    this.loadData();
    this.loadMenus();
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

  loadMenus(): void {
    this.menuService.getAll().subscribe({
      next: (menus) => {
        this.allMenus = menus;
      },
      error: (err) => {
        console.error('Erreur chargement menus:', err);
      }
    });
  }

  openAdd(): void {
    this.editMode = false;
    this.currentRole = { nom: '', description: '', idmenus: [] };
    this.selectedMenuIds = new Set();
    this.showModal = true;
  }

  openEdit(role: Role): void {
    this.editMode = true;
    this.currentRole = { ...role };
    // Initialiser les menus sélectionnés à partir du rôle
    this.selectedMenuIds = new Set(
      (role.idmenus || []).map(m => m.idmenu)
    );
    this.showModal = true;
  }

  toggleMenu(menuId: number): void {
    if (this.selectedMenuIds.has(menuId)) {
      this.selectedMenuIds.delete(menuId);
    } else {
      this.selectedMenuIds.add(menuId);
    }
  }

  isMenuSelected(menuId: number): boolean {
    return this.selectedMenuIds.has(menuId);
  }

  selectAllMenus(): void {
    this.allMenus.forEach(m => this.selectedMenuIds.add(m.idmenu));
  }

  deselectAllMenus(): void {
    this.selectedMenuIds.clear();
  }

  getMenuCount(role: Role): number {
    return role.idmenus?.length || 0;
  }

  save(): void {
    if (!this.currentRole.nom) {
      alert('Le nom du rôle est obligatoire.');
      return;
    }

    // Construire la liste des menus sélectionnés (on envoie les objets Menu au backend)
    this.currentRole.idmenus = this.allMenus.filter(m =>
      this.selectedMenuIds.has(m.idmenu)
    );

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

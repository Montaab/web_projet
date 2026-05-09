import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../features/auth/services/auth.service';
import { ResponseLogin } from '../../../core/models/auth.model';
import { MenuService } from '../../../core/services/menu.service';
import { Menu } from '../../../core/models/menu.model';

import { ThemeService } from '../../../core/services/theme.service';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  currentUser: ResponseLogin | null = null;
  showLogoutModal = false;
  menuItems: { path: string; label: string; icon: string }[] = [];
  loading = true;
  roleName = '';

  // Map d'icônes : clé = valeur de memIcon en base, valeur = SVG inline
  private iconMap: { [key: string]: string } = {
    dashboard: '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect x="3" y="3" width="7" height="7"/><rect x="14" y="3" width="7" height="7"/><rect x="14" y="14" width="7" height="7"/><rect x="3" y="14" width="7" height="7"/></svg>',
    articles: '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"/><polyline points="3.29 7 12 12 20.71 7"/><line x1="12" x2="12" y1="22" y2="12"/></svg>',
    categories: '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="m15 5 4 4"/><path d="M13 7 8.7 2.7a2 2 0 0 0-2.8 0L2.7 5.9a2 2 0 0 0 0 2.8L7 13"/><path d="m19 11-4-4"/><path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"/><polyline points="7 17 12 12 17 17"/></svg>',
    clients: '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M22 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>',
    fournisseurs: '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M2 20a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2V8l-7 5V8l-7 5V4a2 2 0 0 0-2-2H4a2 2 0 0 0-2 2Z"/><path d="M17 18h1"/><path d="M12 18h1"/><path d="M7 18h1"/></svg>',
    commandes: '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect width="8" height="4" x="8" y="2" rx="1" ry="1"/><path d="M16 4h2a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h2"/><path d="M12 11h4"/><path d="M12 16h4"/><path d="M8 11h.01"/><path d="M8 16h.01"/></svg>',
    factures: '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M4 2v20l2-1 2 1 2-1 2 1 2-1 2 1 2-1 2 1V2l-2 1-2-1-2 1-2-1-2 1-2-1Z"/><path d="M16 8h-9"/><path d="M16 12h-9"/><path d="M13 16h-6"/></svg>',
    roles: '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M12 12a4 4 0 1 0-4-4 4 4 0 0 0 4 4Z"/><path d="M6 21v-2a4 4 0 0 1 4-4h4a4 4 0 0 1 4 4v2"/><path d="M16 5.5a3.5 3.5 0 0 1 0 7"/><path d="M8 12.5a3.5 3.5 0 0 1 0-7"/></svg>',
    utilisateurs: '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path><circle cx="9" cy="7" r="4"></circle><path d="M23 21v-2a4 4 0 0 0-3-3.87"></path><path d="M16 3.13a4 4 0 0 1 0 7.75"></path></svg>',
    chatbot: '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M21 15a2 2 0 0 1-2 2H7l-4 4V5a2 2 0 0 1 2-2h14a2 2 0 0 1 2 2z"/><path d="M8 10h.01"/><path d="M12 10h.01"/><path d="M16 10h.01"/></svg>',
  };

  // Icône par défaut si memIcon ne correspond à rien
  private defaultIcon = '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10"/></svg>';

  constructor(
    private auth: AuthService,
    public themeService: ThemeService,
    private sanitizer: DomSanitizer,
    private menuService: MenuService
  ) {
    this.currentUser = this.auth.getCurrentUser();
  }

  ngOnInit(): void {
    // Extraire le nom du rôle depuis le JWT
    const token = this.auth.getToken();
    if (token) {
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        this.roleName = payload.role || '';
      } catch { }
    }

    this.loadMenus();
  }

  private loadMenus(): void {
    const roleId = this.currentUser?.idrole;

    if (roleId) {
      this.menuService.getByRole(roleId).subscribe({
        next: (menus) => {
          this.menuItems = menus.map(m => ({
            path: m.memRouterlink,
            label: m.titre,
            icon: this.iconMap[m.memIcon] || this.defaultIcon
          }));
          this.loading = false;
        },
        error: () => {
          // Fallback : afficher tous les menus par défaut si l'API échoue
          this.menuItems = this.getDefaultMenus();
          this.loading = false;
        }
      });
    } else {
      this.menuItems = this.getDefaultMenus();
      this.loading = false;
    }
  }

  private getDefaultMenus(): { path: string; label: string; icon: string }[] {
    return [
      { path: '/dashboard', label: 'Dashboard', icon: this.iconMap['dashboard'] },
      { path: '/articles', label: 'Articles', icon: this.iconMap['articles'] },
      { path: '/categories', label: 'Catégories', icon: this.iconMap['categories'] },
      { path: '/clients', label: 'Clients', icon: this.iconMap['clients'] },
      { path: '/fournisseurs', label: 'Fournisseurs', icon: this.iconMap['fournisseurs'] },
      { path: '/commandes', label: 'Commandes', icon: this.iconMap['commandes'] },
      { path: '/factures', label: 'Factures', icon: this.iconMap['factures'] },
      { path: '/roles', label: 'Rôles', icon: this.iconMap['roles'] },
      { path: '/utilisateurs', label: 'Utilisateurs', icon: this.iconMap['utilisateurs'] },
    ];
  }

  toggleTheme() {
    this.themeService.toggleTheme();
  }

  confirmLogout() {
    this.showLogoutModal = true;
  }

  logout() {
    this.auth.logout();
    this.showLogoutModal = false;
  }

  getSafeIcon(icon: string): SafeHtml {
    return this.sanitizer.bypassSecurityTrustHtml(icon);
  }
}

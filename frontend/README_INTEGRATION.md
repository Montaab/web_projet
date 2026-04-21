# Intégration Frontend-Backend : Angular 17 Standalone & Microservices .NET

Ce document décrit l'architecture et les choix d'implémentation pour l'intégration complète entre le frontend Angular (version 17.3.0 en mode standalone) et les microservices C# .NET.

## 1. Architecture Standalone

L'application Angular a été structurée pour se passer totalement des `NgModules`. 
- **`app.config.ts`** remplace l'ancien `app.module.ts`. Il fournit de manière globale les fonctionnalités comme le `HttpClient`, les Intercepteurs (sous forme de fonctions), les animations (Angular Material) et le Router.
- **Composants Standalone** : Chaque composant (`ArticlesComponent`, `ClientsComponent`, `UserListComponent`, etc.) est décoré avec `standalone: true`. Les dépendances (ex: `CommonModule`, `ReactiveFormsModule`, modules `MatTableModule`...) sont importées directement dans le tableau `imports` du composant.
- **Lazy Loading** : Le routing utilise `loadComponent` au lieu de `loadChildren`, ce qui permet de charger les composants à la demande sans nécessiter la création de modules intermédiaires.

## 2. Configuration API & Environnements

Les URLs des différents microservices backend sont définies dans `src/environments/environment.ts` et `environment.prod.ts` :
- `userApi`: `http://localhost:5001`
- `erpApi`: `http://localhost:5271`
- `authApi`: `http://localhost:5002`

Les services Angular utilisent ces variables d'environnement injectées pour construire leurs requêtes `HttpClient`.

## 3. Sécurité, Intercepteurs et Guards Fonctionnels

Pour s'adapter à la modernité d'Angular 17 :
- **`jwtInterceptor`** (`HttpInterceptorFn`) : Intercepte toutes les requêtes sortantes pour y attacher automatiquement le token JWT stocké dans le `localStorage` sous le header `Authorization: Bearer <token>`.
- **`errorInterceptor`** (`HttpInterceptorFn`) : Capture les erreurs globales (401, 403, 500, mode hors-ligne). Si un token est invalide (401), l'utilisateur est notifié et redirigé vers `/login`. Il utilise `MatSnackBar` pour afficher des messages propres.
- **`authGuard` et `roleGuard`** (`CanActivateFn`) : Protègent les routes. Le `roleGuard` permet de limiter l'accès à certaines pages (ex: la gestion des utilisateurs est restreinte aux rôles de type "Admin").

## 4. Composants Métier, Material UI & Reactive Forms

Des composants exemplaires (Users, Articles, Clients) ont été refondus ou créés pour démontrer l'implémentation attendue :
- **Angular Material** : Utilisation de `MatTable` avec pagination (`MatPaginator`) et tri (`MatSort`). Les indicateurs de chargement (`MatSpinner`) gèrent l'expérience utilisateur pendant les appels HTTP.
- **Reactive Forms** : Tous les formulaires (ajout, modification) utilisent `FormBuilder` et `FormGroup` avec des `Validators` (required, min, email).
- **Modèles de données strictes** : Mapping C# (PascalCase) vers TypeScript (camelCase).
- **Clés Composites** : `LCommandeService` et `LFournisseurService` gèrent correctement les appels PUT et DELETE avec des clés primaires doubles (`id1/id2`).

## 5. Comment Lancer

1. Assurez-vous d'avoir Node.js installé (version LTS).
2. Vérifiez que les APIs C# tournent sur les bons ports (`5001`, `5002`, `5271`).
3. Installez les dépendances :
   ```bash
   npm install
   ```
4. Lancez l'application Angular :
   ```bash
   ng serve
   ```
5. Accédez à `http://localhost:4200`, connectez-vous et naviguez dans les modules ERP.

import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../features/auth/services/auth.service';
export const roleGuard: CanActivateFn = (route, state) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  const currentUser = auth.getCurrentUser();
  const expectedRoles = route.data['roles'] as Array<string>;

  if (currentUser && expectedRoles && expectedRoles.length > 0) {
    // In our auth model, we have `idrole` (number). Let's assume roles are mapped:
    // e.g., 1 = Admin, 2 = User, etc. Or maybe the backend passes a role name.
    // If idrole is just a number, we might need a mapping.
    // Let's assume for now 1 = Admin, 2 = Manager, 3 = User.
    const roleMap: { [key: number]: string } = {
      1: 'Admin',
      2: 'Manager',
      3: 'User'
    };
    
    const userRoleName = roleMap[currentUser.idrole] || 'User';

    if (expectedRoles.includes(userRoleName)) {
      return true;
    }
  }

  // Not authorized
  router.navigate(['/dashboard']);
  return false;
};

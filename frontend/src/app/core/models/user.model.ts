export interface User {
  idUser?: number;
  username: string;
  email: string;
  password?: string;
  telephone?: string;
  idrole?: number;
  role?: string;
  idroleNavigation?: { nom: string };
  isActive: boolean;
}

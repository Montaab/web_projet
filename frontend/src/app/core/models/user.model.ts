export interface User {
  idUser?: number;
  username: string;
  email: string;
  password?: string;
  role: string;
  isActive: boolean;
}

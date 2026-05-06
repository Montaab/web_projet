export interface Role {
  idrole?: number;
  nom: string;
  description: string;
  idprofile?: number | null;
  idroleparent?: number | null;
}

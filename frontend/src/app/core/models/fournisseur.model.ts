import { LFournisseur } from './lfournisseur.model';

export interface Fournisseur {
  idFour?: number;
  nomSociete: string;
  tel: string;
  email: string;
  adresse: string;
  ville: string;
  lFournisseurs?: LFournisseur[];
}

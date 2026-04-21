import { LCommande } from './lcommande.model';

export interface Commande {
  idCom?: number;
  dateCom: string;
  statut: string;
  modePaiement: string;
  idClt: number;
  total?: number;
  lCommandes?: LCommande[]; // Ajout des lignes de commande
}

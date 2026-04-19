export interface Facture {
  idFact?: number;
  dateFact: string;
  montantHt?: number;
  montantTotal: number; // Changement de montantTtc à montantTotal
  modePaiement: string;
  idCom: number;
}

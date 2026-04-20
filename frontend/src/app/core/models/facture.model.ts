export interface Facture {
  idFact?: number;
  dateFact: string;
  montantHt?: number;
  montantTotal: any; // Changement de montantTtc à montantTotal
  modePaiement: string;
  idCom: number;
}

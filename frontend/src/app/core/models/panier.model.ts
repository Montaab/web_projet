export interface Panier {
  id?: number;
  userId: number;
  articleId: number;
  quantite: number;
  dateAjout?: Date;
}

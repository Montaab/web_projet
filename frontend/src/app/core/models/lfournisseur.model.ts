export interface ArticleNavigation {
  idArt: number;
  designation: string;
  description?: string;
  prixUnitaire: number;
  stockDispo?: number;
  imageUrl?: string;
  idScat?: number;
}

export interface LFournisseur {
  idFour: number;
  idArt: number;
  delaiLivraison?: number;
  prixFournisseur: number;
  // Rétrocompatibilité avec l'ancien champ
  prixFour?: number;
  delaiLivraisonJours?: number;
  idArtNavigation?: ArticleNavigation;
}

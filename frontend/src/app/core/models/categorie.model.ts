export interface Categorie {
  idCat?: number;
  codeCat: string;
  libelle: string;
  description: string;
}

export interface Souscategorie {
  idScat: number;
  codeScat: string;
  libelle: string;
  description: string;
  idCat?: number;
}

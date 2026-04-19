export interface Login {
  username: string;
  password: string;
}

export interface ResponseLogin {
  accessToken: string;
  tokenType: string;
  expireIn: number;
  iduser: number;
  nom: string;
  email: string;
  idrole: number;
}

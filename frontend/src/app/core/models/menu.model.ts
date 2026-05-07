export interface Menu {
  idmenu: number;
  titre: string;
  description: string;
  memRouterlink: string;
  memHref: string;
  memIcon: string;
  memTarget: string;
  hassubmenu: boolean;
  parentid: number | null;
  inverseParent: Menu[];
  idroles: any[];
}

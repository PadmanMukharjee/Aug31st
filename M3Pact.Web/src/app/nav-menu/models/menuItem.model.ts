export class MenuItemModel {

  /*-----region properties -----*/

  // model properties
  public nodeName: string;
  public url: string;
  public icon: string;
  public nodeId: string;
  public parentId: string;
  public subNodes: MenuItemModel[];
  public expand: boolean;
  public hasSubNodes: boolean;
  public info: string;

  /*-----end region properties -----*/
}

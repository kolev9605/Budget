export class UpdateAccountModel {
  public id: number;
  public name: string;
  public currencyId: number;

  constructor(id: number, name: string, currencyId: number) {
    this.id = id;
    this.name = name;
    this.currencyId = currencyId;
  }
}

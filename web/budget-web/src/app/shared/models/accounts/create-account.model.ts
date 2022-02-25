export class CreateAccountModel {
  public name: string;
  public currencyId: number;

  constructor(name: string, currencyId: number) {
    this.name = name;
    this.currencyId = currencyId;
  }
}

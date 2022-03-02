export class UpdateAccountModel {
  constructor(
    public id: number,
    public name: string,
    public currencyId: number,
    public initialBalance: number,
  ) {
    this.id = id;
    this.name = name;
    this.currencyId = currencyId;
    this.initialBalance = initialBalance;
  }
}

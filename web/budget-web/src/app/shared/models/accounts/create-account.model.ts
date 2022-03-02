export class CreateAccountModel {
  constructor(public name: string, public currencyId: number, public initialBalance: number) {
    this.name = name;
    this.currencyId = currencyId;
    this.initialBalance = initialBalance;
  }
}

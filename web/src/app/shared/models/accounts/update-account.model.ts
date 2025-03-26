export class UpdateAccountModel {
  constructor(
    public id: string,
    public name: string,
    public initialBalance: number,
    public currencyId: string,
  ) {}
}

export class UpdateAccountModel {
  constructor(
    public id: number,
    public name: string,
    public initialBalance: number,
    public currencyId: number,
  ) {}
}

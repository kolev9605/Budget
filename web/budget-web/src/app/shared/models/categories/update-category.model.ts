export class UpdateCategoryModel {
  constructor(
    public id: number,
    public name: string,
    public categoryType: string,
    public parentCategoryId: number,
  ) {}
}

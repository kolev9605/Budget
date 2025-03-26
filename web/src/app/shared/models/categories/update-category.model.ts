export class UpdateCategoryModel {
  constructor(
    public id: string,
    public name: string,
    public categoryType: string,
    public parentCategoryId: string,
  ) {}
}

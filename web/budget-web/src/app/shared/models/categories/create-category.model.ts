export class CreateCategoryModel {
  constructor(public name: string, public categoryType: string, public parentCategoryId: number) {}
}

export class CategoryModel {
  public id: number;
  public name: string;
  public categoryType: string;
  public parentCategoryId: number;
  public subCategories: CategoryModel[];
}

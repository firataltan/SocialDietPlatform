export interface User {
  id: string;
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  profileImage?: string;
  createdAt: string;
  updatedAt: string;
  dateOfBirth?: string;
  role?: number;
  bio?: string;
  weight?: number;
  height?: number;
  targetWeight?: number;
  profilePictureUrl?: string;
  token?: string;
}

export interface LoginCredentials {
  emailOrUsername: string;
  password: string;
}

export interface UpdateProfilePayload {
  firstName: string;
  lastName: string;
  dateOfBirth?: string;
  bio?: string;
  weight?: number;
  height?: number;
  targetWeight?: number;
  profilePictureUrl?: string;
}

export interface DietPlan {
  id: string;
  name: string;
  description: string;
  startDate: string;
  endDate: string;
  targetCalories: number;
  isActive: boolean;
  dietitian?: User;
  meals: Meal[];
  createdAt: string;
  updatedAt: string;
}

export interface Meal {
  id: string;
  name: string;
  description: string;
  mealTime: string;
  foods: MealFood[];
  totalCalories: number;
  createdAt: string;
  updatedAt: string;
}

export interface MealFood {
  id: string;
  food: Food | null;
  quantity: number;
  unit: string;
  calories: number;
}

export interface Food {
  id: string;
  name: string;
  description: string;
  calories: number;
  protein: number;
  carbs: number;
  fat: number;
  servingSize: number;
  servingUnit: string;
  createdAt: string;
  updatedAt: string;
}

export interface RecipeIngredient {
  foodId: string;
  quantity: number;
  unit: string;
}

export interface RecipeFormValues {
  name: string;
  description: string;
  preparationTime: number;
  cookingTime: number;
  servings: number;
  imageUrl: string;
  categoryId: string;
  ingredients: RecipeIngredient[];
  instructions: string;
}

export interface RecipeFormErrors {
  name?: string;
  description?: string;
  preparationTime?: string;
  cookingTime?: string;
  servings?: string;
  imageUrl?: string;
  categoryId?: string;
  ingredients?: {
    foodId?: string;
    quantity?: string;
    unit?: string;
  }[];
  instructions?: string;
}

export interface RecipeFormTouched {
  name?: boolean;
  description?: boolean;
  preparationTime?: boolean;
  cookingTime?: boolean;
  servings?: boolean;
  imageUrl?: boolean;
  categoryId?: boolean;
  ingredients?: {
    foodId?: boolean;
    quantity?: boolean;
    unit?: boolean;
  }[];
  instructions?: boolean;
}

export type RecipeFormField = keyof RecipeFormValues;
export type RecipeIngredientField = keyof RecipeIngredient;

export interface Recipe {
  id: string;
  name: string;
  description: string;
  instructions: string[];
  preparationTime: number;
  cookingTime: number;
  servings: number;
  imageUrl?: string;
  userId: string;
  categoryId: string;
  ingredients: RecipeIngredient[];
  createdAt: string;
  updatedAt: string;
}

export interface Category {
  id: string;
  name: string;
  description: string;
}

export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface PostDto {
  id: string;
  userId: string;
  userName: string;
  content: string;
  createdAt: string;
  likesCount: number;
  commentsCount: number;
  isLiked: boolean;
  imageUrl?: string;
  user: {
    profileImageUrl?: string;
  };
}

export interface ApiResponse<T> {
  data: T | undefined;
  message?: string;
  status: number;
}

export interface IRecipeService {
  getRecipes: () => Promise<ApiResponse<Recipe[]>>;
  getRecipe: (id: string) => Promise<ApiResponse<Recipe>>;
  updateRecipe: (id: string, updatedRecipe: Partial<Recipe>) => Promise<ApiResponse<Recipe>>;
  createRecipe: (newRecipe: Omit<Recipe, 'id'>) => Promise<ApiResponse<Recipe>>;
  getCategories: () => Promise<ApiResponse<Category[]>>;
  getFoods: () => Promise<ApiResponse<Food[]>>;
}

export interface CommentDto {
  id: string;
  postId: string;
  userId: string;
  userName: string;
  content: string;
  createdAt: string;
  user: {
    profileImageUrl?: string;
  };
} 
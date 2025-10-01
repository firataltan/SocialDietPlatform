import type { ApiResponse, Recipe, Category, Food } from '../types';
import axios from 'axios';

// const API_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';
const API_URL = 'http://localhost:7061/api/v1'; // Correct base API URL including v1

// Mock recipe data (can be removed later if not needed)
const mockRecipes: Recipe[] = [
  {
    id: '1',
    name: 'Kinoa Salatası', // Changed from title to name
    description: 'Sağlıklı ve doyurucu kinoa salatası.',
    ingredients: [{ foodId: 'mock-food-1', quantity: 150, unit: 'g' }, { foodId: 'mock-food-2', quantity: 1, unit: 'adet' }], // Updated to match RecipeIngredient[]
    instructions: ['Kinoayı haşlayın', 'Sebzeleri doğrayın', 'Hepsini karıştırın'],
    preparationTime: 15, // Changed from prepTime
    cookingTime: 15, // Changed from cookTime
    servings: 2,
    // calories, protein, carbs, fat will likely be calculated on backend or derived from ingredients
    // imageUrl: 'https://source.unsplash.com/random/400x300/?salad',
    imageUrl: '',
    userId: 'mock-user-id', // Ensure this matches expected backend user ID type
    categoryId: 'mock-category-id-1', // Example category ID
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString(),
    // Removed totalCalories
  },
  {
    id: '2',
    name: 'Mercimek Çorbası', // Changed from title to name
    description: 'Besleyici ve lezzetli mercimek çorbası.',
    ingredients: [{ foodId: 'mock-food-3', quantity: 200, unit: 'ml' }], // Updated to match RecipeIngredient[]
    instructions: ['Mercimeği yıkayın', 'Sebzeleri kavurun', 'Hepsini pişirin'],
    preparationTime: 10, // Changed from prepTime
    cookingTime: 25, // Changed from cookTime
    servings: 4,
    // calories, protein, carbs, fat will likely be calculated on backend or derived from ingredients
    // imageUrl: 'https://source.unsplash.com/random/400x300/?soup',
    imageUrl: '',
    userId: 'mock-user-id', // Ensure this matches expected backend user ID type
    categoryId: 'mock-category-id-2', // Example category ID
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString(),
    // Removed totalCalories
  },
];

export interface IRecipeService {
  getRecipes: () => Promise<ApiResponse<Recipe[]>>;
  getRecipe: (id: string) => Promise<ApiResponse<Recipe>>;
  updateRecipe: (id: string, updatedRecipe: Partial<Recipe>) => Promise<ApiResponse<Recipe>>;
  createRecipe: (newRecipe: Omit<Recipe, 'id' | 'createdAt' | 'updatedAt'>) => Promise<ApiResponse<Recipe>>;
  getCategories: () => Promise<ApiResponse<Category[]>>;
  getFoods: () => Promise<ApiResponse<Food[]>>;
}

export const recipeService: IRecipeService = {
  getRecipes: async (): Promise<ApiResponse<Recipe[]>> => {
    try {
      const response = await axios.get(`${API_URL}/recipes`);
      return {
        data: response.data,
        status: response.status,
        message: response.data.message
      };
    } catch (error) {
      console.error('Error fetching recipes:', error);
      return {
        data: [],
        status: 500,
        message: 'Failed to fetch recipes'
      };
    }
  },

  getRecipe: async (id: string): Promise<ApiResponse<Recipe>> => {
    try {
      const response = await axios.get(`${API_URL}/recipes/${id}`);
      return {
        data: response.data,
        status: response.status,
        message: response.data.message
      };
    } catch (error) {
      console.error('Error fetching recipe:', error);
      return {
        data: undefined,
        status: 500,
        message: 'Failed to fetch recipe'
      };
    }
  },

  updateRecipe: async (id: string, updatedRecipe: Partial<Recipe>): Promise<ApiResponse<Recipe>> => {
    try {
      const response = await axios.put(`${API_URL}/recipes/${id}`, updatedRecipe);
      return {
        data: response.data,
        status: response.status,
        message: response.data.message
      };
    } catch (error) {
      console.error('Error updating recipe:', error);
      return {
        data: undefined,
        status: 500,
        message: 'Failed to update recipe'
      };
    }
  },

  createRecipe: async (newRecipe: Omit<Recipe, 'id' | 'createdAt' | 'updatedAt'>): Promise<ApiResponse<Recipe>> => {
    try {
      const response = await axios.post(`${API_URL}/recipes`, newRecipe);
      return {
        data: response.data,
        status: response.status,
        message: response.data.message
      };
    } catch (error) {
      console.error('Error creating recipe:', error);
      return {
        data: undefined,
        status: 500,
        message: 'Failed to create recipe'
      };
    }
  },

  getCategories: async (): Promise<ApiResponse<Category[]>> => {
    try {
      const response = await axios.get(`${API_URL}/recipes/categories`);
      return {
        data: response.data,
        status: response.status,
        message: response.data.message
      };
    } catch (error) {
      console.error('Error fetching categories:', error);
      return {
        data: [],
        status: 500,
        message: 'Failed to fetch categories'
      };
    }
  },

  getFoods: async (): Promise<ApiResponse<Food[]>> => {
    try {
      const response = await axios.get(`${API_URL}/foods`);
      return {
        data: response.data,
        status: response.status,
        message: response.data.message
      };
    } catch (error) {
      console.error('Error fetching foods:', error);
      return {
        data: [],
        status: 500,
        message: 'Failed to fetch foods'
      };
    }
  },

  // Add other service methods like deleteRecipe here
}; 
import api from './api';
import type { DietPlan, Meal, MealFood, Food } from '../types';

export const dietPlanService = {
  // Get all diet plans for current user
  getDietPlans: async (): Promise<DietPlan[]> => {
    const response = await api.get<DietPlan[]>('/v1/DietPlans/my-plans');
    return response.data;
  },

  // Get a specific diet plan
  getDietPlan: async (id: string): Promise<DietPlan> => {
    const response = await api.get<DietPlan>(`/v1/DietPlans/${id}`);
    return response.data;
  },

  // Create a new diet plan
  createDietPlan: async (dietPlan: Omit<DietPlan, 'id' | 'meals' | 'createdAt' | 'updatedAt'>): Promise<DietPlan> => {
    const response = await api.post<DietPlan>('/v1/DietPlans', dietPlan);
    return response.data;
  },

  // Update an existing diet plan
  updateDietPlan: async (id: string, dietPlan: Omit<DietPlan, 'id' | 'meals' | 'createdAt' | 'updatedAt'>): Promise<DietPlan> => {
    const response = await api.put<DietPlan>(`/v1/DietPlans/${id}`, dietPlan);
    return response.data;
  },

  // Delete a diet plan
  deleteDietPlan: async (id: string): Promise<void> => {
    await api.delete(`/v1/DietPlans/${id}`);
  },

  // Add a meal to a diet plan
  addMeal: async (dietPlanId: string, meal: Omit<Meal, 'id' | 'foods' | 'totalCalories' | 'createdAt' | 'updatedAt'>): Promise<Meal> => {
    const response = await api.post<Meal>(`/v1/DietPlans/${dietPlanId}/meals`, meal);
    return response.data;
  },

  // Update a meal in a diet plan
  updateMeal: async (dietPlanId: string, mealId: string, meal: Omit<Meal, 'id' | 'foods' | 'totalCalories' | 'createdAt' | 'updatedAt'>): Promise<Meal> => {
    const response = await api.put<Meal>(`/v1/DietPlans/${dietPlanId}/meals/${mealId}`, meal);
    return response.data;
  },

  // Delete a meal from a diet plan
  deleteMeal: async (dietPlanId: string, mealId: string): Promise<void> => {
    await api.delete(`/v1/DietPlans/${dietPlanId}/meals/${mealId}`);
  },

  // Search foods
  searchFoods: async (query: string): Promise<Food[]> => {
    const response = await api.get<Food[]>(`/v1/Foods/search?query=${query}`);
    return response.data;
  }
}; 
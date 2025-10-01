import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { ThemeProvider, CssBaseline } from '@mui/material';
import { theme } from './theme';
import Layout from './components/Layout/Layout';
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
import Profile from './pages/Profile';
import DietPlans from './pages/DietPlans';
import DietPlanForm from './pages/DietPlanForm';
import Recipes from './pages/Recipes';
import CommunityPage from './pages/CommunityPage';
import CreatePostPage from './pages/CreatePostPage';
import RecipeForm from './pages/RecipeForm';
import RecipeDetailEdit from './pages/RecipeDetailEdit';
import { AuthProvider } from './contexts/AuthContext';
import ProtectedRoute from './components/ProtectedRoute';

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <AuthProvider>
        <Router>
          <Routes>
            <Route path="/" element={<Layout />}>
              <Route index element={<Home />} />
              <Route path="login" element={<Login />} />
              <Route path="register" element={<Register />} />
              <Route
                path="profile"
                element={
                  <ProtectedRoute>
                    <Profile />
                  </ProtectedRoute>
                }
              />
              <Route
                path="diet-plans"
                element={
                  <ProtectedRoute>
                    <DietPlans />
                  </ProtectedRoute>
                }
              />
              <Route
                path="diet-plans/new"
                element={
                  <ProtectedRoute>
                    <DietPlanForm />
                  </ProtectedRoute>
                }
              />
              <Route
                path="diet-plans/:id"
                element={
                  <ProtectedRoute>
                    <DietPlanForm />
                  </ProtectedRoute>
                }
              />
              <Route
                path="recipes"
                element={
                  <ProtectedRoute>
                    <Recipes />
                  </ProtectedRoute>
                }
              />
              <Route
                path="recipes/new"
                element={
                  <ProtectedRoute>
                    <RecipeForm />
                  </ProtectedRoute>
                }
              />
              <Route
                path="recipes/:id"
                element={
                  <ProtectedRoute>
                    <RecipeDetailEdit />
                  </ProtectedRoute>
                }
              />
              <Route
                path="community"
                element={
                  <ProtectedRoute>
                    <CommunityPage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="community/new"
                element={
                  <ProtectedRoute>
                    <CreatePostPage />
                  </ProtectedRoute>
                }
              />
            </Route>
          </Routes>
        </Router>
      </AuthProvider>
    </ThemeProvider>
  );
}

export default App;

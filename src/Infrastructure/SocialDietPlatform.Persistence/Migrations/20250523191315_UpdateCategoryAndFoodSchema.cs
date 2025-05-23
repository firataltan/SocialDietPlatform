using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialDietPlatform.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategoryAndFoodSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealFoods_Foods_FoodId",
                table: "MealFoods");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Foods_FoodId",
                table: "RecipeIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredients_RecipeId",
                table: "RecipeIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealFoods",
                table: "MealFoods");

            migrationBuilder.DropIndex(
                name: "IX_MealFoods_MealId",
                table: "MealFoods");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Follow_NoSelfFollow",
                table: "Follows");

            migrationBuilder.RenameColumn(
                name: "PrepTimeMinutes",
                table: "Recipes",
                newName: "PreparationTime");

            migrationBuilder.RenameColumn(
                name: "CookTimeMinutes",
                table: "Recipes",
                newName: "CookingTime");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCalories",
                table: "Recipes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "RecipeIngredients",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Calories",
                table: "RecipeIngredients",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "MealFoods",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Calories",
                table: "MealFoods",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients",
                columns: new[] { "RecipeId", "FoodId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealFoods",
                table: "MealFoods",
                columns: new[] { "MealId", "FoodId" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Follows_SelfFollow",
                table: "Follows",
                sql: "FollowerId != FollowingId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealFoods_Foods_FoodId",
                table: "MealFoods",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Foods_FoodId",
                table: "RecipeIngredients",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealFoods_Foods_FoodId",
                table: "MealFoods");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Foods_FoodId",
                table: "RecipeIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealFoods",
                table: "MealFoods");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Follows_SelfFollow",
                table: "Follows");

            migrationBuilder.DropColumn(
                name: "TotalCalories",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "PreparationTime",
                table: "Recipes",
                newName: "PrepTimeMinutes");

            migrationBuilder.RenameColumn(
                name: "CookingTime",
                table: "Recipes",
                newName: "CookTimeMinutes");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "RecipeIngredients",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Calories",
                table: "RecipeIngredients",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "MealFoods",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Calories",
                table: "MealFoods",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealFoods",
                table: "MealFoods",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_RecipeId",
                table: "RecipeIngredients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_MealFoods_MealId",
                table: "MealFoods",
                column: "MealId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Follow_NoSelfFollow",
                table: "Follows",
                sql: "\"FollowerId\" != \"FollowingId\"");

            migrationBuilder.AddForeignKey(
                name: "FK_MealFoods_Foods_FoodId",
                table: "MealFoods",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Foods_FoodId",
                table: "RecipeIngredients",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

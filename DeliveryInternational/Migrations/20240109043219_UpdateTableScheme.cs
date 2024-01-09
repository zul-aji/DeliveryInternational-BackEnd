using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryInternational.Migrations
{
    public partial class UpdateTableScheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Dishes_DishId",
                table: "Ratings");

            migrationBuilder.AlterColumn<Guid>(
                name: "DishId",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Dishes_DishId",
                table: "Ratings",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "DishId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Dishes_DishId",
                table: "Ratings");

            migrationBuilder.AlterColumn<Guid>(
                name: "DishId",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Dishes_DishId",
                table: "Ratings",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "DishId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

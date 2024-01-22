using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryInternational.Migrations
{
    public partial class ReconfigureDishandRatingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Dishes_DishId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_DishId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Dishes");

            migrationBuilder.AlterColumn<Guid>(
                name: "DishId",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Dishes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Dishes");

            migrationBuilder.AlterColumn<Guid>(
                name: "DishId",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "Ratings",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "Dishes",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_DishId",
                table: "Ratings",
                column: "DishId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Dishes_DishId",
                table: "Ratings",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "DishId");
        }
    }
}

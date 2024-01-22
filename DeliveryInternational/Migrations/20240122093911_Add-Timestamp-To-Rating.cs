using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryInternational.Migrations
{
    public partial class AddTimestampToRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "Ratings",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Ratings");
        }
    }
}

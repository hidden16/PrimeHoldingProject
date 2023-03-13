using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeHoldingProject.Infrastructure.Migrations
{
    public partial class ChangesInTaskEntityMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ManagerId",
                table: "Tasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ManagerId",
                table: "Tasks",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Managers_ManagerId",
                table: "Tasks",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Managers_ManagerId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ManagerId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Tasks");
        }
    }
}

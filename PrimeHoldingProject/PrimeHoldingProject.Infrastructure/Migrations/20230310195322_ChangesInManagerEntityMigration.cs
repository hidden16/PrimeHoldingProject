using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeHoldingProject.Infrastructure.Migrations
{
    public partial class ChangesInManagerEntityMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Salary",
                table: "Managers",
                type: "decimal(7,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Managers");
        }
    }
}

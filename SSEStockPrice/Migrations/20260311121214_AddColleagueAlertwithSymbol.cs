using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSEStockPrice.Migrations
{
    /// <inheritdoc />
    public partial class AddColleagueAlertwithSymbol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "ColleagueAlert",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "ColleagueAlert");
        }
    }
}

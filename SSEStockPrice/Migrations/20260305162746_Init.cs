using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSEStockPrice.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SSEPrice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    OpenPrice = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    CurrentPrice = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    HighPrice = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    LowPrice = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    Volume = table.Column<long>(type: "bigint", nullable: false),
                    ChangePercent = table.Column<decimal>(type: "decimal(6,4)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SSEPrice", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SSEPrice");
        }
    }
}

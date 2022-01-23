using Microsoft.EntityFrameworkCore.Migrations;

namespace ATMService.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "MoneyDenomination",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyDenomination", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoneyStorage",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoneyDenominationId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyStorage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoneyStorage_MoneyDenomination_MoneyDenominationId",
                        column: x => x.MoneyDenominationId,
                        principalSchema: "dbo",
                        principalTable: "MoneyDenomination",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MoneyDenomination",
                columns: new[] { "Id", "Key", "Value" },
                values: new object[,]
                {
                    { 1, "1000", 1000 },
                    { 2, "2000", 2000 },
                    { 3, "5000", 5000 },
                    { 4, "10000", 10000 },
                    { 5, "20000", 20000 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoneyDenomination_Value",
                schema: "dbo",
                table: "MoneyDenomination",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoneyStorage_MoneyDenominationId",
                schema: "dbo",
                table: "MoneyStorage",
                column: "MoneyDenominationId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoneyStorage",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MoneyDenomination",
                schema: "dbo");
        }
    }
}

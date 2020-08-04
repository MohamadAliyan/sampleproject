using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sample.Repository.Migrations
{
    public partial class addinvoicedetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Invoice");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Invoice",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "InvoiceDetail",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<int>(nullable: false),
                    LastModifierId = table.Column<int>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    InvoiceId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceDetail_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetail_InvoiceId",
                table: "InvoiceDetail",
                column: "InvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceDetail");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Invoice");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

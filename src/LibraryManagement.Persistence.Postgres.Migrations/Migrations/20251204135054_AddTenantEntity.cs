using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Persistence.Postgres.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_TenantId",
                table: "Books",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BookKeyword_TenantId",
                table: "BookKeyword",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_TenantId",
                table: "Authors",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Tenants_TenantId",
                table: "Authors",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookKeyword_Tenants_TenantId",
                table: "BookKeyword",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Tenants_TenantId",
                table: "Books",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Tenants_TenantId",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_BookKeyword_Tenants_TenantId",
                table: "BookKeyword");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Tenants_TenantId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Books_TenantId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_BookKeyword_TenantId",
                table: "BookKeyword");

            migrationBuilder.DropIndex(
                name: "IX_Authors_TenantId",
                table: "Authors");
        }
    }
}

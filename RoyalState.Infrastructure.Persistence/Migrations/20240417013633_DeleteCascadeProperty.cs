using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoyalState.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCascadeProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_SaleTypes_SaleTypeId",
                table: "Properties");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_SaleTypes_SaleTypeId",
                table: "Properties",
                column: "SaleTypeId",
                principalTable: "SaleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_SaleTypes_SaleTypeId",
                table: "Properties");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_SaleTypes_SaleTypeId",
                table: "Properties",
                column: "SaleTypeId",
                principalTable: "SaleTypes",
                principalColumn: "Id");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoyalState.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHasDataForSeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Identification", "LastModifiedBy", "LastModifiedDate", "UserId" },
                values: new object[] { 1, "DefaultAppUser", new DateTime(2024, 4, 20, 20, 42, 54, 981, DateTimeKind.Local).AddTicks(6582), "402-3698447-8", null, null, "2d124d85-4239-4c56-b1a8-b5a59c2c7d12" });

            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "ImageUrl", "LastModifiedBy", "LastModifiedDate", "UserId" },
                values: new object[] { 1, "DefaultAppUser", new DateTime(2024, 4, 20, 20, 42, 54, 981, DateTimeKind.Local).AddTicks(6836), "https://mission.org/wp-content/uploads/2019/07/0-AfrrZIuFCim8RNNb.jpg", null, null, "34796422-cda8-4aa2-bc8a-cdc567efae06" });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "ImageUrl", "LastModifiedBy", "LastModifiedDate", "UserId" },
                values: new object[] { 1, "DefaultAppUser", new DateTime(2024, 4, 20, 20, 42, 54, 981, DateTimeKind.Local).AddTicks(6817), "https://img.freepik.com/free-photo/handsome-bearded-guy-posing-against-white-wall_273609-20597.jpg", null, null, "9a0c2574-2bfb-47cf-a311-a9442fa83a0c" });

            migrationBuilder.InsertData(
                table: "Developers",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Identification", "LastModifiedBy", "LastModifiedDate", "UserId" },
                values: new object[] { 1, "DefaultAppUser", new DateTime(2024, 4, 20, 20, 42, 54, 981, DateTimeKind.Local).AddTicks(6794), "402-6328445-9", null, null, "0267618d-1d2b-41ae-b467-cbf9cd3fe956" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Developers",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}

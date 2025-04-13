using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpProject.Migrations
{
    /// <inheritdoc />
    public partial class CertificationAndDocumentsRemovalFromAdditinalDetailsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificationsPath",
                table: "AdditionalDetails");

            migrationBuilder.DropColumn(
                name: "Education",
                table: "AdditionalDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CertificationsPath",
                table: "AdditionalDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Education",
                table: "AdditionalDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

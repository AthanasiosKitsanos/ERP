using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpProject.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalDetailsEducationColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersonalDocumentsPath",
                table: "AdditionalDetails",
                newName: "Education");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Education",
                table: "AdditionalDetails",
                newName: "PersonalDocumentsPath");
        }
    }
}

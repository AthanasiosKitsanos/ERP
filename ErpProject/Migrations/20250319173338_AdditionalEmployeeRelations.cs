using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpProject.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalEmployeeRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmergencyNumbers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Education = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Certifications = table.Column<byte[]>(type: "varbinary(8000)", maxLength: 8000, nullable: false),
                    PersonalDocuments = table.Column<byte[]>(type: "varbinary(8000)", maxLength: 8000, nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalDetails_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Identifications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TIN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkAuth = table.Column<bool>(type: "bit", nullable: false),
                    TaxInformation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identifications_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleEpmloyee",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleEpmloyee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleEpmloyee_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleEpmloyee_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalDetails_EmployeeId",
                table: "AdditionalDetails",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Identifications_EmployeeId",
                table: "Identifications",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleEpmloyee_EmployeeId",
                table: "RoleEpmloyee",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleEpmloyee_RoleId",
                table: "RoleEpmloyee",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalDetails");

            migrationBuilder.DropTable(
                name: "Identifications");

            migrationBuilder.DropTable(
                name: "RoleEpmloyee");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}

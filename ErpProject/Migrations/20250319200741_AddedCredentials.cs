using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpProject.Migrations
{
    /// <inheritdoc />
    public partial class AddedCredentials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCredentials",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastLogIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountStatusId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeCredentials_AccountStatus_AccountStatusId",
                        column: x => x.AccountStatusId,
                        principalTable: "AccountStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeCredentials_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCredentials_AccountStatusId",
                table: "EmployeeCredentials",
                column: "AccountStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCredentials_EmployeeId",
                table: "EmployeeCredentials",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeCredentials");

            migrationBuilder.DropTable(
                name: "AccountStatus");
        }
    }
}

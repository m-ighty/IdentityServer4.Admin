using Microsoft.EntityFrameworkCore.Migrations;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.SqlServer.Migrations.Identity
{
    public partial class AddedUserOrganizationTreatmentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationTreatmentTypes",
                table: "OrganizationTreatmentTypes");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OrganizationTreatmentTypes",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationTreatmentTypes",
                table: "OrganizationTreatmentTypes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserOrganizationTreatmentTypes",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    OrganizationTreatmentTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrganizationTreatmentTypes", x => new { x.UserId, x.OrganizationTreatmentTypeId });
                    table.ForeignKey(
                        name: "FK_UserOrganizationTreatmentTypes_OrganizationTreatmentTypes_OrganizationTreatmentTypeId",
                        column: x => x.OrganizationTreatmentTypeId,
                        principalTable: "OrganizationTreatmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserOrganizationTreatmentTypes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationTreatmentTypes_OrganizationId_TreatmentTypeId",
                table: "OrganizationTreatmentTypes",
                columns: new[] { "OrganizationId", "TreatmentTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganizationTreatmentTypes_OrganizationTreatmentTypeId",
                table: "UserOrganizationTreatmentTypes",
                column: "OrganizationTreatmentTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserOrganizationTreatmentTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationTreatmentTypes",
                table: "OrganizationTreatmentTypes");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationTreatmentTypes_OrganizationId_TreatmentTypeId",
                table: "OrganizationTreatmentTypes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrganizationTreatmentTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationTreatmentTypes",
                table: "OrganizationTreatmentTypes",
                columns: new[] { "OrganizationId", "TreatmentTypeId" });
        }
    }
}

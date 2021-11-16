using Microsoft.EntityFrameworkCore.Migrations;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.SqlServer.Migrations.Identity
{
    public partial class AddedTreatmentTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TreatmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationTreatmentTypes",
                columns: table => new
                {
                    OrganizationId = table.Column<int>(nullable: false),
                    TreatmentTypeId = table.Column<int>(nullable: false),
                    OrganizationCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationTreatmentTypes", x => new { x.OrganizationId, x.TreatmentTypeId });
                    table.ForeignKey(
                        name: "FK_OrganizationTreatmentTypes_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationTreatmentTypes_TreatmentTypes_TreatmentTypeId",
                        column: x => x.TreatmentTypeId,
                        principalTable: "TreatmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationTreatmentTypes_OrganizationCode",
                table: "OrganizationTreatmentTypes",
                column: "OrganizationCode",
                unique: true,
                filter: "[OrganizationCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationTreatmentTypes_TreatmentTypeId",
                table: "OrganizationTreatmentTypes",
                column: "TreatmentTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationTreatmentTypes");

            migrationBuilder.DropTable(
                name: "TreatmentTypes");
        }
    }
}

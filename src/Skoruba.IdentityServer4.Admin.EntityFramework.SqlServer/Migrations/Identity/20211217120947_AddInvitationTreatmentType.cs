using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.SqlServer.Migrations.Identity
{
    public partial class AddInvitationTreatmentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserInvitationsTreatmentTypes",
                columns: table => new
                {
                    UserInvitationId = table.Column<Guid>(nullable: false),
                    TreatmentTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInvitationsTreatmentTypes", x => new { x.UserInvitationId, x.TreatmentTypeId });
                    table.ForeignKey(
                        name: "FK_UserInvitationsTreatmentTypes_TreatmentTypes_TreatmentTypeId",
                        column: x => x.TreatmentTypeId,
                        principalTable: "TreatmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInvitationsTreatmentTypes_UserInvitations_UserInvitationId",
                        column: x => x.UserInvitationId,
                        principalTable: "UserInvitations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInvitationsTreatmentTypes_TreatmentTypeId",
                table: "UserInvitationsTreatmentTypes",
                column: "TreatmentTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInvitationsTreatmentTypes");
        }
    }
}

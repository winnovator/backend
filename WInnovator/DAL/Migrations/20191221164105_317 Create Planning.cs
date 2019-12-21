using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WInnovator.DAL.Migrations
{
    public partial class _317CreatePlanning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultTimeNeeded",
                table: "WorkingForm",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "WorkingForm",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "WorkingForm",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "PhaseId",
                table: "WorkingForm",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Resume",
                table: "WorkingForm",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DesignShopWorkingForm",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "DesignShopWorkingForm",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Implementer",
                table: "DesignShopWorkingForm",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "PhaseId",
                table: "DesignShopWorkingForm",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Resume",
                table: "DesignShopWorkingForm",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeAllocated",
                table: "DesignShopWorkingForm",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Starttime",
                table: "DesignShop",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateTable(
                name: "Phase",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phase", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingForm_PhaseId",
                table: "WorkingForm",
                column: "PhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DesignShopWorkingForm_PhaseId",
                table: "DesignShopWorkingForm",
                column: "PhaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_DesignShopWorkingForm_Phase_PhaseId",
                table: "DesignShopWorkingForm",
                column: "PhaseId",
                principalTable: "Phase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingForm_Phase_PhaseId",
                table: "WorkingForm",
                column: "PhaseId",
                principalTable: "Phase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DesignShopWorkingForm_Phase_PhaseId",
                table: "DesignShopWorkingForm");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingForm_Phase_PhaseId",
                table: "WorkingForm");

            migrationBuilder.DropTable(
                name: "Phase");

            migrationBuilder.DropIndex(
                name: "IX_WorkingForm_PhaseId",
                table: "WorkingForm");

            migrationBuilder.DropIndex(
                name: "IX_DesignShopWorkingForm_PhaseId",
                table: "DesignShopWorkingForm");

            migrationBuilder.DropColumn(
                name: "DefaultTimeNeeded",
                table: "WorkingForm");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "WorkingForm");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "WorkingForm");

            migrationBuilder.DropColumn(
                name: "PhaseId",
                table: "WorkingForm");

            migrationBuilder.DropColumn(
                name: "Resume",
                table: "WorkingForm");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "DesignShopWorkingForm");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "DesignShopWorkingForm");

            migrationBuilder.DropColumn(
                name: "Implementer",
                table: "DesignShopWorkingForm");

            migrationBuilder.DropColumn(
                name: "PhaseId",
                table: "DesignShopWorkingForm");

            migrationBuilder.DropColumn(
                name: "Resume",
                table: "DesignShopWorkingForm");

            migrationBuilder.DropColumn(
                name: "TimeAllocated",
                table: "DesignShopWorkingForm");

            migrationBuilder.DropColumn(
                name: "Starttime",
                table: "DesignShop");
        }
    }
}

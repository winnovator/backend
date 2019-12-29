using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Diagnostics.CodeAnalysis;

namespace WInnovator.DAL.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class Migrationadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DesignShopWorkingForm_DesignShop_IsCurrentWorkingFormId",
                table: "DesignShopWorkingForm");

            migrationBuilder.DropIndex(
                name: "IX_DesignShopWorkingForm_IsCurrentWorkingFormId",
                table: "DesignShopWorkingForm");

            migrationBuilder.DropColumn(
                name: "IsCurrentWorkingFormId",
                table: "DesignShopWorkingForm");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentDesignShopWorkingFormId",
                table: "DesignShop",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DesignShopWorkingForm",
                table: "DesignShop",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DesignShop_DesignShopWorkingForm",
                table: "DesignShop",
                column: "DesignShopWorkingForm",
                unique: true,
                filter: "[DesignShopWorkingForm] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_DesignShop_DesignShopWorkingForm_DesignShopWorkingForm",
                table: "DesignShop",
                column: "DesignShopWorkingForm",
                principalTable: "DesignShopWorkingForm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DesignShop_DesignShopWorkingForm_DesignShopWorkingForm",
                table: "DesignShop");

            migrationBuilder.DropIndex(
                name: "IX_DesignShop_DesignShopWorkingForm",
                table: "DesignShop");

            migrationBuilder.DropColumn(
                name: "DesignShopWorkingForm",
                table: "DesignShop");

            migrationBuilder.AddColumn<Guid>(
                name: "IsCurrentWorkingFormId",
                table: "DesignShopWorkingForm",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentDesignShopWorkingFormId",
                table: "DesignShop",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateIndex(
                name: "IX_DesignShopWorkingForm_IsCurrentWorkingFormId",
                table: "DesignShopWorkingForm",
                column: "IsCurrentWorkingFormId",
                unique: true,
                filter: "[IsCurrentWorkingFormId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_DesignShopWorkingForm_DesignShop_IsCurrentWorkingFormId",
                table: "DesignShopWorkingForm",
                column: "IsCurrentWorkingFormId",
                principalTable: "DesignShop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

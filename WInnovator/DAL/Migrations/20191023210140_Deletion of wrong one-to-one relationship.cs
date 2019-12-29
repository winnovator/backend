using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WInnovator.DAL.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class Deletionofwrongonetoonerelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DesignShop_DesignShopWorkingForm_DesignShopWorkingForm",
                table: "DesignShop");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId",
                table: "IdentityRoleClaim<string>");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserClaim<string>_IdentityUser_UserId",
                table: "IdentityUserClaim<string>");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserLogin<string>_IdentityUser_UserId",
                table: "IdentityUserLogin<string>");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserRole<string>_IdentityRole_RoleId",
                table: "IdentityUserRole<string>");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserRole<string>_IdentityUser_UserId",
                table: "IdentityUserRole<string>");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserToken<string>_IdentityUser_UserId",
                table: "IdentityUserToken<string>");

            migrationBuilder.DropIndex(
                name: "IX_DesignShop_DesignShopWorkingForm",
                table: "DesignShop");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserToken<string>",
                table: "IdentityUserToken<string>");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserRole<string>",
                table: "IdentityUserRole<string>");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserLogin<string>",
                table: "IdentityUserLogin<string>");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserClaim<string>",
                table: "IdentityUserClaim<string>");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUser",
                table: "IdentityUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityRoleClaim<string>",
                table: "IdentityRoleClaim<string>");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityRole",
                table: "IdentityRole");

            migrationBuilder.DropColumn(
                name: "CurrentDesignShopWorkingFormId",
                table: "DesignShop");

            migrationBuilder.DropColumn(
                name: "DesignShopWorkingForm",
                table: "DesignShop");

            migrationBuilder.RenameTable(
                name: "IdentityUserToken<string>",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "IdentityUserRole<string>",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "IdentityUserLogin<string>",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "IdentityUserClaim<string>",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "IdentityUser",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "IdentityRoleClaim<string>",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "IdentityRole",
                newName: "AspNetRoles");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityUserRole<string>_RoleId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityUserLogin<string>_UserId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityUserClaim<string>_UserId",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityRoleClaim<string>_RoleId",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrentWorkingForm",
                table: "DesignShopWorkingForm",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims");

            migrationBuilder.DropColumn(
                name: "IsCurrentWorkingForm",
                table: "DesignShopWorkingForm");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "IdentityUserToken<string>");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "IdentityUser");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "IdentityUserRole<string>");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "IdentityUserLogin<string>");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "IdentityUserClaim<string>");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "IdentityRole");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "IdentityRoleClaim<string>");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "IdentityUserRole<string>",
                newName: "IX_IdentityUserRole<string>_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "IdentityUserLogin<string>",
                newName: "IX_IdentityUserLogin<string>_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "IdentityUserClaim<string>",
                newName: "IX_IdentityUserClaim<string>_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "IdentityRoleClaim<string>",
                newName: "IX_IdentityRoleClaim<string>_RoleId");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentDesignShopWorkingFormId",
                table: "DesignShop",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DesignShopWorkingForm",
                table: "DesignShop",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserToken<string>",
                table: "IdentityUserToken<string>",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUser",
                table: "IdentityUser",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserRole<string>",
                table: "IdentityUserRole<string>",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserLogin<string>",
                table: "IdentityUserLogin<string>",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserClaim<string>",
                table: "IdentityUserClaim<string>",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityRole",
                table: "IdentityRole",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityRoleClaim<string>",
                table: "IdentityRoleClaim<string>",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId",
                table: "IdentityRoleClaim<string>",
                column: "RoleId",
                principalTable: "IdentityRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserClaim<string>_IdentityUser_UserId",
                table: "IdentityUserClaim<string>",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserLogin<string>_IdentityUser_UserId",
                table: "IdentityUserLogin<string>",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_IdentityRole_RoleId",
                table: "IdentityUserRole<string>",
                column: "RoleId",
                principalTable: "IdentityRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_IdentityUser_UserId",
                table: "IdentityUserRole<string>",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserToken<string>_IdentityUser_UserId",
                table: "IdentityUserToken<string>",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

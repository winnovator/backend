using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace WInnovator.DAL.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AppuseraccountforDesignShop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUseraccount",
                table: "DesignShop",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppUseraccount",
                table: "DesignShop");
        }
    }
}

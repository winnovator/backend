using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WInnovator.Data.Migrations
{
    public partial class AddingDesignShopandUploadImagefunctionality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DesignShop",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignShop", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UploadImageStore",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DesignShopId = table.Column<Guid>(nullable: false),
                    UploadedImage = table.Column<byte[]>(nullable: true),
                    Mimetype = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadImageStore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadImageStore_DesignShop_DesignShopId",
                        column: x => x.DesignShopId,
                        principalTable: "DesignShop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UploadImageStore_DesignShopId",
                table: "UploadImageStore",
                column: "DesignShopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploadImageStore");

            migrationBuilder.DropTable(
                name: "DesignShop");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace WInnovator.DAL.Migrations
{
    public partial class RenamingDescriptiontoNameofmodelWorkingForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "WorkingForm",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "WorkingForm",
                newName: "Description");
        }
    }
}

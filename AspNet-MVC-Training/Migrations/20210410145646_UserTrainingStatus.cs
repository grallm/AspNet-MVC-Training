using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNet_MVC_Training.Migrations
{
    public partial class UserTrainingStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Finished",
                table: "UserTraining",
                newName: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "UserTraining",
                newName: "Finished");
        }
    }
}

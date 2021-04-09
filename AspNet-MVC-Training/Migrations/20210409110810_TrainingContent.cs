using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNet_MVC_Training.Migrations
{
    public partial class TrainingContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Training",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Training",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Video",
                table: "Training",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Training");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Training");

            migrationBuilder.DropColumn(
                name: "Video",
                table: "Training");
        }
    }
}

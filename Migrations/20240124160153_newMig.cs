using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsApp.Migrations
{
    /// <inheritdoc />
    public partial class newMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Photo_PID",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_User_UID",
                table: "Articles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photo",
                table: "Photo");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Photo",
                newName: "Photos");

            migrationBuilder.AlterColumn<int>(
                name: "PID",
                table: "Articles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photos",
                table: "Photos",
                column: "PID");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Photos_PID",
                table: "Articles",
                column: "PID",
                principalTable: "Photos",
                principalColumn: "PID");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Users_UID",
                table: "Articles",
                column: "UID",
                principalTable: "Users",
                principalColumn: "UID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Photos_PID",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Users_UID",
                table: "Articles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photos",
                table: "Photos");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Photos",
                newName: "Photo");

            migrationBuilder.AlterColumn<int>(
                name: "PID",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "UID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photo",
                table: "Photo",
                column: "PID");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Photo_PID",
                table: "Articles",
                column: "PID",
                principalTable: "Photo",
                principalColumn: "PID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_User_UID",
                table: "Articles",
                column: "UID",
                principalTable: "User",
                principalColumn: "UID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

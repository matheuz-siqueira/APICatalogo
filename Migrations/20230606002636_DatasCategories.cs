using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    public partial class DatasCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .Sql("Insert into Categories(Name, ImageUrl) Values('Bebidas', 'bebidas.jpg')");            

            migrationBuilder
                .Sql("Insert into Categories(Name, ImageUrl) Values('Lanches', 'lanches.jpg')");

            migrationBuilder
                .Sql("Insert into Categories(Name, ImageUrl) Values('Sobremesas', 'sobremesas.jpg')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Categories");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations
{
    public partial class SeedCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            DateTime now = DateTime.UtcNow;

            migrationBuilder.InsertData(
                table: "Categories",
                columns: ["Id", "Name", "Active", "CreatedAt", "UpdatedAt"],
                values: new object[,]
                {
                    { Guid.NewGuid(), "Tecnologia", true, now, now },
                    { Guid.NewGuid(), "Educação", true, now, now },
                    { Guid.NewGuid(), "Ciência", true, now, now },
                    { Guid.NewGuid(), "Saúde", true, now, now },
                    { Guid.NewGuid(), "Economia", true, now, now },
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "DELETE FROM \"Categories\" WHERE \"Name\" IN ('Tecnologia', 'Educação', 'Ciência', 'Saúde', 'Economia')"
            );
        }
    }
}

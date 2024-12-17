using Microsoft.EntityFrameworkCore.Migrations;

public partial class addSpecsId : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Xóa ràng buộc khóa ngoại phụ thuộc vào ProductId
        migrationBuilder.DropForeignKey(
            name: "FK_CartDetails_Products_ProductId",
            table: "CartDetails");

        // Xóa chỉ mục trên ProductId
        migrationBuilder.DropIndex(
            name: "IX_CartDetails_productId",
            table: "CartDetails");

        // Xóa khóa chính phụ thuộc vào ProductId
        migrationBuilder.DropPrimaryKey(
            name: "PK_CartDetails",
            table: "CartDetails");

        // Xóa cột ProductId
        migrationBuilder.DropColumn(
            name: "ProductId",
            table: "CartDetails");

        // Thêm cột specId
        migrationBuilder.AddColumn<int>(
            name: "specId",
            table: "CartDetails",
            type: "int",
            nullable: false,
            defaultValue: 0);

        // Thêm ràng buộc khóa ngoại mới cho specId
        migrationBuilder.AddForeignKey(
            name: "FK_CartDetails_specs_specId",
            table: "CartDetails",
            column: "specId",
            principalTable: "specs",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Hoàn tác lại khi rollback
        migrationBuilder.DropForeignKey(
            name: "FK_CartDetails_specs_specId",
            table: "CartDetails");

        migrationBuilder.DropColumn(
            name: "specId",
            table: "CartDetails");

        migrationBuilder.AddColumn<int>(
            name: "ProductId",
            table: "CartDetails",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddForeignKey(
            name: "FK_CartDetails_Products_ProductId",
            table: "CartDetails",
            column: "ProductId",
            principalTable: "Products",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}

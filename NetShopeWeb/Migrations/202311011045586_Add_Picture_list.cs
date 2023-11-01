namespace NetShopeWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Picture_list : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductPictures",
                c => new
                    {
                        ProductPictureID = c.Int(nullable: false, identity: true),
                        PicturePath = c.String(),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductPictureID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);
            
            DropColumn("dbo.Products", "PicturePath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "PicturePath", c => c.String());
            DropForeignKey("dbo.ProductPictures", "ProductID", "dbo.Products");
            DropIndex("dbo.ProductPictures", new[] { "ProductID" });
            DropTable("dbo.ProductPictures");
        }
    }
}

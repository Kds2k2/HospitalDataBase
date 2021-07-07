namespace MyHospitalDataBaseInterface.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Visit");
            AlterColumn("dbo.Visit", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Visit", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Visit");
            AlterColumn("dbo.Visit", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Visit", new[] { "Id", "PatientId" });
        }
    }
}

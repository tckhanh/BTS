namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_StoreProcedure : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("GetCertificateStatistic",
                p => new
                {
                    fromDate = p.String(),
                    toDate = p.String()
                }
                ,
                @"
                    SELECT     b.OperatorID as DN, b.CityID as Tinh, COUNT(b.CertificateNum) AS SoLuong
                    FROM         BTSCertificates b 
                    WHERE     (NOT (b.CertificateNum IS NULL)) AND (b.IssuedDate >= CONVERT(DATETIME, '2016-01-01', 102)) and (b.IssuedDate <= CONVERT(DATETIME, '2016-05-01', 102))
                    GROUP BY b.OperatorID, b.CityID 
                    ORDER BY b.OperatorID, b.CityID
                "
                );
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.GetCertificateStatistic");
        }
    }
}

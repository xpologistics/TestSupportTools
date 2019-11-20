using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Xpo.LastMile.TestSupportTool.Data.Models;

namespace Xpo.LastMile.TestSupportTool.Data
{
    public class Repository : IRepository
    {
        public Repository()
        {

        }

        public void testmethod(String referenceNumber)
        {
            var CLMOutput = GetCLMOutput(referenceNumber);
            var DMSOutput = GetDMSOutput(referenceNumber);
        }

        private IEnumerable<CLMOutput> GetCLMOutput(string referenceNumber)
        {
            IEnumerable<CLMOutput> result;
            try
            {
                using (var db = new SqlConnection("Data Source=lmtdw2560.qaamer.qacorp.xpo.com; Initial Catalog=RouteManagement; User Id=Developer; Password=Developer1;"))
                {
                    db.Open();
                    string CLMQueryString = (@"select
                    SalesOrderId,
                    WorkOrerId,
                    CustomerOrderNumber,
                    SalesOrderReferenceNumber,
                    AccountName,
                    ScheduledDate,
                    CONVERT(VARCHAR, ServiceWindowStart, 120) + '-' + CONVERT(VARCHAR, ServiceWindowEnd, 120) as ServiceWindow,
                    ConsumerName,
                    ConsumerAddress,
                    PhoneNumber,
                    EmailAddress,
                    LocationTypeName,
                    DestinationAddress,
                    FacilityName,
                    FacilityTypeName,
                    PostalZoneAreaName,
                    CSG,
                    sku,
                    ProductName,
                    Quantity,
                    SalesOrderProductId,
                    Weight,
                    WeightUnitName,
                    Height,
                    Length,
                    Width,
                    DimensionUnitName,
                    cube,
                    Barcode,
                    OriginInstruction,
                    DstinationInstruction,
                    ExpectedHubReceiptDate,
                    Distance from
                    (
                    select so.SalesOrderId, wo.WorkOrderId as WorkOrerId, con.CustomerOrderNumber as CustomerOrderNumber, srn.SalesOrderReferenceNumber as SalesOrderReferenceNumber,
                    ac.AccountName as AccountName, CONVERT(date, wo.DestinationEarlyScheduledArrival) as ScheduledDate,
                    CONVERT(time, wo.DestinationEarlyScheduledArrival) as ServiceWindowStart, CONVERT(time, wo.DestinationLateScheduledArrival) as ServiceWindowEnd,
                    (CASE WHEN(ct.FirstName is null)Then ' ' ELSE ct.FirstName END) + ' ' + (CASE WHEN(ct.LastName is NULL) THEN ' ' ELSE ct.LastName END) as ConsumerName,
                    (CASE WHEN(loc1.StreetNumber is null)THEN ' ' ELSE loc1.StreetNumber END)+' ' + loc1.Street + ',' + loc1.Locality + ',' + loc1.Admin1 + ',' + loc1.PostalCode + ',' + loc1.CountryCode as ConsumerAddress,
                    (CASE WHEN(ct.PhoneNumber is null)THEN '9999999999' ELSE ct.PhoneNumber END) as PhoneNumber, ct.EmailAddress as EmailAddress, lt.LocationTypeName as LocationTypeName, 
                    (CASE WHEN(loc.StreetNumber is null)THEN ' ' ELSE loc.StreetNumber END)+' ' + loc.Street + ',' + loc.Locality + ',' + loc.Admin1 + ',' + loc.PostalCode + ',' + loc.CountryCode as DestinationAddress, 
                    fac.FacilityName as FacilityName, factype.FacilityTypeName as FacilityTypeName,fac.PostalZoneAreaName as PostalZoneAreaName, csg.ContractServiceGroupName as CSG, 
                    sop.SKU AS sku, sop.Description as ProductName, sop.Quantity as Quantity, sop.SalesOrderProductId as SalesOrderProductId, 
                    fr.Weight as Weight, wt.WeightUnitName as WeightUnitName, fr.Height as Height, fr.Length as Length, fr.Width as Width, dim.DimensionUnitName as DimensionUnitName, 
                    fr.Length* fr.Width* fr.Height * 0.000578704 as cube,
                    PackageIdentifier as Barcode, woi1.Instructions as OriginInstruction,woi2.Instructions as DstinationInstruction, 
                    CONVERT(date, wo.ExpectedHubReceiptDate) as ExpectedHubReceiptDate, wo.Distance as Distance
                    from[OrderManagement].[dbo].[SalesOrders] so with(NOLOCK)
                    Left Outer Join[OrderManagement].[dbo].[WorkOrders] wo with(NOLOCK) on wo.SalesOrderId = so.SalesOrderId
                    Left Outer Join[OrderManagement].[dbo].[CustomerOrderNumbers] con with(NOLOCK) on con.SalesOrderId = so.SalesOrderId
                    Left Outer Join[OrderManagement].[dbo].[SalesOrderReferenceNumbers] srn with(NOLOCK) on srn.CustomerOrderNumberId = con.CustomerOrderNumberId
                    Left Outer Join[OrderManagement].[dbo].[Contacts] ct with(NOLOCK) on ct.ContactId = so.OriginContactId
                    Left Outer Join[AccountManagement].[dbo].[Accounts] ac with(NOLOCK) on ac.AccountId = so.AccountId
                    Left Outer Join[LocationManagement].[dbo].[Locations] loc with(NOLOCK) on loc.LocationId = so.DestinationLocationId
                    Left Outer Join[LocationManagement].[dbo].[Locations] loc1 with(NOLOCK) on loc1.LocationId = so.OriginLocationId
                    Left Outer Join[LocationManagement].[dbo].[LocationType] lt with(NOLOCK) on lt.LocationTypeId = loc.LocationTypeId
                    Left Outer Join[FacilityManagement].[dbo].[Facilities] fac with(NOLOCK) on fac.FacilityId = so.DestinationFacilityId
                    Left Outer Join[FacilityManagement].[dbo].[FacilityType] factype with(NOLOCK) on factype.FacilityTypeId = fac.FacilityTypeId
                    Left Outer Join[OrderManagement].[dbo].[WorkOrderServiceGroups] wsg with(NOLOCK) on wsg.WorkOrderId = wo.WorkOrderId
                    Left Outer Join[AccountManagement].[dbo].[ContractServiceGroups] csg with(NOLOCK) on csg.ContractServiceGroupId = wsg.ContractServiceGroupId
                    Left Outer Join[OrderManagement].[dbo].[SalesOrderProducts] sop with(NOLOCK) on sop.CustomerOrderNumberId = con.CustomerOrderNumberId
                    Left Outer Join[AccountManagement].[dbo].[AccountProducts] ap with(NOLOCK) on ap.AccountId = so.AccountId and ap.SKU = sop.SKU
                    Left Outer Join[Reference].[dbo].[DimensionUnits] dim with(NOLOCK) on dim.DimensionUnitId = ap.DimensionUnitId
                    Left Outer Join[Reference].[dbo].[WeightUnits] wt with(NOLOCK) on wt.WeightUnitId = ap.WeightUnitId
                    Left Outer Join[OrderManagement].[dbo].[ProductFreight] pf with(NOLOCK) on pf.SalesOrderProductId = sop.SalesOrderProductId
                    Left Outer Join[FreightManagement].[dbo].[Freight] fr with(NOLOCK) on fr.FreightId = pf.FreightId
                    Left Outer Join[OrderManagement].[dbo].[WorkOrderInstructions] woi1 with(NOLOCK) on woi1.WorkOrderId = wo.WorkOrderId and woi1.IsOriginInstructions = 1
                    Left Outer Join[OrderManagement].[dbo].[WorkOrderInstructions] woi2 with(NOLOCK) on woi2.WorkOrderId = wo.WorkOrderId and woi2.IsOriginInstructions = 0
                    where con.CustomerOrderNumber = '{0}' and csg.IsEnabled = 1 and csg.IsAutoBook = 0) CLM");
                    
                    result = db.Query<CLMOutput>(string.Format(CLMQueryString, referenceNumber));
                    db.Close();
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            
        }

        private IEnumerable<CLMOutput> GetDMSOutput(string referenceNumber)
        {
            IEnumerable<CLMOutput> result;
            try
            {
                using (var db = new SqlConnection(@"Data Source=DMSDBPRD04SCL.3pdelivery.com\VPRODUCTION01; integrated security=SSPI; persist security info=False; Trusted_Connection = Yes"))
                {
                    db.Open();
                    string CLMQueryString = (@"");

                    result = db.Query<CLMOutput>(string.Format(CLMQueryString, referenceNumber));
                    db.Close();
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}

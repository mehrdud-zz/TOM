using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Web;

namespace ModelsLayer
{

    public partial class ClaimsEntities: IClaimsEntities
    { 
        public override int SaveChanges()
        {
            var trackables = (ChangeTracker.Entries<ClaimFieldTemplate>());
            if (trackables != null)
            {

                foreach (var item in trackables)
                {
                    if (String.IsNullOrEmpty(item.Entity.Code))
                    {
                        item.Entity.Code =
                            (DateTime.Now.Ticks.ToString() +
                             (item.Entity.Name != null
                                 ? item.Entity.Name.Trim().Replace(" ", String.Empty)
                                 : String.Empty));

                        if (item.Entity.Code.Length > 50)
                            item.Entity.Code = item.Entity.Code.Substring(0, 50);
                    }
                }
            }

            try
            {
                return base.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {

                var sb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                                    eve.Entry.Entity.GetType().Name,
                                                    eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                    ve.PropertyName,
                                                    ve.ErrorMessage));
                    }
                }
                throw new DbEntityValidationException(sb.ToString(), e);
            }
        }
    }


    public interface IClaimsEntities
    {
       
        DbSet<ClaimField> ClaimFields { get; }
        DbSet<ClaimFieldGroup> ClaimFieldGroups { get; }
        DbSet<ClaimFieldGroupTemplate> ClaimFieldGroupTemplates { get; }
        DbSet<ClaimFieldTemplate> ClaimFieldTemplates { get; }
        DbSet<ClaimStatu> ClaimStatus { get; }
        DbSet<ClaimTemplate> ClaimTemplates { get; }
        DbSet<Client> Clients { get; }
        DbSet<Country> Countries { get; }
        DbSet<Currency> Currencies { get; }
        DbSet<FieldType> FieldTypes { get; }
        DbSet<WillisEmployee> WillisEmployees { get; }
        DbSet<ReportTemplate> ReportTemplates { get; } 
        DbSet<Claim> Claims { get; }
        DbSet<User> Users { get; }
        DbSet<ReportField> ReportFields { get; }
        DbSet<PageElement> PageElements { get; }
        DbSet<PageSetup> PageSetups { get; }

        int SaveChanges();

         DbEntityEntry Entry(object entity);
    }
}
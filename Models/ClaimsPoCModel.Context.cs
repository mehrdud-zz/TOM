﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ModelsLayer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ClaimsEntities : DbContext
    {
        public ClaimsEntities()
            : base("name=ClaimsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        virtual public DbSet<ClaimField> ClaimFields { get; set; }
        virtual public DbSet<ClaimFieldGroup> ClaimFieldGroups { get; set; }
        virtual public DbSet<ClaimFieldGroupTemplate> ClaimFieldGroupTemplates { get; set; }
        virtual public DbSet<ClaimFieldTemplate> ClaimFieldTemplates { get; set; }
        virtual public DbSet<ClaimStatu> ClaimStatus { get; set; }
        virtual public DbSet<ClaimTemplate> ClaimTemplates { get; set; }
        virtual public DbSet<Client> Clients { get; set; }
        virtual public DbSet<Country> Countries { get; set; }
        virtual public DbSet<Currency> Currencies { get; set; }
        virtual public DbSet<FieldType> FieldTypes { get; set; }
        virtual public DbSet<WillisEmployee> WillisEmployees { get; set; }
        virtual public DbSet<ReportTemplate> ReportTemplates { get; set; }
        virtual public DbSet<aspnet_Applications> aspnet_Applications { get; set; }
        virtual public DbSet<aspnet_Membership> aspnet_Membership { get; set; }
        virtual public DbSet<aspnet_Paths> aspnet_Paths { get; set; }
        virtual public DbSet<aspnet_PersonalizationAllUsers> aspnet_PersonalizationAllUsers { get; set; }
        virtual public DbSet<aspnet_PersonalizationPerUser> aspnet_PersonalizationPerUser { get; set; }
        virtual public DbSet<aspnet_Profile> aspnet_Profile { get; set; }
        virtual public DbSet<aspnet_Roles> aspnet_Roles { get; set; }
        virtual public DbSet<aspnet_SchemaVersions> aspnet_SchemaVersions { get; set; }
        virtual public DbSet<aspnet_Users> aspnet_Users { get; set; }
        virtual public DbSet<aspnet_WebEvent_Events> aspnet_WebEvent_Events { get; set; }
        virtual public DbSet<Claim> Claims { get; set; }
        virtual public DbSet<User> Users { get; set; }
        virtual public DbSet<ReportField> ReportFields { get; set; }
        virtual public DbSet<PageElement> PageElements { get; set; }
        virtual public DbSet<PageSetup> PageSetups { get; set; }
    }
}
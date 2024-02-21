using System;
using System.Collections.Generic;
using System.Text;
using Insurance.Models;
using Insurance.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Insurance.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
       

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

    
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }     
        public DbSet<Menu> Menus { get; set; }          
        public DbSet<Aaction> Aactions { get; set; }
        //public DbSet<Setup> Setups { get; set; }       
        public DbSet<UserRightAction> UserRightActions { get; set; }        
        public DbSet<RoleRightAction> RoleRightActions { get; set; }
    
        public DbSet<Audit > AuditLogs { get; set; }



        //---------------------------------------------------------------

       public DbSet<Student> Student { get; set; }
      

        //----------------------Insurance App----------------------------

        public DbSet<DropDownList> DropDownList { get; set; }
    
        public DbSet<Team> Teams { get; set; }
        public DbSet<UserTeam> UserTeam { get; set; }

        public DbSet<SMTPSettings> SMTPSetting { get; set; }
  
        public DbSet<ExceptionsLogs> ExceptionsLogs { get; set; }  

       


        //---------------------------------------------------------------
        public virtual int SaveChanges(string userId = null)
        {
            OnBeforeSaveChanges(userId);
            var result = base.SaveChanges();
            return result;
        }
        public virtual int SaveChangesWH()
        {        
            var result = base.SaveChanges();
            return result;
        }

        private void OnBeforeSaveChanges(string userId)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = userId;
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }
        }
    }
}



using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class M3Pact_DevContext : DbContext
    {
        public virtual DbSet<Apimethod> Apimethod { get; set; }
        public virtual DbSet<ApimethodsRoleMap> ApimethodsRoleMap { get; set; }
        public virtual DbSet<ApiResource> ApiResource { get; set; }
        public virtual DbSet<Application> Application { get; set; }
        public virtual DbSet<ApplicationApiResource> ApplicationApiResource { get; set; }
        public virtual DbSet<ApplicationType> ApplicationType { get; set; }
        public virtual DbSet<BusinessDays> BusinessDays { get; set; }
        public virtual DbSet<BusinessUnit> BusinessUnit { get; set; }
        public virtual DbSet<CheckList> CheckList { get; set; }
        public virtual DbSet<CheckListAttribute> CheckListAttribute { get; set; }
        public virtual DbSet<CheckListAttributeMap> CheckListAttributeMap { get; set; }
        public virtual DbSet<CheckListType> CheckListType { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<ClientCheckListMap> ClientCheckListMap { get; set; }
        public virtual DbSet<ClientCheckListQuestionResponse> ClientCheckListQuestionResponse { get; set; }
        public virtual DbSet<ClientCheckListStatusDetail> ClientCheckListStatusDetail { get; set; }
        public virtual DbSet<ClientCloseMonth> ClientCloseMonth { get; set; }
        public virtual DbSet<ClientConfigStep> ClientConfigStep { get; set; }
        public virtual DbSet<ClientConfigStepDetail> ClientConfigStepDetail { get; set; }
        public virtual DbSet<ClientConfigStepStatus> ClientConfigStepStatus { get; set; }
        public virtual DbSet<ClientKpimap> ClientKpimap { get; set; }
        public virtual DbSet<ClientKpiuserMap> ClientKpiuserMap { get; set; }
        public virtual DbSet<ClientPayer> ClientPayer { get; set; }
        public virtual DbSet<ClientTarget> ClientTarget { get; set; }
        public virtual DbSet<DateDimension> DateDimension { get; set; }
        public virtual DbSet<DepositLog> DepositLog { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Holiday> Holiday { get; set; }
        public virtual DbSet<Kpi> Kpi { get; set; }
        public virtual DbSet<Kpialert> Kpialert { get; set; }
        public virtual DbSet<Kpimeasure> Kpimeasure { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<M3metricsQuestion> M3metricsQuestion { get; set; }
        public virtual DbSet<Month> Month { get; set; }
        public virtual DbSet<Payer> Payer { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<RoleAction> RoleAction { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Screen> Screen { get; set; }
        public virtual DbSet<ScreenAction> ScreenAction { get; set; }
        public virtual DbSet<ScreenUserRoleMap> ScreenUserRoleMap { get; set; }
        public virtual DbSet<Site> Site { get; set; }
        public virtual DbSet<Speciality> Speciality { get; set; }
        public virtual DbSet<System> System { get; set; }
        public virtual DbSet<Title> Title { get; set; }
        public virtual DbSet<UserClientMap> UserClientMap { get; set; }
        public virtual DbSet<UserLogin> UserLogin { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=ggk-upl-dev-mdn\sql16;Database=M3Pact_Dev;user id=MeridianUser;password=Hyderabad007");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Apimethod>(entity =>
            {
                entity.ToTable("APIMethod");

                entity.Property(e => e.ApimethodId).HasColumnName("APIMethodId");

                entity.Property(e => e.ApicontrollerName)
                    .IsRequired()
                    .HasColumnName("APIControllerName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Apimethod1)
                    .IsRequired()
                    .HasColumnName("APIMethod")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("date");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ApimethodsRoleMap>(entity =>
            {
                entity.ToTable("APIMethodsRoleMap");

                entity.Property(e => e.ApimethodId).HasColumnName("APIMethodId");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("date");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Apimethod)
                    .WithMany(p => p.ApimethodsRoleMap)
                    .HasForeignKey(d => d.ApimethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_APIMethodId_APIMethodsRoleMap");

                
            });

            modelBuilder.Entity<ApiResource>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Application>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.RedirectUrl)
                    .HasColumnName("RedirectURL")
                    .IsUnicode(false);

                entity.Property(e => e.Secret)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Application)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK__Applicati__TypeI__4C0144E4");
            });

            modelBuilder.Entity<ApplicationApiResource>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApiResourceId).HasColumnName("ApiResourceID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.HasOne(d => d.ApiResource)
                    .WithMany(p => p.ApplicationApiResource)
                    .HasForeignKey(d => d.ApiResourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__ApiRe__50C5FA01");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationApiResource)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__Appli__51BA1E3A");
            });

            modelBuilder.Entity<ApplicationType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<BusinessDays>(entity =>
            {
                entity.Property(e => e.BusinessDaysId).HasColumnName("BusinessDaysID");

                entity.Property(e => e.BusinessDays1).HasColumnName("BusinessDays");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MonthId).HasColumnName("MonthID");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.HasOne(d => d.Month)
                    .WithMany(p => p.BusinessDays)
                    .HasForeignKey(d => d.MonthId)
                    .HasConstraintName("FK__BusinessD__Month__498EEC8D");
            });

            modelBuilder.Entity<BusinessUnit>(entity =>
            {
                entity.Property(e => e.BusinessUnitId).HasColumnName("BusinessUnitID");

                entity.Property(e => e.BusinessUnitCode).HasMaxLength(255);

                entity.Property(e => e.BusinessUnitDescription).HasMaxLength(255);

                entity.Property(e => e.BusinessUnitName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.BusinessUnit)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("FK__BusinessU__SiteI__1EA48E88");
            });

            modelBuilder.Entity<CheckList>(entity =>
            {
                entity.Property(e => e.CheckListId).HasColumnName("CheckListID");

                entity.Property(e => e.CheckListName)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CheckListTypeId).HasColumnName("CheckListTypeID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.HasOne(d => d.CheckListType)
                    .WithMany(p => p.CheckList)
                    .HasForeignKey(d => d.CheckListTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CheckList_CheckListType");
            });

            modelBuilder.Entity<CheckListAttribute>(entity =>
            {
                entity.Property(e => e.CheckListAttributeId).HasColumnName("CheckListAttributeID");

                entity.Property(e => e.AttributeCode)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.AttributeName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");
            });

            modelBuilder.Entity<CheckListAttributeMap>(entity =>
            {
                entity.Property(e => e.CheckListAttributeMapId).HasColumnName("CheckListAttributeMapID");

                entity.Property(e => e.CheckListAttributeId).HasColumnName("CheckListAttributeID");

                entity.Property(e => e.CheckListAttributeValueId)
                    .HasColumnName("CheckListAttributeValueID")
                    .HasMaxLength(50);

                entity.Property(e => e.CheckListId).HasColumnName("CheckListID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.CheckListAttribute)
                    .WithMany(p => p.CheckListAttributeMap)
                    .HasForeignKey(d => d.CheckListAttributeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CheckListAttributeMap_CheckListAttribute");

                entity.HasOne(d => d.CheckList)
                    .WithMany(p => p.CheckListAttributeMap)
                    .HasForeignKey(d => d.CheckListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CheckListAttributeMap_CheckList");
            });

            modelBuilder.Entity<CheckListType>(entity =>
            {
                entity.Property(e => e.CheckListTypeId).HasColumnName("CheckListTypeID");

                entity.Property(e => e.CheckListTypeCode).HasMaxLength(60);

                entity.Property(e => e.CheckListTypeName)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.Acronym).HasMaxLength(100);

                entity.Property(e => e.BillingManagerId).HasColumnName("BillingManagerID");

                entity.Property(e => e.BusinessUnitId).HasColumnName("BusinessUnitID");

                entity.Property(e => e.ClientCode)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ContactEmail).HasMaxLength(100);

                entity.Property(e => e.ContactName).HasMaxLength(100);

                entity.Property(e => e.ContactPhone).HasMaxLength(100);

                entity.Property(e => e.ContractEndDate).HasColumnType("datetime");

                entity.Property(e => e.ContractStartDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MonthlyCheckListId).HasColumnName("MonthlyCheckListID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PercentageOfCash).HasColumnType("numeric(5, 2)");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.RelationShipManagerId).HasColumnName("RelationShipManagerID");

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.SpecialityId).HasColumnName("SpecialityID");

                entity.Property(e => e.SystemId).HasColumnName("SystemID");


                entity.Property(e => e.WeeklyCheckListId).HasColumnName("WeeklyCheckListID");

                entity.HasOne(d => d.BillingManager)
                    .WithMany(p => p.ClientBillingManager)
                    .HasForeignKey(d => d.BillingManagerId)
                    .HasConstraintName("FK__Client__BillingM__2022C2A6");

                entity.HasOne(d => d.BusinessUnit)
                    .WithMany(p => p.Client)
                    .HasForeignKey(d => d.BusinessUnitId)
                    .HasConstraintName("FK__Client__Business__236943A5");

                entity.HasOne(d => d.MonthlyCheckList)
                    .WithMany(p => p.ClientMonthlyCheckList)
                    .HasForeignKey(d => d.MonthlyCheckListId)
                    .HasConstraintName("FK__Client__MonthlyC__245D67DE");

                entity.HasOne(d => d.RelationShipManager)
                    .WithMany(p => p.ClientRelationShipManager)
                    .HasForeignKey(d => d.RelationShipManagerId)
                    .HasConstraintName("FK__Client__Relation__2116E6DF");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.Client)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("FK__Client__Site");

                entity.HasOne(d => d.Speciality)
                    .WithMany(p => p.Client)
                    .HasForeignKey(d => d.SpecialityId)
                    .HasConstraintName("FK__Client__Speciali__25518C17");

                entity.HasOne(d => d.System)
                    .WithMany(p => p.Client)
                    .HasForeignKey(d => d.SystemId)
                    .HasConstraintName("FK__Client__SystemID__14E61A24");

                entity.HasOne(d => d.WeeklyCheckList)
                    .WithMany(p => p.ClientWeeklyCheckList)
                    .HasForeignKey(d => d.WeeklyCheckListId)
                    .HasConstraintName("FK__Client__WeeklyCh__2739D489");
            });

            modelBuilder.Entity<ClientCheckListMap>(entity =>
            {
                entity.Property(e => e.ClientCheckListMapId).HasColumnName("ClientCheckListMapID");

                entity.Property(e => e.CheckListId).HasColumnName("CheckListID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.CheckList)
                    .WithMany(p => p.ClientCheckListMap)
                    .HasForeignKey(d => d.CheckListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientCheckListMap_CheckList");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientCheckListMap)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientCheckListMap_Client");
            });

            modelBuilder.Entity<ClientCheckListQuestionResponse>(entity =>
            {
                entity.Property(e => e.ClientCheckListQuestionResponseId).HasColumnName("ClientCheckListQuestionResponseID");

                entity.Property(e => e.CheckListAttributeMapId).HasColumnName("CheckListAttributeMapID");

                entity.Property(e => e.ClientCheckListMapId).HasColumnName("ClientCheckListMapID");

                entity.Property(e => e.ClientCheckListStatusDetailId).HasColumnName("ClientCheckListStatusDetailID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FreeFormResponse).HasMaxLength(500);

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.HasOne(d => d.CheckListAttributeMap)
                    .WithMany(p => p.ClientCheckListQuestionResponse)
                    .HasForeignKey(d => d.CheckListAttributeMapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientCheckListQuestionResponse_CheckListAttributeMap");

                entity.HasOne(d => d.ClientCheckListMap)
                    .WithMany(p => p.ClientCheckListQuestionResponse)
                    .HasForeignKey(d => d.ClientCheckListMapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientCheckListQuestionResponse_ClientCheckListMap");

                entity.HasOne(d => d.ClientCheckListStatusDetail)
                    .WithMany(p => p.ClientCheckListQuestionResponse)
                    .HasForeignKey(d => d.ClientCheckListStatusDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientCheckListQuestionResponse_ClientCheckListStatusDetail");
            });

            modelBuilder.Entity<ClientCheckListStatusDetail>(entity =>
            {
                entity.Property(e => e.ClientCheckListStatusDetailId).HasColumnName("ClientCheckListStatusDetailID");

                entity.Property(e => e.CheckListEffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.ChecklistStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.ClientCheckListMapId).HasColumnName("ClientCheckListMapID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.HasOne(d => d.ClientCheckListMap)
                    .WithMany(p => p.ClientCheckListStatusDetail)
                    .HasForeignKey(d => d.ClientCheckListMapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientCheckListStatusDetail_ClientCheckListMap");
            });

            modelBuilder.Entity<ClientCloseMonth>(entity =>
            {
                entity.Property(e => e.ClientCloseMonthId).HasColumnName("ClientCloseMonthID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.ClosedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MonthId).HasColumnName("MonthID");

                entity.Property(e => e.MonthStatus)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ReOpenDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientCloseMonth)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientClo__Clien__3A179ED3");

                entity.HasOne(d => d.Month)
                    .WithMany(p => p.ClientCloseMonth)
                    .HasForeignKey(d => d.MonthId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientClo__Month__3B0BC30C");
            });

            modelBuilder.Entity<ClientConfigStep>(entity =>
            {
                entity.Property(e => e.ClientConfigStepId).HasColumnName("ClientConfigStepID");

                entity.Property(e => e.ClientConfigStepDescription)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ClientConfigStepName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.ScreenCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ScreenCodeNavigation)
                    .WithMany(p => p.ClientConfigStep)
                    .HasPrincipalKey(p => p.ScreenCode)
                    .HasForeignKey(d => d.ScreenCode)
                    .HasConstraintName("FK__ClientCon__Scree__7D98A078");
            });

            modelBuilder.Entity<ClientConfigStepDetail>(entity =>
            {
                entity.Property(e => e.ClientConfigStepDetailId).HasColumnName("ClientConfigStepDetailID");

                entity.Property(e => e.ClientConfigStepId).HasColumnName("ClientConfigStepID");

                entity.Property(e => e.ClientConfigStepStatusId).HasColumnName("ClientConfigStepStatusID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.HasOne(d => d.ClientConfigStep)
                    .WithMany(p => p.ClientConfigStepDetail)
                    .HasForeignKey(d => d.ClientConfigStepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientCon__Clien__32767D0B");

                entity.HasOne(d => d.ClientConfigStepStatus)
                    .WithMany(p => p.ClientConfigStepDetail)
                    .HasForeignKey(d => d.ClientConfigStepStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientCon__Clien__336AA144");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientConfigStepDetail)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientCon__Clien__318258D2");
            });

            modelBuilder.Entity<ClientConfigStepStatus>(entity =>
            {
                entity.Property(e => e.ClientConfigStepStatusId).HasColumnName("ClientConfigStepStatusID");

                entity.Property(e => e.ClientConfigStepStatusDescription)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ClientConfigStepStatusName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");
            });

            modelBuilder.Entity<ClientKpimap>(entity =>
            {
                entity.ToTable("ClientKPIMap");

                entity.Property(e => e.ClientKpimapId).HasColumnName("ClientKPIMapID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.ClientStandard)
                    .HasColumnName("Client Standard")
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.IsSla).HasColumnName("IsSLA");

                entity.Property(e => e.Kpiid).HasColumnName("KPIID");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientKpimap)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientKPI__Clien__6C390A4C");

                entity.HasOne(d => d.Kpi)
                    .WithMany(p => p.ClientKpimap)
                    .HasForeignKey(d => d.Kpiid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientKPI__KPIID__6D2D2E85");
            });

            modelBuilder.Entity<ClientKpiuserMap>(entity =>
            {
                entity.ToTable("ClientKPIUserMap");

                entity.Property(e => e.ClientKpiuserMapId).HasColumnName("ClientKPIUserMapID");

                entity.Property(e => e.ClientKpimapId).HasColumnName("ClientKPIMapID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.HasOne(d => d.ClientKpimap)
                    .WithMany(p => p.ClientKpiuserMap)
                    .HasForeignKey(d => d.ClientKpimapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientKPI__Clien__75C27486");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ClientKpiuserMap)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientKPI__UserI__76B698BF");
            });

            modelBuilder.Entity<ClientPayer>(entity =>
            {
                entity.Property(e => e.ClientPayerId).HasColumnName("ClientPayerID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsM3feeExempt).HasColumnName("IsM3FeeExempt");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PayerId).HasColumnName("PayerID");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientPayer)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK__ClientPay__Clien__282DF8C2");

                entity.HasOne(d => d.Payer)
                    .WithMany(p => p.ClientPayer)
                    .HasForeignKey(d => d.PayerId)
                    .HasConstraintName("FK__ClientPay__Payer__29221CFB");
            });

            modelBuilder.Entity<ClientTarget>(entity =>
            {
                entity.Property(e => e.ClientTargetId).HasColumnName("ClientTargetID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.GrossCollectionRate).HasColumnType("numeric(5, 2)");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MonthId).HasColumnName("MonthID");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientTarget)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK__ClientTar__Clien__3F115E1A");

                entity.HasOne(d => d.Month)
                    .WithMany(p => p.ClientTarget)
                    .HasForeignKey(d => d.MonthId)
                    .HasConstraintName("FK__ClientTar__Month__40058253");
            });

            modelBuilder.Entity<DateDimension>(entity =>
            {
                entity.HasKey(e => e.DateKey);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DowinMonth).HasColumnName("DOWInMonth");

                entity.Property(e => e.FirstDayOfMonth).HasColumnType("date");

                entity.Property(e => e.FirstDayOfNextMonth).HasColumnType("date");

                entity.Property(e => e.FirstDayOfNextYear).HasColumnType("date");

                entity.Property(e => e.FirstDayOfQuarter).HasColumnType("date");

                entity.Property(e => e.FirstDayOfYear).HasColumnType("date");

                entity.Property(e => e.HolidayText).HasMaxLength(64);

                entity.Property(e => e.IsoweekOfYear).HasColumnName("ISOWeekOfYear");

                entity.Property(e => e.LastDayOfMonth).HasColumnType("date");

                entity.Property(e => e.LastDayOfQuarter).HasColumnType("date");

                entity.Property(e => e.LastDayOfYear).HasColumnType("date");

                entity.Property(e => e.Mmyyyy)
                    .IsRequired()
                    .HasColumnName("MMYYYY")
                    .HasColumnType("char(6)");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MonthName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MonthYear)
                    .IsRequired()
                    .HasColumnType("char(7)");

                entity.Property(e => e.QuarterName)
                    .IsRequired()
                    .HasMaxLength(6);

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(7)");

                entity.Property(e => e.WeekDayName)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<DepositLog>(entity =>
            {
                entity.Property(e => e.DepositLogId).HasColumnName("DepositLogID");

                entity.Property(e => e.Amount).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.ClientPayerId).HasColumnName("ClientPayerID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DepositDateId).HasColumnName("DepositDateID");

                entity.Property(e => e.IsM3feeExempt).HasColumnName("IsM3FeeExempt");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.HasOne(d => d.ClientPayer)
                    .WithMany(p => p.DepositLog)
                    .HasForeignKey(d => d.ClientPayerId)
                    .HasConstraintName("FK__DepositLo__Clien__69FBBC1F");

                entity.HasOne(d => d.DepositDate)
                    .WithMany(p => p.DepositLog)
                    .HasForeignKey(d => d.DepositDateId)
                    .HasConstraintName("FK__DepositLo__Depos__6AEFE058");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BusinessUnit).HasMaxLength(50);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.MobileNumber).HasMaxLength(50);

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("('A')");

                entity.Property(e => e.ReportsTo).HasMaxLength(100);

                entity.Property(e => e.Role).HasMaxLength(100);

                entity.Property(e => e.Site).HasMaxLength(100);

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<Holiday>(entity =>
            {
                entity.Property(e => e.HolidayId).HasColumnName("HolidayID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.HolidayDate).HasColumnType("datetime");

                entity.Property(e => e.HolidayDescription).HasMaxLength(255);

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");
            });

            modelBuilder.Entity<Kpi>(entity =>
            {
                entity.ToTable("KPI");

                entity.Property(e => e.Kpiid).HasColumnName("KPIID");

                entity.Property(e => e.AlertLevel).HasMaxLength(500);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Kpidescription)
                    .IsRequired()
                    .HasColumnName("KPIDescription")
                    .HasMaxLength(60);

                entity.Property(e => e.KpimeasureId).HasColumnName("KPIMeasureID");

                entity.Property(e => e.Measure).HasMaxLength(50);

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.HasOne(d => d.CheckListType)
                    .WithMany(p => p.Kpi)
                    .HasForeignKey(d => d.CheckListTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KPI_CHECKLISTTYPEID");

                entity.HasOne(d => d.Kpimeasure)
                    .WithMany(p => p.Kpi)
                    .HasForeignKey(d => d.KpimeasureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KPI_KPIMEASUREID");
            });

            modelBuilder.Entity<Kpialert>(entity =>
            {
                entity.ToTable("KPIAlert");

                entity.Property(e => e.KpialertId).HasColumnName("KPIAlertID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EscalateAlertTitle).HasMaxLength(400);

                entity.Property(e => e.EscalateTriggerTime).HasMaxLength(60);

                entity.Property(e => e.IncludeKpitarget).HasColumnName("IncludeKPItarget");

                entity.Property(e => e.IsSla).HasColumnName("IsSLA");

                entity.Property(e => e.Kpiid).HasColumnName("KPIID");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.SendAlertTitle).HasMaxLength(400);

                entity.HasOne(d => d.Kpi)
                    .WithMany(p => p.Kpialert)
                    .HasForeignKey(d => d.Kpiid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KPIAlert_KPIID");
            });

            modelBuilder.Entity<Kpimeasure>(entity =>
            {
                entity.ToTable("KPIMeasure");

                entity.Property(e => e.KpimeasureId).HasColumnName("KPIMeasureID");

                entity.Property(e => e.CheckListTypeId).HasColumnName("CheckListTypeID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Measure).HasMaxLength(100);

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.HasOne(d => d.CheckListType)
                    .WithMany(p => p.Kpimeasure)
                    .HasForeignKey(d => d.CheckListTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KPIMeasure_CHECKLISTTYPEID");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.LogLevel).HasMaxLength(50);

                entity.Property(e => e.Time).HasColumnType("datetime");
            });

            modelBuilder.Entity<M3metricsQuestion>(entity =>
            {
                entity.ToTable("M3MetricsQuestion");

                entity.Property(e => e.M3metricsQuestionId).HasColumnName("M3MetricsQuestionID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.M3metricsQuestionCode)
                    .IsRequired()
                    .HasColumnName("M3MetricsQuestionCode")
                    .HasMaxLength(60);

                entity.Property(e => e.M3metricsQuestionText)
                    .IsRequired()
                    .HasColumnName("M3MetricsQuestionText")
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");
            });

            modelBuilder.Entity<Month>(entity =>
            {
                entity.Property(e => e.MonthId).HasColumnName("MonthID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MonthCode)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.MonthName)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");
            });

            modelBuilder.Entity<Payer>(entity =>
            {
                entity.Property(e => e.PayerId).HasColumnName("PayerID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PayerCode).HasMaxLength(255);

                entity.Property(e => e.PayerDescription).HasMaxLength(255);

                entity.Property(e => e.PayerName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.IsKpi).HasColumnName("IsKPI");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.QuestionCode).HasMaxLength(50);

                entity.Property(e => e.QuestionText)
                    .IsRequired()
                    .HasMaxLength(750);

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.CheckListType)
                    .WithMany(p => p.Question)
                    .HasForeignKey(d => d.CheckListTypeId)
                    .HasConstraintName("FK__Question__CheckL__0504B816");
            });

            modelBuilder.Entity<RoleAction>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("date");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleAction)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleId_RoleActions");

                entity.HasOne(d => d.ScreenAction)
                    .WithMany(p => p.RoleAction)
                    .HasForeignKey(d => d.ScreenActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ScreenActionId_RoleActions");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("('A')");

                entity.Property(e => e.RoleCode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RoleDesc)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Screen>(entity =>
            {
                entity.HasIndex(e => e.ScreenCode)
                    .HasName("UQ_Screen_ScreenCode")
                    .IsUnique();

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Icon)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("date");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ScreenCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ScreenDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ScreenName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ScreenPath)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ScreenAction>(entity =>
            {
                entity.HasIndex(e => e.ActionName)
                    .HasName("UQ__ScreenAc__491F5DD0DF9F787B")
                    .IsUnique();

                entity.Property(e => e.ActionName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("date");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Screen)
                    .WithMany(p => p.ScreenAction)
                    .HasForeignKey(d => d.ScreenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SCREENACTIONS_SCREENID");
            });

            modelBuilder.Entity<ScreenUserRoleMap>(entity =>
            {
                entity.HasKey(e => e.ScreenUserRoleId);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.DisplayScreenName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("date");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.ScreenUserRoleMap)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleId_ScreenUserRoleMap");

                entity.HasOne(d => d.Screen)
                    .WithMany(p => p.ScreenUserRoleMap)
                    .HasForeignKey(d => d.ScreenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ScreenId_ScreenUserRoleMap");
            });

            modelBuilder.Entity<Site>(entity =>
            {
                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.SiteCode).HasMaxLength(255);

                entity.Property(e => e.SiteDescription).HasMaxLength(255);

                entity.Property(e => e.SiteName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Speciality>(entity =>
            {
                entity.Property(e => e.SpecialityId).HasColumnName("SpecialityID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.SpecialityCode).HasMaxLength(255);

                entity.Property(e => e.SpecialityDescription).HasMaxLength(255);

                entity.Property(e => e.SpecialityName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<System>(entity =>
            {
                entity.Property(e => e.SystemId).HasColumnName("SystemID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.SystemCode).HasMaxLength(255);

                entity.Property(e => e.SystemDescription).HasMaxLength(255);

                entity.Property(e => e.SystemName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Title>(entity =>
            {
                entity.Property(e => e.TitleId).HasColumnName("TitleID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.TitleCode).HasMaxLength(255);

                entity.Property(e => e.TitleDescription).HasMaxLength(255);

                entity.Property(e => e.TitleName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<UserClientMap>(entity =>
            {
                entity.Property(e => e.UserClientMapId).HasColumnName("UserClientMapID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("('A')");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.UserClientMap)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersClinetMap_Client");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserClientMap)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserClien__UserI__29AC2CE0");
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(60);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ForgotPasswordToken).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(255);

                entity.Property(e => e.LastPasswordChanged).HasColumnType("datetime");

                entity.Property(e => e.LastSuccessfulLogin).HasColumnType("datetime");

                entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");

                entity.Property(e => e.MiddleName).HasMaxLength(255);

                entity.Property(e => e.MobileNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("UserID")
                    .HasMaxLength(255);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserLogin)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__UserLogin__RoleI__1F2E9E6D");
            });

            modelBuilder.HasSequence("ClientUser_Sequence")
                .StartsAt(100)
                .HasMin(100);

            modelBuilder.HasSequence("Question_Sequence")
                .StartsAt(100)
                .HasMin(100);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class M3PactContext : DbContext
    {
        public virtual DbSet<AdminConfigValues> AdminConfigValues { get; set; }
        public virtual DbSet<ApiResource> ApiResource { get; set; }
        public virtual DbSet<Application> Application { get; set; }
        public virtual DbSet<ApplicationApiResource> ApplicationApiResource { get; set; }
        public virtual DbSet<ApplicationType> ApplicationType { get; set; }
        public virtual DbSet<Attribute> Attribute { get; set; }
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
        public virtual DbSet<ClientConfig> ClientConfig { get; set; }
        public virtual DbSet<ClientConfigStep> ClientConfigStep { get; set; }
        public virtual DbSet<ClientConfigStepDetail> ClientConfigStepDetail { get; set; }
        public virtual DbSet<ClientConfigStepStatus> ClientConfigStepStatus { get; set; }
        public virtual DbSet<ClientHeatMapItemScore> ClientHeatMapItemScore { get; set; }
        public virtual DbSet<ClientHeatMapRisk> ClientHeatMapRisk { get; set; }
        public virtual DbSet<ClientKpimap> ClientKpimap { get; set; }
        public virtual DbSet<ClientKpiuserMap> ClientKpiuserMap { get; set; }
        public virtual DbSet<ClientPayer> ClientPayer { get; set; }
        public virtual DbSet<ClientTarget> ClientTarget { get; set; }
        public virtual DbSet<ClientUserNoticeAlerts> ClientUserNoticeAlerts { get; set; }
        public virtual DbSet<ControlType> ControlType { get; set; }
        public virtual DbSet<DateDimension> DateDimension { get; set; }
        public virtual DbSet<DepositLog> DepositLog { get; set; }
        public virtual DbSet<DepositLogMonthlyDetails> DepositLogMonthlyDetails { get; set; }
        public virtual DbSet<DeviatedClientKpi> DeviatedClientKpi { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<HeatMapItem> HeatMapItem { get; set; }
        public virtual DbSet<Holiday> Holiday { get; set; }
        public virtual DbSet<JobAppSettings> JobAppSettings { get; set; }
        public virtual DbSet<JobProcessGroup> JobProcessGroup { get; set; }
        public virtual DbSet<JobRun> JobRun { get; set; }
        public virtual DbSet<JobStatus> JobStatus { get; set; }
        public virtual DbSet<Kpi> Kpi { get; set; }
        public virtual DbSet<Kpialert> Kpialert { get; set; }
        public virtual DbSet<Kpimeasure> Kpimeasure { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<M3metricClientKpiDaily> M3metricClientKpiDaily { get; set; }
        public virtual DbSet<M3metricsQuestion> M3metricsQuestion { get; set; }
        public virtual DbSet<MailRecepientsDetailsDayWise> MailRecepientsDetailsDayWise { get; set; }
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
        public IConfiguration _Configuration { get; }
        public M3PactContext(DbContextOptions<M3PactContext> options, IConfiguration configuration) : base(options)
        {
            _Configuration = configuration;

        }

        // Unable to generate entity type for table 'dbo.Sheet2$'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminConfigValues>(entity =>
            {
                entity.HasKey(e => e.AdminConfigId);

                entity.Property(e => e.AttributeValue).HasMaxLength(250);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.HasOne(d => d.Attribute)
                    .WithMany(p => p.AdminConfigValues)
                    .HasForeignKey(d => d.AttributeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AttributeId_AdminConfigValues");
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

            modelBuilder.Entity<Attribute>(entity =>
            {
                entity.Property(e => e.AttributeCode)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.AttributeDescription).HasMaxLength(255);

                entity.Property(e => e.AttributeName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.AttributeType).HasMaxLength(250);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.HasOne(d => d.ControlType)
                    .WithMany(p => p.Attribute)
                    .HasForeignKey(d => d.ControlTypeId)
                    .HasConstraintName("FK_Attribute_ControlType");
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
                entity.HasIndex(e => e.AttributeCode)
                    .HasName("UQ_CheckListAttribute_AttributeCode")
                    .IsUnique();

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
                    .IsRequired()
                    .HasColumnName("CheckListAttributeValueID")
                    .HasMaxLength(50);

                entity.Property(e => e.CheckListId).HasColumnName("CheckListID");

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

                entity.HasOne(d => d.CheckListAttribute)
                    .WithMany(p => p.CheckListAttributeMap)
                    .HasForeignKey(d => d.CheckListAttributeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_CheckListAttributeMap_CheckListAttribute");

                entity.HasOne(d => d.CheckList)
                    .WithMany(p => p.CheckListAttributeMap)
                    .HasForeignKey(d => d.CheckListId)
                    .OnDelete(DeleteBehavior.Cascade)
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
                entity.HasIndex(e => e.ClientCode)
                    .HasName("UQ_Client_ClientCode")
                    .IsUnique();

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
                    .HasConstraintName("FK__Client__BillingM__1F83A428");

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
                    .HasConstraintName("FK__Client__Relation__2077C861");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.Client)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("FK__Client__Site__23694358");

                entity.HasOne(d => d.Speciality)
                    .WithMany(p => p.Client)
                    .HasForeignKey(d => d.SpecialityId)
                    .HasConstraintName("FK__Client__Speciali__25518C17");

                entity.HasOne(d => d.System)
                    .WithMany(p => p.Client)
                    .HasForeignKey(d => d.SystemId)
                    .HasConstraintName("FK__Client__SystemID__1CA7377D");

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
                    .HasConstraintName("FK__ClientClo__Clien__2630A1B7");

                entity.HasOne(d => d.Month)
                    .WithMany(p => p.ClientCloseMonth)
                    .HasForeignKey(d => d.MonthId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientClo__Month__3B0BC30C");
            });

            modelBuilder.Entity<ClientConfig>(entity =>
            {
                entity.Property(e => e.AttributeValue).HasMaxLength(250);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.HasOne(d => d.Attribute)
                    .WithMany(p => p.ClientConfig)
                    .HasForeignKey(d => d.AttributeId)
                    .HasConstraintName("FK__ClientCon__Attri__3BEAD8AC");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientConfig)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK__ClientCon__Clien__3AF6B473");
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
                    .HasConstraintName("FK__ClientCon__Scree__2DD1C37F");
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
                    .HasConstraintName("FK__ClientCon__Clien__2EC5E7B8");

                entity.HasOne(d => d.ClientConfigStepStatus)
                    .WithMany(p => p.ClientConfigStepDetail)
                    .HasForeignKey(d => d.ClientConfigStepStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientCon__Clien__336AA144");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientConfigStepDetail)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientCon__Clien__24485945");
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

            modelBuilder.Entity<ClientHeatMapItemScore>(entity =>
            {
                entity.Property(e => e.ClientHeatMapItemScoreId).HasColumnName("ClientHeatMapItemScoreID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.HeatMapItemDate).HasColumnType("date");

                entity.Property(e => e.HeatMapItemId).HasColumnName("HeatMapItemID");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientHeatMapItemScore)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ClientHeatMapItemScore_Client");

                entity.HasOne(d => d.HeatMapItem)
                    .WithMany(p => p.ClientHeatMapItemScore)
                    .HasForeignKey(d => d.HeatMapItemId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ClientHeatMapItemScore_HeatMapItem");
            });

            modelBuilder.Entity<ClientHeatMapRisk>(entity =>
            {
                entity.HasIndex(e => new { e.ClientId, e.M3dailyDate, e.M3weeklyDate, e.M3monthlyDate, e.ChecklistWeeklyDate, e.ChecklistMonthlyDate })
                    .HasName("UQ_ClientHeatMapRisk_Dates")
                    .IsUnique();

                entity.Property(e => e.ClientHeatMapRiskId).HasColumnName("ClientHeatMapRiskID");

                entity.Property(e => e.ChecklistMonthlyDate).HasColumnType("date");

                entity.Property(e => e.ChecklistWeeklyDate).HasColumnType("date");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTime).HasColumnType("datetime");

                entity.Property(e => e.M3dailyDate)
                    .HasColumnName("M3DailyDate")
                    .HasColumnType("date");

                entity.Property(e => e.M3monthlyDate)
                    .HasColumnName("M3MonthlyDate")
                    .HasColumnType("date");

                entity.Property(e => e.M3weeklyDate)
                    .HasColumnName("M3WeeklyDate")
                    .HasColumnType("date");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientHeatMapRisk)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ClientHeatMapRisk_Client");
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
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__ClientKPI__Clien__2818EA29");

                entity.HasOne(d => d.Kpi)
                    .WithMany(p => p.ClientKpimap)
                    .HasForeignKey(d => d.Kpiid)
                    .OnDelete(DeleteBehavior.Cascade)
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
                    .HasConstraintName("FK__ClientKPI__UserI__36670980");
            });

            modelBuilder.Entity<ClientPayer>(entity =>
            {
                entity.Property(e => e.ClientPayerId).HasColumnName("ClientPayerID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.IsM3feeExempt).HasColumnName("IsM3FeeExempt");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PayerId).HasColumnName("PayerID");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.StartDate).HasColumnType("date");

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

            modelBuilder.Entity<ClientUserNoticeAlerts>(entity =>
            {
                entity.HasKey(e => e.ClientUserNoticeAlertId);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus).HasColumnType("char(1)");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientUserNoticeAlerts)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientUse__Clien__064DE20A");

                entity.HasOne(d => d.UserLogin)
                    .WithMany(p => p.ClientUserNoticeAlerts)
                    .HasForeignKey(d => d.UserLoginId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientUse__UserL__07420643");
            });

            modelBuilder.Entity<ControlType>(entity =>
            {
                entity.Property(e => e.ControlName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
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

            modelBuilder.Entity<DepositLogMonthlyDetails>(entity =>
            {
                entity.HasKey(e => e.CloseMonthId);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MonthStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.ProjectedCash).HasColumnType("numeric(15, 2)");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.TotalDepositAmount).HasColumnType("numeric(15, 2)");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.DepositLogMonthlyDetails)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK__DepositLo__Clien__4297D63B");

                entity.HasOne(d => d.Month)
                    .WithMany(p => p.DepositLogMonthlyDetails)
                    .HasForeignKey(d => d.MonthId)
                    .HasConstraintName("FK__DepositLo__Month__438BFA74");
            });

            modelBuilder.Entity<DeviatedClientKpi>(entity =>
            {
                entity.ToTable("DeviatedClientKPI");

                entity.Property(e => e.DeviatedClientKpiid).HasColumnName("DeviatedClientKPIId");

                entity.Property(e => e.ActualResponse).HasMaxLength(50);

                entity.Property(e => e.CheckListDate).HasColumnType("date");

                entity.Property(e => e.ExpectedResponse).HasMaxLength(50);

                entity.Property(e => e.QuestionCode).HasMaxLength(50);

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.SubmittedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ChecklistType)
                    .WithMany(p => p.DeviatedClientKpi)
                    .HasForeignKey(d => d.ChecklistTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviatedClientKPI_ChecklistType ");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.DeviatedClientKpi)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviatedClientKPI_Client");
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

                entity.Property(e => e.Sso).HasColumnName("SSO");

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<HeatMapItem>(entity =>
            {
                entity.Property(e => e.HeatMapItemId).HasColumnName("HeatMapItemID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.Kpiid).HasColumnName("KPIID");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.Kpi)
                    .WithMany(p => p.HeatMapItem)
                    .HasForeignKey(d => d.Kpiid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_HeatMapItem_KPI");
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

            modelBuilder.Entity<JobAppSettings>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.SettingName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SettingValue)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<JobProcessGroup>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ProcessGroupCode)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ProcessGroupName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");
            });

            modelBuilder.Entity<JobRun>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.JobProcessGroup)
                    .WithMany(p => p.JobRun)
                    .HasForeignKey(d => d.JobProcessGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobRun_JobProcessGroup");

                entity.HasOne(d => d.JobStatus)
                    .WithMany(p => p.JobRun)
                    .HasForeignKey(d => d.JobStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobRun_JobStatus");
            });

            modelBuilder.Entity<JobStatus>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.JobStatusCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.JobStatusName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(100);

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

                entity.Property(e => e.CheckListTypeId).HasDefaultValueSql("((3))");

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

                entity.Property(e => e.Standard).HasMaxLength(50);

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

            modelBuilder.Entity<M3metricClientKpiDaily>(entity =>
            {
                entity.ToTable("M3MetricClientKpiDaily");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActualValue).HasMaxLength(500);

                entity.Property(e => e.AlertLevel).HasMaxLength(500);

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.InsertedDate).HasColumnType("date");

                entity.Property(e => e.KpiId).HasColumnName("KpiID");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.M3metricClientKpiDaily)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_M3MetricClientKpiDaily_Client");

                entity.HasOne(d => d.Kpi)
                    .WithMany(p => p.M3metricClientKpiDaily)
                    .HasForeignKey(d => d.KpiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_M3MetricClientKpiDaily_KPI");
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

                entity.Property(e => e.M3metricsUnit)
                    .HasColumnName("M3MetricsUnit")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");
            });

            modelBuilder.Entity<MailRecepientsDetailsDayWise>(entity =>
            {
                entity.Property(e => e.AlertType)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DeviatedClientKpiid).HasColumnName("DeviatedClientKPIId");

                entity.Property(e => e.SentDate).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("UserID")
                    .HasMaxLength(510);

                entity.HasOne(d => d.DeviatedClientKpi)
                    .WithMany(p => p.MailRecepientsDetailsDayWise)
                    .HasForeignKey(d => d.DeviatedClientKpiid)
                    .HasConstraintName("FK_MailRecepientsDetailsDayWise_DeviatedClientKPI");
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

                entity.Property(e => e.EndDate).HasColumnType("date");

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

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.CheckListTypeId).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01 00:00:00.000')");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('9900-12-31 00:00:00.000')");

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

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01 00:00:00.000')");

                entity.HasOne(d => d.CheckListType)
                    .WithMany(p => p.Question)
                    .HasForeignKey(d => d.CheckListTypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Question_CheckListType");
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
                    .HasName("UNIQUEACTIONNAME")
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
                    .HasConstraintName("FK__UserClien__UserI__3572E547");
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
                    .HasConstraintName("FK__UserLogin__RoleI__30AE302A");
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

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace SK_Stanicni_Racuni.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Komitent> Komitents { get; set; }
        public virtual DbSet<SlogKalk> SlogKalks { get; set; }
        public virtual DbSet<SlogKalkP> SlogKalkPs { get; set; }
        public virtual DbSet<SlogKalkPdv> SlogKalkPdvs { get; set; }
        public virtual DbSet<SlogKola> SlogKolas { get; set; }
        public virtual DbSet<SlogRoba> SlogRobas { get; set; }
        public virtual DbSet<Ugovori> Ugovoris { get; set; }
        public virtual DbSet<UicOperateri> UicOperateris { get; set; }
        public virtual DbSet<UicStanice> UicStanices { get; set; }
        public virtual DbSet<UserTab> UserTabs { get; set; }
        public virtual DbSet<ZsStanice> ZsStanices { get; set; }
        public virtual DbSet<ZsVrsteSaobracaja> ZsVrsteSaobracajas { get; set; }
        public virtual DbSet<ZsVsStavke> ZsVsStavkes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=192.168.2.200\\SQLEXPRESS01;Database=WinRoba;user id=elsUser;password=els5577;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Komitent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Komitent");

                entity.Property(e => e.Adresa)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Land)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Mesto)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Naziv)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Osoba)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Pib)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PIB")
                    .IsFixedLength(true);

                entity.Property(e => e.Sapsifra).HasColumnName("SAPSifra");

                entity.Property(e => e.Telefon)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Zemlja)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SlogKalk>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SlogKalk");

                entity.Property(e => e.CarAgent)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CarBrojFakture)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CarDatum).HasColumnType("smalldatetime");

                entity.Property(e => e.CarDokumenti)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CarFaktura).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CarJci)
                    .HasMaxLength(18)
                    .IsUnicode(false)
                    .HasColumnName("CarJCI")
                    .IsFixedLength(true);

                entity.Property(e => e.CarKnjiga)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CarPostupakEnd)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CarPrimalacNaziv)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CarPrimalacPib)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CarPrimalacPIB")
                    .IsFixedLength(true);

                entity.Property(e => e.CarPrimalacSediste)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CarPrimalacZemlja)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CarRokCarinjenja).HasColumnType("smalldatetime");

                entity.Property(e => e.CarStanica)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CarStanica2)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CarValuta)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CarXml)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DatumIzlaza).HasColumnType("smalldatetime");

                entity.Property(e => e.DatumObrade).HasColumnType("smalldatetime");

                entity.Property(e => e.DatumUlaza).HasColumnType("smalldatetime");

                entity.Property(e => e.DodPrev).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.FrDoPrelaza)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.FrNaknade)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.FrRacun)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Incoterms)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Isporuka).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.IsporukaVal)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.IzjavaIznos).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.IzjavaVal)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Najava)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Najava2)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.NarPos)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ObrGodina)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ObrMesec)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Obrada)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.OstatakDin).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.OtpDatum).HasColumnType("smalldatetime");

                entity.Property(e => e.OtpStanica)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.OtpUprava)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PlatilacFrr).HasColumnName("PlatilacFRR");

                entity.Property(e => e.PlatilacNfr).HasColumnName("PlatilacNFR");

                entity.Property(e => e.Pouzece).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.PouzeceVal)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PrDatum).HasColumnType("smalldatetime");

                entity.Property(e => e.PrStanica)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PrUprava)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Prevoz)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.RNakFr)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("rNakFr");

                entity.Property(e => e.RNakFrDin)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("rNakFrDin");

                entity.Property(e => e.RNakUp)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("rNakUp");

                entity.Property(e => e.RNakUpDin)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("rNakUpDin");

                entity.Property(e => e.RPrevFr)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("rPrevFr");

                entity.Property(e => e.RPrevFrDin)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("rPrevFrDin");

                entity.Property(e => e.RPrevUp)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("rPrevUp");

                entity.Property(e => e.RPrevUpDin)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("rPrevUpDin");

                entity.Property(e => e.RSumaFr)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("rSumaFr");

                entity.Property(e => e.RSumaFrDin)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("rSumaFrDin");

                entity.Property(e => e.RSumaUp)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("rSumaUp");

                entity.Property(e => e.RSumaUpDin)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("rSumaUpDin");

                entity.Property(e => e.RecId).HasColumnName("RecID");

                entity.Property(e => e.Referent1)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Referent2)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Saobracaj)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SatVoza)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SatVoza2)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Server)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SifraK211)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SifraTarife)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SkmZs).HasColumnName("SkmZS");

                entity.Property(e => e.Stanica)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.StanicaRee)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TkmZs).HasColumnName("TkmZS");

                entity.Property(e => e.TlNakCo)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("tlNakCo");

                entity.Property(e => e.TlNakCo82)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("tlNakCo82");

                entity.Property(e => e.TlNakFr)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("tlNakFr");

                entity.Property(e => e.TlNakFrDin)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("tlNakFrDin");

                entity.Property(e => e.TlNakUp)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("tlNakUp");

                entity.Property(e => e.TlNakUpDin)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("tlNakUpDin");

                entity.Property(e => e.TlPrevFr)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("tlPrevFr");

                entity.Property(e => e.TlPrevFrDin)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("tlPrevFrDin");

                entity.Property(e => e.TlPrevUp)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("tlPrevUp");

                entity.Property(e => e.TlPrevUpDin)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("tlPrevUpDin");

                entity.Property(e => e.TlRealizovan)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("tlRealizovan")
                    .IsFixedLength(true);

                entity.Property(e => e.TlSumaFr)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("tlSumaFr");

                entity.Property(e => e.TlSumaFrDin)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("tlSumaFrDin");

                entity.Property(e => e.TlSumaUp)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("tlSumaUp");

                entity.Property(e => e.TlSumaUpDin)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("tlSumaUpDin");

                entity.Property(e => e.TlValuta)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("tlValuta")
                    .IsFixedLength(true);

                entity.Property(e => e.Ugovor)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.VrPos)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.VrPrevoza)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.VrednostRobe).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.VrednostRobeVal)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ZsIzPrelaz)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ZsUlPrelaz)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<SlogKalkP>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SlogKalkPS");

                entity.Property(e => e.PodStanica1)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PodStanica2)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.RecId).HasColumnName("RecID");

                entity.Property(e => e.Stanica)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<SlogKalkPdv>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SlogKalkPDV");

                entity.Property(e => e.Pdv1)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("PDV1");

                entity.Property(e => e.Pdv2)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("PDV2");

                entity.Property(e => e.RecId).HasColumnName("RecID");

                entity.Property(e => e.Stanica)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<SlogKola>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SlogKola");

                entity.Property(e => e.Ibk)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("IBK")
                    .IsFixedLength(true);

                entity.Property(e => e.Icf)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ICF")
                    .IsFixedLength(true);

                entity.Property(e => e.Kontrola)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Naknada).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.OtpDatum).HasColumnType("smalldatetime");

                entity.Property(e => e.OtpStanica)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.OtpUprava)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Prevoznina).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RecId).HasColumnName("RecID");

                entity.Property(e => e.Serija)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Stanica)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Stitna)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TipKola)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Uprava)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Vlasnik)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<SlogRoba>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SlogRoba");

                entity.Property(e => e.Nhm)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("NHM")
                    .IsFixedLength(true);

                entity.Property(e => e.Nhmrazred)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("NHMRazred")
                    .IsFixedLength(true);

                entity.Property(e => e.OtpDatum).HasColumnType("smalldatetime");

                entity.Property(e => e.OtpStanica)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.OtpUprava)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.RecId).HasColumnName("RecID");

                entity.Property(e => e.Rid)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("RID")
                    .IsFixedLength(true);

                entity.Property(e => e.Ridklasa)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("RIDKlasa")
                    .IsFixedLength(true);

                entity.Property(e => e.Ridrazred)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("RIDRazred")
                    .IsFixedLength(true);

                entity.Property(e => e.Rmasa).HasColumnName("RMasa");

                entity.Property(e => e.Smasa).HasColumnName("SMasa");

                entity.Property(e => e.SmasaDec)
                    .HasColumnType("decimal(9, 2)")
                    .HasColumnName("SMasaDec");

                entity.Property(e => e.Stanica)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UtiBrPlombe)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UtiIb)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("UtiIB")
                    .IsFixedLength(true);

                entity.Property(e => e.UtiNaknada).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UtiNhm)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("UtiNHM")
                    .IsFixedLength(true);

                entity.Property(e => e.UtiPredajniList)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UtiRaster).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VozStav).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.VozStavSifra)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Ugovori>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Ugovori");

                entity.Property(e => e.BrojUgovora)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BrojUgovoraStari)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DatumUnosa).HasColumnType("date");

                entity.Property(e => e.Saobracaj)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TipUgovora)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UgPath).HasColumnType("date");

                entity.Property(e => e.UserId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("UserID")
                    .IsFixedLength(true);

                entity.Property(e => e.VaziDo)
                    .HasColumnType("date")
                    .HasColumnName("VaziDO");

                entity.Property(e => e.VaziOd)
                    .HasColumnType("date")
                    .HasColumnName("VaziOD");

                entity.Property(e => e.VrstaKursaF)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.VrstaKursaT)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.VrstaObracuna)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<UicOperateri>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("UicOperateri");

                entity.Property(e => e.Log)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("(log)")
                    .IsFixedLength(true);

                entity.Property(e => e.Naziv)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Operater)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Oznaka)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Podaci)
                    .HasMaxLength(78)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SifraOperatera)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SifraUprave)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Zemlja)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<UicStanice>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("UicStanice");

                entity.Property(e => e.Kb)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("KB")
                    .IsFixedLength(true);

                entity.Property(e => e.Naziv)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SifraStanice)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SifraStanice1)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("_SifraStanice")
                    .IsFixedLength(true);

                entity.Property(e => e.SifraUprave)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("_SifraUprave")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<UserTab>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("UserTab");

                entity.Property(e => e.Grupa)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Lozinka)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Naziv)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Stanica)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("UserID")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<ZsStanice>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ZsStanice");

                entity.Property(e => e.Carina)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CeonaRampa)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Dencano)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Dizalica)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Ekspres)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.KolskaVaga)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Kolsko)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Magacin)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.NadzornaStanicaNaziv)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NadzornaStanicaSifra)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ObicnaRampa)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Prikaz)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Prtljag)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Putnicka)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SifraStanice)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SifraStanice1)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("_SifraStanice")
                    .IsFixedLength(true);

                entity.Property(e => e.SifraUprave)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("_SifraUprave")
                    .IsFixedLength(true);

                entity.Property(e => e.SkupPodskup)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TovarniProfil)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Vrsta)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.X).HasColumnName("_X");

                entity.Property(e => e.Y).HasColumnName("_Y");

                entity.Property(e => e.ZiveZivotinje)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<ZsVrsteSaobracaja>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ZsVrsteSaobracaja");

                entity.Property(e => e.Saobracaj)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SifraVs).HasColumnName("SifraVS");
            });

            modelBuilder.Entity<ZsVsStavke>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ZsVsStavke");

                entity.Property(e => e.Naziv)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SifraVs).HasColumnName("SifraVS");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

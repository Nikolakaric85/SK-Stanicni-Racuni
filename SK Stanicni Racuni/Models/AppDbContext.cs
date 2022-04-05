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
        public virtual DbSet<SlogKalkPdv> SlogKalkPdvs { get; set; }
        public virtual DbSet<SlogKola> SlogKolas { get; set; }
        public virtual DbSet<SlogRoba> SlogRobas { get; set; }
        public virtual DbSet<SrFaktura> SrFakturas { get; set; }
        public virtual DbSet<SrK121a> SrK121as { get; set; }
        public virtual DbSet<SrK161f> SrK161fs { get; set; }
        public virtual DbSet<Ugovori> Ugovoris { get; set; }
        public virtual DbSet<Ugovori1> Ugovoris1 { get; set; }
        public virtual DbSet<UicOperateri> UicOperateris { get; set; }
        public virtual DbSet<UicStanice> UicStanices { get; set; }
        public virtual DbSet<UserTab> UserTabs { get; set; }
        public virtual DbSet<ZsPrelazi> ZsPrelazis { get; set; }
        public virtual DbSet<ZsStanice> ZsStanices { get; set; }
        public virtual DbSet<ZsTarifa> ZsTarifas { get; set; }
        public virtual DbSet<ZsVrsteSaobracaja> ZsVrsteSaobracajas { get; set; }
        public virtual DbSet<ZsVsStavke> ZsVsStavkes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Data Source=10.3.4.37;Initial Catalog=WinRoba; User ID=radnik; password=roba2006;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Komitent>(entity =>
            {
                entity.HasKey(e => e.Sifra);

                entity.ToTable("Komitent");

                entity.HasIndex(e => e.Sifra, "IX_Komitent")
                    .IsUnique();

                entity.Property(e => e.Sifra).ValueGeneratedNever();

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

                entity.Property(e => e.Mb)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MB")
                    .IsFixedLength(true)
                    .HasComment("''");

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

                entity.Property(e => e.Tr)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TR")
                    .IsFixedLength(true)
                    .HasComment("''");

                entity.Property(e => e.Zemlja)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SlogKalk>(entity =>
            {
                entity.HasKey(e => new { e.RecId, e.Stanica });

                entity.ToTable("SlogKalk");

                entity.HasIndex(e => new { e.OtpUprava, e.OtpStanica, e.OtpBroj, e.OtpDatum }, "IX_SlogKalk")
                    .IsUnique();

                entity.Property(e => e.RecId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("RecID");

                entity.Property(e => e.Stanica)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CarAgent)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CarBrojFakture)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CarDatum).HasColumnType("datetime");

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

                entity.Property(e => e.CarRokCarinjenja).HasColumnType("datetime");

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

                entity.Property(e => e.DatumIzlaza).HasColumnType("datetime");

                entity.Property(e => e.DatumObrade).HasColumnType("datetime");

                entity.Property(e => e.DatumUlaza).HasColumnType("datetime");

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

                entity.Property(e => e.K165a)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.K165aDatum).HasColumnName("K165a_datum");

                entity.Property(e => e.K165aIznos)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("K165a_iznos")
                    .HasDefaultValueSql("((0))");

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

                entity.Property(e => e.OtpDatum).HasColumnType("datetime");

                entity.Property(e => e.OtpRbb).HasDefaultValueSql("((1))");

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

                entity.Property(e => e.PrDatum).HasColumnType("datetime");

                entity.Property(e => e.PrRbb).HasDefaultValueSql("((1))");

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

            modelBuilder.Entity<SlogKalkPdv>(entity =>
            {
                entity.HasKey(e => new { e.RecId, e.Stanica });

                entity.ToTable("SlogKalkPDV");

                entity.Property(e => e.RecId).HasColumnName("RecID");

                entity.Property(e => e.Stanica)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Pdv1)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("PDV1");

                entity.Property(e => e.Pdv2)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("PDV2");
            });

            modelBuilder.Entity<SlogKola>(entity =>
            {
                entity.HasKey(e => new { e.RecId, e.Stanica, e.KolaStavka })
                    .IsClustered(false);

                entity.ToTable("SlogKola");

                entity.HasIndex(e => new { e.OtpUprava, e.OtpStanica, e.OtpBroj, e.OtpDatum, e.KolaStavka }, "IX_SlogKola")
                    .IsUnique();

                entity.Property(e => e.RecId).HasColumnName("RecID");

                entity.Property(e => e.Stanica)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

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

                entity.Property(e => e.OtpDatum).HasColumnType("datetime");

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

                entity.Property(e => e.Serija)
                    .HasMaxLength(11)
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

                entity.HasOne(d => d.SlogKalk)
                    .WithMany(p => p.SlogKolas)
                    .HasForeignKey(d => new { d.RecId, d.Stanica })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SlogKola_SlogKalk");
            });

            modelBuilder.Entity<SlogRoba>(entity =>
            {
                entity.HasKey(e => new { e.RecId, e.Stanica, e.KolaStavka, e.RobaStavka })
                    .IsClustered(false);

                entity.ToTable("SlogRoba");

                entity.HasIndex(e => new { e.OtpUprava, e.OtpStanica, e.OtpBroj, e.OtpDatum, e.KolaStavka, e.RobaStavka }, "IX_SlogRoba")
                    .IsUnique();

                entity.Property(e => e.RecId).HasColumnName("RecID");

                entity.Property(e => e.Stanica)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

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

                entity.Property(e => e.OtpDatum).HasColumnType("datetime");

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

                entity.HasOne(d => d.SlogKola)
                    .WithMany(p => p.SlogRobas)
                    .HasForeignKey(d => new { d.RecId, d.Stanica, d.KolaStavka })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SlogRoba_SlogKola");
            });

            modelBuilder.Entity<SrFaktura>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SR_Faktura");

                entity.Property(e => e.Blagajna).HasComment("Šifra blagajne ");

                entity.Property(e => e.BlagajnaTip)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("P=prispeća; O=Otpravljanja");

                entity.Property(e => e.Blagajnik)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DatumIzdavanja).HasColumnType("date");

                entity.Property(e => e.FakturaBroj).HasMaxLength(20);

                entity.Property(e => e.FakturaDatum)
                    .HasColumnType("date")
                    .HasComment("Datum nastanka obaveze");

                entity.Property(e => e.FakturaDatumP)
                    .HasColumnType("date")
                    .HasComment("Datum plaćanja");

                entity.Property(e => e.FakturaDatumPromet)
                    .HasColumnType("date")
                    .HasComment("Datum prometa usluge");

                entity.Property(e => e.FakturaOsnovica)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FakturaPdv)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("FakturaPDV")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FakturaTekst).HasMaxLength(200);

                entity.Property(e => e.FakturaTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Fjcena)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("FJCena")
                    .HasComment("Jedinična cena");

                entity.Property(e => e.Fjedinica)
                    .HasMaxLength(50)
                    .HasColumnName("FJedinica")
                    .HasComment("Jedinična mera");

                entity.Property(e => e.Fkolicina)
                    .HasMaxLength(50)
                    .HasColumnName("FKolicina")
                    .HasComment("Količina");

                entity.Property(e => e.Fopdv)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("FOPdv")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("Oslobođeno PDV-a");

                entity.Property(e => e.Fopdvcst)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FOPDVCST")
                    .IsFixedLength(true)
                    .HasComment("Član, Stav, Tačka zakona za oslobađanje od PDV-a");

                entity.Property(e => e.Fpath)
                    .HasMaxLength(200)
                    .HasColumnName("FPATH")
                    .HasComment("Putanja za skenirani dokument");

                entity.Property(e => e.FpozivNaBroj)
                    .HasMaxLength(50)
                    .HasColumnName("FPozivNaBroj");

                entity.Property(e => e.Kurs).HasColumnType("decimal(12, 4)");

                entity.Property(e => e.OtpDatum).HasColumnType("datetime");

                entity.Property(e => e.PrDatum).HasColumnType("datetime");

                entity.Property(e => e.Primalac).HasMaxLength(150);

                entity.Property(e => e.PrimalacAdresa).HasMaxLength(150);

                entity.Property(e => e.PrimalacMb)
                    .HasMaxLength(10)
                    .HasColumnName("PrimalacMB")
                    .IsFixedLength(true);

                entity.Property(e => e.PrimalacPib)
                    .HasMaxLength(10)
                    .HasColumnName("PrimalacPIB")
                    .IsFixedLength(true);

                entity.Property(e => e.PrimalacTelefon)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.PrimalacTr)
                    .HasMaxLength(10)
                    .HasColumnName("PrimalacTR")
                    .IsFixedLength(true);

                entity.Property(e => e.PrimalacUg)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PrimalacZemlja).HasMaxLength(50);

                entity.Property(e => e.Stanica)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TekuciRacun).HasMaxLength(80);

                entity.Property(e => e.VrstaUslugaOpis).HasMaxLength(2000);

                entity.Property(e => e.VrstaUslugaSifra)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<SrK121a>(entity =>
            {
                entity.ToTable("SR_K121a");

                entity.Property(e => e.Blagajnik)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BlagajnikFr)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("BlagajnikFR")
                    .IsFixedLength(true);

                entity.Property(e => e.Datum).HasColumnType("date");

                entity.Property(e => e.DatumVracanjaFr)
                    .HasColumnType("date")
                    .HasColumnName("DatumVracanjaFR");

                entity.Property(e => e.Iznos)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ObracunFr)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("ObracunFR");

                entity.Property(e => e.OtpDatum).HasColumnType("date");

                entity.Property(e => e.Pošiljalac).HasMaxLength(150);

                entity.Property(e => e.PrStanica)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Primalac).HasMaxLength(150);

                entity.Property(e => e.PrimalacAdresa).HasMaxLength(150);

                entity.Property(e => e.PrimalacZemlja).HasMaxLength(50);

                entity.Property(e => e.Saobracaj)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Stanica)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<SrK161f>(entity =>
            {
                entity.ToTable("SR_K161f");

                entity.Property(e => e.BlagajnaTip)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("P=prispeća; O=Otpravljanja");

                entity.Property(e => e.Blagajnik)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.FakturaBroj).HasMaxLength(20);

                entity.Property(e => e.FakturaDatum)
                    .HasColumnType("date")
                    .HasComment("Datum nastanka obaveze");

                entity.Property(e => e.FakturaDatumP).HasColumnType("date");

                entity.Property(e => e.FakturaOsnovica)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FakturaPdv)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("FakturaPDV")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Kurs).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.NaplacenoNb)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasColumnName("NaplacenoNB")
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.Primalac).HasMaxLength(150);

                entity.Property(e => e.PrimalacAdresa).HasMaxLength(150);

                entity.Property(e => e.PrimalacMb)
                    .HasMaxLength(10)
                    .HasColumnName("PrimalacMB")
                    .IsFixedLength(true);

                entity.Property(e => e.PrimalacPib)
                    .HasMaxLength(10)
                    .HasColumnName("PrimalacPIB")
                    .IsFixedLength(true);

                entity.Property(e => e.PrimalacTelefon)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.PrimalacTr)
                    .HasMaxLength(10)
                    .HasColumnName("PrimalacTR")
                    .IsFixedLength(true);

                entity.Property(e => e.PrimalacZemlja).HasMaxLength(50);

                entity.Property(e => e.Saobracaj).HasMaxLength(1);

                entity.Property(e => e.Stanica)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.VrstaUslugaOpis).HasMaxLength(200);

                entity.Property(e => e.VrstaUslugaSifra)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Ugovori>(entity =>
            {
                entity.HasKey(e => new { e.BrojUgovora, e.VrstaObracuna })
                    .HasName("XPKUgovori");

                entity.ToTable("Ugovori");

                entity.Property(e => e.BrojUgovora)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("Centralni obracun ili Redovni");

                entity.Property(e => e.VrstaObracuna)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("CO=centralni obracun KP=komercijalna povlastica");

                entity.Property(e => e.BrojUgovoraStari)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DatumUnosa).HasColumnType("date");

                entity.Property(e => e.Saobracaj)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("E=uvoz, I=izvoz, U=unutrasnji");

                entity.Property(e => e.TipUgovora)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UgPath).HasMaxLength(500);

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
                    .HasDefaultValueSql("((3))")
                    .IsFixedLength(true)
                    .HasComment("1=SKG; 3=NBS");
            });

            modelBuilder.Entity<Ugovori1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Ugovori", "roba214kp");

                entity.Property(e => e.BrojUgovora)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Saobracaj)
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
                entity.HasKey(e => e.SifraStanice);

                entity.ToTable("UicStanice");

                entity.Property(e => e.SifraStanice)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Kb)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("KB")
                    .IsFixedLength(true);

                entity.Property(e => e.Naziv)
                    .HasMaxLength(30)
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
                entity.HasKey(e => e.UserId);

                entity.ToTable("UserTab");

                entity.Property(e => e.UserId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("UserID")
                    .IsFixedLength(true);

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
            });

            modelBuilder.Entity<ZsPrelazi>(entity =>
            {
                entity.HasKey(e => e.SifraPrelaza);

                entity.ToTable("ZsPrelazi");

                entity.Property(e => e.SifraPrelaza)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.GranicnaUprava)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Naziv)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.SifraCarina)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SifraPrelaza4)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<ZsStanice>(entity =>
            {
                entity.HasKey(e => e.SifraStanice);

                entity.ToTable("ZsStanice");

                entity.Property(e => e.SifraStanice)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Blagajna).HasDefaultValueSql("(1)");

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
                    .IsFixedLength(true)
                    .HasComment("C=cvor; P=na pruzi");

                entity.Property(e => e.X).HasColumnName("_X");

                entity.Property(e => e.Y).HasColumnName("_Y");

                entity.Property(e => e.ZiveZivotinje)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.SifraStaniceNavigation)
                    .WithOne(p => p.ZsStanice)
                    .HasForeignKey<ZsStanice>(d => d.SifraStanice)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ZsStanice_UicStanice");
            });

            modelBuilder.Entity<ZsTarifa>(entity =>
            {
                entity.HasKey(e => new { e.SifraTarife, e.TarifaStavka, e.SifraVs })
                    .HasName("XPKZsTarifa");

                entity.ToTable("ZsTarifa");

                entity.Property(e => e.SifraTarife)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TarifaStavka)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SifraVs).HasColumnName("SifraVS");

                entity.Property(e => e.Naziv)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Naziv tarife");

                entity.Property(e => e.Opis)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Pun opis tarife");

                entity.Property(e => e.VaziDo).HasColumnType("datetime");

                entity.Property(e => e.VaziOd).HasColumnType("datetime");
            });

            modelBuilder.Entity<ZsVrsteSaobracaja>(entity =>
            {
                entity.HasKey(e => e.SifraVs);

                entity.ToTable("ZsVrsteSaobracaja");

                entity.Property(e => e.SifraVs)
                    .ValueGeneratedNever()
                    .HasColumnName("SifraVS");

                entity.Property(e => e.Saobracaj)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ZsVsStavke>(entity =>
            {
                entity.HasKey(e => new { e.SifraVs, e.SifraVsStavka })
                    .HasName("PK_Table1");

                entity.ToTable("ZsVsStavke");

                entity.Property(e => e.SifraVs).HasColumnName("SifraVS");

                entity.Property(e => e.Naziv)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

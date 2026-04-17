using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

public partial class ERPDbContext : DbContext
{
    public ERPDbContext()
    {
    }

    public ERPDbContext(DbContextOptions<ERPDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Categorie> Categories { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Commande> Commandes { get; set; }

    public virtual DbSet<Facture> Factures { get; set; }

    public virtual DbSet<Fournisseur> Fournisseurs { get; set; }

    public virtual DbSet<LCommande> LCommandes { get; set; }

    public virtual DbSet<LFournisseur> LFournisseurs { get; set; }

    public virtual DbSet<LPanier> LPaniers { get; set; }

    public virtual DbSet<Panier> Paniers { get; set; }

    public virtual DbSet<Souscategorie> Souscategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=ERP;Username=postgres;Password=0000");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.IdArt).HasName("article_pkey");

            entity.ToTable("article");

            entity.Property(e => e.IdArt).HasColumnName("id_art");
            entity.Property(e => e.DateAjout)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_ajout");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Designation)
                .HasMaxLength(255)
                .HasColumnName("designation");
            entity.Property(e => e.IdScat).HasColumnName("id_scat");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url");
            entity.Property(e => e.PrixUnitaire)
                .HasPrecision(10, 2)
                .HasColumnName("prix_unitaire");
            entity.Property(e => e.StockDispo).HasColumnName("stock_dispo");

            entity.HasOne(d => d.IdScatNavigation).WithMany(p => p.Articles)
                .HasForeignKey(d => d.IdScat)
                .HasConstraintName("article_id_scat_fkey");
        });

        modelBuilder.Entity<Categorie>(entity =>
        {
            entity.HasKey(e => e.IdCat).HasName("categorie_pkey");

            entity.ToTable("categorie");

            entity.Property(e => e.IdCat).HasColumnName("id_cat");
            entity.Property(e => e.CodeCat)
                .HasMaxLength(50)
                .HasColumnName("code_cat");
            entity.Property(e => e.DateCreation)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_creation");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Libelle)
                .HasMaxLength(100)
                .HasColumnName("libelle");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClt).HasName("client_pkey");

            entity.ToTable("client");

            entity.Property(e => e.IdClt).HasColumnName("id_clt");
            entity.Property(e => e.Adresse).HasColumnName("adresse");
            entity.Property(e => e.DateInscription)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_inscription");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Nom)
                .HasMaxLength(100)
                .HasColumnName("nom");
            entity.Property(e => e.Prenom)
                .HasMaxLength(100)
                .HasColumnName("prenom");
            entity.Property(e => e.Telephone)
                .HasMaxLength(20)
                .HasColumnName("telephone");
        });

        modelBuilder.Entity<Commande>(entity =>
        {
            entity.HasKey(e => e.IdCom).HasName("commande_pkey");

            entity.ToTable("commande");

            entity.Property(e => e.IdCom).HasColumnName("id_com");
            entity.Property(e => e.DateCom).HasColumnName("date_com");
            entity.Property(e => e.IdClt).HasColumnName("id_clt");
            entity.Property(e => e.ModePaiement)
                .HasMaxLength(50)
                .HasColumnName("mode_paiement");
            entity.Property(e => e.Statut)
                .HasMaxLength(50)
                .HasColumnName("statut");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");

            entity.HasOne(d => d.IdCltNavigation).WithMany(p => p.Commandes)
                .HasForeignKey(d => d.IdClt)
                .HasConstraintName("commande_id_clt_fkey");
        });

        modelBuilder.Entity<Facture>(entity =>
        {
            entity.HasKey(e => e.IdFact).HasName("facture_pkey");

            entity.ToTable("facture");

            entity.Property(e => e.IdFact).HasColumnName("id_fact");
            entity.Property(e => e.DateFact).HasColumnName("date_fact");
            entity.Property(e => e.IdCom).HasColumnName("id_com");
            entity.Property(e => e.ModePaiement)
                .HasMaxLength(50)
                .HasColumnName("mode_paiement");
            entity.Property(e => e.MontantTotal)
                .HasPrecision(10, 2)
                .HasColumnName("montant_total");

            entity.HasOne(d => d.IdComNavigation).WithMany(p => p.Factures)
                .HasForeignKey(d => d.IdCom)
                .HasConstraintName("facture_id_com_fkey");
        });

        modelBuilder.Entity<Fournisseur>(entity =>
        {
            entity.HasKey(e => e.IdFour).HasName("fournisseur_pkey");

            entity.ToTable("fournisseur");

            entity.Property(e => e.IdFour).HasColumnName("id_four");
            entity.Property(e => e.Adresse).HasColumnName("adresse");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.NomSociete)
                .HasMaxLength(150)
                .HasColumnName("nom_societe");
            entity.Property(e => e.Tel)
                .HasMaxLength(20)
                .HasColumnName("tel");
            entity.Property(e => e.Ville)
                .HasMaxLength(100)
                .HasColumnName("ville");
        });

        modelBuilder.Entity<LCommande>(entity =>
        {
            entity.HasKey(e => new { e.IdCom, e.IdArt }).HasName("l_commande_pkey");

            entity.ToTable("l_commande");

            entity.Property(e => e.IdCom).HasColumnName("id_com");
            entity.Property(e => e.IdArt).HasColumnName("id_art");
            entity.Property(e => e.PrixAchat)
                .HasPrecision(10, 2)
                .HasColumnName("prix_achat");
            entity.Property(e => e.Quantite).HasColumnName("quantite");
            entity.Property(e => e.Remise)
                .HasPrecision(5, 2)
                .HasColumnName("remise");

            entity.HasOne(d => d.IdArtNavigation).WithMany(p => p.LCommandes)
                .HasForeignKey(d => d.IdArt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("l_commande_id_art_fkey");

            entity.HasOne(d => d.IdComNavigation).WithMany(p => p.LCommandes)
                .HasForeignKey(d => d.IdCom)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("l_commande_id_com_fkey");
        });

        modelBuilder.Entity<LFournisseur>(entity =>
        {
            entity.HasKey(e => new { e.IdFour, e.IdArt }).HasName("l_fournisseur_pkey");

            entity.ToTable("l_fournisseur");

            entity.Property(e => e.IdFour).HasColumnName("id_four");
            entity.Property(e => e.IdArt).HasColumnName("id_art");
            entity.Property(e => e.DelaiLivraison).HasColumnName("delai_livraison");
            entity.Property(e => e.PrixFournisseur)
                .HasPrecision(10, 2)
                .HasColumnName("prix_fournisseur");

            entity.HasOne(d => d.IdArtNavigation).WithMany(p => p.LFournisseurs)
                .HasForeignKey(d => d.IdArt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("l_fournisseur_id_art_fkey");

            entity.HasOne(d => d.IdFourNavigation).WithMany(p => p.LFournisseurs)
                .HasForeignKey(d => d.IdFour)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("l_fournisseur_id_four_fkey");
        });

        modelBuilder.Entity<LPanier>(entity =>
        {
            entity.HasKey(e => new { e.IdPan, e.IdArt }).HasName("l_panier_pkey");

            entity.ToTable("l_panier");

            entity.Property(e => e.IdPan).HasColumnName("id_pan");
            entity.Property(e => e.IdArt).HasColumnName("id_art");
            entity.Property(e => e.DateAjout)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_ajout");
            entity.Property(e => e.Quantite).HasColumnName("quantite");

            entity.HasOne(d => d.IdArtNavigation).WithMany(p => p.LPaniers)
                .HasForeignKey(d => d.IdArt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("l_panier_id_art_fkey");

            entity.HasOne(d => d.IdPanNavigation).WithMany(p => p.LPaniers)
                .HasForeignKey(d => d.IdPan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("l_panier_id_pan_fkey");
        });

        modelBuilder.Entity<Panier>(entity =>
        {
            entity.HasKey(e => e.IdPan).HasName("panier_pkey");

            entity.ToTable("panier");

            entity.Property(e => e.IdPan).HasColumnName("id_pan");
            entity.Property(e => e.DateCreation)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_creation");
            entity.Property(e => e.IdClt).HasColumnName("id_clt");
        });

        modelBuilder.Entity<Souscategorie>(entity =>
        {
            entity.HasKey(e => e.IdScat).HasName("souscategorie_pkey");

            entity.ToTable("souscategorie");

            entity.Property(e => e.IdScat).HasColumnName("id_scat");
            entity.Property(e => e.CodeScat)
                .HasMaxLength(50)
                .HasColumnName("code_scat");
            entity.Property(e => e.DateCreation)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_creation");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdCat).HasColumnName("id_cat");
            entity.Property(e => e.Libelle)
                .HasMaxLength(100)
                .HasColumnName("libelle");

            entity.HasOne(d => d.IdCatNavigation).WithMany(p => p.Souscategories)
                .HasForeignKey(d => d.IdCat)
                .HasConstraintName("souscategorie_id_cat_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

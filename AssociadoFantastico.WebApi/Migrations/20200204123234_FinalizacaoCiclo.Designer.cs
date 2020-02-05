﻿// <auto-generated />
using System;
using AssociadoFantastico.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AssociadoFantastico.WebApi.Migrations
{
    [DbContext(typeof(AssociadoFantasticoContext))]
    [Migration("20200204123234_FinalizacaoCiclo")]
    partial class FinalizacaoCiclo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AssociadoFantastico.Domain.Entities.Associado", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Aplausogramas");

                    b.Property<string>("CentroCusto");

                    b.Property<Guid>("CicloId");

                    b.Property<Guid>("GrupoId");

                    b.Property<Guid>("UsuarioId");

                    b.HasKey("Id");

                    b.HasIndex("CicloId");

                    b.HasIndex("GrupoId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Associados");
                });

            modelBuilder.Entity("AssociadoFantastico.Domain.Entities.Ciclo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Ano");

                    b.Property<DateTime?>("DataFinalizacao");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<Guid>("EmpresaId");

                    b.Property<int>("Semestre");

                    b.HasKey("Id");

                    b.HasIndex("EmpresaId");

                    b.ToTable("Ciclos");
                });

            modelBuilder.Entity("AssociadoFantastico.Domain.Entities.Elegivel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Apuracao");

                    b.Property<Guid>("AssociadoId");

                    b.Property<string>("Foto")
                        .HasMaxLength(255);

                    b.Property<Guid>("VotacaoId");

                    b.Property<int>("Votos");

                    b.HasKey("Id");

                    b.HasIndex("AssociadoId");

                    b.HasIndex("VotacaoId", "AssociadoId")
                        .IsUnique();

                    b.ToTable("Elegiveis");
                });

            modelBuilder.Entity("AssociadoFantastico.Domain.Entities.Empresa", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Empresas");
                });

            modelBuilder.Entity("AssociadoFantastico.Domain.Entities.Grupo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Ativo");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("Grupos");
                });

            modelBuilder.Entity("AssociadoFantastico.Domain.Entities.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Area")
                        .HasMaxLength(255);

                    b.Property<string>("Cargo")
                        .HasMaxLength(255);

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasMaxLength(11);

                    b.Property<Guid>("EmpresaId");

                    b.Property<string>("Matricula")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("Perfil");

                    b.HasKey("Id");

                    b.HasIndex("Cpf")
                        .IsUnique();

                    b.HasIndex("EmpresaId");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("AssociadoFantastico.Domain.Entities.Votacao", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CicloId");

                    b.Property<string>("TipoVotacao")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CicloId");

                    b.ToTable("Votacoes");

                    b.HasDiscriminator<string>("TipoVotacao").HasValue("Votacao");
                });

            modelBuilder.Entity("AssociadoFantastico.Domain.Entities.Voto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CandidatoId");

                    b.Property<Guid>("EleitorId");

                    b.Property<DateTime>("Horario");

                    b.Property<string>("Ip")
                        .HasMaxLength(20);

                    b.Property<Guid>("VotacaoId");

                    b.HasKey("Id");

                    b.HasIndex("CandidatoId");

                    b.HasIndex("EleitorId")
                        .IsUnique();

                    b.HasIndex("VotacaoId");

                    b.ToTable("Votos");
                });

            modelBuilder.Entity("AssociadoFantastico.Domain.Entities.VotacaoAssociadoFantastico", b =>
                {
                    b.HasBaseType("AssociadoFantastico.Domain.Entities.Votacao");

                    b.HasDiscriminator().HasValue("Associado Fantástico");
                });

            modelBuilder.Entity("AssociadoFantastico.Domain.Entities.VotacaoAssociadoSuperFantastico", b =>
                {
                    b.HasBaseType("AssociadoFantastico.Domain.Entities.Votacao");

                    b.HasDiscriminator().HasValue("Associado Super Fantástico");
                });

            modelBuilder.Entity("AssociadoFantastico.Domain.Entities.Associado", b =>
                {
                    b.HasOne("AssociadoFantastico.Domain.Entities.Ciclo", "Ciclo")
                        .WithMany("Associados")
                        .HasForeignKey("CicloId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AssociadoFantastico.Domain.Entities.Grupo", "Grupo")
                        .WithMany("Associados")
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AssociadoFantastico.Domain.Entities.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AssociadoFantastico.Domain.Entities.Ciclo", b =>
                {
                    b.HasOne("AssociadoFantastico.Domain.Entities.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("EmpresaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AssociadoFantastico.Domain.Entities.Elegivel", b =>
                {
                    b.HasOne("AssociadoFantastico.Domain.Entities.Associado", "Associado")
                        .WithMany()
                        .HasForeignKey("AssociadoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AssociadoFantastico.Domain.Entities.Votacao", "Votacao")
                        .WithMany("Elegiveis")
                        .HasForeignKey("VotacaoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AssociadoFantastico.Domain.Entities.Usuario", b =>
                {
                    b.HasOne("AssociadoFantastico.Domain.Entities.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("EmpresaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AssociadoFantastico.Domain.Entities.Votacao", b =>
                {
                    b.HasOne("AssociadoFantastico.Domain.Entities.Ciclo", "Ciclo")
                        .WithMany("Votacoes")
                        .HasForeignKey("CicloId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("AssociadoFantastico.Domain.Entities.Dimensionamento", "Dimensionamento", b1 =>
                        {
                            b1.Property<Guid>("VotacaoId");

                            b1.Property<int>("Acrescimo");

                            b1.Property<int>("Intervalo");

                            b1.HasKey("VotacaoId");

                            b1.ToTable("Votacoes");

                            b1.HasOne("AssociadoFantastico.Domain.Entities.Votacao")
                                .WithOne("Dimensionamento")
                                .HasForeignKey("AssociadoFantastico.Domain.Entities.Dimensionamento", "VotacaoId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("AssociadoFantastico.Domain.Entities.Periodo", "PeriodoPrevisto", b1 =>
                        {
                            b1.Property<Guid>("VotacaoId");

                            b1.Property<DateTime?>("DataFim")
                                .IsRequired();

                            b1.Property<DateTime?>("DataInicio")
                                .IsRequired();

                            b1.HasKey("VotacaoId");

                            b1.ToTable("Votacoes");

                            b1.HasOne("AssociadoFantastico.Domain.Entities.Votacao")
                                .WithOne("PeriodoPrevisto")
                                .HasForeignKey("AssociadoFantastico.Domain.Entities.Periodo", "VotacaoId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("AssociadoFantastico.Domain.Entities.Periodo", "PeriodoRealizado", b1 =>
                        {
                            b1.Property<Guid>("VotacaoId");

                            b1.Property<DateTime?>("DataFim");

                            b1.Property<DateTime?>("DataInicio");

                            b1.HasKey("VotacaoId");

                            b1.ToTable("Votacoes");

                            b1.HasOne("AssociadoFantastico.Domain.Entities.Votacao")
                                .WithOne("PeriodoRealizado")
                                .HasForeignKey("AssociadoFantastico.Domain.Entities.Periodo", "VotacaoId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("AssociadoFantastico.Domain.Entities.Voto", b =>
                {
                    b.HasOne("AssociadoFantastico.Domain.Entities.Elegivel", "Candidato")
                        .WithMany()
                        .HasForeignKey("CandidatoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AssociadoFantastico.Domain.Entities.Associado", "Eleitor")
                        .WithOne()
                        .HasForeignKey("AssociadoFantastico.Domain.Entities.Voto", "EleitorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AssociadoFantastico.Domain.Entities.Votacao", "Votacao")
                        .WithMany("Votos")
                        .HasForeignKey("VotacaoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

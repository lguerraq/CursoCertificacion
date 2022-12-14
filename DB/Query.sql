USE [master]
GO
/****** Object:  Database [db_CurCer]    Script Date: 14/10/2022 17:52:12 ******/
CREATE DATABASE [db_CurCer]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DB_A506B1_avaspro_Data', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\DB_A506B1_avaspro_DATA.mdf' , SIZE = 8192KB , MAXSIZE = 256000KB , FILEGROWTH = 10%)
 LOG ON 
( NAME = N'DB_A506B1_avaspro_Log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\DB_A506B1_avaspro_Log.LDF' , SIZE = 3072KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [db_CurCer] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [db_CurCer].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [db_CurCer] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [db_CurCer] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [db_CurCer] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [db_CurCer] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [db_CurCer] SET ARITHABORT OFF 
GO
ALTER DATABASE [db_CurCer] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [db_CurCer] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [db_CurCer] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [db_CurCer] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [db_CurCer] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [db_CurCer] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [db_CurCer] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [db_CurCer] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [db_CurCer] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [db_CurCer] SET  DISABLE_BROKER 
GO
ALTER DATABASE [db_CurCer] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [db_CurCer] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [db_CurCer] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [db_CurCer] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [db_CurCer] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [db_CurCer] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [db_CurCer] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [db_CurCer] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [db_CurCer] SET  MULTI_USER 
GO
ALTER DATABASE [db_CurCer] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [db_CurCer] SET DB_CHAINING OFF 
GO
ALTER DATABASE [db_CurCer] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [db_CurCer] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [db_CurCer] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [db_CurCer] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [db_CurCer] SET QUERY_STORE = OFF
GO
USE [db_CurCer]
GO
/****** Object:  Table [dbo].[Configuracion]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Configuracion](
	[IdConfiguracion] [int] IDENTITY(1,1) NOT NULL,
	[Variable] [varchar](50) NOT NULL,
	[Valor] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Configuracion] PRIMARY KEY CLUSTERED 
(
	[IdConfiguracion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Conversacion]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Conversacion](
	[IdConversacion] [int] IDENTITY(1,1) NOT NULL,
	[FechaHora] [datetime] NULL,
	[NombreInicioConversacion] [varchar](250) NULL,
	[IdUsuario] [int] NULL,
 CONSTRAINT [PK_Conversacion] PRIMARY KEY CLUSTERED 
(
	[IdConversacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConversacionDetalle]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConversacionDetalle](
	[IdConversacionDetalle] [int] IDENTITY(1,1) NOT NULL,
	[IdConversacion] [int] NULL,
	[Descripcion] [varchar](500) NULL,
	[FechaHora] [datetime] NULL,
 CONSTRAINT [PK_ConversacionDetalle] PRIMARY KEY CLUSTERED 
(
	[IdConversacionDetalle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Correo]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Correo](
	[IdCorreo] [int] IDENTITY(1,1) NOT NULL,
	[Asunto] [varchar](150) NULL,
	[Origen] [varchar](50) NULL,
	[NombreOrigen] [varchar](150) NULL,
	[Mensaje] [nvarchar](max) NULL,
	[EstadoCorreo] [int] NULL,
	[FechaEnvio] [datetime] NULL,
	[NumeroEnvio] [int] NULL,
	[Estado] [bit] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Correo] PRIMARY KEY CLUSTERED 
(
	[IdCorreo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CorreoDifusion]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CorreoDifusion](
	[IdCorreoDifusion] [int] IDENTITY(1,1) NOT NULL,
	[IdCorreo] [int] NULL,
	[IdEvento] [int] NULL,
	[IdPersona] [int] NOT NULL,
	[Correo] [varchar](200) NOT NULL,
	[Estado] [int] NOT NULL,
	[ErrorMensaje] [varchar](max) NULL,
	[ErrorStackTrace] [varchar](max) NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[NumeroEnvio] [int] NULL,
	[Pago] [bit] NULL,
 CONSTRAINT [PK__CorreoDi__80B7FCF21D16A575] PRIMARY KEY CLUSTERED 
(
	[IdCorreoDifusion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cuestionario]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cuestionario](
	[IdCuestionario] [int] IDENTITY(1,1) NOT NULL,
	[IdLeccion] [int] NOT NULL,
	[Nombre] [varchar](150) NOT NULL,
	[Descripcion] [varchar](250) NULL,
	[Peso] [int] NULL,
	[Estado] [bit] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Cuestionario] PRIMARY KEY CLUSTERED 
(
	[IdCuestionario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CuestionarioRespuesta]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CuestionarioRespuesta](
	[IdCuestionarioRespuesta] [int] IDENTITY(1,1) NOT NULL,
	[IdCuestionarioTomado] [int] NOT NULL,
	[IdPregunta] [int] NOT NULL,
	[IdRespuesta] [int] NOT NULL,
	[Estado] [bit] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_CuestionarioRespuesta] PRIMARY KEY CLUSTERED 
(
	[IdCuestionarioRespuesta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CuestionarioTomado]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CuestionarioTomado](
	[IdCuestionarioTomado] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdCuestionario] [int] NOT NULL,
	[Nota] [decimal](12, 2) NOT NULL,
	[Intento] [int] NOT NULL,
	[Estado] [bit] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_CuestionarioTomado] PRIMARY KEY CLUSTERED 
(
	[IdCuestionarioTomado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Desafiliado]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Desafiliado](
	[IdDesafiliado] [int] IDENTITY(1,1) NOT NULL,
	[Correo] [varchar](250) NULL,
	[Valor] [int] NULL,
	[Observacion] [varchar](250) NULL,
	[Estado] [bit] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Desafiliado] PRIMARY KEY CLUSTERED 
(
	[IdDesafiliado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Docente]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Docente](
	[IdDocuente] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](150) NULL,
	[NombreFoto] [varchar](150) NULL,
	[Profesion] [varchar](250) NULL,
	[Especialista] [varchar](250) NULL,
	[Detalle] [varchar](max) NULL,
	[Estado] [bit] NOT NULL,
	[rowid] [uniqueidentifier] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Docente] PRIMARY KEY CLUSTERED 
(
	[IdDocuente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Documento]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Documento](
	[IdDocumento] [int] IDENTITY(1,1) NOT NULL,
	[IdDocumentoPadre] [int] NULL,
	[IdEmpresa] [int] NULL,
	[Tipo] [int] NULL,
	[Nombre] [varchar](150) NULL,
	[NombreOriginal] [varchar](150) NULL,
	[Extension] [varchar](10) NULL,
	[Tamaño] [int] NULL,
	[EstadoDocumento] [int] NULL,
	[FechaDescarga] [datetime] NULL,
	[Estado] [bit] NOT NULL,
	[rowid] [uniqueidentifier] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Documento] PRIMARY KEY CLUSTERED 
(
	[IdDocumento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentoHistorico]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentoHistorico](
	[IdDocumentoHistorico] [int] IDENTITY(1,1) NOT NULL,
	[IdDocumento] [int] NOT NULL,
	[IdDocumentoPadre] [int] NULL,
	[IdEmpresa] [int] NULL,
	[Tipo] [int] NULL,
	[Nombre] [varchar](150) NULL,
	[NombreOriginal] [varchar](150) NULL,
	[Extension] [varchar](10) NULL,
	[Tamaño] [int] NULL,
	[EstadoDocumento] [int] NULL,
	[FechaDescarga] [datetime] NULL,
	[Estado] [bit] NOT NULL,
	[rowid] [uniqueidentifier] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_DocumentoHistorico] PRIMARY KEY CLUSTERED 
(
	[IdDocumentoHistorico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empresa]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empresa](
	[IdEmpresa] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NULL,
	[Ruc] [varchar](20) NULL,
	[RazonSocial] [varchar](250) NULL,
	[NombreComercial] [varchar](250) NULL,
	[DireccionFiscal] [varchar](250) NULL,
	[Logo] [varbinary](max) NULL,
	[Frecuencia] [int] NULL,
	[Estado] [bit] NOT NULL,
	[rowid] [uniqueidentifier] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Empresa] PRIMARY KEY CLUSTERED 
(
	[IdEmpresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Evento]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Evento](
	[IdEvento] [int] IDENTITY(1,1) NOT NULL,
	[Tipo] [int] NULL,
	[Modalidad] [int] NULL,
	[NombreEvento] [varchar](250) NULL,
	[Descripcion] [varchar](1000) NULL,
	[Fecha] [datetime] NULL,
	[Expositor] [varchar](max) NULL,
	[ImagenEvento] [varchar](150) NULL,
	[DocumentoFotocheck] [varchar](150) NULL,
	[DocumentoCertificado] [varchar](150) NULL,
	[DocumentoCertificadoImprimir] [varchar](150) NULL,
	[DocumentoCertificadoExpositor] [varchar](150) NULL,
	[Horas] [int] NULL,
	[Activo] [bit] NULL,
	[NotaAprobatoria] [decimal](12, 2) NULL,
	[Costo] [varchar](10) NULL,
	[CostoValor] [decimal](12, 2) NULL,
	[CostoValorPromocional] [decimal](12, 2) NULL,
	[DetallarCertificado] [bit] NULL,
	[GenerarCertificado] [bit] NULL,
	[Estado] [bit] NOT NULL,
	[rowid] [uniqueidentifier] NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Evento] PRIMARY KEY CLUSTERED 
(
	[IdEvento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventoCorreo]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventoCorreo](
	[IdEventoCorreo] [int] IDENTITY(1,1) NOT NULL,
	[IdEvento] [int] NOT NULL,
	[Asunto] [varchar](150) NULL,
	[Origen] [varchar](50) NULL,
	[NombreOrigen] [varchar](150) NULL,
	[Mensaje] [nvarchar](max) NULL,
	[EstadoCorreo] [int] NULL,
	[FechaEnvio] [datetime] NULL,
	[NumeroEnvio] [int] NULL,
	[Estado] [bit] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_EventoCorreo] PRIMARY KEY CLUSTERED 
(
	[IdEventoCorreo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventoTema]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventoTema](
	[IdEventoTema] [int] IDENTITY(1,1) NOT NULL,
	[IdEvento] [int] NULL,
	[TipoTema] [int] NULL,
 CONSTRAINT [PK_EventoTema] PRIMARY KEY CLUSTERED 
(
	[IdEventoTema] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventoUsuario]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventoUsuario](
	[IdEventoUsuario] [int] IDENTITY(1,1) NOT NULL,
	[IdEvento] [int] NULL,
	[IdUsuario] [int] NULL,
	[FechaInicio] [datetime] NULL,
	[FechaFin] [datetime] NULL,
	[FechaUltimoAcceso] [datetime] NULL,
	[NotaFinal] [decimal](12, 2) NULL,
	[Abierto] [bit] NULL,
	[Estado] [bit] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_EventoUsuario] PRIMARY KEY CLUSTERED 
(
	[IdEventoUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventoUsuarioVirtualVideo]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventoUsuarioVirtualVideo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdEventoUsuario] [int] NULL,
	[IdVirtualVideo] [int] NULL,
	[Estado] [bit] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_EventoUsuarioVirtualVideo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Expositor]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Expositor](
	[IdExpositor] [int] IDENTITY(1,1) NOT NULL,
	[Nombres] [varchar](50) NULL,
	[ApellidoPaterno] [varchar](50) NULL,
	[ApellidoMaterno] [varchar](50) NULL,
	[Correo] [varchar](50) NULL,
	[Telefono] [varchar](9) NULL,
	[Estado] [bit] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Expositor] PRIMARY KEY CLUSTERED 
(
	[IdExpositor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Galeria]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Galeria](
	[IdGaleria] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](150) NULL,
	[Nombre] [varchar](100) NULL,
	[Activo] [bit] NULL,
	[Estado] [bit] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Galeria] PRIMARY KEY CLUSTERED 
(
	[IdGaleria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Inscripcion]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inscripcion](
	[IdInscripcion] [int] IDENTITY(1,1) NOT NULL,
	[IdEvento] [int] NULL,
	[IdPersona] [int] NULL,
	[EstadoPago] [int] NULL,
	[TipoPago] [int] NULL,
	[EntregaCertificado] [bit] NULL,
	[TipoModalidad] [int] NULL,
	[Monto] [decimal](9, 2) NULL,
	[NombreBanco] [varchar](250) NULL,
	[FechaOperacion] [datetime] NULL,
	[NumeroOperacion] [varchar](50) NULL,
	[NumeroCertificado] [int] NULL,
	[Certificado] [varchar](150) NULL,
	[Nota] [decimal](9, 2) NULL,
	[TipoInscripcion] [int] NULL,
	[Ruc] [varchar](20) NULL,
	[Estado] [bit] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NULL,
 CONSTRAINT [PK_Inscripcion] PRIMARY KEY CLUSTERED 
(
	[IdInscripcion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Leccion]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Leccion](
	[IdLeccion] [int] IDENTITY(1,1) NOT NULL,
	[IdModulo] [int] NULL,
	[Tipo] [int] NULL,
	[Nombre] [varchar](100) NULL,
	[Descripcion] [nvarchar](max) NULL,
	[Duracion] [int] NULL,
	[TipoUrl] [int] NULL,
	[Url] [varchar](1000) NULL,
	[Orden] [int] NULL,
	[Peso] [int] NULL,
	[Estado] [bit] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Leccion] PRIMARY KEY CLUSTERED 
(
	[IdLeccion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Modulo]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modulo](
	[IdModulo] [int] IDENTITY(1,1) NOT NULL,
	[IdEvento] [int] NULL,
	[Nombre] [varchar](100) NULL,
	[Descripcion] [varchar](max) NULL,
	[Expositor] [varchar](150) NULL,
	[Horas] [int] NULL,
	[Peso] [int] NULL,
	[Estado] [bit] NULL,
	[rowid] [uniqueidentifier] NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Modulo] PRIMARY KEY CLUSTERED 
(
	[IdModulo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Opcion]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Opcion](
	[IdOpcion] [int] IDENTITY(1,1) NOT NULL,
	[IdPadre] [int] NULL,
	[Icono] [varchar](50) NULL,
	[Descipcion] [varchar](135) NULL,
	[UrlDescripcion] [varchar](100) NULL,
	[Orden] [int] NULL,
	[Estado] [bit] NULL,
	[FechaCreacion] [datetime] NULL,
	[UsuarioCreacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
	[UsuarioModificacion] [int] NULL,
 CONSTRAINT [PK_Opcion] PRIMARY KEY CLUSTERED 
(
	[IdOpcion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OpcionUsuarioTipo]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OpcionUsuarioTipo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdOpcion] [int] NOT NULL,
	[IdUsuarioTipo] [int] NOT NULL,
	[Estado] [bit] NULL,
	[FechaCreacion] [datetime] NULL,
	[UsuarioCreacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
	[UsuarioModificacion] [int] NULL,
 CONSTRAINT [PK_OpcionUsuarioTipo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pais]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pais](
	[IdPais] [int] IDENTITY(1,1) NOT NULL,
	[NombrePais] [varchar](200) NULL,
 CONSTRAINT [PK_Pais] PRIMARY KEY CLUSTERED 
(
	[IdPais] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Persona]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Persona](
	[IdPersona] [int] IDENTITY(1,1) NOT NULL,
	[Nombres] [varchar](50) NULL,
	[ApellidoPaterno] [varchar](50) NULL,
	[ApellidoMaterno] [varchar](50) NULL,
	[NumeroDocumento] [varchar](11) NULL,
	[CIP] [varchar](50) NULL,
	[Celular] [varchar](20) NULL,
	[Correo] [varchar](50) NULL,
	[Estado] [bit] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
	[IdProfesion] [int] NULL,
	[TipoOcupacion] [int] NULL,
	[DescripcionOcupacion] [varchar](150) NULL,
	[IdPais] [int] NULL,
	[Ciudad] [varchar](200) NULL,
 CONSTRAINT [PK_Persona] PRIMARY KEY CLUSTERED 
(
	[IdPersona] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Portada]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Portada](
	[IdPortada] [int] IDENTITY(1,1) NOT NULL,
	[NombreImagen] [varchar](500) NULL,
	[Descripcion] [varchar](500) NULL,
	[SubTitulo1] [varchar](500) NULL,
	[SubTitulo2] [varchar](500) NULL,
	[TextoEnlace] [varchar](200) NULL,
	[UrlEnlace] [varchar](500) NULL,
	[Estado] [bit] NULL,
	[FechaCreacion] [datetime] NULL,
	[UsuarioCreacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
	[UsuarioModificacion] [int] NULL,
 CONSTRAINT [PK_Portada] PRIMARY KEY CLUSTERED 
(
	[IdPortada] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pregunta]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pregunta](
	[IdPregunta] [int] IDENTITY(1,1) NOT NULL,
	[IdCuestionario] [int] NOT NULL,
	[Tipo] [int] NOT NULL,
	[Nombre] [varchar](1000) NOT NULL,
	[Explicacion] [varchar](1500) NULL,
	[Ayuda] [varchar](250) NULL,
	[Puntaje] [int] NOT NULL,
	[Estado] [bit] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Pregunta] PRIMARY KEY CLUSTERED 
(
	[IdPregunta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Profesion]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profesion](
	[IdProfesion] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](250) NULL,
 CONSTRAINT [PK_Profesion] PRIMARY KEY CLUSTERED 
(
	[IdProfesion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Respuesta]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Respuesta](
	[IdRespuesta] [int] IDENTITY(1,1) NOT NULL,
	[IdPregunta] [int] NOT NULL,
	[Descripcion] [varchar](250) NOT NULL,
	[EsCorrecta] [bit] NOT NULL,
	[Estado] [bit] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Respuesta] PRIMARY KEY CLUSTERED 
(
	[IdRespuesta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Suceso]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suceso](
	[IdSuceso] [int] IDENTITY(1,1) NOT NULL,
	[NombreSuceso] [varchar](250) NULL,
	[Descripcion] [varchar](1000) NULL,
	[Fecha] [datetime] NULL,
	[ImagenSuceso] [varchar](150) NULL,
	[Horas] [int] NULL,
	[Activo] [bit] NULL,
	[Estado] [bit] NULL,
	[rowid] [uniqueidentifier] NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Suceso] PRIMARY KEY CLUSTERED 
(
	[IdSuceso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tipo]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tipo](
	[IdTipo] [int] IDENTITY(1,1) NOT NULL,
	[NombreTipo] [varchar](50) NULL,
	[Grupo] [varchar](50) NULL,
	[Valor] [int] NULL,
	[Abreviatura] [varchar](10) NULL,
 CONSTRAINT [PK_Tipo] PRIMARY KEY CLUSTERED 
(
	[IdTipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Universidad]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Universidad](
	[IdUniversidad] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](200) NULL,
	[Estado] [bit] NULL,
 CONSTRAINT [PK_Universidad] PRIMARY KEY CLUSTERED 
(
	[IdUniversidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Login] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[NumeroDocumento] [varchar](20) NULL,
	[Nombres] [varchar](50) NULL,
	[ApellidoPaterno] [varchar](50) NULL,
	[ApellidoMaterno] [varchar](50) NULL,
	[Correo] [varchar](100) NULL,
	[IdUsuarioTipo] [int] NULL,
	[Estado] [bit] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuarioActividad]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioActividad](
	[IdUsuarioActividad] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NULL,
	[FechaCreacion] [datetime] NULL,
	[Token] [varchar](250) NULL,
 CONSTRAINT [PK_UsuarioActividad] PRIMARY KEY CLUSTERED 
(
	[IdUsuarioActividad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuarioActividadHistorico]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioActividadHistorico](
	[IdUsuarioActividad] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NULL,
	[FechaCreacion] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuarioHistorico]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioHistorico](
	[IdUsuarioHistorico] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NULL,
	[FechaCreacion] [datetime] NULL,
 CONSTRAINT [PK_UsuarioHistorico] PRIMARY KEY CLUSTERED 
(
	[IdUsuarioHistorico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuarioTipo]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioTipo](
	[IdUsuarioTipo] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](50) NULL,
 CONSTRAINT [PK_UsuarioTipo] PRIMARY KEY CLUSTERED 
(
	[IdUsuarioTipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VirtualContenido]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VirtualContenido](
	[IdVirtualContenido] [int] IDENTITY(1,1) NOT NULL,
	[IdEvento] [int] NULL,
	[Contenido] [nvarchar](max) NULL,
	[Estado] [bit] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_VirtualContenido] PRIMARY KEY CLUSTERED 
(
	[IdVirtualContenido] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VirtualVideo]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VirtualVideo](
	[IdVirtualVideo] [int] IDENTITY(1,1) NOT NULL,
	[IdEvento] [int] NULL,
	[Url] [varchar](250) NULL,
	[Estado] [bit] NOT NULL,
	[UsuarioCreacion] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[UsuarioModificacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
 CONSTRAINT [PK_VirtualVideo] PRIMARY KEY CLUSTERED 
(
	[IdVirtualVideo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Correo] ADD  CONSTRAINT [DF__Correo__Estado__4CA06362]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Correo] ADD  CONSTRAINT [DF__Correo__UsuarioC__4D94879B]  DEFAULT ((1)) FOR [UsuarioCreacion]
GO
ALTER TABLE [dbo].[Correo] ADD  CONSTRAINT [DF__Correo__FechaCre__4E88ABD4]  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Docente] ADD  CONSTRAINT [DF_Docente_Estado]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Docente] ADD  CONSTRAINT [DF_Docente_rowid]  DEFAULT (newid()) FOR [rowid]
GO
ALTER TABLE [dbo].[Docente] ADD  CONSTRAINT [DF_Docente_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Evento] ADD  CONSTRAINT [DF_Evento_rowid]  DEFAULT (newid()) FOR [rowid]
GO
ALTER TABLE [dbo].[Galeria] ADD  CONSTRAINT [DF_Galeria_Estado]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Galeria] ADD  CONSTRAINT [DF_Galeria_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[ConversacionDetalle]  WITH CHECK ADD  CONSTRAINT [FK_ConversacionDetalle_Conversacion] FOREIGN KEY([IdConversacion])
REFERENCES [dbo].[Conversacion] ([IdConversacion])
GO
ALTER TABLE [dbo].[ConversacionDetalle] CHECK CONSTRAINT [FK_ConversacionDetalle_Conversacion]
GO
ALTER TABLE [dbo].[CorreoDifusion]  WITH CHECK ADD  CONSTRAINT [FK_CorreoDifusion_Correo] FOREIGN KEY([IdCorreo])
REFERENCES [dbo].[Correo] ([IdCorreo])
GO
ALTER TABLE [dbo].[CorreoDifusion] CHECK CONSTRAINT [FK_CorreoDifusion_Correo]
GO
ALTER TABLE [dbo].[CuestionarioRespuesta]  WITH CHECK ADD  CONSTRAINT [FK_CuestionarioRespuesta_CuestionarioTomado] FOREIGN KEY([IdCuestionarioTomado])
REFERENCES [dbo].[CuestionarioTomado] ([IdCuestionarioTomado])
GO
ALTER TABLE [dbo].[CuestionarioRespuesta] CHECK CONSTRAINT [FK_CuestionarioRespuesta_CuestionarioTomado]
GO
ALTER TABLE [dbo].[CuestionarioTomado]  WITH CHECK ADD  CONSTRAINT [FK_CuestionarioTomado_Cuestionario] FOREIGN KEY([IdCuestionario])
REFERENCES [dbo].[Cuestionario] ([IdCuestionario])
GO
ALTER TABLE [dbo].[CuestionarioTomado] CHECK CONSTRAINT [FK_CuestionarioTomado_Cuestionario]
GO
ALTER TABLE [dbo].[Documento]  WITH CHECK ADD  CONSTRAINT [FK_Documento_Empresa] FOREIGN KEY([IdEmpresa])
REFERENCES [dbo].[Empresa] ([IdEmpresa])
GO
ALTER TABLE [dbo].[Documento] CHECK CONSTRAINT [FK_Documento_Empresa]
GO
ALTER TABLE [dbo].[Empresa]  WITH CHECK ADD  CONSTRAINT [FK_Empresa_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Empresa] CHECK CONSTRAINT [FK_Empresa_Usuario]
GO
ALTER TABLE [dbo].[EventoCorreo]  WITH CHECK ADD  CONSTRAINT [FK_EventoCorreo_Evento] FOREIGN KEY([IdEvento])
REFERENCES [dbo].[Evento] ([IdEvento])
GO
ALTER TABLE [dbo].[EventoCorreo] CHECK CONSTRAINT [FK_EventoCorreo_Evento]
GO
ALTER TABLE [dbo].[EventoTema]  WITH CHECK ADD  CONSTRAINT [FK_EventoTema_Evento] FOREIGN KEY([IdEvento])
REFERENCES [dbo].[Evento] ([IdEvento])
GO
ALTER TABLE [dbo].[EventoTema] CHECK CONSTRAINT [FK_EventoTema_Evento]
GO
ALTER TABLE [dbo].[EventoUsuario]  WITH CHECK ADD  CONSTRAINT [FK_EventoUsuario_Evento] FOREIGN KEY([IdEvento])
REFERENCES [dbo].[Evento] ([IdEvento])
GO
ALTER TABLE [dbo].[EventoUsuario] CHECK CONSTRAINT [FK_EventoUsuario_Evento]
GO
ALTER TABLE [dbo].[EventoUsuario]  WITH CHECK ADD  CONSTRAINT [FK_EventoUsuario_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[EventoUsuario] CHECK CONSTRAINT [FK_EventoUsuario_Usuario]
GO
ALTER TABLE [dbo].[EventoUsuarioVirtualVideo]  WITH CHECK ADD  CONSTRAINT [FK_EventoUsuarioVirtualVideo_EventoUsuario] FOREIGN KEY([IdEventoUsuario])
REFERENCES [dbo].[EventoUsuario] ([IdEventoUsuario])
GO
ALTER TABLE [dbo].[EventoUsuarioVirtualVideo] CHECK CONSTRAINT [FK_EventoUsuarioVirtualVideo_EventoUsuario]
GO
ALTER TABLE [dbo].[EventoUsuarioVirtualVideo]  WITH CHECK ADD  CONSTRAINT [FK_EventoUsuarioVirtualVideo_VirtualVideo] FOREIGN KEY([IdVirtualVideo])
REFERENCES [dbo].[VirtualVideo] ([IdVirtualVideo])
GO
ALTER TABLE [dbo].[EventoUsuarioVirtualVideo] CHECK CONSTRAINT [FK_EventoUsuarioVirtualVideo_VirtualVideo]
GO
ALTER TABLE [dbo].[Inscripcion]  WITH CHECK ADD  CONSTRAINT [FK_Inscripcion_Evento] FOREIGN KEY([IdEvento])
REFERENCES [dbo].[Evento] ([IdEvento])
GO
ALTER TABLE [dbo].[Inscripcion] CHECK CONSTRAINT [FK_Inscripcion_Evento]
GO
ALTER TABLE [dbo].[Inscripcion]  WITH CHECK ADD  CONSTRAINT [FK_Inscripcion_Persona] FOREIGN KEY([IdPersona])
REFERENCES [dbo].[Persona] ([IdPersona])
GO
ALTER TABLE [dbo].[Inscripcion] CHECK CONSTRAINT [FK_Inscripcion_Persona]
GO
ALTER TABLE [dbo].[Leccion]  WITH CHECK ADD  CONSTRAINT [FK_Leccion_Modulo] FOREIGN KEY([IdModulo])
REFERENCES [dbo].[Modulo] ([IdModulo])
GO
ALTER TABLE [dbo].[Leccion] CHECK CONSTRAINT [FK_Leccion_Modulo]
GO
ALTER TABLE [dbo].[Modulo]  WITH CHECK ADD  CONSTRAINT [FK_Modulo_Evento] FOREIGN KEY([IdEvento])
REFERENCES [dbo].[Evento] ([IdEvento])
GO
ALTER TABLE [dbo].[Modulo] CHECK CONSTRAINT [FK_Modulo_Evento]
GO
ALTER TABLE [dbo].[OpcionUsuarioTipo]  WITH CHECK ADD  CONSTRAINT [FK_OpcionUsuarioTipo_Opcion] FOREIGN KEY([IdOpcion])
REFERENCES [dbo].[Opcion] ([IdOpcion])
GO
ALTER TABLE [dbo].[OpcionUsuarioTipo] CHECK CONSTRAINT [FK_OpcionUsuarioTipo_Opcion]
GO
ALTER TABLE [dbo].[OpcionUsuarioTipo]  WITH CHECK ADD  CONSTRAINT [FK_OpcionUsuarioTipo_UsuarioTipo] FOREIGN KEY([IdUsuarioTipo])
REFERENCES [dbo].[UsuarioTipo] ([IdUsuarioTipo])
GO
ALTER TABLE [dbo].[OpcionUsuarioTipo] CHECK CONSTRAINT [FK_OpcionUsuarioTipo_UsuarioTipo]
GO
ALTER TABLE [dbo].[Persona]  WITH CHECK ADD  CONSTRAINT [FK_Persona_Pais] FOREIGN KEY([IdPais])
REFERENCES [dbo].[Pais] ([IdPais])
GO
ALTER TABLE [dbo].[Persona] CHECK CONSTRAINT [FK_Persona_Pais]
GO
ALTER TABLE [dbo].[Persona]  WITH CHECK ADD  CONSTRAINT [FK_Persona_Profesion] FOREIGN KEY([IdProfesion])
REFERENCES [dbo].[Profesion] ([IdProfesion])
GO
ALTER TABLE [dbo].[Persona] CHECK CONSTRAINT [FK_Persona_Profesion]
GO
ALTER TABLE [dbo].[Pregunta]  WITH CHECK ADD  CONSTRAINT [FK_Pregunta_Cuestionario] FOREIGN KEY([IdCuestionario])
REFERENCES [dbo].[Cuestionario] ([IdCuestionario])
GO
ALTER TABLE [dbo].[Pregunta] CHECK CONSTRAINT [FK_Pregunta_Cuestionario]
GO
ALTER TABLE [dbo].[Respuesta]  WITH CHECK ADD  CONSTRAINT [FK_Respuesta_Pregunta] FOREIGN KEY([IdPregunta])
REFERENCES [dbo].[Pregunta] ([IdPregunta])
GO
ALTER TABLE [dbo].[Respuesta] CHECK CONSTRAINT [FK_Respuesta_Pregunta]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_UsuarioTipo] FOREIGN KEY([IdUsuarioTipo])
REFERENCES [dbo].[UsuarioTipo] ([IdUsuarioTipo])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_UsuarioTipo]
GO
ALTER TABLE [dbo].[UsuarioActividad]  WITH CHECK ADD  CONSTRAINT [FK_UsuarioActividad_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[UsuarioActividad] CHECK CONSTRAINT [FK_UsuarioActividad_Usuario]
GO
ALTER TABLE [dbo].[UsuarioHistorico]  WITH CHECK ADD  CONSTRAINT [FK_UsuarioHistorico_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[UsuarioHistorico] CHECK CONSTRAINT [FK_UsuarioHistorico_Usuario]
GO
ALTER TABLE [dbo].[VirtualContenido]  WITH CHECK ADD  CONSTRAINT [FK_VirtualContenido_Evento] FOREIGN KEY([IdEvento])
REFERENCES [dbo].[Evento] ([IdEvento])
GO
ALTER TABLE [dbo].[VirtualContenido] CHECK CONSTRAINT [FK_VirtualContenido_Evento]
GO
ALTER TABLE [dbo].[VirtualVideo]  WITH CHECK ADD  CONSTRAINT [FK_VirtualVideo_Evento] FOREIGN KEY([IdEvento])
REFERENCES [dbo].[Evento] ([IdEvento])
GO
ALTER TABLE [dbo].[VirtualVideo] CHECK CONSTRAINT [FK_VirtualVideo_Evento]
GO
/****** Object:  StoredProcedure [dbo].[CantidadMensajesByMes]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CantidadMensajesByMes]
AS
BEGIN
	SELECT
		YEAR(FechaCreacion) Año,
		CASE
			WHEN MONTH(FechaCreacion)=1 THEN 'ENERO'
			WHEN MONTH(FechaCreacion)=2 THEN 'FEBRERO'
			WHEN MONTH(FechaCreacion)=3 THEN 'MARZO'
			WHEN MONTH(FechaCreacion)=4 THEN 'ABRIL'
			WHEN MONTH(FechaCreacion)=5 THEN 'MAYO'
			WHEN MONTH(FechaCreacion)=6 THEN 'JUNIO'
			WHEN MONTH(FechaCreacion)=7 THEN 'JULIO'
			WHEN MONTH(FechaCreacion)=8 THEN 'AGOSTO'
			WHEN MONTH(FechaCreacion)=9 THEN 'SEPTIEMBRE'
			WHEN MONTH(FechaCreacion)=10 THEN 'OCTUBRE'
			WHEN MONTH(FechaCreacion)=11 THEN 'NOVIEMBRE'
			WHEN MONTH(FechaCreacion)=12 THEN 'DICIEMBRE'
			END Mes, 
		COUNT(1) TotalEnviados,
		CASE WHEN COUNT(1)>10000 THEN 10000 ELSE COUNT(1) END  EnviadosGratis,
		CASE 
			WHEN (COUNT(1)-10000)>0 THEN (COUNT(1)-10000)
			ELSE 0 END EnviadosAdicionales,
		0.005 CostoEnvioAdicional,
		CASE 
			WHEN (COUNT(1)-10000)>0 THEN (COUNT(1)-10000)/200.00
			ELSE 0 END TotalCostoAdicional
	FROM CorreoDifusion
	WHERE Estado=1
	GROUP by YEAR(FechaCreacion),MONTH(FechaCreacion)
	ORDER by MONTH(FechaCreacion)
END
GO
/****** Object:  StoredProcedure [dbo].[ListCantidadMensajesByMes]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ListCantidadMensajesByMes]
AS
BEGIN
	SELECT
		CAST(ROW_NUMBER() OVER(ORDER BY MONTH(FechaCreacion) DESC) AS INT) AS Row,
		CAST(YEAR(FechaCreacion) AS INT) Año,
		CAST(MONTH(FechaCreacion) AS INT) Mes,
		COUNT(1) TotalEnviados,
		CASE WHEN COUNT(1)>10000 THEN 10000 ELSE COUNT(1) END  EnviadosGratis,
		CASE 
			WHEN (COUNT(1)-10000)>0 THEN (COUNT(1)-10000)
			ELSE 0 END EnviadosAdicionales,
		0.005 CostoEnvioAdicional,
		CASE 
			WHEN (COUNT(1)-10000)>0 THEN (COUNT(1)-10000)/200.00
			ELSE 0 END TotalCostoAdicional
	FROM CorreoDifusion WITH(NOLOCK)
	WHERE Estado=1 AND Pago IS NULL
	GROUP by YEAR(FechaCreacion),MONTH(FechaCreacion)
END
GO
/****** Object:  StoredProcedure [dbo].[ListEventoUsuarioVirtualVideoByEvento]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ListEventoUsuarioVirtualVideoByEvento]
(
	@IdEvento INT,
	@IdEventoUsuario INT
)
AS
BEGIN
	SELECT 
		vv.IdVirtualVideo,
        vv.IdEvento,
        vv.Url,
        euvv.Id IdEventoUsuarioVirtualVideo
	FROM VirtualVideo vv 
	LEFT JOIN EventoUsuarioVirtualVideo euvv on vv.IdVirtualVideo=euvv.IdVirtualVideo AND euvv.Estado=1 AND euvv.IdEventoUsuario=@IdEventoUsuario
	WHERE vv.IdEvento=@IdEvento and vv.Estado=1 
END
GO
/****** Object:  StoredProcedure [dbo].[ListHistorialActividadUsuario]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ListHistorialActividadUsuario]
AS
BEGIN	
	SELECT 
		ut.Descripcion TipoUsuario,
		u.Nombres,
		u.ApellidoPaterno + ' ' + u.ApellidoMaterno Apellidos,	
		DATEADD(HOUR,2,uh.FechaCreacion ) FechaIngreso
	FROM dbo.UsuarioHistorico uh 
	INNER JOIN dbo.Usuario u ON uh.IdUsuario=u.IdUsuario
	INNER JOIN dbo.UsuarioTipo ut ON u.IdUsuarioTipo=ut.IdUsuarioTipo
	WHERE u.IdUsuario<>1
	ORDER BY u.IdUsuario,uh.FechaCreacion

END
GO
/****** Object:  StoredProcedure [dbo].[ListVirtualVideoByUsuarioByEvento]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ListVirtualVideoByUsuarioByEvento]
(
	@IdEvento INT,
	@IdUsuario INT
)
AS
BEGIN
	DECLARE @IdTipoUsuario INT;

	SET @IdTipoUsuario=(SELECT u.IdUsuarioTipo FROM Usuario u WHERE u.IdUsuario=@IdUsuario);

	IF(@IdTipoUsuario=1 OR @IdTipoUsuario=2)
		BEGIN
			SELECT 
				vv.IdVirtualVideo,
				vv.IdEvento,
				vv.Url,
				CAST(1 AS BIT) MostrarVideo
			FROM VirtualVideo vv 
			WHERE vv.IdEvento=@IdEvento and vv.Estado=1
		END
	ELSE
		BEGIN
			SELECT 
				vv.IdVirtualVideo,
				vv.IdEvento,
				vv.Url,
				(CASE WHEN RECORD.Id IS NOT NULL THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END) MostrarVideo
			FROM VirtualVideo vv 
			LEFT JOIN (SELECT 
							euvv.Id,
							euvv.IdVirtualVideo
						FROM EventoUsuario eu 
						INNER JOIN EventoUsuarioVirtualVideo euvv ON eu.IdEventoUsuario=euvv.IdEventoUsuario
						WHERE eu.Estado=1 AND euvv.Estado=1 AND eu.IdEvento=@IdEvento AND eu.IdUsuario=@IdUsuario) RECORD ON RECORD.IdVirtualVideo=vv.IdVirtualVideo
			WHERE vv.IdEvento=@IdEvento and vv.Estado=1
		END
	
END
GO
/****** Object:  StoredProcedure [dbo].[UspListCorreoPaged]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UspListCorreoPaged]
(
	@search VARCHAR(20),
	@start INT,
	@length INT
)
AS
BEGIN

	DECLARE @end INT;

	SET @end=(@start+@length);

	SELECT 
		RECORD.*,
		(SELECT COUNT(1) FROM CorreoDifusion WITH(NOLOCK) WHERE IdCorreo=RECORD.IdCorreo) Cantidad
	FROM
	(
		SELECT 
			COUNT(1) OVER() TotalRegistros,
			ROW_NUMBER() OVER(ORDER BY p.IdCorreo DESC) AS RowNum, 
			p.IdCorreo,
            p.Asunto,
            p.Origen,
            p.NombreOrigen,
            p.EstadoCorreo,
            p.FechaEnvio,
            p.NumeroEnvio           
		FROM Correo p WITH(NOLOCK)
		WHERE p.Estado=1
	) RECORD WHERE RECORD.RowNum>@start AND RECORD.RowNum<=@end
	ORDER BY RECORD.IdCorreo DESC
END
GO
/****** Object:  StoredProcedure [dbo].[UspListLeccionByModuloPaged]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UspListLeccionByModuloPaged]
(
	@IdModulo INT,
	@search VARCHAR(20),
	@start INT,
	@length INT
)
AS
BEGIN

	DECLARE @end INT;

	SET @end=(@start+@length);

	SELECT 
		RECORD.* 
	FROM
	(
		SELECT 
			COUNT(p.IdModulo) OVER() TotalRegistros,
			ROW_NUMBER() OVER(ORDER BY p.IdModulo ASC) AS RowNum, 
			p.IdLeccion,
			p.IdModulo,
			p.Tipo,
			t.NombreTipo,
			p.Nombre,
            p.Descripcion,
            p.Duracion
		FROM Leccion p WITH(NOLOCK)
		INNER JOIN Tipo t WITH(NOLOCK) 
		ON t.Valor = p.Tipo AND t.Grupo = 'TIPO LECCION'
		WHERE p.Estado=1 and p.IdModulo=@IdModulo
		AND 
		(
			p.Nombre LIKE ('%' + @search + '%')
		)
	) RECORD WHERE RECORD.RowNum>@start AND RECORD.RowNum<=@end

END
GO
/****** Object:  StoredProcedure [dbo].[UspListModuloByEventoPaged]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UspListModuloByEventoPaged]
(
	@IdEvento INT,
	@search VARCHAR(20),
	@start INT,
	@length INT
)
AS
BEGIN

	DECLARE @end INT;

	SET @end=(@start+@length);

	SELECT 
		RECORD.* 
	FROM
	(
		SELECT 
			COUNT(p.IdModulo) OVER() TotalRegistros,
			ROW_NUMBER() OVER(ORDER BY p.IdModulo ASC) AS RowNum, 
			p.IdModulo,
			p.IdEvento,
			p.Nombre,
            p.Descripcion,
			p.Expositor,
            p.Horas
		FROM Modulo p WITH(NOLOCK)
		WHERE p.Estado=1 and p.IdEvento=@IdEvento
		AND 
		(
			p.Nombre LIKE ('%' + @search + '%')
		)
	) RECORD WHERE RECORD.RowNum>@start AND RECORD.RowNum<=@end

END
GO
/****** Object:  StoredProcedure [dbo].[UspListPersonaPaged]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UspListPersonaPaged]
(
	@search VARCHAR(20),
	@start INT,
	@length INT
)
AS
BEGIN

	DECLARE @end INT;

	SET @end=(@start+@length);

	SELECT 
		RECORD.* 
	FROM
	(
		SELECT 
			COUNT(p.IdPersona) OVER() TotalRegistros,
			ROW_NUMBER() OVER(ORDER BY p.IdPersona DESC) AS RowNum, 
			p.IdPersona,
            p.Nombres,
            p.ApellidoPaterno,
            p.ApellidoMaterno,
            p.NumeroDocumento,
            p.TipoOcupacion,
            p.DescripcionOcupacion,
            ocu.NombreTipo TipoOcupacionNombre,
            ocu.Abreviatura TipoOcupacionAbreviatura,
            p.CIP,
            p.Celular,
             p.Correo,
            p.IdProfesion,
            pf.Descripcion,
			p.IdPais,
			p.Ciudad
		FROM persona p WITH(NOLOCK)
		INNER JOIN Tipo ocu WITH(NOLOCK) ON p.TipoOcupacion=ocu.Valor
		LEFT JOIN Profesion pf WITH(NOLOCK) ON p.IdProfesion=pf.IdProfesion
		WHERE p.Estado=1 AND ocu.Grupo = 'OCUPACION'
		AND 
		(
			p.Nombres LIKE ('%' + @search + '%') OR
            p.ApellidoMaterno LIKE  ('%' + @search + '%') OR
            p.ApellidoMaterno LIKE ('%' + @search + '%') OR
            p.NumeroDocumento LIKE ('%' + @search + '%')
		)
	) RECORD WHERE RECORD.RowNum>@start AND RECORD.RowNum<=@end

END
GO
/****** Object:  StoredProcedure [dbo].[UspListPortadaPaged]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UspListPortadaPaged]
(
	@search VARCHAR(20),
	@start INT,
	@length INT
)
AS
BEGIN

	DECLARE @end INT;

	SET @end=(@start+@length);

	SELECT 
		RECORD.* 
	FROM
	(
		SELECT 
			COUNT(p.IdPortada) OVER() TotalRegistros,
			ROW_NUMBER() OVER(ORDER BY p.IdPortada ASC) AS RowNum, 
			p.IdPortada,
            p.NombreImagen,
            p.Descripcion
		FROM Portada p WITH(NOLOCK)
		WHERE p.Estado=1
	) RECORD WHERE RECORD.RowNum>@start AND RECORD.RowNum<=@end

END
GO
/****** Object:  StoredProcedure [dbo].[UspListPreguntaByCuestionarioPaged]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UspListPreguntaByCuestionarioPaged]
(
	@IdCuestionario INT,
	@search VARCHAR(20),
	@start INT,
	@length INT
)
AS
BEGIN

	DECLARE @end INT;

	SET @end=(@start+@length);

	SELECT 
		RECORD.* 
	FROM
	(
		SELECT 
			COUNT(p.IdPregunta) OVER() TotalRegistros,
			ROW_NUMBER() OVER(ORDER BY p.IdPregunta ASC) AS RowNum, 
			p.IdPregunta,
			p.Nombre,
            p.Explicacion,
			p.Ayuda,
            p.Puntaje
		FROM Pregunta p WITH(NOLOCK)
		WHERE p.Estado=1 and p.IdCuestionario=@IdCuestionario
		AND 
		(
			p.Nombre LIKE ('%' + @search + '%')
		)
	) RECORD WHERE RECORD.RowNum>@start AND RECORD.RowNum<=@end

END
GO
/****** Object:  StoredProcedure [dbo].[UspListUsuarioPaged]    Script Date: 14/10/2022 17:52:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UspListUsuarioPaged]
(
	@search VARCHAR(20),
	@start INT,
	@length INT
)
AS
BEGIN

	DECLARE @end INT;

	SET @end=(@start+@length);

	SELECT 
		RECORD.* 
	FROM
	(
		SELECT 
			COUNT(p.IdUsuario) OVER() TotalRegistros,
			ROW_NUMBER() OVER(ORDER BY p.IdUsuario DESC) AS RowNum, 
			p.IdUsuario,
			p.Login,
			p.Password,
            p.Nombres,
            p.ApellidoPaterno,
            p.ApellidoMaterno,
			p.IdUsuarioTipo,
			ocu.Descripcion UsuarioTipo
		FROM Usuario p WITH(NOLOCK)
		INNER JOIN UsuarioTipo ocu WITH(NOLOCK) ON p.IdUsuarioTipo=ocu.IdUsuarioTipo
		WHERE p.Estado=1 and p.IdUsuario>1
		AND 
		(
			p.Nombres LIKE ('%' + @search + '%') OR
            p.ApellidoMaterno LIKE  ('%' + @search + '%') OR
            p.ApellidoMaterno LIKE ('%' + @search + '%') OR
            p.Login LIKE ('%' + @search + '%')
		)
	) RECORD WHERE RECORD.RowNum>@start AND RECORD.RowNum<=@end

END
GO
USE [master]
GO
ALTER DATABASE [db_CurCer] SET  READ_WRITE 
GO

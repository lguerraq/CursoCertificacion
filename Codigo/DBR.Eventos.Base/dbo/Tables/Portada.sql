CREATE TABLE [dbo].[Portada] (
    [IdPortada]           INT           IDENTITY (1, 1) NOT NULL,
    [NombreImagen]        VARCHAR (500) NULL,
    [Descripcion]         VARCHAR (500) NULL,
    [SubTitulo1]          VARCHAR (500) NULL,
    [SubTitulo2]          VARCHAR (500) NULL,
    [TextoEnlace]         VARCHAR (200) NULL,
    [UrlEnlace]           VARCHAR (500) NULL,
    [Estado]              BIT           NULL,
    [FechaCreacion]       DATETIME      NULL,
    [UsuarioCreacion]     INT           NULL,
    [FechaModificacion]   DATETIME      NULL,
    [UsuarioModificacion] INT           NULL,
    CONSTRAINT [PK_Portada] PRIMARY KEY CLUSTERED ([IdPortada] ASC)
);




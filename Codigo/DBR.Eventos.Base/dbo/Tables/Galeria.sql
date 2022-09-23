CREATE TABLE [dbo].[Galeria] (
    [IdGaleria]           INT           IDENTITY (1, 1) NOT NULL,
    [Descripcion]         VARCHAR (150) NULL,
    [Nombre]              VARCHAR (100) NULL,
    [Activo]              BIT           NULL,
    [Estado]              BIT           CONSTRAINT [DF_Galeria_Estado] DEFAULT ((1)) NOT NULL,
    [UsuarioCreacion]     INT           NOT NULL,
    [FechaCreacion]       DATETIME      CONSTRAINT [DF_Galeria_FechaCreacion] DEFAULT (getdate()) NOT NULL,
    [UsuarioModificacion] INT           NULL,
    [FechaModificacion]   DATETIME      NULL,
    CONSTRAINT [PK_Galeria] PRIMARY KEY CLUSTERED ([IdGaleria] ASC)
);




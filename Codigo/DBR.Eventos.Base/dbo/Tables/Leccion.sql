CREATE TABLE [dbo].[Leccion] (
    [IdLeccion]           INT            IDENTITY (1, 1) NOT NULL,
    [IdModulo]            INT            NULL,
    [Tipo]                INT            NULL,
    [Nombre]              VARCHAR (100)  NULL,
    [Descripcion]         NVARCHAR (MAX) NULL,
    [Duracion]            INT            NULL,
    [TipoUrl]             INT            NULL,
    [Url]                 VARCHAR (1000) NULL,
    [Orden]               INT            NULL,
    [Peso]                INT            NULL,
    [Estado]              BIT            NOT NULL,
    [UsuarioCreacion]     INT            NOT NULL,
    [FechaCreacion]       DATETIME       NOT NULL,
    [UsuarioModificacion] INT            NULL,
    [FechaModificacion]   DATETIME       NULL,
    CONSTRAINT [PK_Leccion] PRIMARY KEY CLUSTERED ([IdLeccion] ASC),
    CONSTRAINT [FK_Leccion_Modulo] FOREIGN KEY ([IdModulo]) REFERENCES [dbo].[Modulo] ([IdModulo])
);






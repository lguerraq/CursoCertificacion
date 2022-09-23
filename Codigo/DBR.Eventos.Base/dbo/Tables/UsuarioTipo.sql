CREATE TABLE [dbo].[UsuarioTipo] (
    [IdUsuarioTipo] INT          IDENTITY (1, 1) NOT NULL,
    [Descripcion]   VARCHAR (50) NULL,
    CONSTRAINT [PK_UsuarioTipo] PRIMARY KEY CLUSTERED ([IdUsuarioTipo] ASC)
);


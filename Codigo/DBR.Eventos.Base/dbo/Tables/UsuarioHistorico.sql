CREATE TABLE [dbo].[UsuarioHistorico] (
    [IdUsuarioHistorico] INT      IDENTITY (1, 1) NOT NULL,
    [IdUsuario]          INT      NULL,
    [FechaCreacion]      DATETIME NULL,
    CONSTRAINT [PK_UsuarioHistorico] PRIMARY KEY CLUSTERED ([IdUsuarioHistorico] ASC),
    CONSTRAINT [FK_UsuarioHistorico_Usuario] FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuario] ([IdUsuario])
);

